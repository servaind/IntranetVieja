using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_partesDiariosPC : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "partesDiariosPC.aspx.";
    private const int DiasPanelControl = 7;

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


    protected void Page_Load(object sender, EventArgs e)
    {
        FechaBase = Funciones.GetDate(DateTime.Now);
    }
    /// <summary>
    /// Obtiene las filas del panel de control.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPanelControl(int pagina)
    {
        List<object[]> result = new List<object[]>();

        DateTime hasta = GetFecha(pagina);
        DateTime desde = hasta.AddDays(-DiasPanelControl);
        List<DateTime> fechas = GetFechas(pagina);
        PanelControlPD pc = GPanelesControlPD.GetPanelControlPD();
        pc.CargarFilas(desde, hasta);

        foreach (FilaPCParteDiario fila in pc.Filas)
        {
            List<object> f = new List<object>();
            f.Add(fila.IdPersona);
            f.Add(fila.Persona);
            for (int i = 0; i < DiasPanelControl; i++)
            {
                List<object> datos = new List<object>();
                object[] d = fila[fechas[i]];
                if (d != null)
                {
                    // idParteDiario | estado | idTipoLicencia | idEstadoAutoriz | desc dia semana | url
                    datos.AddRange(d);
                    datos.Add(String.Format("{0} {1}", Funciones.GetDiaSemana(fechas[i]), fechas[i].ToShortDateString()));
                    datos.Add(Encriptacion.GetURLEncriptada("general/parteDiarioVer.aspx", "id=" + d[0]));
                }
                else
                {
                    // idPersona | desc dia semana | dia | url
                    datos.AddRange(new object[] { fila.IdPersona, String.Format("{0} {1}", Funciones.GetDiaSemana(fechas[i]), 
                        fechas[i].ToShortDateString()), fechas[i].ToShortDateString(),
                        Encriptacion.GetURLEncriptada("general/parteDiarioAdmin.aspx", "f=" + fechas[i].ToShortDateString())});
                }

                f.Add(datos.ToArray());
            }
            result.Add(f.ToArray());
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene los encabezados para las filas.
    /// </summary>
    [WebMethod()]
    public static string[] GetEncabezado(int pagina)
    {
        List<string> result = new List<string>();

        List<DateTime> fechas = GetFechas(pagina);
        foreach (DateTime fecha in fechas)
        {
            result.Add(String.Format("{0} {1}", Funciones.GetDiaSemana(fecha), fecha.ToShortDateString()));
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene los estados para los partes diarios.
    /// </summary>
    [WebMethod()]
    public static object[][] GetEstadosPD()
    {
        List<object[]> result = new List<object[]>();

        result.Add(new object[] { (int)TiposLicencia.Casamiento, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Casamiento), 
            "/images/partesDiarios/casamiento.png" });
        result.Add(new object[] { (int)TiposLicencia.Examen, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Examen), 
            "/images/partesDiarios/examen.png" });
        result.Add(new object[] { (int)TiposLicencia.Franco, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Franco), 
            "/images/partesDiarios/franco.png" });
        result.Add(new object[] { (int)TiposLicencia.LicenciaSinAviso, 
            GLicencias.GetDescripcionTipoLicencia(TiposLicencia.LicenciaSinAviso), "/images/partesDiarios/sin_aviso.png" });
        result.Add(new object[] { (int)TiposLicencia.ModificacionHorario, 
            GLicencias.GetDescripcionTipoLicencia(TiposLicencia.ModificacionHorario), "/images/partesDiarios/modif_horario.png" });
        result.Add(new object[] { (int)TiposLicencia.Nacimiento, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Nacimiento), 
            "/images/partesDiarios/nacimiento.png" });
        result.Add(new object[] { (int)TiposLicencia.SinGoceHaberes, 
            GLicencias.GetDescripcionTipoLicencia(TiposLicencia.SinGoceHaberes), "/images/partesDiarios/sin_goce.png" });
        result.Add(new object[] { (int)TiposLicencia.Vacaciones, GLicencias.GetDescripcionTipoLicencia(TiposLicencia.Vacaciones),
            "/images/partesDiarios/vacaciones.png" });

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene la fecha medio del panel de control.
    /// </summary>
    private static DateTime GetFecha(int pagina)
    {
        return FechaBase.AddDays(pagina * DiasPanelControl);
    }
    /// <summary>
    /// Obtiene las fechas para el panel de control
    /// </summary>
    private static List<DateTime> GetFechas(int pagina)
    {
        List<DateTime> result = new List<DateTime>();
        DateTime fechaBase = GetFecha(pagina);

        for (int i = 0; i < DiasPanelControl; i++)
        {
            result.Add(fechaBase.AddDays(i + 1 - DiasPanelControl));
        }

        return result;
    }
    /// <summary>
    /// Envía un e-mail de recordatorio para la carga de un parte diario.
    /// </summary>
    [WebMethod()]
    public static string EnviarEmailRecordatorio(int idPersona, string fecha)
    {
        string result;

        try
        {
            GPartesDiarios.EnviarEmailRecordatorio(idPersona, Convert.ToDateTime(fecha));

            result = "El recordatorio fue enviado.";
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }

        return result;
    }
}