using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_detalleAsistencia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Personas.
        cbPersona.DataSource = GPersonal.GetPersonas(false);
        cbPersona.DataTextField = "Nombre";
        cbPersona.DataValueField = "ID";
        cbPersona.DataBind();

        // Filtros.
        cbFiltro.DataSource = GLicencias.GetTiposLicenciasFiltros();
        cbFiltro.DataTextField = "Value";
        cbFiltro.DataValueField = "Key";
        cbFiltro.DataBind();
    }
    /// <summary>
    /// Obtiene los partes diarios que coincidan con el filtro.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPartesDiarios(int idPersona, string desde, string hasta, int filtro)
    {
        List<object[]> result = new List<object[]>();

        try
        {
            Dictionary<int, DateTime> pd = GPartesDiarios.GetPartesDiarios(idPersona, Convert.ToDateTime(desde), 
                Convert.ToDateTime(hasta), (TiposLicencia)filtro);

            foreach (int idParteDiario in pd.Keys)
            {
                result.Add(new object[] { idParteDiario, pd[idParteDiario] });
            }
        }
        catch
        {

        }

        return result.ToArray();
    }
}