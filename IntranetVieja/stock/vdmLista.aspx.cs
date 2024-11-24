using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_vdmLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Personas.
        cbFiltroSolicito.DataSource = GPersonal.GetPersonasActivas();
        cbFiltroSolicito.DataTextField = "Nombre";
        cbFiltroSolicito.DataValueField = "ID";
        cbFiltroSolicito.DataBind();

        // Imputaciones.
        cbFiltroImputacion.DataSource = GImputaciones.GetImputacionesActivas();
        cbFiltroImputacion.DataTextField = "Numero";
        cbFiltroImputacion.DataValueField = "ID";
        cbFiltroImputacion.DataBind();

        // Estados.
        cbFiltroEstado.DataSource = GValeDeMateriales.GetEstadosVDM();
        cbFiltroEstado.DataTextField = "Value";
        cbFiltroEstado.DataValueField = "Key";
        cbFiltroEstado.DataBind();
    }
    /// <summary>
    /// Obtiene una lista con las solicitudes.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetSolicitudes(int pagina, int idSolicito, string codigo, int idImputacion, int estado)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (idSolicito != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Solicito, idSolicito));
        }
        if (codigo.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Codigo, codigo));
        }
        if (idImputacion != Constantes.IdImputacionInvalida)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Imputacion, idImputacion));
        }
        if (Enum.IsDefined(typeof(EstadosVDM), estado))
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Estado, estado));
        }

        List<ValeDeMateriales> vdms = GValeDeMateriales.GetValesDeMateriales(pagina, filtros);

        foreach (ValeDeMateriales vdm in vdms)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + vdm.ID),
                vdm.GetNumero(),
                vdm.FechaSolicitud.ToShortDateString(),
                vdm.Solicito.Nombre,
                GValeDeMateriales.EstadoVDM(vdm.Estado, false)
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int idSolicito, string codigo, int idImputacion, int estado)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (idSolicito != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Solicito, idSolicito));
        }
        if (codigo.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Codigo, codigo));
        }
        if (idImputacion != Constantes.IdImputacionInvalida)
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Imputacion, idSolicito));
        }
        if (Enum.IsDefined(typeof(EstadosVDM), estado))
        {
            filtros.Add(new Filtro((int)FiltrosVDM.Estado, estado));
        }

        result = GValeDeMateriales.GetCantidadPaginas(filtros);

        return result;
    }
}