using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sistemas_personalAccesos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.Administrador))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        // Personas.
        cbPersona.DataSource = GPersonal.GetPersonas(false);
        cbPersona.DataTextField = "Nombre";
        cbPersona.DataValueField = "ID";
        cbPersona.DataBind();
    }
    /// <summary>
    /// Obtiene las secciones para la persona.
    /// </summary>
    [WebMethod()]
    public static int[] GetSeccionesPersona(int idPersona)
    {
        List<int> result = new List<int>();

        List<Seccion> secciones = GSecciones.GetSecciones(idPersona);
        secciones.ForEach(s => result.Add(s.ID));

        return result.ToArray();
    }
    /// <summary>
    /// Actualiza las secciones de la persona.
    /// </summary>
    [WebMethod()]
    public static void ActualizarSeccionesPersona(int idPersona, int[] secciones)
    {
        try
        {
            List<int> s = new List<int>();
            foreach (int seccion in secciones)
            {
                s.Add(seccion);
            }

            GSecciones.ActualizarSeccionesPersonal(idPersona, s);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
}