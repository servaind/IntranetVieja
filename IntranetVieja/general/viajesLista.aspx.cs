using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_viajesLista : System.Web.UI.Page
{
    // Variables.
    private bool puedeAdministrador;

    // Propiedades.
    public bool PuedeAdministrador
    {
        get { return this.puedeAdministrador; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }
        
        // Imputaciones.
        cbFiltroImputacion.DataSource = GImputaciones.GetImputacionesActivas();
        cbFiltroImputacion.DataTextField = "Numero";
        cbFiltroImputacion.DataValueField = "ID";
        cbFiltroImputacion.DataBind();

        // Vehículos.
        cbFiltroVehiculo.DataSource = GSolicitudesViaje.GetVehiculosSolViaje();
        cbFiltroVehiculo.DataTextField = "Value";
        cbFiltroVehiculo.DataValueField = "Key";
        cbFiltroVehiculo.DataBind();

        // Estados.
        cbFiltroEstado.DataSource = GSolicitudesViaje.GetEstadosSolViaje();
        cbFiltroEstado.DataTextField = "Value";
        cbFiltroEstado.DataValueField = "Key";
        cbFiltroEstado.DataBind();

        this.puedeAdministrador = GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeEditar);
    }
    /// <summary>
    /// Obtiene una lista con las solicitudes de viaje.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetSolicitudes(int pagina, string destinatario, int idImputacion, int idVehiculo, int estado)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (destinatario.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Destino, destinatario));
        }
        if (idImputacion != Constantes.IdImputacionInvalida)
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Imputacion, idImputacion));
        }
        if (Enum.IsDefined(typeof(VehiculosSolViaje), idVehiculo))
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Vehiculo, idVehiculo));
        }
        if (Enum.IsDefined(typeof(EstadosSolViaje), estado))
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Estado, estado));
        }

        List<SolicitudViaje> svs = GSolicitudesViaje.GetSolicitudesViaje(pagina, filtros);

        foreach (SolicitudViaje sv in svs)
        {
            object[] fila = new object[] { 
                sv.IDViaje.ToString("00000"),
                sv.FechaSolicitud.ToShortDateString(),
                sv.Destinatario,
                sv.Imputacion.Numero,
                sv.Vehiculo.ToString(),
                sv.Estado.ToString(),
                Encriptacion.GetURLEncriptada("general/viajeAdmin.aspx", "id=" + sv.IDViaje),
                Encriptacion.GetParametroEncriptado("id=" + sv.IDViaje.ToString() + "&e=" + (int)sv.Estado),
                GetColorEstado(sv.Estado)
            };

            result.Add(fila);
        }

        return result;
    }
    /// <summary>
    /// Obtiene el color para el estado.
    /// </summary>
    private static string GetColorEstado(EstadosSolViaje estado)
    {
        switch (estado)
        {
            case EstadosSolViaje.Enviada:
                return "blue";
            case EstadosSolViaje.Leida:
                return "blue";
            case EstadosSolViaje.Aprobada:
                return "yellow";
            case EstadosSolViaje.Confirmada:
                return "green";
            default:
                return "red";
        }
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(string destinatario, int idImputacion, int idVehiculo, int estado)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (destinatario.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Destino, destinatario));
        }
        if (idImputacion != Constantes.IdImputacionInvalida)
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Imputacion, idImputacion));
        }
        if (Enum.IsDefined(typeof(VehiculosSolViaje), idVehiculo))
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Vehiculo, idVehiculo));
        }
        if (Enum.IsDefined(typeof(EstadosSolViaje), estado))
        {
            filtros.Add(new Filtro((int)FiltrosSolViaje.Estado, estado));
        }

        result = GSolicitudesViaje.GetCantidadPaginas(filtros);

        return result;
    }
    /// <summary>
    /// Confirma la solicitud de viaje.
    /// </summary>
    [WebMethod()]
    public static string[] ConfirmarSolicitud(string s)
    {
        string[] result;
        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(s);

        if (parametros.Count != 2 || !parametros.ContainsKey("id") || !parametros.ContainsKey("e"))
        {
            throw new Exception("La operación no pudo ser completada.");
        }

        int estado = Convert.ToInt32(parametros["e"]);
        if (!Enum.IsDefined(typeof(EstadosSolViaje), estado))
        {
            throw new Exception("La operación no pudo ser completada.");
        }

        if ((EstadosSolViaje)estado != EstadosSolViaje.Aprobada)
        {
            throw new Exception("La operación no pudo ser completada.");
        }

        try
        {
            GSolicitudesViaje.AprobarEstadoSolViaje(Convert.ToInt32(parametros["id"]));

            result = new string[] { EstadosSolViaje.Confirmada.ToString(), GetColorEstado(EstadosSolViaje.Confirmada) };
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
}