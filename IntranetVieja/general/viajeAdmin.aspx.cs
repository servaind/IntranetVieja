using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_viajeAdmin : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "viajeAdmin.aspx.";

    // Variables.
    private SolicitudViaje sv;

    // Propiedades.
    public static int IdSV
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdSV") == null)
            {
                GSessions.CrearSession(PrefSession + "IdSV", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdSV");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdSV") == null)
            {
                GSessions.CrearSession(PrefSession + "IdSV", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdSV", value);
            }
        }
    }
    public SolicitudViaje SV
    {
        get { return this.sv; }
    }
    public bool PuedeAdministrador
    {
        get
        {
            bool result;

            result = this.sv != null && this.sv.Estado != EstadosSolViaje.Cancelada && this.sv.Estado != EstadosSolViaje.Confirmada 
                && GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeEditar);

            return result;
        }
    }
    public string FechaInicio
    {
        get
        {
            string result;

            DateTime fecha = DateTime.Now.AddHours(GSolicitudesViaje.MinHsAntipo);
            if (fecha.DayOfWeek == DayOfWeek.Saturday) fecha = fecha.AddDays(2);
            else if (fecha.DayOfWeek == DayOfWeek.Sunday) fecha = fecha.AddDays(3);

            result = fecha.ToShortDateString();
            
            return result;
        }
    }
    public bool HorarioPermitido
    {
        get
        {
            bool result;

            //DateTime ahora = DateTime.Now;
            //DateTime horaLimite = new DateTime(ahora.Year, ahora.Month, ahora.Day, GSolicitudesViaje.MaxHsSolicitud, 0, 0);

            //if (ahora <= horaLimite)
            //{
            //    result = true;
            //}
            //else
            //{
            //    result = ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday;
            //}
            result = true;

            return result;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        // Vehículos.
        cbVehiculo.DataSource = GSolicitudesViaje.GetVehiculosSolViaje();
        cbVehiculo.DataTextField = "Value";
        cbVehiculo.DataValueField = "Key";
        cbVehiculo.DataBind();

        // Importancia.
        cbImportancia.DataSource = GSolicitudesViaje.GetImportanciasSolViaje();
        cbImportancia.DataTextField = "Value";
        cbImportancia.DataValueField = "Key";
        cbImportancia.DataBind();

        // Imputaciones.
        cbImputacion.DataSource = GImputaciones.GetImputacionesActivas();
        cbImputacion.DataTextField = "Numero";
        cbImputacion.DataValueField = "ID";
        cbImputacion.DataBind();

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);
        
        int idSV;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idSV))
        {
            this.sv = GSolicitudesViaje.GetSolicitudViaje(idSV);

            if (this.sv == null)
            {
                Response.Redirect("viajeAdmin.aspx");
                return;
            }

            if (this.sv.Estado == EstadosSolViaje.Enviada && GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeEditar))
            {
                // Confirmo la lectura de la solicitud.
                try
                {
                    GSolicitudesViaje.AprobarEstadoSolViaje(idSV);
                    Response.Redirect(Request.Url.ToString());
                    return;
                }
                catch
                {
                    Response.Redirect(Constantes.UrlIntraDefault);
                    return;
                }
            }

            IdSV = idSV;

            CargarSV(this.sv);
        }
    }
    /// <summary>
    /// Carga la solicitud de viaje.
    /// </summary>
    private void CargarSV(SolicitudViaje sv)
    {
        lblNumero.InnerText = sv.IDViaje.ToString();
        lblFecha.InnerText = sv.FechaSolicitud.ToShortDateString();
        if (sv.Estado >= EstadosSolViaje.Leida)
        {
            lblFechaLectura.InnerText = String.Format("{0} - {1}", sv.FechaLectura.ToShortDateString(), sv.Lectura.Nombre);
        }
        if (sv.Estado >= EstadosSolViaje.Aprobada && sv.Estado != EstadosSolViaje.Cancelada)
        {
            lblFechaAprobacion.InnerText = String.Format("{0} - {1}", sv.FechaAprobo.ToShortDateString(), sv.Aprobo.Nombre);
        }
        if (sv.Estado == EstadosSolViaje.Confirmada)
        {
            lblFechaConfirmacion.InnerText = String.Format("{0} - {1}", sv.FechaConfirmo.ToShortDateString(), sv.Confirmo.Nombre);
        }
        if (sv.Estado == EstadosSolViaje.Cancelada)
        {
            lblFechaCancelacion.InnerText = String.Format("{0} - {1}", sv.FechaCancelo.ToShortDateString(), sv.Cancelo.Nombre);
        }

        cbVehiculo.Value = ((int)sv.Vehiculo).ToString();
        cbImportancia.Value = ((int)sv.Importancia).ToString();

        txtMotivo.Value = sv.Motivo;
        txtDescripcion.Value = sv.Descripcion;
        txtOrigen.Value = sv.Origen;
        txtRuta.Value = sv.Ruta;
        txtFinRecorrido.Value = sv.FinRecorrido;
        txtFechaCumplimiento.Value = sv.FechaCumplimiento.ToShortDateString();
        txtHoraCumplimiento.Value = sv.HoraCumplimiento;
        txtFechaLimite.Value = sv.FechaLimite.ToShortDateString();
        txtHoraLimite.Value = sv.HoraLimite;

        txtDestinatario.Value = sv.Destinatario;
        txtDireccion.Value = sv.Direccion;
        txtLocalidad.Value = sv.Localidad;
        txtContacto.Value = sv.Contacto;
        txtTelefono.Value = sv.Telefono;
        txtHorarioAtencion.Value = sv.HorarioAtencion;
        txtDocumentoRef.Value = sv.DocumentoReferencia;
        chkRetFac.Checked = sv.RetornaFactura;
        chkRetRec.Checked = sv.RetornaRecibo;
        chkRetRem.Checked = sv.RetornaRemito;
        txtRetOtro.Value = sv.RetornaOtro;

        txtCondComer.Value = sv.CondicionesComerciales;
        if (sv.Imputacion != null)
        {
            cbImputacion.Value = sv.Imputacion.ID.ToString();
        }
        txtImporte.Value = sv.Importe.ToString("0.00");
        chkEfectivo.Checked = sv.Efectivo;
        chkCheque.Checked = sv.Cheque;
        if (sv.Cheque)
        {
            txtAlaOrden.Value = sv.AlaOrden;
        }
        txtObservaciones.Value = sv.Observaciones;

        // Deshabilitar controles.
        cbVehiculo.Attributes["disabled"] = "disabled";
        cbImportancia.Attributes["disabled"] = "disabled";

        txtMotivo.Attributes["readonly"] = "readonly";
        txtDescripcion.Attributes["readonly"] = "readonly";
        txtOrigen.Attributes["readonly"] = "readonly";
        txtRuta.Attributes["readonly"] = "readonly";
        txtFinRecorrido.Attributes["readonly"] = "readonly";
        txtFechaCumplimiento.Attributes["readonly"] = "readonly";
        txtHoraCumplimiento.Attributes["readonly"] = "readonly";
        txtFechaLimite.Attributes["readonly"] = "readonly";
        txtHoraLimite.Attributes["readonly"] = "readonly";

        txtDestinatario.Attributes["readonly"] = "readonly";
        txtDireccion.Attributes["readonly"] = "readonly";
        txtLocalidad.Attributes["readonly"] = "readonly";
        txtContacto.Attributes["readonly"] = "readonly";
        txtTelefono.Attributes["readonly"] = "readonly";
        txtHorarioAtencion.Attributes["readonly"] = "readonly";
        txtDocumentoRef.Attributes["readonly"] = "readonly";
        chkRetFac.Attributes["disabled"] = "disabled";
        chkRetRec.Attributes["disabled"] = "disabled";
        chkRetRem.Attributes["disabled"] = "disabled";
        txtRetOtro.Attributes["disabled"] = "disabled";

        txtCondComer.Attributes["readonly"] = "readonly";
        cbImputacion.Attributes["disabled"] = "disabled";
        txtImporte.Attributes["readonly"] = "readonly";
        chkEfectivo.Attributes["disabled"] = "disabled";
        chkCheque.Attributes["disabled"] = "disabled";
        txtAlaOrden.Attributes["readonly"] = "readonly";
        txtObservaciones.Attributes["readonly"] = "readonly";
    }
    /// <summary>
    /// Genera una nueva solicitud de viaje.
    /// </summary>
    [WebMethod()]
    public static string NuevaSolicitudViaje(int idVehiculo, int idImportancia, string motivo,
        string descripcion, string origen, string ruta, string finRecorrido, string fechaCumplimiento, string horaCumplimiento,
        string fechaLimite, string horaLimite, string destinatario, string direccion, string localidad,
        string contacto, string telefono, string horarioAtencion, string docReferencia, int retFactura, int retRemito,
        int retRecibo, string retOtro, string condComerciales, int idImputacion, string importe, int efectivo,
        int cheque, string aLaOrden, string observaciones)
    {
        string result;

        DateTime fCumplimiento;
        DateTime fLimite;

        try
        {
            fCumplimiento = Convert.ToDateTime(fechaCumplimiento);
            fLimite = Convert.ToDateTime(fechaLimite);
        }
        catch
        {
            throw new Exception("Las fechas ingresadas no tienen un formato válido. Deben ser de la forma DD/MM/AAAA.");
        }

        int numero = Constantes.ValorInvalido;
        try
        {
            GSolicitudesViaje.NuevaSolicitudViaje((VehiculosSolViaje)idVehiculo, (ImporanciasSolViaje)idImportancia, motivo, 
                descripcion, origen, ruta, finRecorrido, fCumplimiento, horaCumplimiento, fLimite, horaLimite, destinatario, 
                direccion, localidad, contacto, telefono, horarioAtencion, docReferencia, retFactura == 1, retRemito == 1,
                retRecibo == 1, retOtro, condComerciales, idImputacion, Convert.ToSingle(Funciones.GetDecimalNumber(importe)),
                efectivo == 1, cheque == 1, aLaOrden, observaciones, true, out numero);

            result = "Solicitud de viaje enviada. Nº de referencia: " + numero;
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue generada correctamente, pero se produjo un error al intentar informar "
                    + "al responsable. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Aprueba la solicitud de viaje.
    /// </summary>
    [WebMethod()]
    public static string AprobarSolicitudViaje()
    {
        string result;

        try
        {
            GSolicitudesViaje.AprobarEstadoSolViaje(IdSV);

            result = "Solicitud de viaje aprobada.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue generada correctamente, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Rechaza la solicitud de viaje.
    /// </summary>
    [WebMethod()]
    public static string RechazarSolicitudViaje()
    {
        string result;

        try
        {
            GSolicitudesViaje.RechazarEstadoSolViaje(IdSV);

            result = "Solicitud de viaje rechazada.";
        }
        catch (EmailException)
        {
            throw new Exception("La solicitud fue generada correctamente, pero se produjo un error al intentar informar "
                    + "al solicitante. Por favor, contáctese con el área de sistemas.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Confirma la solicitud de viaje.
    /// </summary>
    [WebMethod()]
    public static string ConfirmarSolicitudViaje()
    {
        string result;

        try
        {
            GSolicitudesViaje.AprobarEstadoSolViaje(IdSV);

            result = "Solicitud de viaje confirmada.";
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
}