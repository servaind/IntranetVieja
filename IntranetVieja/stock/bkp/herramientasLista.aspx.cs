using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_herramientasLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.HerramientaAdministrador))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
        }

        List<Persona> personas = GPersonal.GetPersonasActivas();

        // Personas para el filtro.
        cbFiltroPersonaCargo.DataSource = personas;
        cbFiltroPersonaCargo.DataTextField = "Nombre";
        cbFiltroPersonaCargo.DataValueField = "ID";
        cbFiltroPersonaCargo.DataBind();

        // Personas para los registros históricos.
        cbRegHistPersCargo.DataSource = personas;
        cbRegHistPersCargo.DataTextField = "Nombre";
        cbRegHistPersCargo.DataValueField = "ID";
        cbRegHistPersCargo.DataBind();
    }

    /// <summary>
    /// Obtiene una lista con las herramientas.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetHerramientas(int pagina, int numeroH, string numeroI, string tipo, string descripcion, int idPersonaCargo)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (numeroH != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroHerramienta, numeroH));
        }
        if (!String.IsNullOrEmpty(numeroI))
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroInstrumento, numeroI));
        }
        if (tipo.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Tipo, tipo.Trim()));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Descripcion, descripcion.Trim()));
        }
        if (idPersonaCargo != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.PersonaCargo, idPersonaCargo));
        }

        List<Herramienta> herramientas = GHerramientas.GetHerramientas(pagina, filtros, true);

        foreach (Herramienta herramienta in herramientas)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + herramienta.ID),
                herramienta.Clasificacion == ClasifHerramienta.Herramienta ? herramienta.NumeroHerramienta.ToString() : "",
                herramienta.Clasificacion == ClasifHerramienta.Instrumento ? herramienta.NumeroHerramienta.ToString() : "",
                herramienta.TipoHerramienta.Descripcion,
                herramienta.Descripcion,
                herramienta.PersonaACargo.Nombre,
                Encriptacion.GetParametroEncriptado("path=" + herramienta.NumeroHerramienta + "\\" + herramienta.NumeroHerramienta + ".jpg&idPath=" + (int)PathImage.ListadoInstrumentos),
                herramienta.Clasificacion == ClasifHerramienta.Instrumento && HayManualInstrumento(herramienta.NumeroHerramienta.ToString()) ? 
                    Encriptacion.GetParametroEncriptado("f=" + herramienta.NumeroHerramienta + "\\" + herramienta.NumeroHerramienta + ".pdf&n=Manual de instrumento Nº " + herramienta.NumeroHerramienta + ".pdf&idPath=" + (int)PathImage.ListadoInstrumentos)
                    : "",
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene si hay un manual de instrumento para el instrumento.
    /// </summary>
    /// <returns></returns>
    private static bool HayManualInstrumento(string numeroInst)
    {
        bool result = false;
        string path = @"\\10.0.0.4\Usuarios\Liliana.villamil\SGI MULTISITIO\03 Listas de control\BUENOS AIRES\08 LC - Control de los dispositivos de medición y seguimiento\FORM PG 7.6-01-01 Lista de control de Equipos\EQUIPOS SGI\" + numeroInst + "\\" + numeroInst + ".pdf";

        result = File.Exists(path);

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int numeroH, string numeroI, string tipo, string descripcion, int idPersonaCargo)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (numeroH != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroHerramienta, numeroH));
        }
        if (!String.IsNullOrEmpty(numeroI))
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroInstrumento, numeroI));
        }
        if (tipo.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Tipo, tipo.Trim()));
        }
        if (descripcion.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.Descripcion, descripcion.Trim()));
        }
        if (idPersonaCargo != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.PersonaCargo, idPersonaCargo));
        }

        result = GHerramientas.GetCantidadPaginasHerramientas(filtros);
        
        return result;
    }
    /// <summary>
    /// Obtiene los registros históricos de la herramienta.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetHerramientasRegHistLista(int pagina, int idHerramienta)
    {
        Herramienta herramienta = GHerramientas.GetHerramienta(idHerramienta);
        if (herramienta == null)
        {
            throw new Exception("No se ha podido encontrar la herramienta solicitada.");
        }

        List<object[]> result = new List<object[]>();

        // En la primer fila envío el detalle de la herramienta.
        result.Add(new object[] { herramienta.ID.ToString("[0000]") + " " + herramienta.ToString() });

        List<RegHistHerramienta> registros = GHerramientas.GetRegHistHerramienta(pagina, idHerramienta);

        foreach (RegHistHerramienta historico in registros)
        {
            result.Add(new object[] { 
                historico.Fecha.ToShortDateString(), 
                historico.PersonaACargo.Nombre,
                historico.Ubicacion, 
                historico.Descripcion});
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas de registros históricos disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginasRegHist(int idHerramienta)
    {
        int result;

        result = GHerramientas.GetCantidadPaginasRegHistHerramienta(idHerramienta);

        return result;
    }
    /// <summary>
    /// Crea un nuevo registro histórico para la herramienta.
    /// </summary>
    [WebMethod()]
    public static void NuevoRegHist(int idHerramienta, int idPersonaCargo, string ubicacion, string descripcion)
    {
        if (idHerramienta == Constantes.ValorInvalido || idPersonaCargo == Constantes.IdPersonaInvalido ||
            ubicacion == null || ubicacion.Trim().Length == 0 || descripcion == null || descripcion.Trim().Length == 0)
        {
            throw new Exception("Todos los campos son obligatorios y deben ser completados.");
        }

        if (!GHerramientas.NuevoRegHistHerramienta(idHerramienta, DateTime.Now, idPersonaCargo, ubicacion, descripcion,
            Constantes.Usuario.ID))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en la herramienta. Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Elimina una herramienta.
    /// </summary>
    [WebMethod()]
    public static void EliminarHerramienta(int idHerramienta, string motivo)
    {
        if (motivo == null || motivo.Trim().Length == 0)
        {
            throw new Exception("Debe ingresar un motivo para los cambios realizados.");
        }

        if (!GHerramientas.CambiarEstadoHerramienta(idHerramienta, false, motivo))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en la herramienta. Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Obtiene los eventos de la herramienta.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetHerramientasEventosLista(int pagina, int idHerramienta)
    {
        Herramienta herramienta = GHerramientas.GetHerramienta(idHerramienta);
        if (herramienta == null)
        {
            throw new Exception("No se ha podido encontrar la herramienta solicitada.");
        }

        List<object[]> result = new List<object[]>();

        // En la primer fila envío el detalle de la herramienta.
        result.Add(new object[] { herramienta.ID.ToString("[0000]") + " " + herramienta.ToString() });

        List<EventoHerramienta> eventos = GHerramientas.GetEventosHerramienta(pagina, idHerramienta);

        foreach (EventoHerramienta evento in eventos)
        {
            result.Add(new object[] { 
                evento.ID,
                evento.Fecha.ToShortDateString(), 
                evento.Descripcion 
            });
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas de eventos disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginasEventos(int idHerramienta)
    {
        int result;

        result = GHerramientas.GetCantidadPaginasEventosHerramienta(idHerramienta);

        return result;
    }
    /// <summary>
    /// Obtiene un evento.
    /// </summary>
    [WebMethod()]
    public static object[] GetEvento(int idEvento)
    {
        EventoHerramienta evento = GHerramientas.GetEventoHerramienta(idEvento);
        if (evento == null)
        {
            throw new Exception("No se ha podido encontrar el evento seleccionado.");
        }

        object[] result;

        result = new object[] { evento.Fecha.ToShortDateString(), evento.Descripcion };

        return result;
    }
    /// <summary>
    /// Crea un nuevo evento para la herramienta.
    /// </summary>
    [WebMethod()]
    public static void NuevoEvento(int idHerramienta, string fecha, string descripcion)
    {
        if (idHerramienta == Constantes.ValorInvalido || fecha == null || fecha.Trim().Length == 0 ||
            descripcion == null || descripcion.Trim().Length == 0)
        {
            throw new Exception("Todos los campos son obligatorios y deben ser completados.");
        }

        DateTime fechaEvento;
        if (!DateTime.TryParse(fecha, out fechaEvento))
        {
            throw new Exception("La fecha ingresada no es válida.");
        }

        if (fechaEvento <= DateTime.Now)
        {
            throw new Exception("La fecha ingresada debe ser mayor a la fecha actual.");
        }

        if (!GHerramientas.NuevoEventoHerramienta(idHerramienta, descripcion, fechaEvento))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en la herramienta. Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Elimina un evento.
    /// </summary>
    [WebMethod()]
    public static void EliminarEvento(int idEvento)
    {
        if (!GHerramientas.EliminarEventoHerramienta(idEvento))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en la herramienta. Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }
    }
    /// <summary>
    /// Actualiza un evento.
    /// </summary>
    [WebMethod()]
    public static void ActualizarEvento(int idHerramienta, string fecha, string descripcion, int idEvento)
    {
        if (idHerramienta == Constantes.ValorInvalido || fecha == null || fecha.Trim().Length == 0 ||
            descripcion == null || descripcion.Trim().Length == 0)
        {
            throw new Exception("Todos los campos son obligatorios y deben ser completados.");
        }

        DateTime fechaEvento;
        if (!DateTime.TryParse(fecha, out fechaEvento))
        {
            throw new Exception("La fecha ingresada no es válida.");
        }

        if (fechaEvento <= DateTime.Now)
        {
            throw new Exception("La fecha ingresada debe ser mayor a la fecha actual.");
        }

        if (!GHerramientas.ActualizarEventoHerramienta(idEvento, descripcion, fechaEvento))
        {
            string msg = "Se produjo un error al intentar guardar los cambios en la herramienta. Verifique que los datos "
                       + "ingresados sean válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese "
                       + "con el área de sistemas.";

            throw new Exception(msg);
        }
    }
}