using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Autorizacion
{
    // Variables.
    private int idAutorizacion;
    private int idSolicito;
    private Persona solicito;
    private DateTime fechaSolic;
    private string motivoAutoriz;
    private int idResponsable;
    private Persona responsable;
    private EstadoAutorizacion estado;
    private SeccionAutorizacion referencia;
    private string motivoRechazo;
    private DateTime fechaAutoriz;

    // Propiedaes.
    public int IdAutorizacion
    {
        get { return this.idAutorizacion; }
    }
    public int IdSolicito
    {
        get { return this.idSolicito; }
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
    public DateTime FechaSolicito
    {
        get { return this.fechaSolic; }
    }
    public string MotivoAutorizacion
    {
        get { return this.motivoAutoriz; }
    }
    public int IdResponsable
    {
        get { return this.idResponsable; }
    }
    public Persona Responsable
    {
        get
        {
            if (this.responsable == null)
            {
                this.responsable = GPersonal.GetPersona(this.idResponsable);
            }

            return this.responsable;
        }
    }
    public EstadoAutorizacion Estado
    {
        get { return this.estado; }
    }
    public SeccionAutorizacion Referencia
    {
        get { return this.referencia; }
    }
    public string MotivoRechazo
    {
        get { return this.motivoRechazo; }
    }
    public DateTime FechaAutorizo
    {
        get { return this.fechaAutoriz; }
    }


    internal Autorizacion(int idAutorizacion, int idSolicito, DateTime fechaSolic, string motivoAutoriz, int idResponsable,
        EstadoAutorizacion estado, SeccionAutorizacion referencia, string motivoRechazo, DateTime fechaAutoriz)
    {
        this.idAutorizacion = idAutorizacion;
        this.idSolicito = idSolicito;
        this.fechaSolic = fechaSolic;
        this.motivoAutoriz = motivoAutoriz;
        this.idResponsable = idResponsable;
        this.estado = estado;
        this.referencia = referencia;
        this.motivoRechazo = motivoRechazo;
        this.fechaAutoriz = fechaAutoriz;
    }
}

/// <summary>
/// Summary description for Autorizaciones
/// </summary>
public static class Autorizaciones
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;


    /// <summary>
    /// Obtiene una autorización.
    /// </summary>
    internal static Autorizacion GetAutorizacion(IDataReader dr)
    {
        Autorizacion result;

        try
        {
            result = new Autorizacion(Convert.ToInt32(dr["idAutorizacion"]), Convert.ToInt32(dr["idSolicito"]),
                Convert.ToDateTime(dr["FechaSolic"]), dr["MotivoAutoriz"].ToString(), Convert.ToInt32(dr["idResponsable"]),
                (EstadoAutorizacion)Convert.ToInt32(dr["idEstado"]), (SeccionAutorizacion)Convert.ToInt32(dr["idReferencia"]),
                dr["MotivoRechazo"].ToString(), Convert.ToDateTime(dr["FechaAutoriz"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una autorización.
    /// </summary>
    internal static Autorizacion GetAutorizacion(DataRow dr)
    {
        Autorizacion result;

        try
        {
            result = new Autorizacion(Convert.ToInt32(dr["idAutorizacion"]), Convert.ToInt32(dr["idSolicito"]),
                Convert.ToDateTime(dr["FechaSolic"]), dr["MotivoAutoriz"].ToString(), Convert.ToInt32(dr["idResponsable"]),
                (EstadoAutorizacion)Convert.ToInt32(dr["idEstado"]), (SeccionAutorizacion)Convert.ToInt32(dr["idReferencia"]),
                dr["MotivoRechazo"].ToString(), Convert.ToDateTime(dr["FechaAutoriz"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una autorización.
    /// </summary>
    public static Autorizacion GetAutorizacion(int idAutorizacion)
    {
        Autorizacion result = null;
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Autorizaciones WHERE idAutorizacion = @idAutorizacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutorizacion", idAutorizacion));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetAutorizacion(dr);
            }

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
    /// <summary>
    /// Genera una nueva autorización.
    /// </summary>
    internal static int NuevaAutorizacion(string motivoAutoriz, int idResponsable, SeccionAutorizacion referencia, 
        IDbTransaction trans)
    {
        int result;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(motivoAutoriz) || idResponsable == Constantes.IdPersonaInvalido)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Autorizaciones (idSolicito, MotivoAutoriz, idResponsable, idEstado, ";
            cmd.CommandText += "idReferencia) VALUES (@idSolicito, @MotivoAutoriz, @idResponsable, @idEstado, @idReferencia); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Autorizaciones;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicito", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@MotivoAutoriz", motivoAutoriz));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idResponsable", idResponsable));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadoAutorizacion.Pendiente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idReferencia", (int)referencia));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Aprueba una autorización.
    /// </summary>
    public static void AprobarAutorizacion(int idAutorizacion)
    {
        ActualizarAutorizacion(idAutorizacion, "", true);
    }
    /// <summary>
    /// Rechaza una autorización.
    /// </summary>
    public static void RechazarAutorizacion(int idAutorizacion, string motivo)
    {
        ActualizarAutorizacion(idAutorizacion, motivo, false);
    }
    /// <summary>
    /// Actualiza una autorización.
    /// </summary>
    private static void ActualizarAutorizacion(int idAutorizacion, string motivo, bool aprobar)
    {
        if (!aprobar && String.IsNullOrEmpty(motivo))
        {
            throw new DatosInvalidosException();
        }

        Autorizacion au = GetAutorizacion(idAutorizacion);
        if (au == null)
        {
            throw new DatosInvalidosException();
        }

        if (au.IdResponsable != Constantes.Usuario.ID || au.Estado != EstadoAutorizacion.Pendiente)
        {
            throw new DatosInvalidosException();
        }

        IDbConnection conn = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_Autorizaciones SET idEstado = @idEstado, FechaAutoriz = @FechaAutoriz ";
            if (!aprobar)
            {
                cmd.CommandText += ", MotivoRechazo = @MotivoRechazo ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@MotivoRechazo", motivo));
            }
            cmd.CommandText += "WHERE idAutorizacion = @idAutorizacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaAutoriz", DateTime.Now));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", aprobar ? (int)EstadoAutorizacion.Aprobada : 
                (int)EstadoAutorizacion.Rechazada));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutorizacion", idAutorizacion));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        EnviarAutorizacion(idAutorizacion);
    }
    /// <summary>
    /// Envía una autorización.
    /// </summary>
    internal static void EnviarAutorizacion(int idAutorizacion)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_AUTORIZACION);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        Autorizacion au = GetAutorizacion(idAutorizacion);
        if (au == null)
        {
            throw new EmailException();
        }

        if (au.Solicito == null || au.Responsable == null)
        {
            throw new EmailException();
        }

        string encabezado = "";
        string asunto = "Solicitud de autorización Nº" + au.IdAutorizacion + " ";
        string tipoMensaje = "info";
        string de;
        string para;
        string cc = "";
        switch (au.Estado)
        {
            case EstadoAutorizacion.Pendiente:
                encabezado = "El usuario " + au.Solicito.Nombre + " solició la siguiente autorización:";
                de = cc = au.Solicito.Email;
                para = au.Responsable.Email;
                break;
            case EstadoAutorizacion.Rechazada:
                encabezado = "La solicitud de autorización ha sido rechazada por el siguiente motivo: " + au.MotivoRechazo + ".";
                asunto += "[RECHAZADA]";
                tipoMensaje = "error";
                de = cc = au.Responsable.Email;
                para = au.Solicito.Email;
                break;
            case EstadoAutorizacion.Aprobada:
                encabezado = "La solicitud de autorización fue aprobada.";
                asunto += "[APROBADA]";
                tipoMensaje = "success";
                de = cc = au.Responsable.Email;
                para = au.Solicito.Email;
                break;
            default:
                throw new EmailException();
        }

        strPlantilla = strPlantilla.Replace("@ENCABEZADO", encabezado);
        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", tipoMensaje);
        strPlantilla = strPlantilla.Replace("@FECHA", au.FechaSolicito.ToShortDateString());
        strPlantilla = strPlantilla.Replace("@REFERENCIA", GetReferencia(au.Referencia));
        strPlantilla = strPlantilla.Replace("@MOTIVO", au.MotivoAutorizacion);
        strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/general/autorizAdmin.aspx", "id=" + idAutorizacion));

        Email email = new Email(de, para, cc, asunto, strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene la referencia para la sección.
    /// </summary>
    public static string GetReferencia(SeccionAutorizacion seccion)
    {
        string result;

        switch (seccion)
        {
            case SeccionAutorizacion.InformacionInternaObra:
                result = "Información Interna de Obra";
                break;
            default:
                result = "<no disponible>";
                break;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una lista con las codificaciones de artículos.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<Autorizacion> GetAutorizaciones(int pagina, List<Filtro> filtros)
    {
        List<Autorizacion> result;

        result = DataAccess.GetDataList<Autorizacion>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetAutorizacion);

        return result;
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosAutorizacion.Solicito:
                    filtroWhere += "AND idSolicito = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosAutorizacion.Estado:
                    filtroWhere += "AND idEstado = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosAutorizacion.Referencia:
                    filtroWhere += "AND idReferencia = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosAutorizacion.Responsable:
                    filtroWhere += "AND idResponsable = " + filtro.Valor + " ";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idAutorizacion) as TotalRegistros FROM tbl_Autorizaciones "
                     + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "");
        }
        else
        {
            consulta = "SELECT * FROM tbl_Autorizaciones " + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "")
                     + " ORDER BY idAutorizacion DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetCantidadPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
    }
    /// <summary>
    /// Obtiene los tipos de estado para las solicitudes.
    /// </summary>
    public static Dictionary<int, string> GetEstadosSolicitudes()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadoAutorizacion.Pendiente, "Pendiente");
        result.Add((int)EstadoAutorizacion.Aprobada, "Aprobada");
        result.Add((int)EstadoAutorizacion.Rechazada, "Rechazada");

        return result;
    }
}