using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_autorizAdmin : System.Web.UI.Page
{
    // Variables.
    private Autorizacion au;
    private const string PrefSession = "autorizAdmin.aspx.";

    // Propiedades.
    public static int IdAutorizacion
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdAutorizacion") == null)
            {
                GSessions.CrearSession(PrefSession + "IdAutorizacion", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdAutorizacion");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdAutorizacion") == null)
            {
                GSessions.CrearSession(PrefSession + "IdAutorizacion", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdAutorizacion", value);
            }
        }
    }
    public Autorizacion AU
    {
        get { return this.au; }
    }
    public bool PuedeVer
    {
        get
        {
            bool result;

            result = this.au != null && (this.au.IdSolicito == Constantes.Usuario.ID
                || this.au.IdResponsable == Constantes.Usuario.ID);

            return result;
        }
    }
    public bool PuedeAutorizar
    {
        get
        {
            bool result;

            result = this.au != null && this.au.Estado == EstadoAutorizacion.Pendiente && 
                this.au.IdResponsable == Constantes.Usuario.ID;

            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idAU;
            if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idAU))
            {
                this.au = Autorizaciones.GetAutorizacion(idAU);

                if (this.au == null || !PuedeVer)
                {
                    Response.Redirect(Constantes.UrlIntraDefault, true);
                    return;
                }

                IdAutorizacion = idAU;

                CompletarSolicitud(this.au);
            }
        }
    }
    /// <summary>
    /// Completa los campos del formulario.
    /// </summary>
    private void CompletarSolicitud(Autorizacion au)
    {
        lblNumero.InnerText = au.IdAutorizacion.ToString();
        lblEmitidaPor.InnerText = au.Solicito.Nombre;
        lblFechaSol.InnerText = au.FechaSolicito.ToShortDateString();
        lblResponsable.InnerText = au.Responsable.Nombre;
        lblMotivoAutoriz.InnerText = au.MotivoAutorizacion;
        lblReferencia.InnerText = Autorizaciones.GetReferencia(au.Referencia);

        if (au.Estado != EstadoAutorizacion.Pendiente)
        {
            lblFechaAutoriz.InnerText = au.FechaAutorizo.ToShortDateString();
            lblMotivoRechazo.InnerText = au.MotivoRechazo;
        }
    }
    /// <summary>
    /// Aprueba la solicitud.
    /// </summary>
    [WebMethod()]
    public static string AprobarSolicitud()
    {
        string result;

        try
        {
            Autorizaciones.AprobarAutorizacion(IdAutorizacion);

            result = "La solicitud de autorización ha sido aprobada y se ha enviado un email notificando al solicitante.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud de autorización fue aprobada, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Rechaza la solicitud.
    /// </summary>
    [WebMethod()]
    public static string RechazarSolicitud(string motivo)
    {
        string result;

        try
        {
            Autorizaciones.RechazarAutorizacion(IdAutorizacion, motivo);

            result = "La solicitud de autorización ha sido rechazada y se ha enviado un email notificando al solicitante.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud de autorización fue rechazada, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }    
}