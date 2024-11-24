using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_licenciaAdmin : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "licenciaAdmin.aspx.";

    // Variables.
    private Licencia licencia;

    // Propiedades.
    public Licencia Licencia
    {
        get { return this.licencia; }
    }
    public static int IdLic
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdLic") == null)
            {
                GSessions.CrearSession(PrefSession + "IdLic", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdLic");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdLic") == null)
            {
                GSessions.CrearSession(PrefSession + "IdLic", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdLic", value);
            }
        }
    }
    public bool PuedeResponsable
    {
        get
        {
            return this.licencia != null && this.licencia.EstadoAutorizacion == EstadosLicencia.NoRecibida &&
                this.licencia.Solicito.IdAutoriza == Constantes.Usuario.ID;
        }
    }
    public bool PuedeRRHH
    {
        get
        {
            return this.licencia != null && this.licencia.EstadoAutorizacion == EstadosLicencia.AprobadaResponsable &&
                GPermisosPersonal.TieneAcceso(PermisosPersona.LicRRHH);
        }
    }
    public string FechaInicio
    {
        get
        {
            string result;

            result = DateTime.Now.ToShortDateString();

            return result;
        }
    }
    

    protected void Page_Load(object sender, EventArgs e)
    {
        // Motivos.
        cbMotivo.DataSource = GLicencias.GetTiposLicencia();
        cbMotivo.DataTextField = "Value";
        cbMotivo.DataValueField = "Key";
        cbMotivo.DataBind();

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idLic;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idLic))
        {
            this.licencia = GLicencias.GetLicencia(idLic);
            if (this.licencia == null)
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }

            IdLic = idLic;

            CargarLicencia(this.licencia);
        }
    }
    /// <summary>
    /// Muestra la licencia.
    /// </summary>
    private void CargarLicencia(Licencia licencia)
    {
        lblSolicito.InnerText = licencia.Solicito.Nombre;
        cbMotivo.Value = ((int)licencia.Tipo).ToString();
        txtFechaDesde.Value = licencia.Inicio.ToShortDateString();
        txtFechaHasta.Value = licencia.Finalizacion.ToShortDateString();
        txtObservaciones.Value = licencia.Observaciones;

        cbMotivo.Attributes["disabled"] = "disabled";
        txtFechaDesde.Attributes["readonly"] = "readonly";
        txtFechaHasta.Attributes["readonly"] = "readonly";
        txtObservaciones.Attributes["readonly"] = "readonly";
    }
    /// <summary>
    /// Genera una nueva solicitud de licencia.
    /// </summary>
    [WebMethod()]
    public static string NuevaLicencia(int motivo, string desde, string hasta, string observaciones)
    {
        string result;

        try
        {
            GLicencias.NuevaLicencia(Convert.ToDateTime(desde), Convert.ToDateTime(hasta), observaciones, (TiposLicencia)motivo);
            result = "La solicitud ha sido generada y enviada al responsable de área.<br />";
            result += "Recuerde, en caso de ser necesario, presentar los justificativos correspondientes.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue generada correctamente, pero se produjo un error al intentar informar "
                    + "al responsable. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Aprueba el estado actual de la licencia.
    /// </summary>
    [WebMethod()]
    public static string AprobarResponsable()
    {
        string result;

        try
        {
            GLicencias.AprobarEstadoActual(IdLic);

            result = "La solicitud ha sido aprobada y enviada a Recursos Humanos.<br />";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue aprobada, pero se produjo un error al intentar informar "
                    + "a Recursos Humanos. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Rechaza el estado actual de la licencia.
    /// </summary>
    [WebMethod()]
    public static string RechazarResponsable()
    {
        string result;

        try
        {
            GLicencias.RechazarEstadoActual(IdLic);

            result = "La solicitud fue rechazada y se ha enviado un e-mail al solicitante informando la decisión.<br />";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue rechazada, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Aprueba el estado actual de la licencia.
    /// </summary>
    [WebMethod()]
    public static string AprobarRRHH()
    {
        string result;

        try
        {
            GLicencias.AprobarEstadoActual(IdLic);

            result = "La solicitud ha sido aprobada y se ha enviado un e-mail al solicitante informando la decisión.<br />";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue aprobada, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Rechaza el estado actual de la licencia.
    /// </summary>
    [WebMethod()]
    public static string RechazarRRHH()
    {
        string result;

        try
        {
            GLicencias.RechazarEstadoActual(IdLic);

            result = "La solicitud fue rechazada y se ha enviado un e-mail al solicitante informando la decisión.<br />";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue rechazada, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
}