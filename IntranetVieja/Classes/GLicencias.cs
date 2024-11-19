/*
 * Historial:
 * ===================================================================================
 * [01/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de Licencia
/// </summary>
public class Licencia
{
    // Variables.
    private int idLicencia;
    private int idSolicito;
    private Persona solicito;
    private DateTime fechaCreacion;
    private DateTime fechaInicio;
    private DateTime fechaFin;
    private string observaciones;
    private TiposLicencia tipo;
    private EstadosLicencia estadoAutorizacion;

    // Propiedades.
    public int ID
    {
        get { return idLicencia; }
    }
    public DateTime Creacion
    {
        get { return fechaCreacion; }
    }
    public DateTime Inicio
    {
        get { return fechaInicio; }
    }
    public DateTime Finalizacion
    {
        get { return fechaFin; }
    }
    public string Observaciones
    {
        get { return observaciones; }
    }
    public TiposLicencia Tipo
    {
        get { return this.tipo; }
    }
    public EstadosLicencia EstadoAutorizacion
    {
        get { return this.estadoAutorizacion; }
    }
    public Persona Solicito
    {
        get
        {
            if (this.solicito == null)
            {
                this.solicito = GPersonal.GetPersona(this.idSolicito);
            }

            return this.solicito;
        }
    }
    public bool Finalizada
    {
        get
        {
            return estadoAutorizacion == EstadosLicencia.Confirmada;
        }
    }
    public bool PuedeCargarParteDiario
    {
        get
        {
            return this.tipo == TiposLicencia.ModificacionHorario;
        }
    }


    internal Licencia(int idLicencia, int idSolicito, DateTime fechaCreacion,
        DateTime fechaInicio, DateTime fechaFin, string observaciones, TiposLicencia tipo,
        EstadosLicencia estadoAutorizacion)
    {
        this.idLicencia = idLicencia;
        this.idSolicito = idSolicito;
        this.fechaCreacion = fechaCreacion;
        this.fechaInicio = fechaInicio;
        this.fechaFin = fechaFin;
        this.observaciones = observaciones;
        this.tipo = tipo;
        this.estadoAutorizacion = estadoAutorizacion;
    }
}

/// <summary>
/// Descripción breve de GsLicencias
/// </summary>
public static class GLicencias
{
    /// <summary>
    /// Obtiene una licencia.
    /// </summary>
    private static Licencia GetLicencia(IDataReader dr)
    {
        Licencia result;

        try
        {
            result = new Licencia(Convert.ToInt32(dr["idLicencia"]),
                    Convert.ToInt32(dr["idSolicito"]),
                    Convert.ToDateTime(dr["FechaCreacion"]),
                    Convert.ToDateTime(dr["FechaInicio"]),
                    Convert.ToDateTime(dr["FechaFin"]),
                    dr["Observaciones"].ToString(),
                    (TiposLicencia)Convert.ToInt32(dr["idTipo"]),
                    (EstadosLicencia)Convert.ToInt32(dr["idEstadoAutoriz"])
                );
        }
        catch
        {		
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una licencia.
    /// </summary>
    private static Licencia GetLicencia(DataRow dr)
    {
        Licencia result;

        try
        {
            result = new Licencia(Convert.ToInt32(dr["idLicencia"]),
                    Convert.ToInt32(dr["idSolicito"]),
                    Convert.ToDateTime(dr["FechaCreacion"]),
                    Convert.ToDateTime(dr["FechaInicio"]),
                    Convert.ToDateTime(dr["FechaFin"]),
                    dr["Observaciones"].ToString(),
                    (TiposLicencia)Convert.ToInt32(dr["idTipo"]),
                    (EstadosLicencia)Convert.ToInt32(dr["idEstadoAutoriz"])
                );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Carga una  Licencia.
    /// </summary>
    public static Licencia GetLicencia(int idLicencia)
    {
        IDbConnection conn = null;
        IDbCommand cmd;;
        IDataReader dr;
        Licencia result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idLicencia, idSolicito, FechaCreacion, FechaInicio, FechaFin, idTipo,";
            cmd.CommandText += "Observaciones, idEstadoAutoriz ";
            cmd.CommandText += "FROM tbl_PDLicencias WHERE idLicencia = @idLicencia";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicencia", idLicencia));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetLicencia(dr);
            }

            dr.Close();
        }
        catch(Exception e)
        {
            result = null;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Almacena una  Licencia.
    /// </summary>

    public static void NuevaLicencia(DateTime fechaInicio, DateTime fechaFin, string observaciones, TiposLicencia tipo)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        if (/*(fechaInicio - DateTime.Now).Days < 0 || */fechaInicio > fechaFin ||
            ((tipo != TiposLicencia.ModificacionHorario) && observaciones.Trim().Length == 0))
        {
            throw new DatosInvalidosException();
        }

        int idLicencia;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

         
            // if(Constantes.Usuario == null){
				// throw new Exception("usuario");
			// }

			// if(Constantes.Usuario.ID == null){
				// throw new Exception("id");
			// }
			
			
            // Compruebo que no existan partes diarios cargados.
            if (!GPartesDiarios.PuedeCargarLicencia(conn, trans, Constantes.Usuario.ID, fechaInicio, fechaFin, tipo))
            {
               throw new IntervaloAsignadoException();
            }

            // Inserto la licencia.
            idLicencia = InsertarLicencia(conn, trans, Constantes.Usuario.ID, fechaInicio, fechaFin, observaciones, tipo);

            // Inserto los partes diarios correspondientes.
            List<DateTime> fechas = Funciones.ListarIntervaloFechas(fechaInicio, fechaFin);
            foreach (DateTime fechaPD in fechas)
            {
                // Guardo el Parte Diario.
                GPartesDiarios.InsertarParteDiario(conn, trans, Constantes.Usuario.ID, fechaPD, idLicencia, 
                    (tipo != TiposLicencia.ModificacionHorario));

                // Agrego el detalle de asistencia.
                AsistenciaFac.AddAsistenciaEntrada(fechaPD, Constantes.Usuario.ID, EstadoAsistencia.Licencia,
                                                   observaciones,
                                                   Constantes.FechaInvalida, Constantes.FechaInvalida,
                                                   (tipo != TiposLicencia.ModificacionHorario), idLicencia, trans);
            }

            // Finalizo la transacción.
			trans.Commit();
        }
        catch(Exception e)
        {
            if (trans != null)
            {
                trans.Rollback();
            }

            throw new Exception(e.Message);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        EnviarLicencia(idLicencia);
    }
    /// <summary>
    /// Inserta una  Licencia.
    /// </summary>
    private static int InsertarLicencia(IDbConnection conn, IDbTransaction trans, int idSolicito, DateTime fechaInicio, 
        DateTime fechaFin, string observaciones, TiposLicencia tipo)
    {
        int result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "INSERT INTO tbl_PDLicencias (idSolicito, FechaInicio, FechaFin, ";
            cmd.CommandText += "Observaciones, idTipo, idEstadoAutoriz) VALUES (";
            cmd.CommandText += "@idSolicito, @FechaInicio, @FechaFin, @Observaciones, @idTipo, ";
            cmd.CommandText += "@idEstadoAutoriz);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_PDLicencias;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicito", idSolicito));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaInicio", fechaInicio));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaFin", fechaFin));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idTipo", (int)tipo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", observaciones));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstadoAutoriz", EstadosLicencia.NoRecibida));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Aprueba el estado actual.
    /// </summary>
    public static void AprobarEstadoActual(int idLicencia)
    {
        ActualizarEstadoLicencia(idLicencia, true);
    }
    /// <summary>
    /// Rechaza el estado actual.
    /// </summary>
    public static void RechazarEstadoActual(int idLicencia)
    {
        ActualizarEstadoLicencia(idLicencia, false);
    }
    /// <summary>
    /// Actualiza el estado de la  Licencia.
    /// </summary>
    private static void ActualizarEstadoLicencia(int idLicencia, bool aprobar)
    {
        Licencia licencia = GetLicencia(idLicencia);
        EstadosLicencia estadoNuevo;
        if (licencia == null)
        {
            throw new ErrorOperacionException();
        }

        switch (licencia.EstadoAutorizacion)
        {
            case EstadosLicencia.NoRecibida:
                if (licencia.Solicito.IdAutoriza != Constantes.Usuario.ID)
                {
                    throw new ErrorOperacionException();
                }
                estadoNuevo = aprobar ? EstadosLicencia.AprobadaResponsable : EstadosLicencia.RechazadaResponsable;
                break;
            case EstadosLicencia.AprobadaResponsable:
                if (!GPermisosPersonal.TieneAcceso(PermisosPersona.LicRRHH))
                {
                    throw new ErrorOperacionException();
                }
                estadoNuevo = aprobar ? EstadosLicencia.Confirmada : EstadosLicencia.RechazadaRRHH;
                break;
            default:
                throw new ErrorOperacionException();
        }

        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);
            cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "UPDATE tbl_PDLicencias SET idEstadoAutoriz = @idEstadoAutoriz WHERE ";
            cmd.CommandText += "idLicencia = @idLicencia";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicencia", idLicencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstadoAutoriz", (int)estadoNuevo));

            cmd.ExecuteNonQuery();

            if (!aprobar)
            {
                GPartesDiarios.EliminarLicencia(conn, trans, idLicencia);

            }

            trans.Commit();
        }
        catch
        {
            if (trans != null)
            {
                trans.Rollback();
            }
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        EnviarLicencia(idLicencia);
    }
	
	
	public static void EnviarLicencia2() // TRUCHO
    {
        //Cargo la Plantilla.
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_LICENCIA);
        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        Licencia licencia = GetLicencia(83829);
        if (licencia == null)
        {
            throw new EmailException();
        }

        string para = "";
        string cc = "";
        string asunto = "";
        string tipo = "info";

        switch (licencia.EstadoAutorizacion)
        {
            case EstadosLicencia.NoRecibida:
                para = licencia.Solicito.Autoriza.Email;
                asunto = "Solicitud";
                break;
            case EstadosLicencia.AprobadaResponsable:
                para = Constantes.EmailRRHH;
                cc = licencia.Solicito.Email;
                asunto = "Aprobada por responsable de área";
                break;
            case EstadosLicencia.RechazadaResponsable:
                para = licencia.Solicito.Email;
                asunto = "Rechazada por responsable de área";
                tipo = "error";
                break;
            case EstadosLicencia.Confirmada:
                para = licencia.Solicito.Email;
                cc = licencia.Solicito.Autoriza.Email;
                asunto = "Confirmada";
                tipo = "success";
                break;
            case EstadosLicencia.RechazadaRRHH:
                para = licencia.Solicito.Email;
                cc = licencia.Solicito.Autoriza.Email;
                asunto = "Rechazada por RR.HH.";
                tipo = "error";
                break;
        }
        asunto = String.Format("Solicitud de licencia ({0} - {1}) [{2}]", licencia.Inicio.ToShortDateString(),
            licencia.Finalizacion.ToShortDateString(), asunto);

        if (para.Length > 0)
        {
            // Reemplazo las variables.
            strPlantilla = strPlantilla.Replace("@ENCABEZADO", "");
            strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", tipo);
            strPlantilla = strPlantilla.Replace("@EMITIDA_POR", licencia.Solicito.Nombre);
            strPlantilla = strPlantilla.Replace("@MOTIVO", GetDescripcionTipoLicencia(licencia.Tipo));
            strPlantilla = strPlantilla.Replace("@DESDE", licencia.Inicio.ToShortDateString());
            strPlantilla = strPlantilla.Replace("@HASTA", licencia.Finalizacion.ToShortDateString());
            strPlantilla = strPlantilla.Replace("@OBSERVACIONES", licencia.Observaciones);
            strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/rrhh/licenciaAdmin.aspx",
                "id=" + "83829"));

            // Envío el E-mail.
            Email email = new Email(Constantes.Usuario.Email, para, cc, asunto, strPlantilla);
            if (!email.Enviar())
            {
                throw new EmailException();
            }
        }
    }
	
	
    /// <summary>
    /// Envia la licencia a quien corresponda.
    /// </summary>
    public static void EnviarLicencia(int idLicencia)
    {
        //Cargo la Plantilla.
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_LICENCIA);
        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        Licencia licencia = GetLicencia(idLicencia);
        if (licencia == null)
        {
            throw new EmailException();
        }

        string para = "";
        string cc = "";
        string asunto = "";
        string tipo = "info";

        switch (licencia.EstadoAutorizacion)
        {
            case EstadosLicencia.NoRecibida:
                para = licencia.Solicito.Autoriza.Email;
                asunto = "Solicitud";
                break;
            case EstadosLicencia.AprobadaResponsable:
                para = Constantes.EmailRRHH;
                cc = licencia.Solicito.Email;
                asunto = "Aprobada por responsable de área";
                break;
            case EstadosLicencia.RechazadaResponsable:
                para = licencia.Solicito.Email;
                asunto = "Rechazada por responsable de área";
                tipo = "error";
                break;
            case EstadosLicencia.Confirmada:
                para = licencia.Solicito.Email;
                cc = licencia.Solicito.Autoriza.Email;
                asunto = "Confirmada";
                tipo = "success";
                break;
            case EstadosLicencia.RechazadaRRHH:
                para = licencia.Solicito.Email;
                cc = licencia.Solicito.Autoriza.Email;
                asunto = "Rechazada por RR.HH.";
                tipo = "error";
                break;
        }
        asunto = String.Format("Solicitud de licencia ({0} - {1}) [{2}]", licencia.Inicio.ToShortDateString(),
            licencia.Finalizacion.ToShortDateString(), asunto);

        if (para.Length > 0)
        {
            // Reemplazo las variables.
            strPlantilla = strPlantilla.Replace("@ENCABEZADO", "");
            strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", tipo);
            strPlantilla = strPlantilla.Replace("@EMITIDA_POR", licencia.Solicito.Nombre);
            strPlantilla = strPlantilla.Replace("@MOTIVO", GetDescripcionTipoLicencia(licencia.Tipo));
            strPlantilla = strPlantilla.Replace("@DESDE", licencia.Inicio.ToShortDateString());
            strPlantilla = strPlantilla.Replace("@HASTA", licencia.Finalizacion.ToShortDateString());
            strPlantilla = strPlantilla.Replace("@OBSERVACIONES", licencia.Observaciones);
            strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/rrhh/licenciaAdmin.aspx",
                "id=" + idLicencia));

            // Envío el E-mail.
            Email email = new Email(Constantes.Usuario.Email, para, cc, asunto, strPlantilla);
            if (!email.Enviar())
            {
                throw new EmailException();
            }
        }
    }
    /// <summary>
    /// Obtiene una descripción para el tipo de licencia.
    /// </summary>
    public static string GetDescripcionTipoLicencia(TiposLicencia tipo)
    {
        switch (tipo)
        {
            case TiposLicencia.Vacaciones:
                return "Licencia por vacaciones";
            case TiposLicencia.Franco:
                return "Licencia por franco";
            case TiposLicencia.Examen:
                return "Licencia por examen";
            case TiposLicencia.SinGoceHaberes:
                return "Licencia sin goce de haberes";
            case TiposLicencia.Casamiento:
                return "Licencia por casamiento";
            case TiposLicencia.Nacimiento:
                return "Licencia por nacimiento";
            case TiposLicencia.ModificacionHorario:
                return "Modificación en el horario de entrada / salida";
            case TiposLicencia.Presente:
                return "Presente";
        }

        return "<no disponible>";
    }
    /// <summary>
    /// Obtiene los tipos de licencia.
    /// </summary>
    public static Dictionary<int, string> GetTiposLicencia()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)TiposLicencia.Casamiento, GetDescripcionTipoLicencia(TiposLicencia.Casamiento));
        result.Add((int)TiposLicencia.Examen, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Examen));
        result.Add((int)TiposLicencia.Franco, GetDescripcionTipoLicencia(TiposLicencia.Franco));
        result.Add((int)TiposLicencia.ModificacionHorario, GetDescripcionTipoLicencia(TiposLicencia.ModificacionHorario));
        result.Add((int)TiposLicencia.SinGoceHaberes, GetDescripcionTipoLicencia(TiposLicencia.SinGoceHaberes));
        result.Add((int)TiposLicencia.Vacaciones, GetDescripcionTipoLicencia(TiposLicencia.Vacaciones));
        result.Add((int)TiposLicencia.Nacimiento, GetDescripcionTipoLicencia(TiposLicencia.Nacimiento));

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de licencia para filtros.
    /// </summary>
    public static Dictionary<int, string> GetTiposLicenciasFiltros()
    {
        Dictionary<int, string> result;

        result = GetTiposLicencia();
        result.Add((int)TiposLicencia.Presente, GetDescripcionTipoLicencia(TiposLicencia.Presente));

        return result;
    }
}
