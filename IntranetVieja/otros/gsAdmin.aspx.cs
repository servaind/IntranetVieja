using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class otros_gsAdmin : System.Web.UI.Page
{
    // Constantes.
    private DayOfWeek[] DiasDeResultados = { DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

    // Variables.
    private DateTime fecha;

    // Propiedades.
    public DateTime Fecha
    {
        get { return this.fecha; }
    }
    public bool MostrarResultados
    {
        get
        {
            bool result = false;
            foreach (DayOfWeek dia in DiasDeResultados)
            {
                if (dia == this.fecha.DayOfWeek)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
    public bool PuedeVotar
    {
        get
        {
            return !this.MostrarResultados && GranServaind.PuedeVotar();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.fecha = DateTime.Now;
    }

    /// <summary>
    /// Vota a un participante.
    /// </summary>
    [WebMethod()]
    public static void Votar(string idParticipante)
    {
        try
        {
            int id = Convert.ToInt32(Encriptacion.Desencriptar(idParticipante));
            GranServaind.VotarParticipante(id);
        }
        catch
        {
            throw new Exception("El voto no pudo ser computado.");
        }
    }
    /// <summary>
    /// Obtiene los resultados para la semana actual.
    /// </summary>
    [WebMethod()]
    public static object[][] GetResultados()
    {
        List<object[]> result = new List<object[]>();

        GSResultadoVotacion votacion = GranServaind.GetResultadoVotacion(DateTime.Now);
        foreach (GSVotosParticipante voto in votacion.Votos)
        {
            result.Add(new object[] { voto.Participante.Nombre, voto.Votos });
        }

        return result.ToArray();
    }
}