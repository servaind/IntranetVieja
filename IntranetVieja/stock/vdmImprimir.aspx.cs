using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_vdmImprimir : System.Web.UI.Page
{
    // Variables.
    private ValeDeMateriales vdm;

    // Propiedades.
    public ValeDeMateriales VDM
    {
        get { return this.vdm; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idVDM;
            if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idVDM))
            {
                this.vdm = GValeDeMateriales.GetValeDeMateriales(idVDM);

                if (this.vdm == null)
                {
                    Response.Redirect("vdmAdmin.aspx");
                    return;
                }

                CompletarVDM(this.vdm);
            }
        }
    }

    /// <summary>
    /// Completa el vale de materiales.
    /// </summary>
    private void CompletarVDM(ValeDeMateriales vdm)
    {
        switch (vdm.Estado)
        {
            case EstadosVDM.Enviada:
                // Marco la solicitud como leída.
                if (vdm.Solicito.IdAutoriza == Constantes.Usuario.ID)
                {
                    try
                    {
                        GValeDeMateriales.AprobarEstadoActual(vdm.ID);
                        Response.Redirect(Request.Url.ToString());
                        return;
                    }
                    catch
                    {
                        Response.Redirect(Constantes.UrlIntraDefault);
                    }
                }
                break;
            case EstadosVDM.AprobadaResponsable:
                // Marco la solicitud como recibida por depósito.
                if (GPermisosPersonal.TieneAcceso(PermisosPersona.ValeMaterialesRecibeDep))
                {
                    try
                    {
                        GValeDeMateriales.AprobarEstadoActual(vdm.ID);
                        Response.Redirect(Request.Url.ToString());
                        return;
                    }
                    catch
                    {
                        Response.Redirect(Constantes.UrlIntraDefault);
                    }
                }
                break;
        }

        lblFechaSolicitud.InnerText = vdm.FechaSolicitud.ToString("dd/MM/yyyy");
        lblEmitidaPor.InnerText = vdm.Solicito.Nombre;

        lblRecibidaResponsable.InnerText = vdm.Estado >= EstadosVDM.RecibidaResponsable ? String.Format("{0} - {1}",
            vdm.FechaRecibioResponsable.ToString("dd/MM/yyyy"), vdm.RecibioResponsable.Nombre) : "-";

        lblAprobadaResponsable.InnerText = vdm.Estado >= EstadosVDM.AprobadaResponsable && vdm.Estado != EstadosVDM.RechazadaResponsable
            ? String.Format("{0} - {1}", vdm.FechaAproboResponsable.ToString("dd/MM/yyyy"), vdm.AproboResponsable.Nombre) : "-";

        lblRecibidaDeposito.InnerText = vdm.Estado >= EstadosVDM.RecibidaDeposito && vdm.Estado != EstadosVDM.RechazadaResponsable
            ? String.Format("{0} - {1}", vdm.FechaRecibioDeposito.ToString("dd/MM/yyyy"), vdm.RecibioDeposito.Nombre) : "-";

        lblEntregada.InnerText = vdm.Estado >= EstadosVDM.EntregadaDeposito && vdm.Estado != EstadosVDM.RechazadaResponsable
            ? String.Format("{0} - {1}", vdm.FechaEntregoDeposito.ToString("dd/MM/yyyy"), vdm.EntregoDeposito.Nombre) : "-";

        lblDepartamento.InnerText = vdm.Departamento;
        lblSMTL.InnerText = vdm.SMTL.ToString();
        lblCargo.InnerText = vdm.Cargo;
        lblDestino.InnerText = vdm.Destino;

        vdm.CargarItems();
    }
}