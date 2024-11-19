using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_InformacionObraImprimir : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.IIOVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idIO;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idIO))
        {
            InformacionObra io = InformacionObras.GetInformacionObra(idIO, Constantes.ValorInvalido);

            if (io == null)
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }

            CargarInformeObra(io);
        }
    }
    /// <summary>
    /// Carga la información de obra.
    /// </summary>
    private void CargarInformeObra(InformacionObra io)
    {
        const string Vinieta = "•";
                
        lblFecha.InnerText = io.Fecha.ToShortDateString();
        lblRevision.InnerText = io.Datos.Revision.ToString();
        lblInforma.InnerText = io.Informa.Nombre;
        lblResponsableObra.InnerText = io.Datos.ResponsableObra.Nombre;
        chkTipoObra.Attributes["class"] = io.Datos.TipoTrabajo == TiposTrabajoObra.Obra ? "checked" : "unchecked";
        chkTipoMantenimiento.Attributes["class"] = io.Datos.TipoTrabajo == TiposTrabajoObra.Mantenimiento ? "checked" : "unchecked";
        lblImputacion.InnerText = io.Datos.Imputacion;
        lblCliente.InnerText = io.Datos.Cliente;
        lblOrdenCompra.InnerText = io.Datos.OrdenCompra.ToString();
        lblFechaEntrega.InnerText = io.Datos.FechaEntrega.ToShortDateString();
        chkSubcontratSi.Attributes["class"] = io.Datos.Subcontratistas ? "checked" : "unchecked";
        chkSubcontratNo.Attributes["class"] = !io.Datos.Subcontratistas ? "checked" : "unchecked";
        lblSubcontratEmpresa.InnerText = io.Datos.SubcontratEmpresa;
        chkPredioTercSi.Attributes["class"] = io.Datos.PredioTerceros ? "checked" : "unchecked";
        chkPredioTercNo.Attributes["class"] = !io.Datos.PredioTerceros ? "checked" : "unchecked";
        lblPredioTercEmpresa.InnerText = io.Datos.PredioTercEmpresa;
        lblUbicacion.InnerText = io.Datos.Ubicacion;
        lblProvincia.InnerText = io.Datos.Provincia;
        lblRespTecCli.InnerText = io.Datos.RespTecCliente;
        lblRespTecCliTel.InnerText = io.Datos.RespTecClienteTel;
        lblRespTecCliEmail.InnerText = io.Datos.RespTecClienteEmail;
        lblRespSegCli.InnerText = io.Datos.RespSegCliente;
        lblRespSegCliTel.InnerText = io.Datos.RespSegClienteTel;
        lblRespSegCliEmail.InnerText = io.Datos.RespSegClienteEmail;
        lblContAdminCliente.InnerText = io.Datos.ContacAdminCliente;
        lblContAdminClienteTel.InnerText = io.Datos.ContacAdminClienteTel;
        lblContAdminClienteEmail.InnerText = io.Datos.ContacAdminClienteEmail;
        lblFechaEstimada.InnerText = io.Datos.FechaInicio.ToShortDateString();
        lblDuracion.InnerText = io.Datos.Duracion;
        lblFechaFinalizacion.InnerText = io.Datos.FechaFinalizacion.ToShortDateString();
        lblDescripcionTareas.InnerText = io.Datos.DescripcionTareas;
        lblGerenteProyecto.InnerText = io.Datos.Gerente.Nombre;
        lblObjetivoProyecto.InnerText = io.Datos.ObjetivoProyecto;
        lblPersonalMant.InnerText = lblPersonalObras.InnerText = lblVehiculos.InnerText = "";
        string htmlMant = "";
        foreach (Persona mant in io.Datos.PersonasMantenimiento)
        {
            htmlMant += Vinieta + mant.Nombre + "</br>";
        }
        lblPersonalMant.InnerHtml = htmlMant;
        string htmlObras = "";
        foreach (Persona obra in io.Datos.PersonasObra)
        {
            htmlObras += Vinieta + obra.Nombre + "</br>";
        }
        lblPersonalObras.InnerHtml = htmlObras;
        string htmlVehic = "";
        foreach (Vehiculo vehic in io.Datos.Vehiculos)
        {
            htmlVehic += Vinieta + vehic.Patente + "</br>";
        }
        lblVehiculos.InnerHtml = htmlVehic;

        int items = io.Datos.PersonasMantenimiento.Count + io.Datos.PersonasObra.Count + io.Datos.Vehiculos.Count;
        marcoGeneral.Style["height"] = String.Format("{0}px", 950 + items * 16);
    }
}