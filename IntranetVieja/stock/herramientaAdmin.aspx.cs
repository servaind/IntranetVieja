using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_herramientaAdmin : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "herramientasAdmin.aspx.";

    // Variables.

    // Propiedades.
    public static int IDHerramienta
    {
        get
        {
            return (int)GSessions.GetSession(PrefSession + "idHerramientaActiva");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "idHerramientaActiva", value);
        }
    }
    private static List<ItemHerramienta> ItemsHerramienta
    {
        get
        {
            return (List<ItemHerramienta>)GSessions.GetSession(PrefSession + "lstItemsHerramienta");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "lstItemsHerramienta", value);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.HerramientaAdministrador))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
        }

        if (!Page.IsPostBack)
        {
            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idHerramienta;
            if (!parametros.ContainsKey("id") || !Int32.TryParse(parametros["id"], out idHerramienta))
            {
                idHerramienta = Constantes.ValorInvalido;
            }

            if (!CrearSessions())
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }

            IDHerramienta = idHerramienta;

            CargarTiposHerramientas();
            CargarPersonal();
            CargarClasifHerramientas();

            if (idHerramienta != Constantes.ValorInvalido)
            {
                MostrarHerramienta(idHerramienta);
            }
        }

        if (!SessionsValidas())
        {
            Response.Redirect("herramientasAdmin.aspx?id=" + Request.QueryString["id"]);
            return;
        }
    }
    /// <summary>
    /// Crea las sesiones necesarias.
    /// </summary>
    private bool CrearSessions()
    {
        bool result;

        result = GSessions.CrearSession(PrefSession + "idHerramientaActiva", Constantes.ValorInvalido) &&
                 GSessions.CrearSession(PrefSession + "lstItemsHerramienta", new List<ItemHerramienta>());

        return result;
    }
    /// <summary>
    /// Verifica si las sesiones son válidas.
    /// </summary>
    private bool SessionsValidas()
    {
        bool result = true;

        result = GSessions.GetSession(PrefSession + "idHerramientaActiva") != null &&
                 GSessions.GetSession(PrefSession + "lstItemsHerramienta") != null;

        return result;
    }
    /// <summary>
    /// Carga los tipos de clasificación de herramientas disponibles.
    /// </summary>
    private void CargarClasifHerramientas()
    {
        cbClasifHerramienta.DataSource = GHerramientas.GetClasificacionHerramienta();
        cbClasifHerramienta.DataTextField = "Value";
        cbClasifHerramienta.DataValueField = "Key";
        cbClasifHerramienta.DataBind();
    }
    /// <summary>
    /// Carga los tipos de herramientas disponibles.
    /// </summary>
    private void CargarTiposHerramientas()
    {
        List<TipoHerramienta> lstTiposHerramientas = GHerramientas.GetTiposHerramientas();

        cbTipoHerramienta.DataSource = lstTiposHerramientas;
        cbTipoHerramienta.DataTextField = "Descripcion";
        cbTipoHerramienta.DataValueField = "ID";
        cbTipoHerramienta.DataBind();
    }
    /// <summary>
    /// Carga el personal disponible.
    /// </summary>
    private void CargarPersonal()
    {
        List<Persona> lstPersonal = GPersonal.GetPersonasActivas();

        cbPersonaCargo.DataSource = lstPersonal;
        cbPersonaCargo.DataTextField = "Nombre";
        cbPersonaCargo.DataValueField = "ID";
        cbPersonaCargo.DataBind();
    }
    /// <summary>
    /// Carga la Herramienta.
    /// </summary>
    private void CargarDatosHerramienta(int idHerramienta)
    {
        Herramienta herramienta = GHerramientas.GetHerramienta(idHerramienta);

        if (herramienta != null)
        {
            txtNumero.Value = herramienta.Clasificacion == ClasifHerramienta.Herramienta ? "" : herramienta.NumeroHerramienta.ToString();
            cbClasifHerramienta.Value = ((int)herramienta.Clasificacion).ToString();
            lblTipoHerramienta.InnerText = herramienta.TipoHerramienta.Descripcion;
            cbTipoHerramienta.Value = herramienta.TipoHerramienta.ID.ToString();
            txtMarca.Value = herramienta.Marca;
            txtDescripcion.Value = herramienta.Descripcion;
            txtNumSerie.Value = herramienta.NumeroSerie;

            herramienta.CargarRegHistoricos();
            if (herramienta.Historicos.Count == 0)
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }
            lblPersonaCargo.InnerText = herramienta.Historicos[0].PersonaACargo.Nombre;
            cbPersonaCargo.Value = herramienta.Historicos[0].PersonaACargo.ID.ToString();
            txtUbicacion.Value = herramienta.Historicos[0].Ubicacion;
            txtNumEAC.Value = herramienta.NumeroEAC.ToString();

            herramienta.CargarItems();

            ItemsHerramienta = herramienta.Items;
        }
        else
        {
            Response.Redirect(Constantes.UrlIntraDefault);
        }
    }
    /// <summary>
    /// Carga los datos de la herramienta.
    /// </summary>
    private void MostrarHerramienta(int idHerramienta)
    {
        lblTitulo.InnerText = "Editar";

        // Parche para Liliana Villamil - 01/03/2012 para que pueda editar las herramientas.
        //txtNumero.Disabled = true;
        //cbClasifHerramienta.Attributes["disabled"] = "disabled";
        //cbTipoHerramienta.Attributes["disabled"] = "disabled";
        //txtMarca.Disabled = true;
        //txtDescripcion.Disabled = true;
        //txtNumSerie.Disabled = true;
        cbPersonaCargo.Attributes["disabled"] = "disabled";
        txtUbicacion.Disabled = true;
        //txtNumEAC.Disabled = true;

        CargarDatosHerramienta(idHerramienta);
    }
    /// <summary>
    /// Guarda una herramienta.
    /// </summary>
    [WebMethod()]
    public static string GuardarHerramienta(int idTipo, string marca, string descripcion, string numSerie, int idPersonaCargo,
        string ubicacion, int numEAC, int clasificacion, string numeroInst)
    {
        if (descripcion == null || descripcion.Trim().Length == 0 || idPersonaCargo == Constantes.IdPersonaInvalido
            || !Enum.IsDefined(typeof(ClasifHerramienta), clasificacion))
        {
            throw new Exception("Los campos marcados con * son obligatorios y deben ser completados.");
        }

        string result;
        int idHerramienta;

        if (!GHerramientas.NuevaHerramienta(idTipo, marca, descripcion, numSerie, idPersonaCargo, ubicacion, numEAC,
            (ClasifHerramienta)clasificacion, numeroInst, ItemsHerramienta, Constantes.Usuario.ID, out idHerramienta))
        {
            string msg = "Se produjo un error al intentar agregar " + ((ClasifHerramienta)clasificacion == 
                ClasifHerramienta.Herramienta ? "la herramienta" : "el instrumento") + ". Verifique que los datos ingresados sean "
                       + "válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese con el área de sistemas.";

            throw new Exception(msg);
        }

        result = ((ClasifHerramienta)clasificacion == ClasifHerramienta.Herramienta ? "La herramienta" : "El instrumento") + " se guardó correctamente en el sistema con el ID <strong>" + idHerramienta + "</strong>.";

        return result;
    }
    /// <summary>
    /// Guarda una herramienta.
    /// </summary>
    [WebMethod()]
    public static string ActualizarHerramienta(int idTipo, string marca, string descripcion, string numSerie, int numEAC, 
        ClasifHerramienta clasificacion, string numeroInst, string motivo)
    {
        if (motivo == null || motivo.Trim().Length == 0)
        {
            throw new Exception("Debe ingresar un motivo para los cambios realizados.");
        }

        string result;
        if (!GHerramientas.ActualizarHerramienta(IDHerramienta, idTipo, marca, descripcion, numSerie, numEAC, 
            (ClasifHerramienta)clasificacion, numeroInst, ItemsHerramienta, motivo))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en " + ((ClasifHerramienta)clasificacion ==
                ClasifHerramienta.Herramienta ? "la herramienta" : "el instrumento") + ". Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }

        result = "Los cambios realizados " + ((ClasifHerramienta)clasificacion == ClasifHerramienta.Herramienta ? 
            "a la herramienta" : "al instrumento") + " se guardaron correctamente.";

        return result;
    }
    /// <summary>
    /// Agrega un ítem a la herramienta.
    /// </summary>
    [WebMethod()]
    public static void AgregarItem(string marca, string descripcion, int cantidad)
    {
        if (descripcion == null || descripcion.Trim().Length == 0)
        {
            throw new Exception("Los campos marcados con * son obligatorios y deben ser completados.");
        }

        if (cantidad <= 0)
        {
            throw new Exception("Debe ingresar una cantidad válida.");
        }

        // Si el ítem ya existe, lo actualizo.
        bool existe = false;
        foreach (ItemHerramienta i in ItemsHerramienta)
        {
            if (i.Marca == marca && i.Descripcion == descripcion)
            {
                i.Cantidad += cantidad;
                existe = true;
                break;
            }
        }

        if (!existe)
        {
            ItemHerramienta item = new ItemHerramienta(IDHerramienta, marca, descripcion, cantidad);
            ItemsHerramienta.Add(item);
        }
    }
    /// <summary>
    /// Actualiza un ítem de la herramienta.
    /// </summary>
    [WebMethod()]
    public static void ActualizarItem(int posicion, string marca, string descripcion, int cantidad)
    {
        if (posicion < 0 || posicion >= ItemsHerramienta.Count)
        {
            throw new Exception("El ítem seleccionado no es válido.");
        }

        if (descripcion.Trim().Length == 0)
        {
            throw new Exception("No se ha ingresado ninguna descripción.");
        }

        if (cantidad <= 0)
        {
            throw new Exception("Debe ingresar una cantidad válida.");
        }

        ItemsHerramienta[posicion].Marca = marca;
        ItemsHerramienta[posicion].Descripcion = descripcion;
        ItemsHerramienta[posicion].Cantidad = cantidad;
    }
    /// <summary>
    /// Obtiene los ítems disponibles.
    /// </summary>
    [WebMethod()]
    public static object[][] GetItems()
    {
        object[][] result = new object[ItemsHerramienta.Count][];

        for (int i = 0; i < ItemsHerramienta.Count; i++)
        {
            ItemHerramienta item = ItemsHerramienta[i];

            result[i] = new object[] { item.Marca, item.Descripcion, item.Cantidad };
        }

        return result;
    }
    /// <summary>
    /// Obtiene un ítem de la herramienta.
    /// </summary>
    [WebMethod()]
    public static object[] GetItem(int posicion)
    {
        if (posicion < 0 || posicion >= ItemsHerramienta.Count)
        {
            throw new Exception("El ítem seleccionado no es válido.");
        }

        object[] result;

        ItemHerramienta item = ItemsHerramienta[posicion];
        result = new object[] { item.Marca, item.Descripcion, item.Cantidad };

        return result;
    }
    /// <summary>
    /// Elimina un ítem de la herramienta.
    /// </summary>
    [WebMethod()]
    public static void EliminarItem(int posicion)
    {
        if (posicion < 0 || posicion >= ItemsHerramienta.Count)
        {
            throw new Exception("El ítem seleccionado no es válido.");
        }

        ItemsHerramienta.RemoveAt(posicion);
    }
}