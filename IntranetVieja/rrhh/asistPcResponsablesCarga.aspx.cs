using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_asistPcResponsablesCarga : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ADP_PanelControlAsistencia))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        // Personas.
        cbPersona.DataSource = GPersonal.GetPersonas(true);
        cbPersona.DataTextField = "Nombre";
        cbPersona.DataValueField = "ID";
        cbPersona.DataBind();
    }

    [WebMethod()]
    public static int[] GetPersonasResponsable(int responsableId)
    {
        List<int> result = new List<int>();

        List<Persona> personas = AsistenciaPanelControlFac.GetPersonasPcResponsable(responsableId);
        personas.ForEach(p => result.Add(p.ID));

        return result.ToArray();
    }

    [WebMethod()]
    public static void UpdatePcResponsable(int responsableId, int[] personas)
    {
        try
        {
            AsistenciaPanelControlFac.UpdatePcResponsable(responsableId, personas.ToList());
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
}