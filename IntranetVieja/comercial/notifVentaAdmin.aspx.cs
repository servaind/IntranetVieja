using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class comercial_notifVentaAdmin : System.Web.UI.Page
{
    // Variables.
    private NotifVenta notifVenta;
    private const string PrefSession = "notifVentaAdmin.aspx.";

    // Propiedades.
    public static int NotifVentaID
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "ID") == null)
            {
                GSessions.CrearSession(PrefSession + "ID", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "ID");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "ID") == null)
            {
                GSessions.CrearSession(PrefSession + "ID", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "ID", value);
            }
        }
    }
    public NotifVenta NotifVenta
    {
        get { return notifVenta; }
    }


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

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int notifVentaID;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out notifVentaID))
        {
            notifVenta = NotifVentas.GetNotifVenta(notifVentaID);

            if (notifVenta == null)
            {
                Response.Redirect("notifVentaAdmin.aspx");
                return;
            }

            NotifVentaID = notifVentaID;
        }

        if (!GPermisosPersonal.TieneAcceso(permisos)) Response.Redirect("/", true);
    }

    [WebMethod]
    public static object Nueva(int vendedor, int tipoVenta, string cliente, string oc, string imputacion, int moneda,
                               decimal montoOC, string fechaEntrega, string datosEnvio, string observaciones, decimal remitoMonto, string remitoDesc, bool calibExterna, string laboratorio)
    {
		
        bool result = true;
        string message;

        try
        {
            if (!Enum.IsDefined(typeof (TipoNotifVenta), tipoVenta)) throw new Exception("El tipo de venta no es válido.");
            if (!Enum.IsDefined(typeof (Moneda), moneda)) throw new Exception("El tipo de moneda no es válido.");

            int notifVentaID = NotifVentas.AddNotifVenta(vendedor, (TipoNotifVenta) tipoVenta, cliente, oc, imputacion,
                                                         (Moneda) moneda, montoOC, fechaEntrega, datosEnvio, observaciones, remitoMonto, remitoDesc, calibExterna, laboratorio);

            message = notifVentaID.ToString();
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new {Success = result, Message = message};
    }

    [WebMethod]
    public static object GuardarVendedor(string observaciones, string remitoDesc, bool confirmar)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVentaRemitoDesc(NotifVentaID, observaciones, remitoDesc, confirmar);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new { Success = result, Message = message };
    }

    [WebMethod]
    public static object GuardarRemitoVendedor(string observaciones, string remitoDestino, string remitoContacto,
                                               string remitoEntrega, string remitoDesc, bool confirmar)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVenta(NotifVentaID, observaciones, remitoDestino, remitoContacto, remitoEntrega,
                                         remitoDesc, confirmar);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new {Success = result, Message = message};
    }

    [WebMethod]
    public static object GuardarRem(int remito)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVentaRem(NotifVentaID, remito);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new { Success = result, Message = message };
    }

    [WebMethod]
    public static object GuardarFac(int factura)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVentaFac(NotifVentaID, factura);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new { Success = result, Message = message };
    }

    [WebMethod]
    public static object GuardarRemitoTransporte(string remitoTransporte)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVenta(NotifVentaID, remitoTransporte);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new { Success = result, Message = message };
    }

    [WebMethod]
    public static object AprobarRechazar(bool aprobar, string motivo)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            NotifVentas.UpdateNotifVenta(NotifVentaID, aprobar, motivo);
        }
        catch (Exception ex)
        {
            result = false;
            message = ex.Message;
        }

        return new { Success = result, Message = message };
    }

    [WebMethod]
    public static string GetClientes(string query)
    {
        string result;

        List<string> suggestions = new List<string>();
        List<string> data = new List<string>();
        List<string> results = Tango.GetClientes(query);
        results.ForEach(r =>
            {
                suggestions.Add(String.Format("'{0}'", r));
                data.Add(String.Format("'{0}'", r));
            });

        result = String.Format("query: \"{0}\", suggestions: [{1}], data: [{2}]", query,
                               Funciones.Concatenate(suggestions, ','),
                               Funciones.Concatenate(data, ','));

        return "{" + result + "}";
    }

    [WebMethod]
    public static string GetOCs(string query)
    {
        string result;

        List<string> suggestions = new List<string>();
        List<string> data = new List<string>();
        List<string> results = NotifVentas.GetOCs(query);
        results.ForEach(r =>
        {
            suggestions.Add(String.Format("'{0}'", r));
            data.Add(String.Format("'{0}'", r));
        });

        result = String.Format("query: \"{0}\", suggestions: [{1}], data: [{2}]", query,
                               Funciones.Concatenate(suggestions, ','),
                               Funciones.Concatenate(data, ','));

        return "{" + result + "}";
    }

    [WebMethod]
    public static object GetTemplate(string oc)
    {
        NotifVenta notifVenta = NotifVentas.GetNotifVenta(oc);
        if (notifVenta != null)
        {
            return new
                {
                    Success = true,
                    TipoVentaID = (int) notifVenta.TipoVenta,
                    notifVenta.Cliente,
                    notifVenta.Imputacion,
                    TipoMonedaID = (int) notifVenta.Moneda,
                    MontoOC = notifVenta.MontoOC == Constantes.ValorInvalido ? "" : notifVenta.MontoOC.ToString("0.00"),
                    Observaciones = notifVenta.Observaciones.Replace("<br>", "\n"),
                    RemitoDesc = notifVenta.RemitoDesc.Replace("<br>", "\n")
                };
        }

        return new {Success = false};
    }

    [WebMethod]
    public static object GetDescripcionImputacion(string numero)
    {
        bool result;
        string descripcion;

        descripcion = NotifVentas.GetDescripcionImputacion(numero);
        result = !String.IsNullOrWhiteSpace(descripcion);

        return new { Success = result, Descripcion = descripcion };
    }
}