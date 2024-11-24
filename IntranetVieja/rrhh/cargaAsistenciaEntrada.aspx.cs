using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_cargaAsistencia : System.Web.UI.Page
{
    private class CargaAsistenciaJS
    {
        // Propiedades.
        public int PersonalID { get; set; }
        public int EstadoID { get; set; }
        public string Observacion { get; set; }
        public string HoraEntrada { get; set; }


        public CargaAsistenciaJS()
        {

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ADP_CargaEntrada))
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

        List<RenglonAsistenciaEntrada> renglones = AsistenciaFac.GetCargaAsistenciaEntrada(Constantes.Usuario.ID, fecha);
        
		List<object> filas = new List<object>();
        renglones.ForEach(
            r =>
            filas.Add(
                new
                    {
                        r.PersonalID,
                        r.Personal,
                        EstadoID = (int) r.Estado,
                        PuedeCargarFecha = r.PuedeCargarFecha,
						TipoCA = r.TipoCA,
						HoraEntrada = r.HoraEntrada,
                        r.Observacion
                    }));

        result = new
            {
                Fecha = fecha.ToShortDateString(),
                FechaAnterior = fecha.AddDays(-1).ToShortDateString(),
                Filas = filas.ToArray(),
				FilasCount = renglones.Count
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

            List<CargaAsistenciaEntrada> asistencias = new List<CargaAsistenciaEntrada>();
            detalles.ForEach(d =>
                {
                    if (!Enum.IsDefined(typeof (EstadoAsistencia), d.EstadoID)) return;

                    DateTime horaEntrada = Constantes.FechaInvalida;
                    DateTime horaSalidaAnterior = Constantes.FechaInvalida;

                    if (!String.IsNullOrEmpty(d.HoraEntrada))
                    {
                        int hEntrada;
                        if (!Int32.TryParse(d.HoraEntrada.Replace(":", ""), out hEntrada)) return;

                        horaEntrada = new DateTime(f.Year, f.Month, f.Day, hEntrada/100,
                                                   hEntrada - ((int) hEntrada/100)*100, 0);
                    }

                    asistencias.Add(new CargaAsistenciaEntrada(d.PersonalID, (EstadoAsistencia) d.EstadoID,
                                                               d.Observacion, horaEntrada));
                });

            if (asistencias.Count > 0) AsistenciaFac.AddAsistenciaEntrada(f, asistencias);
        }
    }
}