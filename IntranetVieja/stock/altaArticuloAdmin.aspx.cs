using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_altaArticuloAdmin : System.Web.UI.Page
{
    // Variables.
    private CodificacionArticulo ca;
    private const string PrefSession = "altaArticuloAdmin.aspx.";

    // Propiedades.
    public static int IdCA
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdCA") == null)
            {
                GSessions.CrearSession(PrefSession + "IdCA", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdCA");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdCA") == null)
            {
                GSessions.CrearSession(PrefSession + "IdCA", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdCA", value);
            }
        }
    }
    public CodificacionArticulo CA
    {
        get { return this.ca; }
    }
    public bool PuedeAdministrador
    {
        get
        {
            bool result;

            result = this.ca != null && this.ca.Estado != EstadosCodArt.Aprobado && this.ca.Estado != EstadosCodArt.Rechazado &&
                GPermisosPersonal.TieneAcceso(PermisosPersona.SAAResponsable);

            return result;
        }
    }
    public bool PuedeSolicitante
    {
        get
        {
            bool result = false;

            result = this.ca == null || (this.ca.Estado == EstadosCodArt.Rechazado &&
                this.ca.IdSolicito == Constantes.Usuario.ID);

            return result;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Unidades de medida.
            cbUnidadMedida.DataSource = CodificacionArticulos.GetUnidadesMedida();
            cbUnidadMedida.DataTextField = "Value";
            cbUnidadMedida.DataValueField = "Key";
            cbUnidadMedida.DataBind();

            cbUnidadMedida.Value = "1";

            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idCA;
            if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idCA))
            {
                this.ca = CodificacionArticulos.GetCodificacionArticulo(idCA);

                if (this.ca == null)
                {
                    Response.Redirect("altaArticuloAdmin.aspx");
                    return;
                }

                IdCA = idCA;

                CompletarSolicitud(this.CA);
            }
        }
    }

    /// <summary>
    /// Completa los campos del formulario.
    /// </summary>
    private void CompletarSolicitud(CodificacionArticulo c)
    {
        txtDescripcionArticulo.Value = c.DescripcionArticulo;
        cbUnidadMedida.Value = c.IdUnidadMedida.ToString();
        txtCodigoArticulo.Value = c.CodigoArticulo;
        txtDescripcionUso.Value = c.DescripcionUso;
        if (c.Solicito != null)
        {
            lblSolicito.InnerText = c.Solicito.Nombre;
            lblSolicito.Visible = true;
        }
        if (c.Estado == EstadosCodArt.Aprobado ||
            (c.Estado == EstadosCodArt.Revision && !GPermisosPersonal.TieneAcceso(PermisosPersona.SAAResponsable)))
        {
            txtDescripcionArticulo.Attributes["readonly"] = "readonly";
            cbUnidadMedida.Attributes["disabled"] = "disabled";
            txtCodigoArticulo.Attributes["readonly"] = "readonly";
            txtDescripcionUso.Attributes["readonly"] = "readonly";

            lblFechaAprobado.InnerText = c.FechaAprobo.ToShortDateString();
        }
    }
    /// <summary>
    /// Genera una nueva solicitud de alta de artículo.
    /// </summary>
    [WebMethod()]
    public static string NuevaAltaArticulo(string descripcionArticulo, int idUnidadMedida, string codigoArticulo,
        string descripcionUso)
    {
        string result;

        try
        {
            int numero;
            CodificacionArticulos.AltaCodificacionArticulo(descripcionArticulo, idUnidadMedida, codigoArticulo, descripcionUso,
                out numero);

            result = "Solicitud de alta de artículo enviada. Nº de referencia: " + numero;
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue generada correctamente, pero se produjo un error al intentar informar "
                    + "al responsable. Por favor, contáctese con el área de sistemas.");
        }
        catch (ElementoExistenteException)
        {
            throw new Exception("El código de artículo ingresado ya se encuentra presente en el sistema.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Guarda la solicitud de alta de artículo.
    /// </summary>
    [WebMethod()]
    public static string GuardarAltaArticulo(string descripcionArticulo, int idUnidadMedida, string codigoArticulo,
        string descripcionUso)
    {
        string result;

        try
        {
            CodificacionArticulos.ActualizarCodificacionArticulo(IdCA, descripcionArticulo, idUnidadMedida, codigoArticulo, 
                descripcionUso, false);

            result = "Solicitud de alta de artículo actualizada.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue actualizada, pero se produjo un error al intentar informar "
                    + "al responsable. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Aprueba la solicitud de alta de artículo.
    /// </summary>
    [WebMethod()]
    public static string AprobarAltaArticulo(string descripcionArticulo, int idUnidadMedida, string codigoArticulo,
        string descripcionUso)
    {
        string result;

        try
        {
            CodificacionArticulos.ActualizarCodificacionArticulo(IdCA, descripcionArticulo, idUnidadMedida, codigoArticulo,
                descripcionUso, true);

            result = "Solicitud de alta de artículo aprobada.";
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
    /// Rechaza la solicitud de alta de artículo.
    /// </summary>
    [WebMethod()]
    public static string RechazarAltaArticulo(string motivo)
    {
        string result;

        try
        {
            CodificacionArticulos.RechazarCodificacionArticulo(IdCA, motivo);

            result = "Solicitud de alta de artículo rechazada.";
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
    /// Marca como No corresponde la solicitud de alta de artículo.
    /// </summary>
    [WebMethod()]
    public static string NoCorrespondeAltaArticulo(string motivo)
    {
        string result;

        try
        {
            CodificacionArticulos.NoCorrespondeCodificacionArticulo(IdCA, motivo);

            result = "Solicitud de alta de artículo marcada como \"No corresponde\".";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue marcada como \"No corresponde\", pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
}