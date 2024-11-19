/*
 * Historial:
 * ===================================================================================
 * [28/06/2011]
 * - Agregado método para verificar si existe un código de artículo.
 * [23/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Clase para obtener datos de la última venta de un artículo.
/// </summary>
public class DetalleVentaArtTango
{
    // Constantes.
    private const int CEROS = 12;

    // Variables.
    private int oc;
    private float precio;
    private float precioDescuento;
    private float cotizacion;
    private DateTime fecha;
    private string moneda;
    private string proveedor;

    // Propiedades.
    /// <summary>
    /// Obtiene el Nº de Orden de Compra.
    /// </summary>
    public int OC
    {
        get
        {
            return oc;
        }
    }
    /// <summary>
    /// Obtiene el nombre del Proveedor.
    /// </summary>
    public string Proveedor
    {
        get
        {
            return proveedor;
        }
    }
    /// <summary>
    /// Obtiene el precio del Artículo en U$S.
    /// </summary>
    public float Precio
    {
        get
        {
            return precio / (moneda == "$" ? cotizacion : 1);
        }
    }
    /// <summary>
    /// Obtiene el precio con descuento del Artículo en U$S.
    /// </summary>
    public float PrecioDescuento
    {
        get
        {
            return precioDescuento / (moneda == "$" ? cotizacion : 1);
        }
    }
    /// <summary>
    /// Obtiene la Moneda.
    /// </summary>
    public string Moneda
    {
        get
        {
            return moneda;
        }
    }
    /// <summary>
    /// Obtiene la cotización de la venta.
    /// </summary>
    public float Cotizacion
    {
        get
        {
            return cotizacion;
        }
    }
    /// <summary>
    /// Obtiene la fecha de la venta.
    /// </summary>
    public DateTime Fecha
    {
        get
        {
            return fecha;
        }
    }


    internal DetalleVentaArtTango(int oc, float precio, float precioDescuento, float cotizacion, DateTime fecha, 
        string moneda, string proveedor)
    {
        this.oc = oc;
        this.precio = precio;
        this.precioDescuento = precioDescuento;
        this.cotizacion = cotizacion;
        this.fecha = fecha;
        this.moneda = moneda;
        this.proveedor = proveedor;
    }
}

/// <summary>
/// Descripción breve de ItemTango
/// </summary>
public class ArticuloTango
{
    // Variables.
    private string codigo;
    private string descripcion;
    private string descripcionAdicional;
    private string un;
    private DetalleVentaArtTango detalleVenta;

    // Propiedades.
    /// <summary>
    /// Obtiene el Código del artículo.
    /// </summary>
    public string Codigo
    {
        get
        {
            return codigo;
        }
    }
    /// <summary>
    /// Obtiene la Descripción del artículo.
    /// </summary>
    public string Descripcion
    {
        get
        {
            string adicional = "";
            if (descripcion != null && descripcionAdicional.Trim().Length > 0)
            {
                adicional = " • " + descripcionAdicional;
            }
            return descripcion + adicional;
        }
    }
    /// <summary>
    /// Obtiene la Un.
    /// </summary>
    public string Un
    {
        get
        {
            return un;
        }
    }
    /// <summary>
    /// Obtiene los datos de la venta.
    /// </summary>
    public DetalleVentaArtTango DetalleVenta
    {
        get
        {
            return detalleVenta;
        }
    }


    internal ArticuloTango(string codigo, string descripcion, string descripcionAdicional,
        string un) : this(codigo, descripcion, descripcionAdicional, un, null)
    {

    }
    internal ArticuloTango(string codigo, string descripcion, string descripcionAdicional, 
        string un, DetalleVentaArtTango detalleVenta)
    {
        this.codigo = codigo;
        this.descripcion = descripcion;
        this.descripcionAdicional = descripcionAdicional;
        this.un = un;
        this.detalleVenta = detalleVenta;
    }
    /// <summary>
    /// Carga los detalles de la venta.
    /// </summary>
    /// <returns></returns>
    public bool CargarDetalleVenta()
    {
        this.detalleVenta = GArticuloTango.GetDetalleVenta(this.codigo);

        return this.detalleVenta != null;
    }
}

/// <summary>
/// Descripción breve de GArticuloTango
/// </summary>
public static class GArticuloTango
{
    /// <summary>
    /// Carga los datos de la venta.
    /// </summary>
    internal static DetalleVentaArtTango GetDetalleVenta(string codigoArticulo)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
        }
        catch
        {
            if (conn != null)
            {
                conn.Close();
            }
            return null;
        }

        DetalleVentaArtTango resultado = GetDetalleVenta(codigoArticulo, conn);

        conn.Close();

        return resultado;
    }
    /// <summary>
    /// Carga los datos de la venta.
    /// </summary>
    internal static DetalleVentaArtTango GetDetalleVenta(string codigoArticulo, IDbConnection conn)
    {
        IDbCommand cmd;
        IDataReader dr;
        DetalleVentaArtTango detalle;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 CPA36.N_ORDEN_CO, CPA01.COD_PROVEE, CPA01.NOM_PROVEE, CPA35.COTIZ, ";
            cmd.CommandText += "CPA36.PRECIO, CPA36.PRECIO_PAN, CPA01.COD_PROVEE, ";
            cmd.CommandText += "CASE CPA35.MON_CTE WHEN 'true' THEN '$' ELSE 'U$S' END AS MONEDA, ";
            cmd.CommandText += "CPA35.FEC_GENER ";
            cmd.CommandText += "FROM CPA36 ";
            cmd.CommandText += "LEFT JOIN CPA35 ON CPA36.N_ORDEN_CO = CPA35.N_ORDEN_CO ";
            cmd.CommandText += "LEFT JOIN CPA01 ON CPA35.COD_PROVEE = CPA01.COD_PROVEE ";
            cmd.CommandText += "WHERE CPA36.COD_ARTICU = @codigo ";
            cmd.CommandText += "AND CPA01.COD_PROVEE <> '999999' AND CPA01.COD_PROVEE <> '999' ";
            cmd.CommandText += "AND CPA01.COD_PROVEE <> '9999' AND CPA01.COD_PROVEE <> '0000' ";
            cmd.CommandText += "ORDER BY CPA35.FEC_GENER DESC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@codigo", codigoArticulo));

            dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                throw new Exception("No hay ventas existentes.");
            }
            detalle = new DetalleVentaArtTango(
                Convert.ToInt32(dr["N_ORDEN_CO"]),
                Convert.ToSingle(dr["PRECIO_PAN"]),
                Convert.ToSingle(dr["PRECIO"]),
                Convert.ToSingle(dr["COTIZ"]),
                DateTime.Parse(dr["FEC_GENER"].ToString()),
                dr["MONEDA"].ToString(),
                dr["NOM_PROVEE"].ToString()
            );
        }
        catch
        {
            detalle = null;
        }

        return detalle;
    }
    /// <summary>
    /// Obtiene el código del Ítem a partir de la Descripción.
    /// </summary>
    public static string GetCodigoArticuloTango(string descripcion)
    {
        string[] desc = descripcion.Split(Constantes.SepDescArtTango);
        string result = "";

        try
        {
            result = GetCodigoArticuloTango(desc[0].Trim(), desc.Length > 1 ? desc[1].Trim() : "");
        }
        catch
        {
            result = "";
        }

        return result;
    }
    /// <summary>
    /// Obtiene el código del Ítem a partir de la Descripción.
    /// </summary>
    public static string GetCodigoArticuloTango(string descripcion, string descripcionAdicional)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
        }
        catch
        {
            if (conn != null)
            {
                conn.Close();
            }
            return Constantes.ValorInvalido.ToString();
        }

        string codigo = GetCodigoArticuloTango(descripcion, descripcionAdicional, conn);

        conn.Close();

        return codigo;
    }
    /// <summary>
    /// Obtiene el código del Ítem a partir de la Descripción.
    /// </summary>
    private static string GetCodigoArticuloTango(string descripcion, string descripcionAdicional, 
        IDbConnection conn)
    {
        string codigo;

        IDbCommand cmd;
        IDataReader dr;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 ";
            cmd.CommandText += "RTRIM(COD_ARTICU) AS COD_ARTICU ";
            cmd.CommandText += "FROM STA11 ";
            cmd.CommandText += "WHERE UPPER(RTRIM(LTRIM(DESCRIPCIO))) = @DESCRIPCIO ";
            if (descripcionAdicional.Trim().Length > 0)
            {
                cmd.CommandText += "AND UPPER(RTRIM(LTRIM(DESC_ADIC))) = @DESC_ADIC";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@DESC_ADIC", descripcionAdicional.Trim().ToUpper()));
            }
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DESCRIPCIO", descripcion.Trim().ToUpper()));

            dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                throw new Exception("El artículo no existe.");
            }

            codigo = dr["COD_ARTICU"].ToString();

            dr.Close();
        }
        catch
        {
            codigo = Constantes.ValorInvalido.ToString();
        }

        return codigo;
    }
    /// <summary>
    /// Carga los datos de un Artículo.
    /// </summary>
    public static ArticuloTango GetArticuloTango(string codigo)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
        }
        catch
        {
            if (conn != null)
            {
                conn.Close();
            }
            return null;
        }

        ArticuloTango result = GetArticuloTango(codigo, conn);

        conn.Close();

        return result;
    }
    /// <summary>
    /// Carga los datos de un Artículo.
    /// </summary>
    public static ArticuloTango GetArticuloTango(string descripcion, string descripcionAdicional)
    {
        IDbConnection conn = null;
        
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
        }
        catch
        {
            if (conn != null)
            {
                conn.Close();
            }
            return null;
        }

        string codigoArticulo = GetCodigoArticuloTango(descripcion, descripcionAdicional);
        ArticuloTango result = GetArticuloTango(codigoArticulo, conn);

        conn.Close();

        return result;
    }
    /// <summary>
    /// Carga los datos del Artículo.
    /// </summary>
    private static ArticuloTango GetArticuloTango(string codigo, IDbConnection conn)
    {
        IDbCommand cmd;
        IDataReader dr;
        ArticuloTango resultado;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 ";
            cmd.CommandText += "art.DESCRIPCIO, ";
            cmd.CommandText += "art.DESC_ADIC, ";
            cmd.CommandText += "med.COD_MEDIDA ";
            cmd.CommandText += "FROM ";
            cmd.CommandText += "STA11 art WITH (NOLOCK) ";
            cmd.CommandText += "INNER JOIN MEDIDA med ON art.ID_MEDIDA_STOCK = med.ID_MEDIDA ";
            cmd.CommandText += "WHERE art.COD_ARTICU = @codigo";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@codigo", codigo));

            dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                dr.Close();
                throw new Exception("El artículo no existe.");
            }

            resultado = new ArticuloTango(
                codigo,
                dr["DESCRIPCIO"].ToString().Trim(),
                dr["DESC_ADIC"].ToString().Trim(),
                dr["COD_MEDIDA"].ToString().Trim()
            );

            dr.Close();
        }
        catch
        {
            resultado = null;
        }

        return resultado;
    }
    /// <summary>
    /// Obtiene una lista de artículos.
    /// </summary>
    public static List<ArticuloTango> GetArticulosTango(List<string> codigosArticulos)
    {
        List<ArticuloTango> result = new List<ArticuloTango>();
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);

            foreach (string codigo in codigosArticulos)
            {
                ArticuloTango at = GetArticuloTango(codigo, conn);
                if (at != null)
                {
                    result.Add(at);
                }
            }
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene si el código de artículo existe en el sistema.
    /// </summary>
    public static bool ExisteCodigoArticulo(string codigo)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        bool result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT COUNT(COD_ARTICU) FROM STA11 WITH (NOLOCK) WHERE COD_ARTICU = @Codigo";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Codigo", codigo));

            result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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

        return result;
    }
    /// <summary>
    /// Busca todos los artículos que coincidan con la descripción.
    /// </summary>
    public static List<string[]> BuscarArticulosTango(string descripcion)
    {
        List<string[]> result = new List<string[]>();

        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            string adic = null;
            conn = DataAccess.GetConnection(BDConexiones.Tango);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT ";
            cmd.CommandText += "COD_ARTICU, DESCRIPCIO, DESC_ADIC ";
            cmd.CommandText += "FROM STA11 ";
            cmd.CommandText += "WHERE UPPER(DESCRIPCIO) like '%" + descripcion.ToUpper() + "%' ";
            cmd.CommandText += "OR UPPER(DESC_ADIC) like '%" + descripcion.ToUpper() +
                "%' ORDER BY DESCRIPCIO";

            dr = cmd.ExecuteReader();
            
            while (dr.Read())
            {
                adic = "";
                if (dr["DESC_ADIC"].ToString().Trim().Length > 0)
                {
                    adic = " " + Constantes.SepDescArtTango + " " + dr["DESC_ADIC"].ToString().Trim();
                }
                result.Add(new string[] { dr["COD_ARTICU"].ToString(), dr["DESCRIPCIO"].ToString().Trim() + adic });
            }

            dr.Close();
        }
        catch
        {
            result.Clear();
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
