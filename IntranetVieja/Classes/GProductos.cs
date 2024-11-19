/*
 * Historial:
 * ===================================================================================
 * [26/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;


public class Producto : IComparable
{
    // Variables.
    private int idProducto;
    private string descripcion;
    private bool activo;
    private List<ItemCotizacion> lstItems;

    // Propiedades.
    public int ID
    {
        get { return idProducto; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public bool Activo
    {
        get { return activo; }
    }
    public List<ItemCotizacion> Items
    {
        get { return lstItems; }
    }


    internal Producto(int idProducto, string descripcion, bool activo, List<ItemCotizacion> lstItems)
    {
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.activo = activo;
        this.lstItems = lstItems;
    }
    internal Producto(int idProducto, string descripcion, bool activo) 
        : this(idProducto, descripcion, activo, new List<ItemCotizacion>())
    {

    }

    public int CompareTo(object obj)
    {
        Producto p = (Producto)obj;

        return this.descripcion.CompareTo(p.Descripcion);
    }
    /// <summary>
    /// Carga los items del producto.
    /// </summary>
    public void CargarItems()
    {
        this.lstItems = GProductos.GetItemsProducto(this.idProducto);
    }
}

/// <summary>
/// Summary description for GProductos
/// </summary>
public class GProductos
{
    /// <summary>
    /// Obtiene un producto.
    /// </summary>
    private static Producto GetProducto(IDataReader dr)
    {
        Producto result;

        try
        {
            result = new Producto(
                Convert.ToInt32(dr["idProducto"]),
                dr["Descripcion"].ToString(),
                Convert.ToInt32(dr["Activo"]) == 1
            );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los productos disponibles.
    /// </summary>
    public static List<Producto> GetProductos()
    {
        List<Producto> result = new List<Producto>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Productos ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(GetProducto(dr));
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
    /// Obtiene un producto.
    /// </summary>
    public static Producto GetProducto(int idProducto)
    {
        Producto result;
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Productos WHERE idProducto = @idProducto";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idProducto", idProducto));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new ElementoInexistenteException();
            }

            result = GetProducto(dr);

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
    /// Obtiene un ítem de un producto.
    /// </summary>
    private static ItemCotizacion GetItemProducto(IDataReader dr)
    {
        ItemCotizacion result;

        try
        {
            result = new ItemCotizacion(
                GArticuloTango.GetArticuloTango(dr["codigoArticulo"].ToString()),
                Convert.ToSingle(dr["Cantidad"]), 0, 0
            );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los productos disponibles.
    /// </summary>
    internal static List<ItemCotizacion> GetItemsProducto(int idProducto)
    {
        List<ItemCotizacion> result = new List<ItemCotizacion>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT codigoArticulo, Cantidad FROM tbl_ProductosItems WHERE idProducto = @idProducto";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idProducto", idProducto));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(GetItemProducto(dr));
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
}