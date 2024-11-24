using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_instrumentosVencimientos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
    }
    /// <summary>
    /// Obtiene los vencimientos.
    /// </summary>
    [WebMethod()]
    public static object GetVencimientos(int mes, int anio)
    {
        List<object> result = new List<object>();

        List<Instrumento> instrumentos = Instrumentos.GetInstrumentosProxVencer(new DateTime(anio, mes, 01, 0, 0, 0));
        instrumentos.ForEach(i => result.Add(new
        {
            ID = Encriptacion.GetParametroEncriptado("id=" + i.ID),
            i.Numero,
            i.Tipo,
            i.Descripcion,
            i.Marca,
            i.Modelo,
            i.NumeroSerie,
            i.Rango,    
            i.UltRegistro.Grupo,
            i.UltRegistro.Ubicacion,
            i.UltRegistro.Responsable,
            CalibUlt = i.UltCalibracion.Fecha.ToShortDateString(),
            CalibProx = i.ProxCalibracion.ToShortDateString(),
            i.CalibProxAVencer,
            i.CalibVencida,
            CalibFrec = Instrumentos.GetFrecuenciaCalibracion(i.CalibFrec),
            PathImagen = Encriptacion.GetParametroEncriptado("path=" + Instrumentos.GetPathImagenInstrumento(i.Numero) +
                                                                                 "\\0.jpg&idPath=" +
                                                                                 (int)PathImage.ListadoInstrumentos),
            HasCertif = Instrumentos.HasCertifInstrumento(i.Numero),
            PathCertif = Encriptacion.GetParametroEncriptado("f=" + Instrumentos.GetPathCertifInstrumento(i.Numero) +
                                                                                 "&idPath=" +
                                                                                 (int)PathImage.ListadoInstrumentos +
                                                                                 "&n=Certificado_de_calibración_" +
                                                                                 i.Numero + ".pdf"),
            HasEAC = Instrumentos.HasEAC(i.Numero),
            PathEAC = Encriptacion.GetParametroEncriptado("f=" + Instrumentos.GetPathEAC(i.Numero) +
                                                                                 "&idPath=" +
                                                                                 (int)PathImage.ListadoInstrumentos +
                                                                                 "&n=EAC_" +
                                                                                 i.Numero + ".pdf"),
            HasManuales = Instrumentos.HasManualesInstrumento(i.Numero)
        }));

        return result.ToArray();
    }
}