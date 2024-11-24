using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_altaArticulosLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Personas.
        cbFiltroSolicito.DataSource = GPersonal.GetPersonasActivas();
        cbFiltroSolicito.DataTextField = "Nombre";
        cbFiltroSolicito.DataValueField = "ID";
        cbFiltroSolicito.DataBind();

        // Estados.
        cbFiltroEstado.DataSource = CodificacionArticulos.GetEstadosSolicitudes();
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
            filtros.Add(new Filtro((int)FiltrosCodArt.Solicito, idSolicito));
        }
        if (Enum.IsDefined(typeof(EstadosCodArt), estado))
        {
            filtros.Add(new Filtro((int)FiltrosCodArt.Estado, estado));
        }

        List<CodificacionArticulo> cas = CodificacionArticulos.GetCodificacionesArticulos(pagina, filtros);

        foreach (CodificacionArticulo ca in cas)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + ca.IdCodificacion),
                ca.GetNumeroReferencia(),
                ca.FechaSolicitud.ToShortDateString(),
                ca.Solicito.Nombre,
                ca.Estado == EstadosCodArt.Aprobado ? ca.CodigoArticulo : "-",
                ca.Estado.ToString(),
                GetColorEstado(ca.Estado)
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene el color para el estado.
    /// </summary>
    private static string GetColorEstado(EstadosCodArt estado)
    {
        switch (estado)
        {
            case EstadosCodArt.Revision:
                return "yellow";
            case EstadosCodArt.Rechazado:
                return "red";
            case EstadosCodArt.Aprobado:
                return "green";
            case EstadosCodArt.NoCorresponde:
                return "blue";
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
            filtros.Add(new Filtro((int)FiltrosCodArt.Solicito, idSolicito));
        }
        if (Enum.IsDefined(typeof(EstadosCodArt), estado))
        {
            filtros.Add(new Filtro((int)FiltrosCodArt.Estado, estado));
        }

        result = CodificacionArticulos.GetCantidadPaginas(filtros);

        return result;
    }
}