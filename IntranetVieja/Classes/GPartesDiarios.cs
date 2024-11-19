/*
 * Historial:
 * ===================================================================================
 * [06/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

/// <summary>
/// Clase para almacenar un Parte Diario.
/// </summary>
public class ParteDiario : IComparable
{
    // Variables.
    private int idParteDiario;
    private DateTime fechaParteDiario;
    private DateTime fechaCreacion;
    private int idPersona;
    private Persona persona;
    private EstadosParteDiario estado;
    private int idLicencia;
    private Licencia licencia;
    private List<ImputacionParteDiario> lstImputaciones;
    private bool finalizado;

    // Propiedades.
    public List<ImputacionParteDiario> Imputaciones
    {
        get { return lstImputaciones; }
    }
    public int ID
    {
        get { return idParteDiario; }
    }
    public DateTime Fecha
    {
        get { return fechaParteDiario; }
    }
    public DateTime FechaCreacion
    {
        get { return fechaCreacion; }
    }
    public Persona Persona
    {
        get
        {
            if (this.persona == null)
            {
                this.persona = GPersonal.GetPersona(this.idPersona);
            }

            return this.persona;
        }
    }
    public EstadosParteDiario Estado
    {
        get { return this.estado; }
    }
    public Licencia Licencia
    {
        get
        {
            if (this.estado != EstadosParteDiario.Licencia)
            {
                return null;
            }

            if (this.licencia == null)
            {
                this.licencia = GLicencias.GetLicencia(this.idLicencia);
            }

            return this.licencia;
        }
    }
    public bool Finalizado
    {
        get { return finalizado; }
    }
    public int TotalHoras
    {
        get
        {
            int result = 0;

            if (this.lstImputaciones != null)
            {
                this.lstImputaciones.ForEach(imp => result += imp.Horas);
            }

            return result;
        }
    }


    internal ParteDiario(int idParteDiario, DateTime fechaParteDiario, DateTime fechaCreacion,
        int idPersona, EstadosParteDiario estado, int idLicencia, bool finalizado)
    {
        this.idParteDiario = idParteDiario;
        this.fechaParteDiario = fechaParteDiario;
        this.fechaCreacion = fechaCreacion;
        this.idPersona = idPersona;
        this.estado = estado;
        this.idLicencia = idLicencia;
        this.finalizado = finalizado;
    }
    /// <summary>
    /// Carga las imputaciones del Parte Diario.
    /// </summary>
    public void CargarImputaciones()
    {
        this.lstImputaciones = GImputacionParteDiario.GetImputacionesPD(this.idParteDiario);
    }

    #region Miembros de IComparable

    public int CompareTo(object parteDiario)
    {
        ParteDiario pd = (ParteDiario)parteDiario;

        return this.Fecha.CompareTo(pd.Fecha);
    }

    #endregion
}
/// <summary>
/// Descripción breve de GPartesDiarios
/// </summary>
public static class GPartesDiarios
{
    // Constantes.
    public const int MaxHorasParteDiario = 12;


    /// <summary>
    /// Obtiene un parte diario.
    /// </summary>
    private static ParteDiario GetParteDiario(IDataReader dr)
    {
        ParteDiario result;

        try
        {
            result = new ParteDiario(
                Convert.ToInt32(dr["idParteDiario"]),
                Convert.ToDateTime(dr["FechaParte"]),
                Convert.ToDateTime(dr["FechaCreacion"]),
                Convert.ToInt32(dr["idPersona"]),
                (EstadosParteDiario)Convert.ToInt32(dr["idEstado"]),
                Convert.ToInt32(dr["idLicencia"]),
                Convert.ToBoolean(dr["Finalizado"])
            );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un parte diario para la persona actual.
    /// </summary>
    public static ParteDiario GetParteDiario(DateTime fecha)
    {
        return GetParteDiario(Constantes.Usuario.ID, fecha);
    }
    /// <summary>
    /// Obtiene un parte diario.
    /// </summary>
    public static ParteDiario GetParteDiario(int idPersona, DateTime fechaParteDiario)
    {
        IDbConnection conn = null;
        ParteDiario result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetParteDiario(conn, idPersona, fechaParteDiario);
        }
        catch
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
    /// Obtiene un parte diario.
    /// </summary>
    private static ParteDiario GetParteDiario(IDbConnection conn, int idPersona, DateTime fechaParteDiario)
    {
        IDbCommand cmd;
        IDataReader dr;
        ParteDiario result = null;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idParteDiario, idPersona, FechaParte, FechaCreacion, idEstado, ";
            cmd.CommandText += "idLicencia, Finalizado FROM ";
            cmd.CommandText += "tbl_PartesDiarios WHERE idPersona = @idPersona AND ";
            cmd.CommandText += "FechaParte=@fechaParte";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@fechaParte", Funciones.GetDate(fechaParteDiario)));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetParteDiario(dr);
            }

            dr.Close();
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un parte diario.
    /// </summary>
    public static ParteDiario GetParteDiario(int idParteDiario)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        ParteDiario result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idParteDiario, idPersona, FechaParte, FechaCreacion, idEstado, ";
            cmd.CommandText += "idLicencia, Finalizado FROM ";
            cmd.CommandText += "tbl_PartesDiarios WHERE idParteDiario = @idParteDiario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));
            dr = cmd.ExecuteReader();

            if(dr.Read())
            {
                result = GetParteDiario(dr);
            }

            dr.Close();
        }
        catch
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
    /// Obtiene el ID del parte diario.
    /// </summary>
    private static int GetIDParteDiario(IDbConnection conn, IDbTransaction trans, int idPersona, DateTime fecha)
    {
        int result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "SELECT idParteDiario FROM tbl_PartesDiarios ";
            cmd.CommandText += "WHERE idPersona = @idPersona AND FechaParte = @FechaParte";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaParte", Funciones.GetDate(fecha)));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            result = Constantes.ValorInvalido;
        }

        return result;
    }
    /// <summary>
    /// Obtiene el estado de un parte diario.
    /// </summary>
    internal static EstadosParteDiario GetEstadoParteDiario(IDbConnection conn, IDbTransaction trans, DateTime fechaParte)
    {
        EstadosParteDiario result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "SELECT idEstado FROM tbl_PartesDiarios WHERE idPersona = @idPersona AND FechaParte = @FechaParte";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaParte", Funciones.GetDate(fechaParte)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", Constantes.Usuario.ID));

            result = (EstadosParteDiario)Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Inserta un Parte Diario.
    /// </summary>
    internal static int InsertarParteDiario(IDbConnection conn, IDbTransaction trans, int idPersona,
        DateTime fechaParte)
    {
        return InsertarParteDiario(conn, trans, idPersona, fechaParte, Constantes.ValorInvalido, false);
    }
    /// <summary>
    /// Inserta el parte diario para una licencia.
    /// </summary>
    internal static void InsertarParteDiarioLicencia(IDbConnection conn, IDbTransaction trans, int idPersona,
        DateTime fechaParte, int idLicencia, bool finalizado)
    {
        int idParteDiario = GetIDParteDiario(conn, trans, idPersona, fechaParte);

        if (idParteDiario == Constantes.ValorInvalido)
        {
            InsertarParteDiario(conn, trans, idPersona, fechaParte, idLicencia, finalizado);
        }
        else
        {
            ActualizarParteDiarioLicencia(conn, trans, idParteDiario, idLicencia, finalizado);
        }
    }
    /// <summary>
    /// Inserta un Parte Diario.
    /// </summary>
    internal static int InsertarParteDiario(IDbConnection conn, IDbTransaction trans, int idPersona,
        DateTime fechaParte, int idLicencia, bool finalizado)
    {
        int result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "INSERT INTO tbl_PartesDiarios (idPersona, FechaParte, idEstado, idLicencia, Finalizado) ";
            cmd.CommandText += "VALUES (@idPersona, @FechaParte, @idEstado, @idLicencia, @Finalizado);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_PartesDiarios;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaParte", Funciones.GetDate(fechaParte)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)(idLicencia >= 0 ? 
                EstadosParteDiario.Licencia : EstadosParteDiario.Presente)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicencia", idLicencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", finalizado));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Actualiza un Parte Diario.
    /// </summary>
    private static void ActualizarParteDiarioLicencia(IDbConnection conn, IDbTransaction trans, int idParteDiario,
        int idLicencia, bool finalizado)
    {
        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "UPDATE tbl_PartesDiarios SET idEstado = @idEstado, idLicencia = @idLicencia, ";
            cmd.CommandText += "Finalizado = @Finalizado WHERE idParteDiario = @idParteDiario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosParteDiario.Licencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicencia", idLicencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", finalizado));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Elimina una licencia.
    /// </summary>
    internal static void EliminarLicencia(IDbConnection conn, IDbTransaction trans, int idLicencia)
    {
        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "UPDATE tbl_PartesDiarios SET idEstado = @idEstado, idLicencia = @idLicenciaNuevo, ";
            cmd.CommandText += "Finalizado = @Finalizado WHERE idLicencia = @idLicencia";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicencia", idLicencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosParteDiario.Presente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idLicenciaNuevo", Constantes.ValorInvalido));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", false));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Obtiene si se puede cargar una licencia en el intervalo.
    /// </summary>
    internal static bool PuedeCargarLicencia(IDbConnection conn, IDbTransaction trans, int idPersona, DateTime desde,
        DateTime hasta, TiposLicencia tipo)
    {
        bool result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "SELECT COUNT(idParteDiario) FROM tbl_PartesDiarios WHERE idPersona = @idPersona ";
            cmd.CommandText += "AND FechaParte BETWEEN @FechaDesde AND @FechaHasta AND (idEstado = @idEstado ";
            cmd.CommandText += "OR Finalizado = @Finalizado)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaDesde", Funciones.GetDate(desde)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaHasta", Funciones.GetDate(hasta)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosParteDiario.Licencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", true));

            result = Convert.ToInt32(cmd.ExecuteScalar()) == 0;
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Crea un parte diario para la persona actual.
    /// </summary>
    public static ParteDiario CrearParteDiario(DateTime fechaParte)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        ParteDiario result;

        result = GetParteDiario(Constantes.Usuario.ID, fechaParte);
        if (Funciones.GetDateEnd(fechaParte) > Funciones.GetDateEnd(DateTime.Now) || (result != null && (result.Finalizado || 
            (result.Licencia != null && !result.Licencia.PuedeCargarParteDiario))))
        {
            throw new DatosInvalidosException();
        }

        if (result == null)
        {
            try
            {
                conn = DataAccess.GetConnection(BDConexiones.Intranet);
                trans = DataAccess.GetTransaction(conn);

                InsertarParteDiario(conn, trans, Constantes.Usuario.ID, fechaParte);

                trans.Commit();
            }
            catch
            {
                if (trans != null)
                {
                    trans.Rollback();
                }

                throw new ErrorOperacionException();
            }
            finally
            {
                if (conn != null) { conn.Close(); }
            }

            result = GetParteDiario(fechaParte);
        }

        return result;
    }
    /// <summary>
    /// Guarda un parte diario.
    /// </summary>
    public static void GuardarParteDiario(int idParteDiario, List<ImputacionParteDiario> imputaciones, bool finalizar)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        if (imputaciones == null || imputaciones.Count == 0)
        {
            throw new DatosInvalidosException();
        }

        ParteDiario pd = GetParteDiario(idParteDiario);
        if (pd != null && (pd.Finalizado || (pd.Licencia != null && !pd.Licencia.PuedeCargarParteDiario)))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Borro las imputaciones asociadas al parte diario.
            GImputacionParteDiario.BorrarImputacionesPD(conn, trans, idParteDiario);

            // Inserto las imputaciones.
            foreach (ImputacionParteDiario imputacion in imputaciones)
            {
                GImputacionParteDiario.InsertarImputacionPD(conn, trans, idParteDiario, imputacion.Imputacion.ID, imputacion.Horas,
                    imputacion.TareasRealizadas, imputacion.TareasProgramadas, imputacion.NovedadesVehiculo,
                    imputacion.NovedadesHerramienta, imputacion.HayItr, imputacion.PersonalIntervinieron);
            }

            if (finalizar)
            {
                FinalizarParteDiario(conn, trans, idParteDiario);
            }

            trans.Commit();
        }
        catch
        {
            if (trans != null)
            {
                trans.Rollback();
            }

            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        // Muevo los ITR.
        if (finalizar)
        {
            try
            {
                RepositorioArchivos rep = GRepositorioArchivos.GetRepositorioArchivos(RepositoriosArchivos.ITR);

                imputaciones.ForEach(i =>
                {
                    if (i.HayItr)
                    {
                        string file = ITR.GetNombreITR(pd.Fecha, i.Imputacion.Numero,
                                                                          Constantes.Usuario.Usuario);
                        File.Move(Constantes.PATH_TEMP + file, rep.CarpetaActual.Path + file);

                        NotifVentas.UpdateNotifVenta(i.Imputacion.DescripcionCompleta, file);
                    }
                });
            }
            catch
            {
                throw new Exception("El parte diario se guardó correctamente, pero no se pudieron almacenar los ITR.");
            }
        }
    }
    /// <summary>
    /// Finaliza un parte diario.
    /// </summary>
    private static void FinalizarParteDiario(IDbConnection conn, IDbTransaction trans, int idParteDiario)
    {
        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "UPDATE tbl_PartesDiarios SET Finalizado = @Finalizado WHERE ";
            cmd.CommandText += "idParteDiario = @idParteDiario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", true));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Envía un e-mail de recordatorio de carga de parte diario.
    /// </summary>
    public static void EnviarEmailRecordatorio(int idPersona, DateTime fechaParte)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_PD_RECORDATORIO);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        Persona persona = GPersonal.GetPersona(idPersona);
        if (persona == null)
        {
            throw new EmailException();
        }

        strPlantilla = strPlantilla.Replace("@ENCABEZADO", "Este email es para recordarle la carga de un parte diario.");
        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", "error");
        strPlantilla = strPlantilla.Replace("@FECHA", fechaParte.ToShortDateString());

        string de = Constantes.EmailIntranet;
        /*if (Constantes.Usuario != null)
        {
            de = Constantes.Usuario.Email;
        }*/

        Email email = new Email(de, persona.Email, "", "Parte diario del " + fechaParte.ToShortDateString() 
            + " [RECORDATORIO]", strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene los partes diarios que coincidan con el filtro.
    /// </summary>
    public static Dictionary<int, DateTime> GetPartesDiarios(int idPersona, DateTime desde, DateTime hasta, 
        TiposLicencia filtro)
    {
        Dictionary<int, DateTime> result = new Dictionary<int,DateTime>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idParteDiario, FechaParte FROM ";
            cmd.CommandText += "tbl_PartesDiarios pd ";
            if (filtro == TiposLicencia.Presente)
            {
                cmd.CommandText += "WHERE idEstado = @idEstado AND Finalizado = @Finalizado AND idPersona = @idPersona ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosParteDiario.Presente));
            }
            else
            {
                cmd.CommandText += "RIGHT JOIN tbl_PDLicencias lic ON lic.idLicencia = pd.idLicencia ";
                cmd.CommandText += "WHERE lic.idSolicito = @idPersona AND lic.idEstadoAutoriz = @idEstadoAutoriz ";
                cmd.CommandText += "AND lic.idTipo = @idTipo ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstadoAutoriz", (int)EstadosLicencia.Confirmada));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idTipo", (int)filtro));
            }
            cmd.CommandText += "AND FechaParte BETWEEN @Desde AND @Hasta ORDER BY FechaParte";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Desde", Funciones.GetDate(desde)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Hasta", Funciones.GetDateEnd(hasta)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idParteDiario"]), Convert.ToDateTime(dr["FechaParte"]));
            }

            dr.Close();
        }
        catch
        {
            
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Controla la carga de los partes diarios.
    /// </summary>
    public static void ControlarCargaPartesDiarios(DateTime desde, DateTime hasta)
    {
        if (desde > hasta)
        {
            throw new DatosInvalidosException();
        }

        PanelControlPD pc = null;

        pc = new PanelControlPD(GPersonal.GetPersonas(Constantes.ValorInvalido, true, true));
        pc.CargarFilas(desde, hasta);

        foreach (FilaPCParteDiario fila in pc.Filas)
        {
            DateTime fecAux = desde;
            while (fecAux <= hasta)
            {
                object[] f = fila[fecAux];
                if (f == null)
                {
                    // No hay parte diario -> envío el recordatorio.
                    GPartesDiarios.EnviarEmailRecordatorio(fila.IdPersona, fecAux);
                }

                fecAux = fecAux.AddDays(1);
            }
        }
    }
}
