using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rrhh_panelControlAsistencia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod()]
    public static List<object> GetPanelControl(int pagina)
    {
        List<object> result = new List<object>();

        List<AsistenciaPanelControl> paneles =
            AsistenciaPanelControlFac.GetPanelesControl(
                DateTime.Now.AddDays(pagina*AsistenciaPanelControlFac.DiasPanelControl));

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
								ModoEntrada = celda.ModoEntrada,
								ModoSalida = celda.ModoSalida,
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
                result.Add(new {Titulo = titulo, Datos = filas.ToArray()});
            });

        return result;
    }

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

    [WebMethod()]
    public static object GetDetalleAsistencia(int id)
    {
        DetalleAsistencia detalle = AsistenciaFac.GetDetalleAsistencia(id);

        if (detalle == null) return detalle;
        else
            return
                new
                    {
                        Fecha = detalle.Fecha.ToShortDateString(),
                        EstadoID = (int) detalle.Estado,
                        HoraEntrada =
                            detalle.HoraEntrada == Constantes.FechaInvalida ? "" : detalle.HoraEntrada.ToString("HH:mm"),
                        HoraSalida =
                            detalle.HoraSalida == Constantes.FechaInvalida ? "" : detalle.HoraSalida.ToString("HH:mm"),
						ModoEntrada = detalle.ModoEntrada,
						ModoSalida = detalle.ModoSalida,
                        Observacion = detalle.Observacion
                    };
    }

    private static List<DateTime> GetFechas(int pagina)
    {
        List<DateTime> result = new List<DateTime>();
        DateTime fechaBase = DateTime.Now.AddDays(pagina * AsistenciaPanelControlFac.DiasPanelControl);

        for (int i = 0; i < AsistenciaPanelControlFac.DiasPanelControl; i++)
        {
            result.Add(fechaBase.AddDays(i - AsistenciaPanelControlFac.DiasPanelControl + 1));
        }

        return result;
    }

    [WebMethod()]
    public static void UpdateAsistencia(int asistenciaID, int personalID, string fecha, int estadoID, string observacion,
                                        string horaEntrada, string horaSalida)
    {
        int hEntrada;
        int hSalida;
        DateTime f;

        if (!Int32.TryParse(horaEntrada.Replace(":", ""), out hEntrada) ||
            !Int32.TryParse(horaSalida.Replace(":", ""), out hSalida))
        {
            throw new Exception("Los horarios deben ser de la forma hh:mm.");
        }

        if (String.IsNullOrEmpty(observacion))
        {
            throw new Exception("No se ha ingresado ninguna observación.");
        }

        if (!DateTime.TryParse(fecha, out f))
        {
            throw new Exception("La fecha ingresada no es válida.");
        }

        AsistenciaFac.UpdateAsistencia(asistenciaID, personalID, f, (EstadoAsistencia)estadoID, observacion, hEntrada, hSalida);
    }
}