using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class general_informacionObraAdmin : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "informeObraAdmin.aspx.";

    // Variables.
    private static InformacionObra io;
    private InformacionObra ioAnterior;

    // Propiedades.
    public static int IdIO
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdIO") == null)
            {
                GSessions.CrearSession(PrefSession + "IdIO", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdIO");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdIO") == null)
            {
                GSessions.CrearSession(PrefSession + "IdIO", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdIO", value);
            }
        }
    }
    public static InformacionObra IO
    {
        get { return io; }
    }
    public InformacionObra IOAnterior
    {
        get { return this.ioAnterior; }
    }
    public bool PuedeVer
    {
        get
        {
            return io != null && (GPermisosPersonal.TieneAcceso(PermisosPersona.IIOGenerar) ||
                GPermisosPersonal.TieneAcceso(PermisosPersona.IIOVer));
        }
    }
    public bool PuedeEditar
    {
        get
        {
            return this.PuedeCerrar;
        }
    }
    public bool PuedeCerrar
    {
        get 
        { 
            return io != null && ((GPermisosPersonal.TieneAcceso(PermisosPersona.IIOGenerar)
                && (io.IdInforma == Constantes.Usuario.ID || io.Datos.IdGerente == Constantes.Usuario.ID)) || GPermisosPersonal.TieneAcceso(PermisosPersona.Administrador)) 
                && io.EsUltima && !io.Datos.AutorizacionPendiente;
        }
    }
    public bool PuedeEditarPersVehic
    {
        get
        {
            return this.PuedeEditar && 
                (DateTime.Now.AddDays(InformacionObras.MinDiasAnticipo) - DateTime.Now).TotalDays >= InformacionObras.MinDiasAnticipo;
        }
    }
    public string FechaInicio
    {
        get
        {
            string result;

            result = DateTime.Now.AddDays(InformacionObras.MinDiasAnticipo).ToShortDateString();

            return result;
        }
    }
    public bool TieneAutorizacion
    {
        get
        {
            return IO != null && IO.Datos.Autorizacion != null && IO.Datos.Autorizacion.Estado == EstadoAutorizacion.Aprobada;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.IIOGenerar) && !GPermisosPersonal.TieneAcceso(PermisosPersona.IIOVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        io = null;

        List<Persona> personas = GPersonal.GetPersonasActivas();

        // Responsable de obra.
        cbResponsableObra.DataSource = personas;
        cbResponsableObra.DataTextField = "Nombre";
        cbResponsableObra.DataValueField = "ID";
        cbResponsableObra.DataBind();

        // Gerente de proyecto.
        cbGerenteProyecto.DataSource = personas;
        cbGerenteProyecto.DataTextField = "Nombre";
        cbGerenteProyecto.DataValueField = "ID";
        cbGerenteProyecto.DataBind();

        // Tipos de trabajo.
        cbTipoTrabajo.DataSource = InformacionObras.GetTiposTrabajo();
        cbTipoTrabajo.DataTextField = "Key";
        cbTipoTrabajo.DataValueField = "Value";
        cbTipoTrabajo.DataBind();

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idIO;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idIO))
        {
            int revision;
            if (!(parametros.ContainsKey("revision") && Int32.TryParse(parametros["revision"], out revision)))
            {
                revision = Constantes.ValorInvalido;
            }
            io = InformacionObras.GetInformacionObra(idIO, revision);

            if (io == null)
            {
                Response.Redirect("informacionObraAdmin.aspx");
                return;
            }

            IdIO = idIO;

            CargarIO(io);

            if (io.Revisiones.Count > 1 && io.Datos.Revision > 1)
            {
                this.ioAnterior = InformacionObras.GetInformacionObra(idIO, io.Datos.Revision - 1);

                MarcarCamposConCambios();
            }
        }
        else
        {
            lblFecha.InnerText = DateTime.Now.ToShortDateString();
            lblRevision.InnerText = "1";
            lblInforma.InnerText = Constantes.Usuario.Nombre;
        }
    }
    /// <summary>
    /// Carga la información para la obra.
    /// </summary>
    private void CargarIO(InformacionObra io)
    {
        lblNumero.InnerText = io.IdObra.ToString();
        lblFecha.InnerText = io.Fecha.ToShortDateString();
        lblRevision.InnerText = io.Datos.Revision.ToString();
        lblInforma.InnerText = io.Informa.Nombre;
        cbResponsableObra.Value = io.Datos.IdResponsableObra.ToString();

        cbTipoTrabajo.Value = ((int)io.Datos.TipoTrabajo).ToString();

        txtImputacion.Value = io.Datos.Imputacion;
        txtCliente.Value = io.Datos.Cliente;
        txtOrdenCompra.Value = io.Datos.OrdenCompra.ToString();
        txtFechaEntrega.Value = io.Datos.FechaEntrega.Equals(Constantes.FechaInvalida) ? "" : io.Datos.FechaEntrega.ToShortDateString();
        chkSubcontratistas.Checked = io.Datos.Subcontratistas;
        if (io.Datos.Subcontratistas)
        {
            txtSubcontratEmpresa.Value = io.Datos.SubcontratEmpresa;
        }

        chkPredioTerceros.Checked = io.Datos.PredioTerceros;
        if (io.Datos.PredioTerceros)
        {
            txtPredioTercEmpresa.Value = io.Datos.PredioTercEmpresa;
        }
        txtUbicacion.Value = io.Datos.Ubicacion;
        txtProvincia.Value = io.Datos.Provincia;
        txtRespTecCliente.Value = io.Datos.RespTecCliente;
        txtRespTecClienteTel.Value = io.Datos.RespTecClienteTel;
        txtRespTecClienteEmail.Value = io.Datos.RespTecClienteEmail;
        txtRespSegCliente.Value = io.Datos.RespSegCliente;
        txtRespSegClienteTel.Value = io.Datos.RespSegClienteTel;
        txtRespSegClienteEmail.Value = io.Datos.RespSegClienteEmail;
        txtContAdminCliente.Value = io.Datos.ContacAdminCliente;
        txtContAdminClienteTel.Value = io.Datos.ContacAdminClienteTel;
        txtContAdminClienteEmail.Value = io.Datos.ContacAdminClienteEmail;
        txtFechaEstimada.Value = io.Datos.FechaInicio.Equals(Constantes.FechaInvalida) ? "" : io.Datos.FechaInicio.ToShortDateString();
        txtDuracion.Value = io.Datos.Duracion;
        txtDescripcionTareas.Value = io.Datos.DescripcionTareas;
        txtFechaFinalizacion.Value = io.Datos.FechaFinalizacion.Equals(Constantes.FechaInvalida) ? "" : 
            io.Datos.FechaFinalizacion.ToShortDateString();
        cbGerenteProyecto.Value = io.Datos.IdGerente.ToString();
        txtObjetivoProyecto.Value = io.Datos.ObjetivoProyecto;

        List<HtmlControl> inputs = new List<HtmlControl>()
            {
                txtImputacion, txtCliente, txtOrdenCompra, txtSubcontratEmpresa, txtPredioTercEmpresa, txtUbicacion,
                txtProvincia, txtRespTecCliente, txtRespTecClienteTel, txtRespTecClienteEmail, txtRespSegCliente,
                txtRespSegClienteTel, txtRespSegClienteEmail, txtContAdminCliente, txtContAdminClienteTel, txtContAdminClienteEmail,
                txtDuracion, txtDescripcionTareas, txtObjetivoProyecto
            };
        List<HtmlControl> checkboxes = new List<HtmlControl>()
            {
                chkSubcontratistas, chkPredioTerceros
            };
        List<HtmlControl> combos = new List<HtmlControl>()
            {
                cbResponsableObra, cbTipoTrabajo, cbGerenteProyecto
            };

        if (!PuedeEditar)
        {
            inputs.ForEach(c => c.Attributes["readonly"] = "readonly");
            checkboxes.ForEach(c => c.Attributes["disabled"] = "disabled");
            combos.ForEach(c => c.Attributes["disabled"] = "disabled");
        }
    }
    /// <summary>
    /// Marca los campos que tienen cambios respecto de la revisión anterior.
    /// </summary>
    private void MarcarCamposConCambios()
    {
        List<HtmlControl> conCambios = new List<HtmlControl>();

        if (io.Datos.IdGerente != ioAnterior.Datos.IdGerente) conCambios.Add(lblGerenteProyecto);
        if (io.Datos.TipoTrabajo != ioAnterior.Datos.TipoTrabajo) conCambios.Add(lblTipoTrabajo);
        if (io.Datos.IdResponsableObra != ioAnterior.Datos.IdResponsableObra) conCambios.Add(lblResponsableObra);
        if (io.Datos.Imputacion != ioAnterior.Datos.Imputacion) conCambios.Add(lblImputacion);
        if (io.Datos.Cliente != ioAnterior.Datos.Cliente) conCambios.Add(lblCliente);
        if (io.Datos.OrdenCompra != ioAnterior.Datos.OrdenCompra) conCambios.Add(lblOrdenCompra);
        if (io.Datos.FechaEntrega != ioAnterior.Datos.FechaEntrega) conCambios.Add(lblFechaEntrega);
        if (io.Datos.SubcontratEmpresa != ioAnterior.Datos.SubcontratEmpresa) conCambios.Add(lblSubcontratEmpresa);
        if (io.Datos.PredioTercEmpresa != ioAnterior.Datos.PredioTercEmpresa) conCambios.Add(lblPredioTercEmpresa);
        if (io.Datos.Ubicacion != ioAnterior.Datos.Ubicacion) conCambios.Add(lblUbicacion);
        if (io.Datos.Provincia != ioAnterior.Datos.Provincia) conCambios.Add(lblProvincia);
        if (io.Datos.RespTecCliente != ioAnterior.Datos.RespTecCliente) conCambios.Add(lblRespTecCliente);
        if (io.Datos.RespTecClienteTel != ioAnterior.Datos.RespTecClienteTel) conCambios.Add(lblRespTecClienteTel);
        if (io.Datos.RespTecClienteEmail != ioAnterior.Datos.RespTecClienteEmail) conCambios.Add(lblRespTecClienteEmail);
        if (io.Datos.RespSegCliente != ioAnterior.Datos.RespSegCliente) conCambios.Add(lblRespSegCliente);
        if (io.Datos.RespSegClienteTel != ioAnterior.Datos.RespSegClienteTel) conCambios.Add(lblRespSegClienteTel);
        if (io.Datos.RespSegClienteEmail != ioAnterior.Datos.RespSegClienteEmail) conCambios.Add(lblRespSegClienteEmail);
        if (io.Datos.ContacAdminCliente != ioAnterior.Datos.ContacAdminCliente) conCambios.Add(lblContAdminCliente);
        if (io.Datos.ContacAdminClienteTel != ioAnterior.Datos.ContacAdminClienteTel) conCambios.Add(lblContAdminClienteTel);
        if (io.Datos.ContacAdminClienteEmail != ioAnterior.Datos.ContacAdminClienteEmail) conCambios.Add(lblContAdminClienteEmail);
        if (io.Datos.FechaInicio != ioAnterior.Datos.FechaInicio) conCambios.Add(lblFechaEstimada);
        if (io.Datos.Duracion != ioAnterior.Datos.Duracion) conCambios.Add(lblDuracion);
        if (io.Datos.FechaFinalizacion != ioAnterior.Datos.FechaFinalizacion) conCambios.Add(lblFechaFinalizacion);
        if (io.Datos.DescripcionTareas != ioAnterior.Datos.DescripcionTareas) conCambios.Add(lblDescripcionTareas);
        if (io.Datos.ObjetivoProyecto != ioAnterior.Datos.ObjetivoProyecto) conCambios.Add(lblObjetivoProyecto);
        if (!Funciones.ListasIguales<Persona>(io.Datos.PersonasMantenimiento, ioAnterior.Datos.PersonasMantenimiento)) conCambios.Add(lblPersonasMant);
        if (!Funciones.ListasIguales<Persona>(io.Datos.PersonasObra, ioAnterior.Datos.PersonasObra)) conCambios.Add(lblPersonasObra);
        if (!Funciones.ListasIguales<Vehiculo>(io.Datos.Vehiculos, ioAnterior.Datos.Vehiculos)) conCambios.Add(lblVehiculos);

        conCambios.ForEach(c => c.Attributes["class"] += " error");
    }
    /// <summary>
    /// Obtiene las personas disponibles.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPersonas()
    {
        List<object[]> result = new List<object[]>();

        List<Persona> personas = GPersonal.GetPersonasActivas();
        foreach (Persona persona in personas)
        {
            result.Add(new object[] { persona.ID, persona.Nombre });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene las personas disponibles.
    /// </summary>
    [WebMethod()]
    public static object[][] GetVehiculos()
    {
        List<object[]> result = new List<object[]>();

        List<Vehiculo> vehiculos = Vehiculos.GetVehiculos();
        foreach (Vehiculo vehiculo in vehiculos)
        {
            result.Add(new object[] { vehiculo.IdVehiculo, vehiculo.Patente });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene las personas asignadas a mantenimiento.
    /// </summary>
    [WebMethod()]
    public static object[] GetPersonasMant()
    {
        List<object> result = new List<object>();

        if (IO != null)
        {
            IO.Datos.PersonasMantenimiento.ForEach(p => result.Add(p.ID));
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene las personas asignadas a obra.
    /// </summary>
    [WebMethod()]
    public static object[] GetPersonasObra()
    {
        List<object> result = new List<object>();

        if (IO != null)
        {
            IO.Datos.PersonasObra.ForEach(p => result.Add(p.ID));
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene las patentes de los vehículos asignados.
    /// </summary>
    [WebMethod()]
    public static object[] GetPatentesVehic()
    {
        List<object> result = new List<object>();

        if (IO != null)
        {
            IO.Datos.Vehiculos.ForEach(v => result.Add(v.IdVehiculo));
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene el historial de la obra.
    /// </summary>
    [WebMethod()]
    public static object[][] GetHistorial()
    {
        List<object[]> result = new List<object[]>();

        if (IO != null)
        {
            foreach (RevisionObra revision in IO.Revisiones)
            {
                result.Add(new object[] { revision.Fecha.ToString("dd/MM/yyyy HH:mm:ss"), revision.Revision,
                    Encriptacion.GetParametroEncriptado("id=" + IdIO + "&revision=" + revision.Revision)});
            }
        }

        return result.ToArray();
    }
    /// <summary>
    /// Da de alta una nueva información para una obra.
    /// </summary>
    [WebMethod()]
    public static string NuevaInformacionObra(int idResponsableObra,
        int tipoTrabajo, string imputacion, string cliente, string ordenCompra, string fechaEntrega,
        bool subcontratistas, string subcontratEmpresa,
        bool predioTerceros, string predioTercEmpresa, string ubicacion, string provincia, string respTecCliente,
        string respTecClienteTel, string respTecClienteEmail, string respSegCliente, string respSegClienteTel,
        string respSegClienteEmail, string contAdminCliente, string contAdminClienteTel, string contAdminClienteEmail,
        string fechaInicio, string fechaFinaliz, string duracion, string descripcionTareas, object[] pMant, object[] pObras, 
        object[] pVehic, int gerenteProyecto, string objetivoProyecto)
    {
        string result;

        try
        {
            DateTime fEntrega = Convert.ToDateTime(fechaEntrega);
            DateTime fInicio = Convert.ToDateTime(fechaInicio);
            DateTime fFinaliz = Convert.ToDateTime(fechaFinaliz);

            List<int> persMant = new List<int>();
            List<int> persObra = new List<int>();
            List<int> patVehic = new List<int>();

            foreach (object mant in pMant)
            {
                persMant.Add(Convert.ToInt32(mant));
            }
            foreach (object obra in pObras)
            {
                persObra.Add(Convert.ToInt32(obra));
            }
            foreach (object pat in pVehic)
            {
                patVehic.Add(Convert.ToInt32(pat));
            }

            InformacionObras.NuevaInformacionObra(idResponsableObra, (TiposTrabajoObra)tipoTrabajo, imputacion, cliente,
                ordenCompra, fEntrega, subcontratistas,
                subcontratEmpresa, predioTerceros, predioTercEmpresa, ubicacion, provincia, respTecCliente, respTecClienteTel,
                respTecClienteEmail, respSegCliente, respSegClienteTel, respSegClienteEmail, contAdminCliente, contAdminClienteTel,
                contAdminClienteEmail, fInicio, duracion, descripcionTareas, fFinaliz, persMant, persObra, patVehic,
                objetivoProyecto, gerenteProyecto);

            result = "La Información Interna de Obra fue generada.";
        }
        catch (EmailException)
        {
            throw new Exception("La Información Interna de Obra fue generada correctamente, pero se produjo un error al "
            + "intentar informar a los responsables. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Agrega un registro histórico para la obra actual.
    /// </summary>
    [WebMethod()]
    public static string ActualizarInformacionObra(int idResponsableObra,
        int tipoTrabajo, string imputacion, string cliente, string ordenCompra, string fechaEntrega,
        bool subcontratistas, string subcontratEmpresa,
        bool predioTerceros, string predioTercEmpresa, string ubicacion, string provincia, string respTecCliente,
        string respTecClienteTel, string respTecClienteEmail, string respSegCliente, string respSegClienteTel,
        string respSegClienteEmail, string contAdminCliente, string contAdminClienteTel, string contAdminClienteEmail,
        string fechaInicio, string fechaFinaliz, string duracion, string descripcionTareas, object[] pMant, object[] pObras,
        object[] pVehic, int gerenteProyecto, string objetivoProyecto)
    {
        string result;

        try
        {
            DateTime fEntrega = Convert.ToDateTime(fechaEntrega);
            DateTime fInicio = Convert.ToDateTime(fechaInicio);
            DateTime fFinaliz = Convert.ToDateTime(fechaFinaliz);

            List<int> persMant = new List<int>();
            List<int> persObra = new List<int>();
            List<int> patVehic = new List<int>();

            foreach (object mant in pMant)
            {
                persMant.Add(Convert.ToInt32(mant));
            }
            foreach (object obra in pObras)
            {
                persObra.Add(Convert.ToInt32(obra));
            }
            foreach (object pat in pVehic)
            {
                patVehic.Add(Convert.ToInt32(pat));
            }

            InformacionObras.AgregarHistoricoObra(IdIO, idResponsableObra, (TiposTrabajoObra)tipoTrabajo, imputacion, cliente,
                ordenCompra, fEntrega, subcontratistas,
                subcontratEmpresa, predioTerceros, predioTercEmpresa, ubicacion, provincia, respTecCliente, respTecClienteTel,
                respTecClienteEmail, respSegCliente, respSegClienteTel, respSegClienteEmail, contAdminCliente, contAdminClienteTel,
                contAdminClienteEmail, fInicio, duracion, descripcionTareas, fFinaliz, persMant, persObra, patVehic,
                objetivoProyecto, gerenteProyecto);

            result = "La Información Interna de Obra fue actualizada.";
        }
        catch (EmailException)
        {
            throw new Exception("La Información Interna de Obra fue actualizada correctamente, pero se produjo un error al "
            + "intentar informar a los responsables. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Solicita una autorización.
    /// </summary>
    [WebMethod()]
    public static string SolicitarAutorizacion(string motivo)
    {
        string result;

        try
        {
            InformacionObras.SolicitarAutorizacion(IdIO, motivo);

            result = "La solicitud de autorización fue enviada.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud de autorización fue generada correctamente, pero se produjo un error al "
            + "intentar informar al responsable. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. <br>Detalle: " + ex.Message);
        }

        return result;
    }
}