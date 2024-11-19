using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_ncImprimir : System.Web.UI.Page
{
    // Variables.
    private NoConformidad nc;

    // Propiedades.
    public NoConformidad NC
    {
        get { return this.nc; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idNC;
        if (!parametros.ContainsKey("id") || !Int32.TryParse(parametros["id"], out idNC))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        this.nc = GNoConformidades.GetNoConformidad(idNC);
        if (!(this.nc != null && (this.nc.Estado == EstadosNC.Cerrada || this.nc.Estado == EstadosNC.NoCorresponde)))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }
    }
}