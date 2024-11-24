using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_herramientasSeguimiento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.InstrumentoSeguimiento))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
        }

        // Frecuencias de herramientas.
        cbFrecuencia.DataSource = GHerramientas.GetFrecuenciasCalibracion();
        cbFrecuencia.DataTextField = "Value";
        cbFrecuencia.DataValueField = "Key";
        cbFrecuencia.DataBind();

        // Tipos de herramientas.
        cbFiltroTipo.DataSource = GHerramientas.GetTiposHerramientas();
        cbFiltroTipo.DataTextField = "Descripcion";
        cbFiltroTipo.DataValueField = "ID";
        cbFiltroTipo.DataBind();

        // Tipos de calibración.
        cbTipoCalibracion.DataSource = GHerramientas.GetTiposCalibracion();
        cbTipoCalibracion.DataTextField = "Value";
        cbTipoCalibracion.DataValueField = "Key";
        cbTipoCalibracion.DataBind();

        // Herramientas.
        cbEquipo.DataSource = GHerramientas.GetHerramientas();
        cbEquipo.DataBind();
    }
    /// <summary>
    /// Obtiene la lista de seguimiento.
    /// </summary>
    [WebMethod()]
    public static object[][] GetListaSeguimiento(int pagina, int numeroI, int tipo, string descripcion, string marca)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (numeroI != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroInstrumento, numeroI));
        }
        if (tipo >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Tipo, tipo));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Descripcion, descripcion.Trim()));
        }
        if (marca.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Marca, marca.Trim()));
        }

        List<object[]> filas = GHerramientas.GetCalibracionesHerramientas(pagina, filtros);

        foreach (object[] fila in filas)
        {
            result.Add(new object[] { Encriptacion.GetParametroEncriptado(fila[0].ToString()), fila[0], fila[1], fila[2], fila[3], fila[4], 
                GHerramientas.GetFrecuenciaCalibracion(Convert.ToInt32(fila[5])), fila[6], fila[7], fila[8],
                GHerramientas.GetTipoCalibracion(Convert.ToInt32(fila[9]))
            });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int numeroI, int tipo, string descripcion, string marca)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (numeroI != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroInstrumento, numeroI));
        }
        if (tipo >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Tipo, tipo));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Descripcion, descripcion.Trim()));
        }
        if (marca.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Marca, marca.Trim()));
        }

        result = GHerramientas.GetCantidadPaginasCalibraciones(filtros);

        return result;
    }
    /// <summary>
    /// Obtiene una calibración.
    /// </summary>
    [WebMethod()]
    public static object[] GetCalibracion(string id)
    {
        object[] result;

        try
        {
            int idHerramienta = Convert.ToInt32(Encriptacion.Desencriptar(id));
            CalibracionHerramienta calib = GHerramientas.GetCalibracionHerramienta(idHerramienta);

            result = new object[] { idHerramienta, (int)calib.Frecuencia, calib.UltimaCalibracion.ToShortDateString(),
                calib.ProximaCalibracion.ToShortDateString(), (int)calib.TipoCalibracion, calib.Observaciones };
        }
        catch
        {
            throw new Exception("Se produjo un error al intentar procesar la operación.");
        }

        return result;
    }
    /// <summary>
    /// Da de alta una nueva calibración.
    /// </summary>
    [WebMethod()]
    public static void NuevaCalibracion(int equipo, int frecuencia, string ultCalibracion, string proxCalibracion, 
        int tipoCalibracion, string observaciones)
    {
        try
        {
            GHerramientas.NuevaCalibracion(equipo, (FrecCalHerramienta)frecuencia, Convert.ToDateTime(ultCalibracion),
                Convert.ToDateTime(proxCalibracion), (TiposCalHerramienta)tipoCalibracion, observaciones);
        }
        catch
        {
            string msg = "Se produjo un error al intentar agregar la calibración. Verifique que los datos ingresados sean "
                       + "válidos e intente nuevamente. <br />Si el problema persiste, contáctese con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Da de alta una nueva calibración.
    /// </summary>
    [WebMethod()]
    public static void ActualizarCalibracion(string equipo, int frecuencia, string ultCalibracion, 
        string proxCalibracion, int tipoCalibracion, string observaciones)
    {
        try
        {
            GHerramientas.ActualizarCalibracion(Convert.ToInt32(Encriptacion.Desencriptar(equipo)), 
                (FrecCalHerramienta)frecuencia, Convert.ToDateTime(ultCalibracion),
                Convert.ToDateTime(proxCalibracion), (TiposCalHerramienta)tipoCalibracion, observaciones);
        }
        catch
        {
            string msg = "Se produjo un error al intentar actualizar la calibración. Verifique que los datos ingresados sean "
                       + "válidos e intente nuevamente. <br />Si el problema persiste, contáctese con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Descarga 
    /// </summary>
    [WebMethod()]
    public static string GetDescargarCalibracion(string equipo)
    {
        string result;

        try
        {
            int idHerramienta = Convert.ToInt32(Encriptacion.Desencriptar(equipo));
            result = GHerramientas.GetPathDatosHerramienta(idHerramienta);
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + result + "&n=" + idHerramienta + ".zip");
        }
        catch
        {
            throw new Exception("No se ha podido localizar los datos de la herramienta.");
        }

        return result;
    }
}