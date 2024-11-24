using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_asistPcResponsable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ADP_PcResponsable))
        {
            Response.Redirect(Constantes.UrlIntranet, true);
            return;
        }
    }

    [WebMethod()]
    public static List<object> GetPanelControl(int pagina)
    {
        List<object> result = new List<object>();

        List<AsistenciaPanelControl> paneles =
            AsistenciaPanelControlFac.GetPcResponsable(Constantes.Usuario.ID,
                DateTime.Now.AddDays(pagina * AsistenciaPanelControlFac.DiasPanelControl));

        paneles.ForEach(panel =>
        {
            string titulo = panel.Base != null ? (panel.Base.Nombre + " (" + panel.Base.Responsable + ")") : String.Empty;
            List<object> filas = new List<object>();
            panel.Datos.ForEach(fila =>
            {
                List<object> celdas = new List<object>();
                fila.Datos.ForEach(celda => celdas.Add(new
                {
                    ID = celda.ID,
                    EstadoID = celda.ID != Constantes.ValorInvalido ? (int)celda.Estado : Constantes.ValorInvalido,
                    Observacion = celda.Observacion,
                    Fecha = celda.Fecha.ToShortDateString(),
                    HoraEntrada = celda.HoraEntrada != Constantes.FechaInvalida
                                     ? celda.HoraEntrada.ToString("HH:mm")
                                     : "00:00",
                    HoraSalida = celda.HoraSalida != Constantes.FechaInvalida
                                     ? celda.HoraSalida.ToString("HH:mm")
                                     : "00:00",
                    LlegoTarde = celda.LlegoTarde,
                    DiaNoHabil = celda.Fecha.DayOfWeek == DayOfWeek.Saturday || celda.Fecha.DayOfWeek == DayOfWeek.Sunday
                }
                                                ));
                filas.Add(new
                {
                    PersonalID = fila.Persona.ID,
                    Personal = fila.Persona.Nombre,
                    Celdas = celdas.ToArray()
                }
                    );
            });
            result.Add(new { Titulo = titulo, Datos = filas.ToArray() });
        });

        return result;
    }
}