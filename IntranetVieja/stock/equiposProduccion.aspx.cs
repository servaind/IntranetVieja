using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_equiposProduccion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// Obtiene los equipos en producción.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetEquiposProduccion()
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        List<EquipoProduccion> equipos = ModuloStock.GetEquiposProduccion();
        
        equipos.ForEach(e => result.Add(new object[] { Encriptacion.GetParametroEncriptado("id=" + e.Equipo.ID), 
            e.Equipo.ArticuloAsociado.Codigo, e.Equipo.ArticuloAsociado.Descripcion, e.Cantidad.ToString("0.00") }));

        return result;
    }
    /// <summary>
    /// Realiza un egreso de un equipo.
    /// </summary>
    [WebMethod()]
    public static void EquipoTerminado(string idEquipo, int cantidad, string descripcion)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idEquipo)["id"]);

            ModuloStock.AddEquipoTerminado(id, cantidad);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. Verifique que la cantidad "
                + "ingresada sea menor a la disponible.<br>Detalle: " + ex.Message);
        }
    }
}