using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_informacionObraLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Persona> personas = GPersonal.GetPersonasActivas();

        // Responsables de obra.
        cbFiltroResponsableObra.DataSource = personas;
        cbFiltroResponsableObra.DataTextField = "Nombre";
        cbFiltroResponsableObra.DataValueField = "ID";
        cbFiltroResponsableObra.DataBind();

        // Informates.
        cbFiltroInformante.DataSource = personas;
        cbFiltroInformante.DataTextField = "Nombre";
        cbFiltroInformante.DataValueField = "ID";
        cbFiltroInformante.DataBind();
    }
    /// <summary>
    /// Obtiene una lista con las informaciones de obras.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetIOs(int pagina, int numero, string cliente, int idResponsableObra, int idInformante,
        string ordenCompra, string imputacion)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (numero != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.NumObra, numero));
        }
        if (cliente.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Cliente, cliente));
        }
        if (idResponsableObra != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Responsable, idResponsableObra));
        }
        if (idInformante != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Informante, idInformante));
        }
        if (!String.IsNullOrEmpty(ordenCompra))
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.OrdenCompra, ordenCompra));
        }
        if (imputacion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Imputacion, imputacion));
        }

        List<object[]> ios = InformacionObras.GetInformacionesObras(pagina, filtros);

        foreach (object[] io in ios)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + io[0]), 
                io[0],
                io[1],
                io[2],
                io[3],
                io[4],
                Funciones.LimitarTexto(io[5].ToString(), 5, "...")
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int numero, string cliente, int idResponsableObra, int idInformante,
        string ordenCompra, string imputacion)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (numero != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.NumObra, numero));
        }
        if (cliente.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Cliente, cliente));
        }
        if (idResponsableObra != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Responsable, idResponsableObra));
        }
        if (idInformante != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Informante, idInformante));
        }
        if (!String.IsNullOrEmpty(ordenCompra))
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.OrdenCompra, ordenCompra));
        }
        if (imputacion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosInformeObra.Imputacion, imputacion));
        }

        result = InformacionObras.GetCantidadPaginas(filtros);

        return result;
    }
}