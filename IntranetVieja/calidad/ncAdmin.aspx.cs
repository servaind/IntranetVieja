using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_ncAdmin : System.Web.UI.Page
{
    // Variables.
    private NoConformidad nc;
    private const string PrefSession = "ncAdmin.aspx.";

    // Propiedades.
    public static int IdNC
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdNC") == null)
            {
                GSessions.CrearSession(PrefSession + "IdNC", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdNC");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdNC") == null)
            {
                GSessions.CrearSession(PrefSession + "IdNC", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdNC", value);
            }
        }
    }
    public NoConformidad NC
    {
        get { return this.nc; }
    }
    public bool PuedeAdministrador
    {
        get
        {
            bool result;

            result = this.nc != null && this.nc.Estado != EstadosNC.Cerrada && this.nc.Estado != EstadosNC.ProcesandoImputado &&
                this.nc.Estado != EstadosNC.NoCorresponde && GPermisosPersonal.TieneAcceso(PermisosPersona.NNCAdministrador);

            return result;
        }
    }
    public bool PuedeImputado
    {
        get
        {
            bool result = false;

            result = this.nc != null && this.nc.Estado == EstadosNC.ProcesandoImputado &&
                (this.nc.Area.EsResponsable() || GPermisosPersonal.TieneAcceso(PermisosPersona.NNCAdministrador));

            return result;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Origen.
            cbEquipo.DataSource = GNoConformidades.ListaOrigen();
            cbEquipo.DataBind();

            // Áreas de responsabilidad.
            cbAreaResponsabilidad.DataSource = GAreas.GetAreas();
            cbAreaResponsabilidad.DataTextField = "Descripcion";
            cbAreaResponsabilidad.DataValueField = "ID";
            cbAreaResponsabilidad.DataBind();

            // Categorías.
            cbCategoria.DataSource = GNoConformidades.GetCategoriasNC();
            cbCategoria.DataTextField = "Key";
            cbCategoria.DataValueField = "Value";
            cbCategoria.DataBind();

            // Conclusión.
            cbCierre.DataSource = GNoConformidades.GetCierresNC();
            cbCierre.DataTextField = "Key";
            cbCierre.DataValueField = "Value";
            cbCierre.DataBind();

            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idNC;
            if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idNC))
            {
                this.nc = GNoConformidades.GetNoConformidad(idNC);

                if (this.nc == null)
                {
                    Response.Redirect("ncAdmin.aspx");
                    return;
                }

                IdNC = idNC;

                CargarNNC(this.nc);
            }
			else {
				Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
			}
        }
    }

    /// <summary>
    /// Carga la NC en el formulario.
    /// </summary>
    private void CargarNNC(NoConformidad nc)
    {
        lblNumero.InnerText = nc.GetNumero();
        lblFecha.InnerText = nc.FechaEmision.ToShortDateString();
        chkNormaISO9001.Checked = nc.NormaISO9001;
        chkNormaISO14001.Checked = nc.NormaISO14001;
        chkNormaOHSAS18001.Checked = nc.NormaOHSAS18001;
        chkNormaIRAM301.Checked = nc.NormaIRAM301;
        if (nc.NormaOHSAS18001)
        {
            lblRevMatrizRiesgo.InnerText = (nc.RevMatrizRiesgo ? "R" : "No r") + "equiere revisión de la Matriz de Riesgos.";
        }
        else
        {
            lblRevMatrizRiesgo.Style["display"] = "none";
        }
        cbAreaResponsabilidad.Value = nc.Area.ID.ToString();
        txtApartado.Value = nc.Apartado;
        cbCategoria.Value = ((int)nc.Categoria).ToString();
        lblEmitidaPor.InnerText = nc.EmitidaPor.Nombre;
        cbEquipo.Value = nc.Equipo;
        txtAsunto.Value = nc.Asunto;
        txtHallazgo.Value = nc.Hallazgo;
        txtAccionInmediata.Value = nc.AccionInmediata;
        txtCausasRaices.Value = nc.CausasRaices;
        txtAccionCorrectiva.Value = nc.AccionCorrectiva;
        cbCierre.Value = ((int)nc.Conclusion).ToString();
        txtComentarios.Value = nc.Comentarios;
        if (nc.Estado == EstadosNC.Cerrada)
        {
            lblCerradaPor.InnerText = String.Format("{0} - {1}", nc.FechaCierre.ToShortDateString(), nc.FirmaCierre.Nombre);
        }

        if (nc.Estado != EstadosNC.Cerrada && (PuedeAdministrador || PuedeImputado))
        {
            if (PuedeImputado)
            {
                chkNormaISO9001.Attributes["disabled"] = "disabled";
                chkNormaISO14001.Attributes["disabled"] = "disabled";
                chkNormaOHSAS18001.Attributes["disabled"] = "disabled";
                chkNormaIRAM301.Attributes["disabled"] = "disabled";
                cbAreaResponsabilidad.Attributes["disabled"] = "disabled";
                txtApartado.Attributes["readonly"] = "readonly";
                cbCategoria.Attributes["disabled"] = "disabled";
                cbEquipo.Attributes["readonly"] = "readonly";
                txtAsunto.Attributes["readonly"] = "readonly";
                txtHallazgo.Attributes["readonly"] = "readonly";
                cbCierre.Attributes["disabled"] = "disabled";
                txtComentarios.Attributes["readonly"] = "readonly";
            }
        }
        else
        {
            chkNormaISO9001.Attributes["disabled"] = "disabled";
            chkNormaISO14001.Attributes["disabled"] = "disabled";
            chkNormaOHSAS18001.Attributes["disabled"] = "disabled";
            chkNormaIRAM301.Attributes["disabled"] = "disabled";
            cbAreaResponsabilidad.Attributes["disabled"] = "disabled";
            txtApartado.Attributes["readonly"] = "readonly";
            cbCategoria.Attributes["disabled"] = "disabled";
            cbEquipo.Attributes["readonly"] = "readonly";
            txtAsunto.Attributes["readonly"] = "readonly";
            txtHallazgo.Attributes["readonly"] = "readonly";
            txtAccionInmediata.Attributes["readonly"] = "readonly";
            txtCausasRaices.Attributes["readonly"] = "readonly";
            txtAccionCorrectiva.Attributes["readonly"] = "readonly";
            cbCierre.Attributes["disabled"] = "disabled";
            txtComentarios.Attributes["readonly"] = "readonly";
        }
    }
    /// <summary>
    /// Genera una Nota de No Conformidad.
    /// </summary>
    [WebMethod()]
    public static string GenerarNoConformidad(string asunto, string equipo, string hallazgo, string accionInmediata, 
        string comentarios)
    {
        string result;
        int numero = Constantes.ValorInvalido;

        try
        {
            GNoConformidades.NuevaNC(asunto, equipo, hallazgo, accionInmediata, comentarios, out numero);

            result = "La solicitud se ha generado correctamente. Nº " + numero;
        }
        catch (ErrorOperacionException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud se ha generado correctamente (Nº " + numero + "), pero se produjo un error al "
                              + "intentar enviar el email de informe. Por favor, contáctese con el Área de Sistemas.");
        }

        return result;
    }
    /// <summary>
    /// Actualiza la No Conformidad en el paso del imputado.
    /// </summary>
    [WebMethod()]
    public static string GuardarNCImputado(string accionInmediata, string causasRaices, string accionCorrectiva)
    {
        try
        {
            GNoConformidades.GuardarNCImputado(IdNC, accionInmediata, causasRaices, accionCorrectiva);
        }
        catch (ErrorOperacionException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (EmailException)
        {
            throw new Exception("La nota de no conformidad se ha guardado correctamente, pero se produjo un error al "
                              + "intentar enviar el email de informe. Por favor, contáctese con el Área de Sistemas.");
        }

        return "La solicitud se actualizó correctamente.";
    }
    /// <summary>
    /// Guarda una nota de no conformidad.
    /// </summary>
    [WebMethod()]
    public static string GuardarNCSGC(int normaISO9001, int normaISO14001, int normaOHSAS18001, int normaIRAM301, int revisionMatrizRiesgo, int idArea, 
        string apartado, int categoria, string equipo,
        string asunto, string hallazgo, string accionInmediata, string causasRaices, string accionCorrectiva,
        byte conclusion, string comentarios)
    {
        return ActualizarNC(normaISO9001 == 1, normaISO14001 == 1, normaOHSAS18001 == 1, normaIRAM301 == 1, revisionMatrizRiesgo == 1, idArea, apartado, 
            (CategoriasNC)categoria, equipo, asunto,
            hallazgo, accionInmediata, causasRaices, accionCorrectiva, (ConclusionesNC)conclusion, comentarios, false);
    }
    /// <summary>
    /// Cierra una nota de no conformidad.
    /// </summary>
    [WebMethod()]
    public static string CerrarNC(int normaISO9001, int normaISO14001, int normaOHSAS18001, int normaIRAM301, int revisionMatrizRiesgo, int idArea, 
        string apartado, int categoria, string equipo,
        string asunto, string hallazgo, string accionInmediata, string causasRaices, string accionCorrectiva,
        byte conclusion, string comentarios)
    {
        return ActualizarNC(normaISO9001 == 1, normaISO14001 == 1, normaOHSAS18001 == 1, normaIRAM301 == 1, revisionMatrizRiesgo == 1, idArea, apartado, 
            (CategoriasNC)categoria, equipo, asunto,
            hallazgo, accionInmediata, causasRaices, accionCorrectiva, (ConclusionesNC)conclusion, comentarios, true);
    }
    /// <summary>
    /// Actualiza la nota de no conformidad.
    /// </summary>
    private static string ActualizarNC(bool normaISO9001, bool normaISO14001, bool normaOHSAS18001, bool normaIRAM301, bool revisionMatrizRiesgo, 
        int idArea, string apartado, CategoriasNC categoria,
        string equipo, string asunto, string hallazgo, string accionInmediata, string causasRaices, string accionCorrectiva,
        ConclusionesNC conclusion, string comentarios, bool cerrar)
    {
        if (GAreas.GetArea(idArea) == null)
        {
            throw new Exception("Está intentando realizar una operación no válida.");
        }

        try
        {
            GNoConformidades.ActualizarNC(IdNC, normaISO9001, normaISO14001, normaOHSAS18001, normaIRAM301, revisionMatrizRiesgo, apartado, 
                categoria, idArea, equipo, hallazgo, accionInmediata, causasRaices, accionCorrectiva, conclusion, comentarios, 
                asunto, cerrar);
        }
        catch (ErrorOperacionException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (EmailException)
        {
            throw new Exception("La nota de no conformidad se ha guardado correctamente, pero se produjo un error al "
                              + "intentar enviar el email de informe. Por favor, contáctese con el Área de Sistemas.");
        }

        return "La solicitud se actualizó correctamente.";
    }
}