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
    // Variables.
    private bool mostrarHerramientas;

    // Propiedades.
    public bool MostrarHerramientas
    {
        get { return this.mostrarHerramientas; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        this.mostrarHerramientas = true;

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int tipoListado;
        if (parametros.ContainsKey("t") && Int32.TryParse(parametros["t"], out tipoListado))
        {
            if (Enum.IsDefined(typeof(TipoListadoHerramientas), tipoListado))
            {
                this.mostrarHerramientas = (TipoListadoHerramientas)tipoListado == TipoListadoHerramientas.Herramientas;
            }
        }

        if (this.MostrarHerramientas)
        {
            if (!GPermisosPersonal.TieneAcceso(PermisosPersona.HerramientaAdministrador))
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }
        }
        else
        {
            if (!GPermisosPersonal.TieneAcceso(PermisosPersona.InstrumentoSeguimiento))
            {
                Response.Redirect(Constantes.UrlIntraDefault);
                return;
            }
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

        // Frecuencias de herramientas.
        cbFrecuencia.DataSource = GHerramientas.GetFrecuenciasCalibracion();
        cbFrecuencia.DataTextField = "Value";
        cbFrecuencia.DataValueField = "Key";
        cbFrecuencia.DataBind();

        // Tipos de calibración.
        cbTipoCalibracion.DataSource = GHerramientas.GetTiposCalibracion();
        cbTipoCalibracion.DataTextField = "Value";
        cbTipoCalibracion.DataValueField = "Key";
        cbTipoCalibracion.DataBind();
    }

    /// <summary>
    /// Obtiene una lista con las herramientas.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetHerramientas(int pagina, int numeroH, string tipo, string descripcion, int idPersonaCargo)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (numeroH != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroHerramienta, numeroH));
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
                herramienta.NumeroHerramienta.ToString(),
                herramienta.TipoHerramienta.Descripcion,
                herramienta.Descripcion,
                herramienta.PersonaACargo.Nombre
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
    public static int GetCantidadPaginas(int numeroH, string tipo, string descripcion, int idPersonaCargo)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (numeroH != Constantes.ValorInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.NumeroHerramienta, numeroH));
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
    public static List<object[]> GetHerramientasRegHistLista(int pagina, string idHerramienta)
    {
        Herramienta herramienta = GHerramientas.GetHerramienta(GetIdHerramienta(idHerramienta));
        if (herramienta == null)
        {
            throw new Exception("No se ha podido encontrar la herramienta solicitada.");
        }

        List<object[]> result = new List<object[]>();

        // En la primer fila envío el detalle de la herramienta.
        result.Add(new object[] { herramienta.ID.ToString("[0000]") + " " + herramienta.ToString() });

        List<RegHistHerramienta> registros = GHerramientas.GetRegHistHerramienta(pagina, GetIdHerramienta(idHerramienta));

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
    public static int GetCantidadPaginasRegHist(string idHerramienta)
    {
        int result;

        result = GHerramientas.GetCantidadPaginasRegHistHerramienta(GetIdHerramienta(idHerramienta));

        return result;
    }
    /// <summary>
    /// Crea un nuevo registro histórico para la herramienta.
    /// </summary>
    [WebMethod()]
    public static void NuevoRegHist(string idHerramienta, int idPersonaCargo, string ubicacion, string descripcion)
    {
        if (idPersonaCargo == Constantes.IdPersonaInvalido ||
            ubicacion == null || ubicacion.Trim().Length == 0 || descripcion == null || descripcion.Trim().Length == 0)
        {
            throw new Exception("Todos los campos son obligatorios y deben ser completados.");
        }

        if (!GHerramientas.NuevoRegHistHerramienta(GetIdHerramienta(idHerramienta), DateTime.Now, idPersonaCargo, ubicacion, 
            descripcion, Constantes.Usuario.ID))
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
    public static void EliminarHerramienta(string idHerramienta, string motivo)
    {
        if (motivo == null || motivo.Trim().Length == 0)
        {
            throw new Exception("Debe ingresar un motivo para los cambios realizados.");
        }

        if (!GHerramientas.CambiarEstadoHerramienta(GetIdHerramienta(idHerramienta), false, motivo))
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
    public static List<object[]> GetHerramientasEventosLista(int pagina, string idHerramienta)
    {
        Herramienta herramienta = GHerramientas.GetHerramienta(GetIdHerramienta(idHerramienta));
        if (herramienta == null)
        {
            throw new Exception("No se ha podido encontrar la herramienta solicitada.");
        }

        List<object[]> result = new List<object[]>();

        // En la primer fila envío el detalle de la herramienta.
        result.Add(new object[] { herramienta.ID.ToString("[0000]") + " " + herramienta.ToString() });

        List<EventoHerramienta> eventos = GHerramientas.GetEventosHerramienta(pagina, GetIdHerramienta(idHerramienta));

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
    public static int GetCantidadPaginasEventos(string idHerramienta)
    {
        int result;

        result = GHerramientas.GetCantidadPaginasEventosHerramienta(GetIdHerramienta(idHerramienta));

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
    public static void NuevoEvento(string idHerramienta, string fecha, string descripcion)
    {
        if (fecha == null || fecha.Trim().Length == 0 ||
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

        if (!GHerramientas.NuevoEventoHerramienta(GetIdHerramienta(idHerramienta), descripcion, fechaEvento))
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
    public static void ActualizarEvento(string idHerramienta, string fecha, string descripcion, int idEvento)
    {
        int idH = GetIdHerramienta(idHerramienta);

        if (fecha == null || fecha.Trim().Length == 0 ||
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
    /// <summary>
    /// Obtiene una calibración.
    /// </summary>
    [WebMethod()]
    public static object[] GetCalibracion(string idInstrumento)
    {
        object[] result;

        try
        {
            int id = GetIdHerramienta(idInstrumento);
            CalibracionHerramienta calib = GHerramientas.GetCalibracionHerramienta(id);

            result = new object[] { id, (int)calib.Frecuencia, calib.UltimaCalibracion.ToShortDateString(),
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
    public static void NuevaCalibracion(string idInstrumento, int frecuencia, string ultCalibracion, string proxCalibracion,
        int tipoCalibracion, string observaciones)
    {
        try
        {
            GHerramientas.NuevaCalibracion(GetIdHerramienta(idInstrumento), (FrecCalHerramienta)frecuencia, 
                Convert.ToDateTime(ultCalibracion), Convert.ToDateTime(proxCalibracion), (TiposCalHerramienta)tipoCalibracion, 
                observaciones);
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
    public static void ActualizarCalibracion(string idInstrumento, int frecuencia, string ultCalibracion,
        string proxCalibracion, int tipoCalibracion, string observaciones)
    {
        try
        {
            GHerramientas.ActualizarCalibracion(GetIdHerramienta(idInstrumento),
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
    public static string GetDescargarCalibracion(string idInstrumento)
    {
        string result;

        try
        {
            int idHerramienta = GetIdHerramienta(idInstrumento);
            result = GHerramientas.GetPathDatosHerramienta(idHerramienta);
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + result + "&n=" + idHerramienta + ".zip");
        }
        catch
        {
            throw new Exception("No se ha podido localizar los datos de la herramienta.");
        }

        return result;
    }
    /// <summary>
    /// Obtiene la lista de seguimiento.
    /// </summary>
    [WebMethod()]
    public static object[][] GetInstrumentos(int pagina, string numeroI, int tipo, string descripcion, int idPersonaCargo)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (!String.IsNullOrEmpty(numeroI))
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
        if (idPersonaCargo != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.PersonaCargo, idPersonaCargo));
        }

        List<object[]> filas = GHerramientas.GetCalibracionesHerramientas(pagina, filtros);

        foreach (object[] fila in filas)
        {
            result.Add(new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + fila[0]), // ID de Herramienta.
                fila[11], // Nº de instrumento
                fila[1], // Tipo
                fila[2], // Modelo
                fila[3], // Marca
                fila[4], // Nº de serie
                String.IsNullOrEmpty(fila[5].ToString()) ? "" : GHerramientas.GetFrecuenciaCalibracion(Convert.ToInt32(fila[5])), // Frecuencia calib.
                String.IsNullOrEmpty(fila[6].ToString()) ? "" : Convert.ToDateTime(fila[6]).ToShortDateString(), // Última calib.
                String.IsNullOrEmpty(fila[7].ToString()) ? "" : Convert.ToDateTime(fila[7]).ToShortDateString(), // Próxima calib.
                fila[8], // Ubicación
                String.IsNullOrEmpty(fila[9].ToString()) ? "" : GHerramientas.GetTipoCalibracion(Convert.ToInt32(fila[9])), // Tipo de calibración
                fila[12], // Persona a cargo
                Encriptacion.GetParametroEncriptado("path=" + fila[11] + "\\" + fila[11] + ".jpg&idPath=" + (int)PathImage.ListadoInstrumentos), // Foto
                HayManualInstrumento(fila[11].ToString()) ? 
                    Encriptacion.GetParametroEncriptado("f=" + fila[11] + "\\" + fila[11] + ".pdf&n=Manual de instrumento Nº " + fila[11] + ".pdf&idPath=" + (int)PathImage.ListadoInstrumentos)
                    : "", // Manual
            });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginasInst(int numeroI, int tipo, string descripcion, int idPersonaCargo)
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
        if (idPersonaCargo != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosHerramienta.PersonaCargo, idPersonaCargo));
        }

        result = GHerramientas.GetCantidadPaginasCalibraciones(filtros);

        return result;
    }
    /// <summary>
    /// Obtiene el ID de la herramienta.
    /// </summary>
    private static int GetIdHerramienta(string idHerramienta)
    {
        int result;
        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(idHerramienta);

        if (!(parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out result)))
        {
            throw new DatosInvalidosException();
        }

        return result;
    }
}