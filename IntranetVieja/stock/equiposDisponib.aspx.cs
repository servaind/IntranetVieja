using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_equiposDisponib : System.Web.UI.Page
{
    // Variables.
    private string idEquipo;
    private int cantidad;

    // Properties.
    public string EquipoID
    {
        get { return this.idEquipo; }
    }
    public int Cantidad
    {
        get { return this.cantidad; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<int, string> equipos = ModuloStock.GetEquipos();
        Dictionary<string, string> cEquipos = new Dictionary<string, string>();

        foreach (int idEquipo in equipos.Keys)
        {
            cEquipos.Add(Encriptacion.GetParametroEncriptado("id=" + idEquipo), equipos[idEquipo]);
        }

        cbEquipoDesc.DataSource = cEquipos;
        cbEquipoDesc.DataTextField = "Value";
        cbEquipoDesc.DataValueField = "Key";
        cbEquipoDesc.DataBind();

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        if (parametros.ContainsKey("id"))
        {
            this.idEquipo = parametros["id"];
            if (!(Request["c"] != null && Int32.TryParse(Request["c"], out this.cantidad)))
            {
                this.cantidad = 1;
            }
        }
        else
        {
            this.idEquipo = "";
            this.cantidad = 1;
        }
    }
    /// <summary>
    /// Obtiene si el equipo se puede armar.
    /// </summary>
    [WebMethod()]
    public static object[][] GetArticulos(string idEquipo, int cantidad)
    {
        List<object[]> result = new List<object[]>();

        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idEquipo)["id"]);

            Equipo equipo = ModuloStock.GetEquipo(id);
            if (equipo == null)
            {
                throw new Exception();
            }

            equipo.Articulos.ForEach(a => result.Add(new object[] { a.Articulo.Codigo, a.Articulo.Descripcion, 
                a.Articulo.Cantidad.ToString("0.00"), (a.Cantidad * cantidad).ToString("0.00") }));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result.ToArray();
    }
}