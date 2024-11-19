using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_autorizLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.AutorizAdministrar))
        {
            Response.Redirect(Constantes.UrlIntraDefault, true);
            return;
        }

        // Personas.
        cbFiltroSolicito.DataSource = GPersonal.GetPersonasActivas();
        cbFiltroSolicito.DataTextField = "Nombre";
        cbFiltroSolicito.DataValueField = "ID";
        cbFiltroSolicito.DataBind();

        // Estados.
        cbFiltroEstado.DataSource = Autorizaciones.GetEstadosSolicitudes();
        cbFiltroEstado.DataTextField = "Value";
        cbFiltroEstado.DataValueField = "Key";
        cbFiltroEstado.DataBind();
    }
    /// <summary>
    /// Obtiene una lista con las solicitudes.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetSolicitudes(int pagina, int idSolicito, int estado)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (idSolicito != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosAutorizacion.Solicito, idSolicito));
        }
        if (Enum.IsDefined(typeof(EstadosCodArt), estado))
        {
            filtros.Add(new Filtro((int)FiltrosAutorizacion.Estado, estado));
        }
        filtros.Add(new Filtro((int)FiltrosAutorizacion.Responsable, Constantes.Usuario.ID));

        List<Autorizacion> autorizaciones = Autorizaciones.GetAutorizaciones(pagina, filtros);

        foreach (Autorizacion au in autorizaciones)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + au.IdAutorizacion),
                au.IdAutorizacion,
                au.FechaSolicito.ToShortDateString(),
                au.Solicito.Nombre,
                Autorizaciones.GetReferencia(au.Referencia),
                au.Estado.ToString(),
                GetColorEstado(au.Estado)
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene el color para el estado.
    /// </summary>
    private static string GetColorEstado(EstadoAutorizacion estado)
    {
        switch (estado)
        {
            case EstadoAutorizacion.Pendiente:
                return "yellow";
            case EstadoAutorizacion.Rechazada:
                return "red";
            case EstadoAutorizacion.Aprobada:
                return "green";
            default:
                return "red";
        }
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int idSolicito, int estado)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (idSolicito != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosAutorizacion.Solicito, idSolicito));
        }
        if (Enum.IsDefined(typeof(EstadosCodArt), estado))
        {
            filtros.Add(new Filtro((int)FiltrosAutorizacion.Estado, estado));
        }
        filtros.Add(new Filtro((int)FiltrosAutorizacion.Responsable, Constantes.Usuario.ID));

        result = Autorizaciones.GetCantidadPaginas(filtros);

        return result;
    }
}