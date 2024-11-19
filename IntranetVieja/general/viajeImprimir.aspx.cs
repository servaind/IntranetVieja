using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_viajeImprimir : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idSV;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idSV))
        {
            SolicitudViaje sv = GSolicitudesViaje.GetSolicitudViaje(idSV);

            if (sv == null)
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }

            if (sv.Estado != EstadosSolViaje.Aprobada && sv.Estado != EstadosSolViaje.Confirmada)
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }

            CargarSV(sv);
        }
    }
    /// <summary>
    /// Carga la solicitud de viaje.
    /// </summary>
    private void CargarSV(SolicitudViaje sv)
    {
        chkMoto.Attributes["class"] = sv.Vehiculo == VehiculosSolViaje.Moto ? "checked" : "unchecked";
        chkTaxi.Attributes["class"] = sv.Vehiculo == VehiculosSolViaje.Taxi ? "checked" : "unchecked";
        chkFlete.Attributes["class"] = sv.Vehiculo == VehiculosSolViaje.Flete ? "checked" : "unchecked";
        chkAuto.Attributes["class"] = sv.Vehiculo == VehiculosSolViaje.Auto ? "checked" : "unchecked";

        chkBaja.Attributes["class"] = sv.Importancia == ImporanciasSolViaje.Baja ? "checked" : "unchecked";
        chkNormal.Attributes["class"] = sv.Importancia == ImporanciasSolViaje.Normal ? "checked" : "unchecked";
        chkAlta.Attributes["class"] = sv.Importancia == ImporanciasSolViaje.Alta ? "checked" : "unchecked";

        lblFechaSolicitud.InnerText = sv.FechaSolicitud.ToShortDateString();
        lblMotivo.InnerText = sv.Motivo;
        lblDescripcion.InnerText = sv.Descripcion;
        lblOrigen.InnerText = sv.Origen;
        lblRuta.InnerText = sv.Ruta;
        lblFinRecorrido.InnerText = sv.FinRecorrido;
        lblFechaCumplimiento.InnerText = sv.FechaCumplimiento.ToShortDateString();
        lblHoraCumplimiento.InnerText = sv.HoraCumplimiento;
        lblFechaLimite.InnerText = sv.FechaLimite.ToShortDateString();
        lblHoraLimite.InnerText = sv.HoraLimite;
        lblDestinatario.InnerText = sv.Destinatario;
        lblDireccion.InnerText = sv.Direccion;
        lblLocalidad.InnerText = sv.Localidad;
        lblContacto.InnerText = sv.Contacto;
        lblTelefono.InnerText = sv.Telefono;
        lblHorarioAtencion.InnerText = sv.HorarioAtencion;
        lblDocumentoRef.InnerText = sv.DocumentoReferencia;
        chkRetFac.Attributes["class"] = sv.RetornaFactura ? "checked" : "unchecked";
        chkRetRem.Attributes["class"] = sv.RetornaRemito ? "checked" : "unchecked";
        chkRetRec.Attributes["class"] = sv.RetornaRecibo ? "checked" : "unchecked";
        lblRetOtro.InnerText += sv.RetornaOtro;
        lblCondicionComercial.InnerText = sv.CondicionesComerciales;
        lblImputación.InnerText = sv.Imputacion != null ? sv.Imputacion.Numero.ToString() : "";
        lblImporte.InnerText = sv.Importe.ToString("0.00");
        chkEfectivo.Attributes["class"] = sv.Efectivo ? "checked" : "unchecked";
        chkCheque.Attributes["class"] = sv.Cheque ? "checked" : "unchecked";
        if (sv.Cheque)
        {
            lblAlaOrden.InnerText = "a la orden de " + sv.AlaOrden;
        }
        lblObervaciones.InnerText = sv.Observaciones;
        lblSolicito.InnerText = sv.Solicito.Nombre;
    }
}