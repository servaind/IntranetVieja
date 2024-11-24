using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_cargaAsistenciaSalida : System.Web.UI.Page
{
    private class CargaAsistenciaJS
    {
        // Propiedades.
        public int PersonalID { get; set; }
        public string HoraSalida { get; set; }


        public CargaAsistenciaJS()
        {

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ADP_CargaSalida))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }
    }

    [WebMethod()]
    public static object GetDetalleAsistencia(int pagina)
    {
        object result = null;

        DateTime fecha = DateTime.Now.AddDays(pagina);

        List<RenglonAsistenciaSalida> renglones = AsistenciaFac.GetCargaAsistenciaSalida(Constantes.Usuario.ID, fecha);
        List<object> filas = new List<object>();
        renglones.ForEach(
            r =>
            filas.Add(
                new
                    {
                        r.PersonalID,
                        r.Personal,
						TipoCA = r.TipoCA,
						HoraSalida = r.HoraSalida
                    }));

        result = new
            {
                Fecha = fecha.ToShortDateString(),
                FechaAnterior = fecha.AddDays(-1).ToShortDateString(),
                Filas = filas.ToArray()
            };

        return result;
    }

    [WebMethod()]
    public static void AddAsistencia(string fecha, string[] renglones)
    {
        DateTime f;

        if (renglones != null && DateTime.TryParse(fecha, out f))
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            List<CargaAsistenciaJS> detalles = renglones.Select(r => s.Deserialize<CargaAsistenciaJS>(r)).ToList();

            List<CargaAsistenciaSalida> asistencias = new List<CargaAsistenciaSalida>();
            detalles.ForEach(d =>
                {
                    DateTime horaSalida = Constantes.FechaInvalida;

                    if (!String.IsNullOrEmpty(d.HoraSalida))
                    {
                        int hEntrada;
                        if (!Int32.TryParse(d.HoraSalida.Replace(":", ""), out hEntrada)) return;

                        horaSalida = new DateTime(f.Year, f.Month, f.Day, hEntrada/100,
                                                   hEntrada - ((int) hEntrada/100)*100, 0);
                    }

                    asistencias.Add(new CargaAsistenciaSalida(d.PersonalID, horaSalida));
                });

            if (asistencias.Count > 0) AsistenciaFac.AddAsistenciaSalida(f, asistencias);
        }
    }
}