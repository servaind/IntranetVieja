using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_altaInstrumento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
	
        cbTipo.DataSource = Instrumentos.GetTiposInstrumentos();
        cbTipo.DataTextField = "Value";
        cbTipo.DataValueField = "Key";
        cbTipo.DataBind();

        cbGrupo.DataSource = Instrumentos.GetGruposInstrumentosDS();
        cbGrupo.DataTextField = "Value";
        cbGrupo.DataValueField = "Key";
        cbGrupo.DataBind();

        cbMarca.DataSource = Instrumentos.GetMarcasInstrumentos();
        cbMarca.DataTextField = "Value";
        cbMarca.DataValueField = "Key";
        cbMarca.DataBind();

        cbResponsable.DataSource = GPersonal.GetPersonasActivas();
        cbResponsable.DataTextField = "Nombre";
        cbResponsable.DataValueField = "ID";
        cbResponsable.DataBind();

        cbFrecuencia.DataSource = Instrumentos.GetFrecuenciasCalibracion();
        cbFrecuencia.DataTextField = "Value";
        cbFrecuencia.DataValueField = "Key";
        cbFrecuencia.DataBind();
    }

    /// <summary>
    /// Agrega un nuevo instrumento.
    /// </summary>
    [WebMethod()]
    public static object AddInstrumento(int numero, int idTipo, string descripcion, int idGrupo, string ubicacion,
        int idMarca, string modelo, string numSerie, string rango, string resolucion, string clase, string incertidumbre,
        int idFrecCalibracion, string ultCalib, int idResponsable, int idFrecMto, int idFrecComprob, DateTime fechaMto, DateTime fechaComprob)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            Instrumentos.AddInstrumento(numero, idTipo, descripcion, idGrupo, ubicacion, idMarca, modelo, numSerie, rango, resolucion, clase, 
                                        incertidumbre, (FrecuenciaCalibracion) idFrecCalibracion, DateTime.Parse(ultCalib), idResponsable,
                                         (FrecuenciaComprobacion)idFrecComprob, (FrecuenciaMantenimiento)idFrecMto, fechaMto, fechaComprob);
        }
        catch
        {
            result = false;
            message = "Se produjo un error al procesar la operación. Verifique que los datos ingresados sean válidos " +
                      "e intente nuevamente.";
        }

        return new {Success = result, Message = message};
    }
}