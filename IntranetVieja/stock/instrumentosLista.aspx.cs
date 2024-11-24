using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_instrumentosLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		if (String.IsNullOrWhiteSpace(Request["old"]))
		{
			Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
		}
			
        cbGrupo.DataSource = Instrumentos.GetGruposInstrumentosDS();
        cbGrupo.DataTextField = "Value";
        cbGrupo.DataValueField = "Key";
        cbGrupo.DataBind();

        cbResponsable.DataSource = GPersonal.GetPersonasActivas();
        cbResponsable.DataTextField = "Nombre";
        cbResponsable.DataValueField = "ID";
        cbResponsable.DataBind();
    }
    /// <summary>
    /// Obtiene los instrumentos.
    /// </summary>
    [WebMethod()]
    public static object GetInstrumentos(int pagina, string query)
    {
        List<object> lista = new List<object>();
        List<Filtro> filtros = new List<Filtro>();

        if (!String.IsNullOrEmpty(query)) filtros.Add(new Filtro((int)FiltroInstrumento.Todo, query));

        List<Instrumento> instrumentos = Instrumentos.GetInstrumentos(pagina, filtros);

        instrumentos.ForEach(i => lista.Add(new
            {
                ID = Encriptacion.GetParametroEncriptado("id=" + i.ID),
                i.Numero,
                i.Tipo,
                i.Descripcion,
                i.Marca,
                i.Modelo,
                i.NumeroSerie,
                i.Rango,
                i.Resolucion,
                i.Clase,
                i.Incertidumbre,
                i.UltRegistro.Grupo,
                i.UltRegistro.Ubicacion,
                i.UltRegistro.Responsable,
                CalibUlt = i.UltCalibracion.Fecha.ToShortDateString(),
                CalibProx = i.ProxCalibracion.ToShortDateString(),
                i.CalibProxAVencer,
                i.CalibVencida,
                CalibFrec = Instrumentos.GetFrecuenciaCalibracion(i.CalibFrec),
                PathImagen = Encriptacion.GetParametroEncriptado("path=" + Instrumentos .GetPathImagenInstrumento(i.Numero) +
                                                                                     "\\0.jpg&idPath=" +
                                                                                     (int) PathImage.ListadoInstrumentos),
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
                HasManuales = Instrumentos.HasManualesInstrumento(i.Numero),
                
                //German
                MtoFrec = Instrumentos.GetFrecuenciaMantenimiento(i.MtoFrec),
                ComprobFrec = Instrumentos.GetFrecuenciaComprobacion(i.ComprobFrec),
                FechaComprobacion = i.FechaComprobacion.ToShortDateString(),
                FechaMantenimiento = i.FechaMantenimiento.ToShortDateString(),
                ProxMantenimiento = i.ProxMantenimiento.ToShortDateString(),
                ProxComprobacion = i.ProxComprobacion.ToShortDateString(),
                ProxCalibracion = i.ProxCalibracion.ToShortDateString()    
            }));
        
        int totalPaginas = Instrumentos.GetInstrumentosPaginas(filtros);
        if (totalPaginas == 0) totalPaginas = 1;

        return new { Lista = lista, TotalPaginas = totalPaginas };
    }
    /// <summary>
    /// Actualiza la fecha de última calibración.
    /// </summary>
    [WebMethod()]
    public static void UpdateCalibInstrumento(string idInstrumento, string fecha)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idInstrumento)["id"]);

            Instrumentos.AddRegistroCalibracion(id, Convert.ToDateTime(fecha));
        }
        catch
        {
            throw new Exception("Se produjo un error al procesar la operación. Verifique que los datos ingresados sean "
                + "válidos e intente nuevamente.");
        }
    }
    /// <summary>
    /// Obtiene las imágenes para un instrumento.
    /// </summary>
    [WebMethod()]
    public static string[] GetInstrumentoImagenes(int numero)
    {
        List<string> result = new List<string>();

        List<string> images = Instrumentos.GetImagenesInstrumento(numero);
        images.ForEach(i => result.Add(Encriptacion.GetParametroEncriptado("path=" + i + "&idPath=" +
                (int)PathImage.ListadoInstrumentos)));

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene los manuales para un instrumento.
    /// </summary>
    [WebMethod()]
    public static object[][] GetInstrumentoManuales(int numero)
    {
        List<object[]> result = new List<object[]>();

        List<FileInfo> manuales = Instrumentos.GetManualesInstrumento(numero);
        manuales.ForEach(m => result.Add(new object[] { 
            Encriptacion.GetParametroEncriptado("f=" + m.FullName + "&idPath=" + (int)PathImage.ListadoInstrumentos +
                "&n=" + m.Name), 
                m.Name,
                GRepositorioArchivos.GetDescripcionTipoArchivo(m.Extension.Replace(".", "")), 
                Funciones.GetFileSize(m.Length) }));

        return result.ToArray();
    }

    [WebMethod()]
    public static object UpdateRegistroInstrumento(string idInstrumento, int idGrupo, string ubicacion,
        int idResponsable)
    {
        bool result = true;
        string message = String.Empty;

        try
        {
            int id = Convert.ToInt32(Encriptacion.GetParametrosURL(idInstrumento)["id"]);

            Instrumentos.AddRegistroInstrumento(id, idGrupo, ubicacion, idResponsable, DateTime.Now);
        }
        catch
        {
            result = false;
            message = "Se produjo un error al procesar la operación. Verifique que los datos ingresados sean válidos " +
                      "e intente nuevamente.";
        }

        return new { Success = result, Message = message };
    }
}