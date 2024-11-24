using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sistemas_imputacionesLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.Administrador))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        Dictionary<int, string> estados = GImputaciones.GetEstadosImputacion();

        // Estados.
        cbFiltroEstado.DataSource = estados;
        cbFiltroEstado.DataTextField = "Value";
        cbFiltroEstado.DataValueField = "Key";
        cbFiltroEstado.DataBind();

        cbImputacionEstado.DataSource = estados;
        cbImputacionEstado.DataTextField = "Value";
        cbImputacionEstado.DataValueField = "Key";
        cbImputacionEstado.DataBind();
    }

    /// <summary>
    /// Obtiene una lista con las imputaciones.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetImputaciones(int pagina, int numero, string descripcion, int estado)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (numero >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Numero, numero));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Descripcion, descripcion));
        }
        if (Enum.IsDefined(typeof(EstadosImputacion), estado))
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Estado, estado));
        }

        List<Imputacion> imputaciones = GImputaciones.GetImputaciones(pagina, filtros);

        foreach (Imputacion imputacion in imputaciones)
        {
            object[] fila = new object[] { 
                imputacion.ID,
                imputacion.Numero,
                imputacion.Descripcion,
                imputacion.Activa ? 1 : 0
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int numero, string descripcion, int estado)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (numero >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Numero, numero));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Descripcion, descripcion));
        }
        if (Enum.IsDefined(typeof(EstadosImputacion), estado))
        {
            filtros.Add(new Filtro((int)FiltrosImputacion.Estado, estado));
        }

        result = GImputaciones.GetCantidadPaginas(filtros);

        return result;
    }
    /// <summary>
    /// Da de alta una nueva imputación.
    /// </summary>
    [WebMethod()]
    public static void NuevaImputacion(int numero, string descripcion, int estado)
    {
        try
        {
            GImputaciones.NuevaImputacion(numero, descripcion, estado == 1);
        }
        catch(Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Da de alta una nueva imputación.
    /// </summary>
    [WebMethod()]
    public static void EditarImputacion(int idImputacion, int numero, string descripcion, int estado)
    {
        try
        {
            GImputaciones.ActualizarImputacion(idImputacion, numero, descripcion, estado == 1);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Obtiene una imputación.
    /// </summary>
    [WebMethod()]
    public static object[] GetImputacion(int idImputacion)
    {
        List<object> result = new List<object>();

        Imputacion imputacion = GImputaciones.GetImputacion(idImputacion);
        if (imputacion != null)
        {
            result.Add(imputacion.Numero);
            result.Add(imputacion.Descripcion);
            result.Add(imputacion.Activa);
        }

        return result.ToArray();
    }
}