using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class comercial_notifVentaLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<PermisosPersona> permisos = new List<PermisosPersona>()
            {
                PermisosPersona.SNV_Visualizacion,
                PermisosPersona.SNV_Vendedor,
                PermisosPersona.SNV_AltaImputacion,
                PermisosPersona.SNV_AltaCliente,
                PermisosPersona.SNV_AltaFacRem,
                PermisosPersona.SNV_NotifCierre,
                PermisosPersona.SNV_NotifOC,
                PermisosPersona.SNV_NotifRecordatorio,
                PermisosPersona.SNV_AltaTransporte,
                PermisosPersona.SNV_NotifProducto
            };

        if (!GPermisosPersonal.TieneAcceso(permisos)) Response.Redirect("/", true);
    }

    [WebMethod]
    public static object GetVentas(int pagina, int numero, string vendedor, string cliente, string oc, string imputacion, int estado)
    {
        List<Filtro> filtros = new List<Filtro>();

        if(numero != Constantes.ValorInvalido) filtros.Add(new Filtro((int)FiltroNotifVenta.ID, numero));
        if (!String.IsNullOrEmpty(vendedor)) filtros.Add(new Filtro((int)FiltroNotifVenta.Vendedor, vendedor));
        if (!String.IsNullOrEmpty(cliente)) filtros.Add(new Filtro((int)FiltroNotifVenta.Cliente, cliente));
        if (!String.IsNullOrEmpty(oc)) filtros.Add(new Filtro((int)FiltroNotifVenta.OC, oc));
        if (!String.IsNullOrEmpty(imputacion)) filtros.Add(new Filtro((int)FiltroNotifVenta.Imputacion, imputacion));
        if (Enum.IsDefined(typeof(EstadoNotifVenta), estado)) filtros.Add(new Filtro((int)FiltroNotifVenta.Estado, estado));

        List<NotifVentaResumen> lista = NotifVentas.GetNotifVentaResumen(pagina, filtros);
        int paginas = NotifVentas.GetNotifVentaResumenPaginas(filtros);

        return new { Lista = lista, TotalPaginas = paginas };
    }


	 /// <summary>
    /// Exporta el listado de ventas a formato Excel 97/2003.
    /// </summary>
    [WebMethod()]
    public static string ExportarListado(int numero, string vendedor, string cliente, string oc, string imputacion, int estado)
    {
        string result;

        List<Filtro> filtros = new List<Filtro>();

        if(numero != Constantes.ValorInvalido) filtros.Add(new Filtro((int)FiltroNotifVenta.ID, numero));
        if (!String.IsNullOrEmpty(vendedor)) filtros.Add(new Filtro((int)FiltroNotifVenta.Vendedor, vendedor));
        if (!String.IsNullOrEmpty(cliente)) filtros.Add(new Filtro((int)FiltroNotifVenta.Cliente, cliente));
        if (!String.IsNullOrEmpty(oc)) filtros.Add(new Filtro((int)FiltroNotifVenta.OC, oc));
        if (!String.IsNullOrEmpty(imputacion)) filtros.Add(new Filtro((int)FiltroNotifVenta.Imputacion, imputacion));
        if (Enum.IsDefined(typeof(EstadoNotifVenta), estado)) filtros.Add(new Filtro((int)FiltroNotifVenta.Estado, estado));

        List<NotifVentaResumenExcel> lista = NotifVentas.GetNotifVentaResumenExcel(filtros);

        try
        {
            string path = NotifVentas.ExportarAExcel(lista);			
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + path + "&n=ListadoVentas.xlsx&d=1");
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result;
    }

}