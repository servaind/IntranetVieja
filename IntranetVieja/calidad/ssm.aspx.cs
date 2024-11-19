using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_ssm : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "ssm.aspx.";

    // Propiedades.
    private static DateTime FechaBase
    {
        get
        {
            return (DateTime)GSessions.GetSession(PrefSession + "FechaBase");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "FechaBase", value);
        }
    }
    public bool PuedeAdministrador
    {
        get
        {
            bool result;

            result = GPermisosPersonal.TieneAcceso(PermisosPersona.SSM_Admin);

            return result;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        FechaBase = Funciones.GetDate(DateTime.Now);

        cbActualizarEstado.DataSource = GSSM.GetEstadosSitios();
        cbActualizarEstado.DataTextField = "Value";
        cbActualizarEstado.DataValueField = "Key";
        cbActualizarEstado.DataBind();
    }
    /// <summary>
    /// Obtiene las filas del panel de control.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPanelControl(int pagina)
    {
        List<object[]> result = new List<object[]>();

        List<SitioSSM> sitios = GSSM.GetSitios();

        DateTime fecha = FechaBase.AddMonths(pagina);
        Dictionary<ItemSSM, Dictionary<int, EstadosSitio>> estados = GSSM.GetEstadosSitios(fecha.Month, fecha.Year, sitios);

        // Formo el encabezado.
        List<object> encabezado = new List<object>();
        encabezado.Add(fecha.ToString("MMMM/yyyy").ToUpper());
        sitios.ForEach(s => encabezado.Add(s.Nombre));
        result.Add(encabezado.ToArray());

        // Agrego las filas.
        foreach (ItemSSM item in estados.Keys)
        {
            List<object> fila = new List<object>();
            fila.Add(item.Nombre);

            foreach (SitioSSM sitio in sitios)
            {
                fila.Add(Encriptacion.Encriptar(sitio.IdSitio.ToString() + Constantes.SepDescArtTango + item.IdItem));
                fila.Add(estados[item].ContainsKey(sitio.IdSitio) ?
                    (int)estados[item][sitio.IdSitio] : (int)EstadosSitio.NoCumplido);
                fila.Add(GSSM.EstadoSitioToString(estados[item].ContainsKey(sitio.IdSitio) ? 
                    estados[item][sitio.IdSitio] : EstadosSitio.NoCumplido));
            }

            result.Add(fila.ToArray());
        }

        return result.ToArray();
    }
    /// <summary>
    /// Envía un e-mail de recordatorio para el cumplimiento del ítem.
    /// </summary>
    [WebMethod()]
    public static string EnviarEmailRecordatorio(string valor)
    {
        string result;

        try
        {
            string[] aux = Encriptacion.Desencriptar(valor).Split(Constantes.SepDescArtTango);
            int idSitio = Convert.ToInt32(aux[0]);
            int idItem = Convert.ToInt32(aux[1]);

            GSSM.EnviarEmailRecordatorio(idSitio, idItem);

            result = "El recordatorio fue enviado.";
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
    /// <summary>
    /// Actualiza el estado para un sitio.
    /// </summary>
    [WebMethod()]
    public static void ActualizarEstadoSitio(string valor, int idEstado, int pagina)
    {
        if (!Enum.IsDefined(typeof(EstadosSitio), idEstado))
        {
            throw new Exception("Parámetros incorrectos.");
        }

        try
        {
            string[] aux = Encriptacion.Desencriptar(valor).Split(Constantes.SepDescArtTango);
            int idSitio = Convert.ToInt32(aux[0]);
            int idItem = Convert.ToInt32(aux[1]);
            DateTime fecha = FechaBase.AddMonths(pagina);

            GSSM.AddEstadoSitio(idSitio, idItem, fecha.Month, fecha.Year, (EstadosSitio)idEstado);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
}