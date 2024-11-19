using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class administracion_vehiculosVencimientos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (String.IsNullOrWhiteSpace(Request["old"]))
		{
			Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
		}
    }
    /// <summary>
    /// Obtiene los vencimientos.
    /// </summary>
    [WebMethod()]
    public static object[] GetVencimientos(int mes, int anio)
    {
        List<object[]> result = new List<object[]>();

        Dictionary<string, List<ItemVencimiento>> vencimientos = Vehiculos.GetVencimientosMes(mes, anio);
        foreach (string documento in vencimientos.Keys)
        {
            List<object> v = new List<object>();
            v.Add(documento);
            foreach (ItemVencimiento item in vencimientos[documento])
            {
                object[] i = new object[] { item.Fecha.ToShortDateString(), item.Patente };
                v.Add(i);
            }

            result.Add(v.ToArray());
        }

        return result.ToArray();
    }
    /// <summary>
    /// Exporta los vencimientos.
    /// </summary>
    [WebMethod()]
    public static string ExportarVencimientos(int mes, int anio)
    {
        string result;

        try
        {
            string path = Vehiculos.ExportarVencimientos(mes, anio);
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + path + "&n=Vencimientos_" + mes + "_" + anio + ".xls&d=1");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result;
    }
}