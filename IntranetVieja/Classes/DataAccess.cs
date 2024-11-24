﻿/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

// Delegados.
public delegate string GetConsultaFiltroDelegate(List<Filtro> filtros, bool cantidad);

public delegate string GetConsultaFiltroDelegateSoloFiltro(List<Filtro> filtros);

public delegate T ProcessObjectDelegate<T>(DataRow dr);

public class DataSourceItem
{
    // Variables.
    private string textField;
    private object valueField;

    // Properties.
    public string TextField
    {
        get { return textField; }
    }
    public object ValueField
    {
        get { return valueField; }
    }


    public DataSourceItem(string textField, object valueField)
    {
        this.textField = textField;
        this.valueField = valueField;
    }
}

/// <summary>
/// Summary description for DataAccess
/// </summary>
internal static class DataAccess
{
    // Constantes.
    private const string ConnectionStringIntranet = "server=10.0.0.15;database=IntranetServaind;uid=sa;pwd=orodis8siciliana$";
    private const string ConnectionStringTango = "server=server-storage1\\axsqlexpress;database=Servaind_SA;"
                                               + "uid=sa;pwd=Axoft1988";
    private const string ConnectionStringProser = "server=localhost;uid=sa;pwd=orodis8siciliana$;database=Proser";


    /// <summary>
    /// Gets an open connection to the db.
    /// </summary>
    internal static IDbConnection GetConnection(BDConexiones conexion)
    {
        IDbConnection result;

        try
        {
            string connstring = "";
            switch (conexion)
            {
                case BDConexiones.Intranet:
                    connstring = ConnectionStringIntranet;
                    break;
                case BDConexiones.Proser:
                    connstring = ConnectionStringProser;
                    break;
                case BDConexiones.Tango:
                    connstring = ConnectionStringTango;
                    break;
            }

            result = new SqlConnection(connstring);
            result.Open();
        }
        catch
        {
            throw new Exception("No se puede establecer la conexión a la base de datos.");
        }

        return result;
    }
    /// <summary>
    /// Get a transaction to the db.
    /// </summary>
    internal static IDbTransaction GetTransaction(IDbConnection conn)
    {
        IDbTransaction result;

        try
        {
            result = conn.BeginTransaction();
        }
        catch
        {
            throw new Exception("No se puede crear la transacción.");
        }

        return result;
    }
    /// <summary>
    /// Get a command to the db.
    /// </summary>
    internal static IDbCommand GetCommand(IDbConnection conn)
    {
        return GetCommand(conn, null);
    }
    /// <summary>
    /// Get a command to the db.
    /// </summary>
    internal static IDbCommand GetCommand(IDbTransaction trans)
    {
        if (trans != null)
        {
            return GetCommand(trans.Connection, trans);
        }

        throw new Exception("No se puede crear el comando.");
    }
    /// <summary>
    /// Get a command to the db.
    /// </summary>
    internal static IDbCommand GetCommand(IDbConnection conn, IDbTransaction trans)
    {
        IDbCommand result;

        try
        {
            result = new SqlCommand("", (SqlConnection)conn);
            if (trans != null)
            {
                result.Transaction = trans;
            }
        }
        catch
        {
            throw new Exception("No se puede crear el comando.");
        }

        return result;
    }
    /// <summary>
    /// Get a data parameter.
    /// </summary>
    internal static IDbDataParameter GetDataParameter(string name, object value)
    {
        IDbDataParameter result;

        try
        {
            result = new SqlParameter(name, value);
        }
        catch
        {
            throw new Exception("No se puede crear el DataParameter.");
        }

        return result;
    }
    /// <summary>
    /// Get a data adapter.
    /// </summary>
    internal static IDbDataAdapter GetDataAdapter(IDbCommand cmd)
    {
        IDbDataAdapter result;

        try
        {
            result = new SqlDataAdapter((SqlCommand)cmd);
        }
        catch
        {
            throw new Exception("No se puede crear el Data Adapter.");
        }

        return result;
    }
    /// <summary>
    /// Get the connection type.
    /// </summary>
    private static BDConexiones GetConnectionType(string connectionString)
    {
        if (connectionString.Equals(ConnectionStringIntranet)) return BDConexiones.Intranet;
        else if (connectionString.Equals(ConnectionStringProser)) return BDConexiones.Proser;
        else if (connectionString.Equals(ConnectionStringTango)) return BDConexiones.Tango;
        else return BDConexiones.Intranet;
    }
    /// <summary>
    /// Obtiene una lista de objetos.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    /// <param name="filtros">Filtros para la consulta.</param>
    /// <param name="maxRegistrosPorPagina">Cantidad máxima de registros por página.</param>
    /// <param name="GetConsultaFiltro">Método que retorna la consulta.</param>
    /// <param name="GetObjeto">Método que recibe un DataRow y devuelve un objeto.</param>
    internal static List<T> GetDataList<T>(BDConexiones conexion, int pagina, List<Filtro> filtros, int maxRegistrosPorPagina,
        GetConsultaFiltroDelegate GetConsultaFiltro, ProcessObjectDelegate<T> GetObjeto)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbDataAdapter adap;
        DataSet ds = new DataSet();
        List<T> result = new List<T>();

        pagina = pagina - 1;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = GetConsultaFiltro(filtros, false);
            adap = DataAccess.GetDataAdapter(cmd);
            ((System.Data.Common.DbDataAdapter)adap).Fill(ds, pagina * maxRegistrosPorPagina, maxRegistrosPorPagina, "List");

            if (ds.Tables["List"].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables["List"].Rows)
                {
                    T obj = GetObjeto(dr);

                    result.Add(obj);
                }
            }
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
    /// Obtiene una lista de objetos.
    /// </summary>
    /// <param name="filtros">Filtros para la consulta.</param>
    /// <param name="GetConsultaFiltro">Método que retorna la consulta.</param>
    /// <param name="GetObjeto">Método que recibe un DataRow y devuelve un objeto.</param>
    internal static List<T> GetDataList<T>(BDConexiones conexion, List<Filtro> filtros,
        GetConsultaFiltroDelegateSoloFiltro GetConsultaFiltro, ProcessObjectDelegate<T> GetObjeto)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbDataAdapter adap;
        DataSet ds = new DataSet();
        List<T> result = new List<T>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = GetConsultaFiltro(filtros);
            adap = DataAccess.GetDataAdapter(cmd);
			
            ((System.Data.Common.DbDataAdapter)adap).Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    T obj = GetObjeto(dr);

                    result.Add(obj);
                }
            }
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
    /// Obtiene la cantidad de páginas de una consulta.
    /// </summary>
    public static int GetCantidadPaginasData(List<Filtro> filtros, int maxRegistrosPorPagina, 
        GetConsultaFiltroDelegate GetConsultaFiltro)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        int cantidadPaginas;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = GetConsultaFiltro(filtros, true);

            int cantidadRegistros = Convert.ToInt32(cmd.ExecuteScalar());
            cantidadPaginas = cantidadRegistros / maxRegistrosPorPagina;
            if (cantidadRegistros % maxRegistrosPorPagina > 0)
            {
                cantidadPaginas++;
            }
        }
        catch
        {
            cantidadPaginas = 0;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return cantidadPaginas;
    }
}