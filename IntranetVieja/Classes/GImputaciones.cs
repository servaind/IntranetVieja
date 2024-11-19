/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de Imputacion
/// </summary>
public class Imputacion : IComparable
{
    // Variables.
    private int idImputacion;
    private int numero;
    private string descripcion;
    private bool activa;

    // Propiedades.
    /// <summary>
    /// Obtiene el ID.
    /// </summary>
    public int ID
    {
        get { return idImputacion; }
    }
    /// <summary>
    /// Obtiene el Número de la Imputación.
    /// </summary>
    public int Numero
    {
        get { return numero; }
    }
    /// <summary>
    /// Obtiene sólo la descripción de la Imputación.
    /// </summary>
    public string Descripcion
    {
        get { return descripcion; }
    }
    /// <summary>
    /// Obtiene la Descripción de la Imputación. (Número - Descripción).
    /// </summary>
    public string DescripcionCompleta
    {
        get { return Numero + " - " + descripcion; }
    }
    /// <summary>
    /// Obtiene si está activa.
    /// </summary>
    public bool Activa
    {
        get { return activa; }
    }


    internal Imputacion(int idImputacion, int numero, string descripcion, bool activa)
    {
        this.idImputacion = idImputacion;
        this.numero = numero;
        this.descripcion = descripcion;
        this.activa = activa;
    }

    public int CompareTo(object obj)
    {
        Imputacion i = (Imputacion)obj;

        return this.Numero.CompareTo(i.Numero);
    }
}

/// <summary>
/// Descripción breve de GImputaciones
/// </summary>
public static class GImputaciones
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;


    /// <summary>
    /// Obtiene una imputación.
    /// </summary>
    private static Imputacion GetImputacion(DataRow dr)
    {
        Imputacion result;

        try
        {
            result = new Imputacion(Convert.ToInt32(dr["idImputacion"]), Convert.ToInt32(dr["Numero"]), 
                dr["Descripcion"].ToString(), Convert.ToInt32(dr["Activa"]) == 1);
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una imputación.
    /// </summary>
    private static Imputacion GetImputacion(IDataReader dr)
    {
        Imputacion result;

        try
        {
            result = new Imputacion(Convert.ToInt32(dr["idImputacion"]), Convert.ToInt32(dr["Numero"]), 
                dr["Descripcion"].ToString(), Convert.ToInt32(dr["Activa"]) == 1);
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Carga una Imputación en base al ID.
    /// </summary>
    public static Imputacion GetImputacion(int idImputacion)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        Imputacion result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idImputacion, Numero, RTRIM(Descripcion) AS Descripcion, Activa FROM ";
            cmd.CommandText += "tbl_Imputaciones WHERE idImputacion = @idImputacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacion", idImputacion));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new ElementoInexistenteException();
            }

            result = GetImputacion(dr);

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
    /// Obtiene una Lista con Todas las Imputaciones disponibles.
    /// </summary>
    public static List<Imputacion> GetImputaciones()
    {
        return GetImputaciones(true);
    }
    /// <summary>
    /// Obtiene una Lista con Todas las Imputaciones activas disponibles.
    /// </summary>
    public static List<Imputacion> GetImputacionesActivas()
    {
        return GetImputaciones(false);
    }
    /// <summary>
    /// Obtiene una Lista con Todas las Imputaciones disponibles.
    /// </summary>
    private static List<Imputacion> GetImputaciones(bool cargarTodas)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<Imputacion> result = new List<Imputacion>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idImputacion, Numero, RTRIM(Descripcion) AS Descripcion, Activa ";
            cmd.CommandText += "FROM tbl_Imputaciones ";
            if (!cargarTodas)
            {
                cmd.CommandText += "WHERE Activa = 1 ";
            }
            cmd.CommandText += "ORDER BY Numero";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Imputacion imputacion = GetImputacion(dr);

                if (imputacion != null)
                {
                    result.Add(imputacion);
                }
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

        result.Sort();

        return result;
    }
    /// <summary>
    /// Crea una nueva Imputación.
    /// </summary>
    public static void NuevaImputacion(int numero, string descripcion)
    {
        NuevaImputacion(numero, descripcion, true);
    }
    /// <summary>
    /// Crea una nueva Imputación.
    /// </summary>
    public static void NuevaImputacion(int numero, string descripcion, bool activa)
    {
        if (numero < 0 || descripcion.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        if (ExisteImputacion(numero))
        {
            throw new ElementoExistenteException();
        }

        IDbConnection conn = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_Imputaciones (Numero,Descripcion,Activa) VALUES ";
            cmd.CommandText += "(@Numero,@Descripcion,@Activa)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));

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
    }
    /// <summary>
    /// Obtiene si existe una Imputación.
    /// </summary>
    public static bool ExisteImputacion(int numero)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        bool result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Count(idImputacion) FROM tbl_Imputaciones WHERE Numero = @Numero";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));

            result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
        catch
        {
            result = false;
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
    /// Da de baja una Imputación.
    /// </summary>
    public static void ActualizarImputacion(int idImputacion, int numero, string descripcion, bool activa)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        if (numero < 0 || descripcion.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_Imputaciones SET Numero = @Numero, Descripcion = @Descripcion, Activa = @Activa WHERE ";
            cmd.CommandText += "idImputacion = @idImputacion";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacion", idImputacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));

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
    }
    /// <summary>
    /// Obtiene las imputaciones que coincidan con el filtro.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<Imputacion> GetImputaciones(int pagina, List<Filtro> filtros)
    {
        List<Imputacion> result;

        result = DataAccess.GetDataList<Imputacion>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetImputacion);

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetCantidadPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
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
                case (int)FiltrosImputacion.Numero:
                    filtroWhere += "AND Numero = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosImputacion.Descripcion:
                    filtroWhere += "AND Descripcion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosImputacion.Estado:
                    filtroWhere += "AND Activa = " + filtro.Valor + " ";
                    break;
                default:
                    filtroWhere += "";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT COUNT(idImputacion) as TotalRegistros";
        }
        else
        {
            consulta = "SELECT *";
        }

        if (filtroWhere.Length > 0)
        {
            filtroWhere = "WHERE " + filtroWhere;
        }
        consulta += " FROM tbl_Imputaciones " + filtroJoin + " " + filtroWhere;

        if (!cantidad)
        {
            consulta += " ORDER BY Numero";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene los tipos de estado para las imputaciones.
    /// </summary>
    public static Dictionary<int, string> GetEstadosImputacion()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosImputacion.Activa, "Activa");
        result.Add((int)EstadosImputacion.Inactiva, "Inactiva");

        return result;
    }
}
