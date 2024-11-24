using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sistemas_personalPermisos : System.Web.UI.Page
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
    /// Obtiene los permisos para la persona.
    /// </summary>
    [WebMethod()]
    public static int[] GetPermisosPersona(int idPersona)
    {
        List<int> result = new List<int>();

        List<PermisoPersonal> permisos = GPermisosPersonal.GetPermisosPersonal(idPersona);
        permisos.ForEach(p => result.Add((int)p.Permiso));

        return result.ToArray();
    }
    /// <summary>
    /// Actualiza los permisos de la persona.
    /// </summary>
    [WebMethod()]
    public static void ActualizarPermisosPersona(int idPersona, int[] permisos)
    {
        try
        {
            List<PermisosPersona> p = new List<PermisosPersona>();
            foreach (int permiso in permisos)
            {
                if (Enum.IsDefined(typeof(PermisosPersona), permiso))
                {
                    p.Add((PermisosPersona)permiso);
                }
            }

            GPermisosPersonal.ActualizarPermisosPersonal(idPersona, p);
        }
        catch(Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
}