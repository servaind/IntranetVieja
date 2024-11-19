/*
 * Historial:
 * ===================================================================================
 * [25/08/2011]
 * - Agregado el estado No corresponde.
 * [19/05/2011]
 * - Versión estable.
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;

public class CodificacionArticulo
{
    // Variables.
    private int idCodificacion;
    private string descripcionArticulo;
    private int idUnidadMedida;
    private string unidadMedida;
    private string codigoArticulo;
    private string descripcionUso;
    private DateTime fechaSolicitud;
    private int idSolicito;
    private Persona solicito;
    private EstadosCodArt estado;
    private DateTime fechaAprobo;

    // Propiedades.
    public int IdCodificacion
    {
        get { return this.idCodificacion; }
    }
    public string DescripcionArticulo
    {
        get { return this.descripcionArticulo; }
    }
    public int IdUnidadMedida
    {
        get { return this.idUnidadMedida; }
    }
    public string UnidadMedida
    {
        get
        {
            if (this.unidadMedida == null || this.unidadMedida.Trim().Length == 0)
            {
                // Cargar la uniadd de medida;
                this.unidadMedida = CodificacionArticulos.GetUnidadMedida(this.idUnidadMedida);
            }

            return this.unidadMedida;
        }
    }
    public string CodigoArticulo
    {
        get { return this.codigoArticulo; }
    }
    public string DescripcionUso
    {
        get { return this.descripcionUso; }
    }
    public DateTime FechaSolicitud
    {
        get { return this.fechaSolicitud; }
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
                // Cargar el solicitante.
                this.solicito = GPersonal.GetPersona(this.idSolicito);
            }

            return this.solicito;
        }
    }
    public EstadosCodArt Estado
    {
        get { return this.estado; }
    }
    public DateTime FechaAprobo
    {
        get { return this.fechaAprobo; }
    }


    internal CodificacionArticulo(int idCodificacion, string descripcionArticulo, int idUnidadMedida, string codigoArticulo,
        string descripcionUso, DateTime fechaSolicitud, int idSolicito, EstadosCodArt estado, DateTime fechaAprobo)
    {
        this.idCodificacion = idCodificacion;
        this.descripcionArticulo = descripcionArticulo;
        this.idUnidadMedida = idUnidadMedida;
        this.codigoArticulo = codigoArticulo;
        this.descripcionUso = descripcionUso;
        this.fechaSolicitud = fechaSolicitud;
        this.idSolicito = idSolicito;
        this.estado = estado;
        this.fechaAprobo = fechaAprobo;
    }

    /// <summary>
    /// Da formato al número de referencia.
    /// </summary>
    public string GetNumeroReferencia()
    {
        return this.idCodificacion.ToString("0000");
    }
}
/// <summary>
/// Summary description for CodificacionArticulos
/// </summary>
public class CodificacionArticulos
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;


    /// <summary>
    /// Obtiene una codificación de un artículo.
    /// </summary>
    private static CodificacionArticulo GetCodificacionArticulo(IDataReader dr)
    {
        CodificacionArticulo result;

        try
        {
            result = new CodificacionArticulo(Convert.ToInt32(dr["idCodificacion"]),
                dr["DescripcionArticulo"].ToString(),
                Convert.ToInt32(dr["idUnidadMedida"]),
                dr["CodigoArticulo"].ToString().Trim(),
                dr["DescripcionUso"].ToString(),
                Convert.ToDateTime(dr["FechaSolicitud"]),
                Convert.ToInt32(dr["IdSolicito"]),
                (EstadosCodArt)Convert.ToInt32(dr["IdEstado"]),
                Convert.ToDateTime(dr["FechaAprobo"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una codificación de un artículo.
    /// </summary>
    private static CodificacionArticulo GetCodificacionArticulo(DataRow dr)
    {
        CodificacionArticulo result;

        try
        {
            result = new CodificacionArticulo(Convert.ToInt32(dr["idCodificacion"]),
                dr["DescripcionArticulo"].ToString(),
                Convert.ToInt32(dr["idUnidadMedida"]),
                dr["CodigoArticulo"].ToString().Trim(),
                dr["DescripcionUso"].ToString(),
                Convert.ToDateTime(dr["FechaSolicitud"]),
                Convert.ToInt32(dr["IdSolicito"]),
                (EstadosCodArt)Convert.ToInt32(dr["IdEstado"]),
                Convert.ToDateTime(dr["FechaAprobo"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una codificación de un artículo.
    /// </summary>
    public static CodificacionArticulo GetCodificacionArticulo(int idCodificacion)
    {
        CodificacionArticulo result = null;
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_CodificacionArticulos WHERE idCodificacion = @idCodificacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idCodificacion", idCodificacion));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetCodificacionArticulo(dr);
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
    /// Obtiene la descripción de una unidad de medida.
    /// </summary>
    internal static string GetUnidadMedida(int idUnidadMedida)
    {
        string result;
        IDbConnection conn = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Descripcion FROM tbl_CodificacionArticulosMedidas WHERE idUnidad = @idUnidadMedida";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idUnidadMedida", idUnidadMedida));

            result = cmd.ExecuteScalar().ToString();
        }
        catch
        {
            result = "-";
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
    /// Obtiene las unidades de medida disponibles.
    /// </summary>
    public static Dictionary<int, string> GetUnidadesMedida()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_CodificacionArticulosMedidas ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idUnidad"]), dr["Descripcion"].ToString());
            }

            dr.Close();
        }
        catch
        {

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
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string filtroJoin = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosCodArt.Solicito:
                    filtroWhere += "AND idSolicito = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosCodArt.Estado:
                    filtroWhere += "AND idEstado = " + filtro.Valor + " ";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idCodificacion) as TotalRegistros FROM tbl_CodificacionArticulos " 
                     + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "");
        }
        else
        {
            consulta = "SELECT * FROM tbl_CodificacionArticulos " + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "") 
                     + " ORDER BY idCodificacion DESC";
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
    /// Obtiene una lista con las codificaciones de artículos.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<CodificacionArticulo> GetCodificacionesArticulos(int pagina, List<Filtro> filtros)
    {
        List<CodificacionArticulo> result;

        result = DataAccess.GetDataList<CodificacionArticulo>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetCodificacionArticulo);

        return result;
    }
    /// <summary>
    /// Da de alta una nueva codificación de artículo.
    /// </summary>
    public static void AltaCodificacionArticulo(string descripcionArticulo, int idUnidadMedida, string codigoArticulo,
        string descripcionUso, out int numeroReferencia)
    {
        bool result;
        IDbConnection conn = null;
        IDbCommand cmd;
        numeroReferencia = Constantes.ValorInvalido;

        if (descripcionArticulo.Trim().Length == 0 || idUnidadMedida < 0 || !EsCodigoArticuloValido(codigoArticulo)
            || descripcionUso.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        if (GArticuloTango.ExisteCodigoArticulo(codigoArticulo))
        {
            throw new ElementoExistenteException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_CodificacionArticulos (DescripcionArticulo, IdUnidadMedida, CodigoArticulo, ";
            cmd.CommandText += "DescripcionUso, IdSolicito, idEstado) VALUES (@DescripcionArticulo, @IdUnidadMedida, ";
            cmd.CommandText += "@CodigoArticulo, @DescripcionUso, @IdSolicito, @idEstado); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_CodificacionArticulos;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DescripcionArticulo", descripcionArticulo.ToUpper()));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@IdUnidadMedida", idUnidadMedida));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CodigoArticulo", codigoArticulo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DescripcionUso", descripcionUso));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@IdSolicito", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosCodArt.Revision));

            numeroReferencia = Convert.ToInt32(cmd.ExecuteScalar());

            result = true;
        }
        catch
        {
            result = false;
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        if (result)
        {
            EnviarCodificacionArticulo(numeroReferencia, "");
        }
    }
    /// <summary>
    /// Actualiza una codificación de artículo.
    /// </summary>
    public static void ActualizarCodificacionArticulo(int idCodificacion, string descripcionArticulo, int idUnidadMedida,
        string codigoArticulo, string descripcionUso, bool aprobada)
    {
        bool result;
        IDbConnection conn = null;
        IDbCommand cmd;
        EstadosCodArt estadoAnt;
        EstadosCodArt estado;

        if (descripcionArticulo.Trim().Length == 0 || idUnidadMedida < 0 || !EsCodigoArticuloValido(codigoArticulo)
            || descripcionUso.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            CodificacionArticulo c = GetCodificacionArticulo(idCodificacion);

            if ((aprobada && c.Estado == EstadosCodArt.Rechazado) || c.Estado == EstadosCodArt.NoCorresponde || 
                c.Estado == EstadosCodArt.Aprobado)
            {
                throw new Exception("Operación no válida.");
            }

            estadoAnt = c.Estado;
            if (estadoAnt == EstadosCodArt.Rechazado)
            {
                estado = EstadosCodArt.Revision;
            }
            else
            {
                estado = estadoAnt;
            }

            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_CodificacionArticulos SET DescripcionArticulo = @DescripcionArticulo, ";
            cmd.CommandText += "IdUnidadMedida = @IdUnidadMedida, CodigoArticulo = @CodigoArticulo, ";
            cmd.CommandText += "DescripcionUso = @DescripcionUso, idEstado = @idEstado ";
            if (aprobada)
            {
                estado = EstadosCodArt.Aprobado;
                cmd.CommandText += ", FechaAprobo = @FechaAprobo ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaAprobo", DateTime.Now));
            }
            cmd.CommandText += "WHERE idCodificacion = @idCodificacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DescripcionArticulo", descripcionArticulo.ToUpper()));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@IdUnidadMedida", idUnidadMedida));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CodigoArticulo", codigoArticulo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DescripcionUso", descripcionUso));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idCodificacion", idCodificacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)estado));

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        if (result && (estadoAnt == EstadosCodArt.Rechazado || aprobada))
        {
            EnviarCodificacionArticulo(idCodificacion, "");
        }
    }
    /// <summary>
    /// Rechaza una codificación de artículo.
    /// </summary>
    public static void RechazarCodificacionArticulo(int idCodificacion, string motivo)
    {
        bool result;
        IDbConnection conn = null;
        IDbCommand cmd;

        if (motivo.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_CodificacionArticulos SET idEstado = @idEstado ";
            cmd.CommandText += "WHERE idCodificacion = @idCodificacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idCodificacion", idCodificacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosCodArt.Rechazado));

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        if (result)
        {
            EnviarCodificacionArticulo(idCodificacion, motivo);
        }
    }
    /// <summary>
    /// Marca como No corresponde una codificación de artículo.
    /// </summary>
    public static void NoCorrespondeCodificacionArticulo(int idCodificacion, string motivo)
    {
        bool result;
        IDbConnection conn = null;
        IDbCommand cmd;

        if (motivo.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_CodificacionArticulos SET idEstado = @idEstado ";
            cmd.CommandText += "WHERE idCodificacion = @idCodificacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idCodificacion", idCodificacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosCodArt.NoCorresponde));

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        if (result)
        {
            EnviarCodificacionArticulo(idCodificacion, motivo);
        }
    }
    /// <summary>
    /// Envía una codificación de artículo.
    /// </summary>
    private static void EnviarCodificacionArticulo(int idCodificacion, string comentarios)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_CODIF_ART);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        CodificacionArticulo ca = GetCodificacionArticulo(idCodificacion);
        if (ca == null)
        {
            throw new EmailException();
        }

        if (ca.Solicito == null)
        {
            throw new EmailException();
        }

        string encabezado = "";
        string asunto = "Solicitud de alta de artículo Nº" + ca.GetNumeroReferencia() + " ";
        string tipoMensaje = "info";
        string de;
        string para;
        string cc = "";
        switch (ca.Estado)
        {
            case EstadosCodArt.Revision:
                encabezado = "El usuario " + ca.Solicito.Nombre + " solició el alta de un artículo.";
                asunto += "[REVISION]";
                de = cc = ca.Solicito.Email;
                para = Constantes.EmailResponsableCodifArt;
                break;
            case EstadosCodArt.Rechazado:
                encabezado = "La solicitud de alta de artículo ha sido rechazada por el siguiente motivo: " + comentarios;
                encabezado += "<br />Ingrese nuevamente a la solicitud para corregir los datos.";
                asunto += "[RECHAZADA]";
                tipoMensaje = "error";
                de = Constantes.EmailResponsableCodifArt;
                para = ca.Solicito.Email;
                break;
            case EstadosCodArt.NoCorresponde:
                encabezado = "La solicitud de alta de artículo ha sido marcada como \"No corresponde\" por el siguiente motivo: " + 
                    comentarios;
                encabezado += "<br />";
                asunto += "[NO CORRESPONDE]";
                tipoMensaje = "error";
                de = Constantes.EmailResponsableCodifArt;
                para = ca.Solicito.Email;
                break;
            case EstadosCodArt.Aprobado:
                encabezado = "La solicitud de alta de artículo fue aprobada.";
                asunto += "[APROBADA]";
                tipoMensaje = "success";
                de = Constantes.EmailResponsableCodifArt;
                para = ca.Solicito.Email;
                break;
            default:
                throw new EmailException();
        }

        strPlantilla = strPlantilla.Replace("@ENCABEZADO", encabezado);
        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", tipoMensaje);
        strPlantilla = strPlantilla.Replace("@DESCRIPCION_ARTICULO", ca.DescripcionArticulo);
        strPlantilla = strPlantilla.Replace("@UNIDAD_MEDIDA", ca.UnidadMedida);
        strPlantilla = strPlantilla.Replace("@CODIGO_ARTICULO", ca.CodigoArticulo);
        strPlantilla = strPlantilla.Replace("@DESCRIPCION_USO", ca.DescripcionUso);
        strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/stock/altaArticuloAdmin.aspx", "id=" + idCodificacion));

        Email email = new Email(de, para, cc, asunto, strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Comprueba si el código de artículo es válido.
    /// </summary>
    private static bool EsCodigoArticuloValido(string codigo)
    {
        bool result = true;

        byte aux;
        for (int i = 0; i < codigo.Length; i++)
        {
            if (!Byte.TryParse(codigo[i].ToString(), out aux))
            {
                result = false;
                break;
            }
        }

        result &= codigo.Trim().Length == 15;

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de estado para las solicitudes.
    /// </summary>
    public static Dictionary<int, string> GetEstadosSolicitudes()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosCodArt.Rechazado, "Rechazado");
        result.Add((int)EstadosCodArt.Revision, "Revision");
        result.Add((int)EstadosCodArt.Aprobado, "Aprobado");
        result.Add((int)EstadosCodArt.NoCorresponde, "No corresponde");

        return result;
    }
}