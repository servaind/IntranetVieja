using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class stock_vdmAdmin : System.Web.UI.Page
{
    // Constantes.
    public const int MaxItems = 30;

    // Variables.
    private ValeDeMateriales vdm;
    private const string PrefSession = "vdmAdmin.aspx.";
    private string paginaActual;

    // Propiedades.
    public static int IdVDM
    {
        get
        {
            if (GSessions.GetSession(PrefSession + "IdVDM") == null)
            {
                GSessions.CrearSession(PrefSession + "IdVDM", Constantes.ValorInvalido);
            }

            return (int)GSessions.GetSession(PrefSession + "IdVDM");
        }
        set
        {
            if (GSessions.GetSession(PrefSession + "IdVDM") == null)
            {
                GSessions.CrearSession(PrefSession + "IdVDM", value);
            }
            else
            {
                GSessions.CambiarValorSession(PrefSession + "IdVDM", value);
            }
        }
    }
    public ValeDeMateriales VDM
    {
        get { return this.vdm; }
    }
    public bool PuedeSolicitante
    {
        get
        {
            return this.vdm == null || ( this.vdm != null && this.vdm.Estado == EstadosVDM.RechazadaResponsable &&
                this.vdm.Solicito.ID == Constantes.Usuario.ID);
        }
    }
    public bool PuedeResponsable
    {
        get 
        {
            return this.vdm != null && this.vdm.Estado == EstadosVDM.RecibidaResponsable && 
                this.vdm.Solicito.IdAutoriza == Constantes.Usuario.ID;
        }
    }
    public bool PuedeDeposito
    {
        get
        {
            return this.vdm != null && this.vdm.Estado == EstadosVDM.RecibidaDeposito &&
                GPermisosPersonal.TieneAcceso(PermisosPersona.ValeMaterialesEntrega);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idVDM;
            if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idVDM))
            {
                this.vdm = GValeDeMateriales.GetValeDeMateriales(idVDM);

                if (this.vdm == null)
                {
                    Response.Redirect("vdmAdmin.aspx");
                    return;
                }

                paginaActual = Constantes.UrlIntranet + "stock/" + "vdmAdmin.aspx?p=" + Encriptacion.GetParametroEncriptado("id=" + idVDM);

                IdVDM = idVDM;

                CompletarVDM(this.vdm);
            }
        }
    }
    /// <summary>
    /// Completa el vale de materiales.
    /// </summary>
    private void CompletarVDM(ValeDeMateriales vdm)
    {
        switch (vdm.Estado)
        {
            case EstadosVDM.Enviada:
                // Marco la solicitud como leída.
                if (vdm.Solicito.IdAutoriza == Constantes.Usuario.ID || (vdm.Solicito.ID == Constantes.Usuario.ID && Constantes.Usuario.IdAutoriza == Constantes.IdPersonaGerencia))
                {
                    try
                    {
                        GValeDeMateriales.AprobarEstadoActual(vdm.ID);
                        Response.Redirect(this.paginaActual, true);
                        return;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                        Response.Redirect(Constantes.UrlIntraDefault);
                    }
                }
                break;
            case EstadosVDM.RecibidaResponsable:
                // Apruebo la solicitud.
                /*if (vdm.Solicito.IdAutoriza == Constantes.Usuario.ID || (vdm.Solicito.ID == Constantes.Usuario.ID && Constantes.Usuario.IdAutoriza == Constantes.IdPersonaGerencia))
                {
                    try
                    {
                        GValeDeMateriales.AprobarEstadoActual(vdm.ID);
                        Response.Redirect(this.paginaActual, true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        Response.Redirect(Constantes.UrlIntraDefault);
                    }
                }*/
                break;
            case EstadosVDM.AprobadaResponsable:
                // Marco la solicitud como recibida por depósito.
                if (GPermisosPersonal.TieneAcceso(PermisosPersona.ValeMaterialesRecibeDep))
                {
                    try
                    {
                        GValeDeMateriales.AprobarEstadoActual(vdm.ID);
                        Response.Redirect(this.paginaActual, true);
                        return;
                    }
                    catch
                    {
                        Response.Redirect(Constantes.UrlIntraDefault);
                    }
                }
                break;
        }

        lblFechaSolicitud.InnerText = vdm.FechaSolicitud.ToString("dd/MM/yyyy");
        lblEmitidaPor.InnerText = vdm.Solicito.Nombre;

        lblRecibidaResponsable.InnerText = vdm.Estado >= EstadosVDM.RecibidaResponsable ? String.Format("{0} - {1}", 
            vdm.FechaRecibioResponsable.ToString("dd/MM/yyyy"), vdm.RecibioResponsable.Nombre) : "-";

        lblAprobadaResponsable.InnerText = vdm.Estado >= EstadosVDM.AprobadaResponsable && vdm.Estado != EstadosVDM.RechazadaResponsable 
            ? String.Format("{0} - {1}", vdm.FechaAproboResponsable.ToString("dd/MM/yyyy"), vdm.AproboResponsable.Nombre) : "-";

        lblRecibidaDeposito.InnerText = vdm.Estado >= EstadosVDM.RecibidaDeposito && vdm.Estado != EstadosVDM.RechazadaResponsable
            ? String.Format("{0} - {1}", vdm.FechaRecibioDeposito.ToString("dd/MM/yyyy"), vdm.RecibioDeposito.Nombre) : "-";

        lblEntregada.InnerText = vdm.Estado >= EstadosVDM.EntregadaDeposito && vdm.Estado != EstadosVDM.RechazadaResponsable
            ? String.Format("{0} - {1}", vdm.FechaEntregoDeposito.ToString("dd/MM/yyyy"), vdm.EntregoDeposito.Nombre) : "-";

        txtDepartamento.Value = vdm.Departamento;
        txtSMTL.Value = vdm.SMTL.ToString();
        txtCargo.Value = vdm.Cargo;
        txtDestino.Value = vdm.Destino;

        txtDepartamento.Attributes["readonly"] = "readonly";
        txtSMTL.Attributes["readonly"] = "readonly";
        txtCargo.Attributes["readonly"] = "readonly";
        txtDestino.Attributes["readonly"] = "readonly";

        vdm.CargarItems();
    }
    /// <summary>
    /// Busca los artículos que coincidan.
    /// </summary>
    [WebMethod()]
    public static object[][] GetArticulos(string descripcion)
    {
        List<object[]> result = new List<object[]>();

        List<string[]> articulos = GArticuloTango.BuscarArticulosTango(descripcion);
        foreach (string[] articulo in articulos)
        {
            result.Add(articulo);
        }

        return result.ToArray();
    }
    /// <summary>
    /// Busca el artículo.
    /// </summary>
    [WebMethod()]
    public static object[] GetArticulo(string codigo)
    {
        object[] result = null;

        ArticuloTango articulo = GArticuloTango.GetArticuloTango(codigo);
        if (articulo != null)
        {
            result = new object[] { codigo, articulo.Descripcion, articulo.Un };
        }

        return result;
    }
    /// <summary>
    /// Genera un nuevo vale de materiales.
    /// </summary>
    [WebMethod()]
    public static string NuevoVDM(string departamento, int smtl, string cargo, string destino, object[][] items)
    {
        string result;
        int numero = Constantes.ValorInvalido;
        List<ItemVDM> itemsVDM = new List<ItemVDM>();

        try
        {
            foreach (object[] item in items)
            {
                itemsVDM.Add(new ItemVDM(GArticuloTango.GetArticuloTango(item[0].ToString()), Convert.ToInt16(item[1]),
                    Convert.ToInt32(item[2]), item[3].ToString()));
            }
        }
        catch
        {
            throw new Exception("Se produjo un error al procesar los datos. Verifique que los datos ingresados sean "
                       + "válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese con el área de sistemas.");
        }

        try
        {
            GValeDeMateriales.AltaVDM(departamento, smtl, cargo, destino, itemsVDM, out numero);
        }
        catch (EmailException)
        {
            throw new Exception("El vale de materiales se ha generado correctamente, pero no se ha podido enviar el e-mail "
                              + "al responsable de área. Nº de referencia " + numero);
        }
        catch(Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        result = "El vale de materiales se ha generado correctamente. Nº de referencia " + numero;

        return result;
    }
    /// <summary>
    /// Genera un nuevo vale de materiales.
    /// </summary>
    [WebMethod()]
    public static string ActualizarVDM(object[][] items)
    {
        string result;
        List<ItemVDM> itemsVDM = new List<ItemVDM>();

        try
        {
            foreach (object[] item in items)
            {
                itemsVDM.Add(new ItemVDM(GArticuloTango.GetArticuloTango(item[0].ToString()), Convert.ToInt16(item[1]),
                    Convert.ToInt32(item[2]), item[3].ToString()));
            }
        }
        catch
        {
            throw new Exception("Se produjo un error al procesar los datos. Verifique que los datos ingresados sean "
                       + "válidos e intente nuevamente. <br /><br />Si el problema persiste, contáctese con el área de sistemas.");
        }

        try
        {
            GValeDeMateriales.ActualizarVDM(IdVDM, itemsVDM);
        }
        catch (EmailException)
        {
            throw new Exception("El vale de materiales se ha generado correctamente, pero no se ha podido enviar el e-mail "
                              + "al responsable de área.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        result = "El vale de materiales se ha generado correctamente.";

        return result;
    }
    /// <summary>
    /// Aprueba un vale de materiales.
    /// </summary>
    [WebMethod()]
    public static string AprobarVDM()
    {
        string result;

        try
        {
            GValeDeMateriales.AprobarEstadoActual(IdVDM);
        }
        catch (EmailException)
        {
            throw new Exception("El vale de materiales fue aprobado, pero no se ha podido enviar el e-mail "
                              + "a depósito.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        result = "El vale de materiales fue aprobado y se ha enviado un e-mail a Depósito informando la acción.";

        return result;
    }
    /// <summary>
    /// Rechaza un vale de materiales.
    /// </summary>
    [WebMethod()]
    public static string RechazarVDM(string motivo)
    {
        string result;

        try
        {
            GValeDeMateriales.RechazarEstadoActual(IdVDM, motivo);
        }
        catch (EmailException)
        {
            throw new Exception("El vale de materiales fue rechazado, pero no se ha podido enviar el e-mail "
                              + "al solicitante.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        result = "El vale de materiales fue rechazado y se ha enviado un e-mail al solicitante informando la acción.";

        return result;
    }
    /// <summary>
    /// Entrega un vale de materiales.
    /// </summary>
    [WebMethod()]
    public static string EntregarVDM()
    {
        string result;

        try
        {
            GValeDeMateriales.AprobarEstadoActual(IdVDM);
        }
        catch (EmailException)
        {
            throw new Exception("El vale de materiales fue entregado, pero no se ha podido enviar el e-mail "
                              + "al solicitante.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        result = "El vale de materiales fue entregado y se ha enviado un e-mail al solicitante informando la acción.";

        return result;
    }
}