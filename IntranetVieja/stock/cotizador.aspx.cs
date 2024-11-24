using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_cotizador : System.Web.UI.Page
{
    // Properties.
    public bool PuedeProductos
    {
        get { return GPermisosPersonal.TieneAcceso(PermisosPersona.CotizadorOnLineEquipos); }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (PuedeProductos)
        {
            cbBuscarProducto.DataSource = GProductos.GetProductos();
            cbBuscarProducto.DataTextField = "Descripcion";
            cbBuscarProducto.DataValueField = "ID";
            cbBuscarProducto.DataBind();
        }
    }
    /// <summary>
    /// Busca los artículos que coincidan.
    /// </summary>
    [WebMethod()]
    public static object[][] GetArticulos(string descripcion)
    {
        List<object[]> result = new List<object[]>();

        List<string[]> articulos = GArticuloTango.BuscarArticulosTango(descripcion);
        foreach (string[] articulo in articulos)
        {
            result.Add(articulo);
        }

        return result.ToArray();
    }
    /// <summary>
    /// Busca el artículo.
    /// </summary>
    [WebMethod()]
    public static object[] GetArticulo(string codigo)
    {
        object[] result = null;

        ArticuloTango articulo = GArticuloTango.GetArticuloTango(codigo);
        if (articulo != null)
        {
            articulo.CargarDetalleVenta();

            if (articulo.DetalleVenta == null)
            {
                result = new object[] { codigo, articulo.Descripcion, "-", "0.000", "0.000", "-", "-No disponible-" };
            }
            else
            {
                result = new object[] { codigo, articulo.Descripcion, articulo.DetalleVenta.OC, 
                    articulo.DetalleVenta.Precio.ToString("0.000"), articulo.DetalleVenta.PrecioDescuento.ToString("0.000"), 
                    articulo.DetalleVenta.Fecha.ToShortDateString(), articulo.DetalleVenta.Proveedor };
            }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los árticulos del producto.
    /// </summary>
    [WebMethod()]
    public static object[][] GetArticulosProducto(int idProducto)
    {
        List<object[]> result = new List<object[]>();

        Producto producto = GProductos.GetProducto(idProducto);
        if (producto == null)
        {
            throw new Exception("El producto solicitado no existe.");
        }

        producto.CargarItems();
        foreach (ItemCotizacion item in producto.Items)
        {
            ArticuloTango articulo = item.Articulo;

            if (articulo != null)
            {
                articulo.CargarDetalleVenta();

                if (articulo.DetalleVenta == null)
                {
                    result.Add(new object[] { articulo.Codigo, articulo.Descripcion, item.Cantidad, "-", "0.000", "0.000", "-", "-No disponible-" });
                }
                else
                {
                    result.Add(new object[] { articulo.Codigo, articulo.Descripcion, item.Cantidad, articulo.DetalleVenta.OC, 
                    articulo.DetalleVenta.Precio.ToString("0.000"), articulo.DetalleVenta.PrecioDescuento.ToString("0.000"), 
                    articulo.DetalleVenta.Fecha.ToShortDateString(), articulo.DetalleVenta.Proveedor });
                }
            }
        }
        
        return result.ToArray();
    }
    /// <summary>
    /// Exporta la cotización a formato Excel 97/2003.
    /// </summary>
    [WebMethod()]
    public static string ExportarCotizacion(object[][] items)
    {
        string result;

        Cotizacion cotizacion = new Cotizacion();

        try
        {
            foreach (object[] item in items)
            {
                cotizacion.AgregarItem(item[0].ToString(), Convert.ToSingle(Funciones.GetDecimalNumber(item[1].ToString())),
                     Convert.ToSingle(Funciones.GetDecimalNumber(item[2].ToString())), 
                     Convert.ToSingle(Funciones.GetDecimalNumber(item[3].ToString())));
            }

            string path = GCotizaciones.ExportarAExcel(cotizacion);
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + path + "&n=Cotizacion.xlsx&d=1");
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }


        return result;
    }
}