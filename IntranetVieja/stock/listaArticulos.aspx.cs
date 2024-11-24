using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_listaArticulos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<int, string> equipos = ModuloStock.GetEquipos();
        Dictionary<string, string> cEquipos = new Dictionary<string, string>();

        foreach (int idEquipo in equipos.Keys)
        {
            cEquipos.Add(Encriptacion.GetParametroEncriptado("id=" + idEquipo), equipos[idEquipo]);
        }

        cbEquipoDesc.DataSource = cEquipos;
        cbEquipoDesc.DataTextField = "Value";
        cbEquipoDesc.DataValueField = "Key";
        cbEquipoDesc.DataBind();
    }
    /// <summary>
    /// Obtiene los artículos.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetArticulos(int pagina)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        List<ArticuloStock> articulos = ModuloStock.GetArticulos(pagina, filtros);

        articulos.ForEach(a => result.Add(new object[] { Encriptacion.GetParametroEncriptado("id=" + a.ID), a.Codigo, a.Descripcion, 
            a.Cantidad.ToString("0.00"), a.PuntoPedido, "-", a.EsEquipo ? 1 : 0 }));

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas de artículos.
    /// </summary>
    [WebMethod()]
    public static int GetArticulosPaginas()
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        result = ModuloStock.GetArticulosPaginas(filtros);
        if (result == 0) result = 1;

        return result;
    }
    /// <summary>
    /// Realiza un ingreso de stock.
    /// </summary>
    [WebMethod()]
    public static void IngresoStock(string idArticulo, float cantidad, string descripcion)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idArticulo)["id"]);

            ModuloStock.IngresoArticulo(id, cantidad, descripcion);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Realiza un ingreso de stock.
    /// </summary>
    [WebMethod()]
    public static void EgresoStock(string idArticulo, float cantidad, string descripcion)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idArticulo)["id"]);

            ModuloStock.EgresoArticulo(id, cantidad, descripcion);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. Verifique que la cantidad "
                + "ingresada sea menor a la disponible.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Agrega un artículo.
    /// </summary>
    [WebMethod()]
    public static void AgregarArticulo(string codigo, int puntoPedido)
    {
        try
        {
            ModuloStock.AddArticulo(codigo, puntoPedido);
        }
        catch (ElementoInexistenteException)
        {
            throw new Exception("El código de artículo ingresado no existe.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. Verifique que el código ingresado no se encuentre en el sistema.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Realiza el egreso de un equipo.
    /// </summary>
    [WebMethod()]
    public static void ProduccionEquipo(string idEquipo, int cantidad)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idEquipo)["id"]);

            ModuloStock.EgresoEquipo(id, cantidad);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. Verifique la disponibilidad de artículos.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Obtiene si el equipo se puede armar.
    /// </summary>
    [WebMethod()]
    public static void EquipoDisponibilidad(string idEquipo, int cantidad)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idEquipo)["id"]);

            List<ArticuloEquipo> faltantes = new List<ArticuloEquipo>();
            if (!ModuloStock.GetDisponibilidadEquipo(id, cantidad, out faltantes))
            {
                throw new Exception("0");
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    /// <summary>
    /// Actualiza el punto de pedido de un artículo.
    /// </summary>
    [WebMethod()]
    public static void ActualizarPuntoPedido(string idArticulo, int puntoPedido)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idArticulo)["id"]);

            ModuloStock.UpdatePtoPedido(id, puntoPedido);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. <br>Detalle: " + ex.Message);
        }
    }
}