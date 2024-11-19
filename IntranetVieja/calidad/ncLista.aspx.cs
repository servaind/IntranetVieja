using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_ncLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Categorías.
        cbFiltroCategoria.DataSource = GNoConformidades.GetCategoriasNC_C();
        cbFiltroCategoria.DataTextField = "Key";
        cbFiltroCategoria.DataValueField = "Value";
        cbFiltroCategoria.DataBind();

        // Áreas de responsabilidad.
        cbFiltroArea.DataSource = GAreas.GetAreas();
        cbFiltroArea.DataTextField = "Descripcion";
        cbFiltroArea.DataValueField = "ID";
        cbFiltroArea.DataBind();

        // Personas.
        cbFiltroEmitidaPor.DataSource = GPersonal.GetPersonasActivas();
        cbFiltroEmitidaPor.DataTextField = "Nombre";
        cbFiltroEmitidaPor.DataValueField = "ID";
        cbFiltroEmitidaPor.DataBind();

        // Estados.
        cbFiltroEstado.DataSource = GNoConformidades.GetEstadosNC();
        cbFiltroEstado.DataTextField = "Key";
        cbFiltroEstado.DataValueField = "Value";
        cbFiltroEstado.DataBind();
    }

    /// <summary>
    /// Obtiene una lista con las solicitudes.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetNoConformidades(int pagina, int categoria, string asunto, int idArea, int idEmitidaPor, 
        int estado, int numero)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (Enum.IsDefined(typeof(CategoriasNC), categoria))
        {
            filtros.Add(new Filtro((int)FiltrosNC.Categoria, categoria));
        }
        if (asunto.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Asunto, asunto.Trim()));
        }
        if (idArea >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Area, idArea));
        }
        if (idEmitidaPor != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosNC.EmitidaPor, idEmitidaPor));
        }
        if (Enum.IsDefined(typeof(EstadosNC), estado))
        {
            filtros.Add(new Filtro((int)FiltrosNC.Estado, estado));
        }
        if (numero != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Numero, numero));
        }

        List<NoConformidad> ncs = GNoConformidades.GetNoConformidades(pagina, filtros);

        foreach (NoConformidad nc in ncs)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + nc.ID),
                nc.GetNumero(),
                GNoConformidades.GetDescripcionCategoriaC(nc.Categoria),
                nc.Asunto,
                nc.Area != null ? nc.Area.Descripcion : "-",
                nc.EmitidaPor.Nombre,
                nc.FechaEmision.ToShortDateString(),
                nc.Estado == EstadosNC.Cerrada ? nc.FechaCierre.ToShortDateString() : "-",
                nc.GetEstado(),
                nc.Area.Responsables[0].Responsable.Nombre
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int categoria, string asunto, int idArea, int idEmitidaPor, int estado, int numero)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (Enum.IsDefined(typeof(CategoriasNC), categoria))
        {
            filtros.Add(new Filtro((int)FiltrosNC.Categoria, categoria));
        }
        if (asunto.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Asunto, asunto.Trim()));
        }
        if (idArea >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Area, idArea));
        }
        if (idEmitidaPor != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosNC.EmitidaPor, idEmitidaPor));
        }
        if (Enum.IsDefined(typeof(EstadosNC), estado))
        {
            filtros.Add(new Filtro((int)FiltrosNC.Estado, estado));
        }
        if (numero != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosNC.Numero, numero));
        }

        result = GNoConformidades.GetCantidadPaginasNCs(filtros);

        return result;
    }
}