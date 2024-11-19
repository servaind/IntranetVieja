/*
 * Historial:
 * ===================================================================================
 * [09/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;


public class PanelControlPD
{
    // Variables.
    private Dictionary<int, string> personas;
    private DateTime fechaDesde;
    private DateTime fechaHasta;
    private List<FilaPCParteDiario> filas;

    // Propiedades.
    public Dictionary<int, string> Personas
    {
        get { return this.personas; }
    }
    public DateTime FechaDesde
    {
        get { return fechaDesde; }
    }
    public DateTime FechaHasta
    {
        get { return fechaHasta; }
    }
    public List<FilaPCParteDiario> Filas
    {
        get { return filas; }
    }


    internal PanelControlPD(Dictionary<int, string> personas)
        : this(personas, DateTime.Now, DateTime.Now, null)
    {

    }
    internal PanelControlPD(Dictionary<int, string> personas, DateTime fechaDesde, DateTime fechaHasta, 
        List<FilaPCParteDiario> filas)
    {
        this.personas = personas;
        this.fechaDesde = fechaDesde;
        this.fechaHasta = fechaHasta;
        this.filas = filas;
    }
    /// <summary>
    /// Carga las filas para el panel de control.
    /// </summary>
    public void CargarFilas(DateTime fechaDesde, DateTime fechaHasta)
    {
        this.fechaDesde = fechaDesde;
        this.fechaHasta = fechaHasta;
        this.filas = GPanelesControlPD.GetFilasPanelControl(this.personas, fechaDesde, fechaHasta);
    }
}
/// <summary>
/// Clase para almacenar una fila del panel de control de partes diarios.
/// </summary>
public class FilaPCParteDiario
{
    // Variables.
    private int idPersona;
    private string persona;
    private Dictionary<DateTime, object[]> partesDiarios;

    // Propiedades.
    public int IdPersona
    {
        get { return this.idPersona; }
    }
    public string Persona
    {
        get { return this.persona; }
    }
    public Dictionary<DateTime, object[]> PartesDiarios
    {
        get { return this.partesDiarios; }
    }
    public object[] this[DateTime fechaParte]
    {
        get
        {
            object[] result = null;

            if (this.partesDiarios.ContainsKey(Funciones.GetDate(fechaParte)))
            {
                result = this.partesDiarios[Funciones.GetDate(fechaParte)];
            }

            return result;
        }
    }


    internal FilaPCParteDiario(int idPersona, string persona, Dictionary<DateTime, object[]> partesDiarios)
    {
        this.idPersona = idPersona;
        this.persona = persona;
        this.partesDiarios = partesDiarios;
    }
}
/// <summary>
/// Summary description for GPanelControlPD
/// </summary>
public class GPanelesControlPD
{
    /// <summary>
    /// Obtiene una lista de partes diarios en un intervalo. Si no existe un parte diario para la fecha, se ingresa como null.
    /// </summary>
    internal static List<FilaPCParteDiario> GetFilasPanelControl(Dictionary<int, string> personas, DateTime fechaInicio, 
        DateTime fechaFin)
    {
        List<FilaPCParteDiario> result = new List<FilaPCParteDiario>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            foreach (int idPersona in personas.Keys)
            {
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT pd.idParteDiario, pd.FechaParte, pd.idEstado, lic.idTipo, ";
                cmd.CommandText += "lic.idEstadoAutoriz FROM tbl_PartesDiarios pd ";
                cmd.CommandText += "LEFT JOIN tbl_PDLicencias lic ON pd.idLicencia = lic.idLicencia ";
                cmd.CommandText += "WHERE pd.idPersona = @idPersona AND pd.FechaParte BETWEEN @Desde AND @Hasta AND ";
                cmd.CommandText += "pd.Finalizado = @Finalizado ORDER BY pd.FechaParte";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Desde", Funciones.GetDate(fechaInicio)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Hasta", Funciones.GetDateEnd(fechaFin)));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Finalizado", true));

                dr = cmd.ExecuteReader();

                Dictionary<DateTime, object[]> partesDiarios = new Dictionary<DateTime, object[]>();
                FilaPCParteDiario fila;
                while (dr.Read())
                {
                    DateTime fecha = Convert.ToDateTime(dr["FechaParte"]);
                    object[] datosPD;
                    int idParteDiario = Convert.ToInt32(dr["idParteDiario"]);
                    EstadosParteDiario idEstado = (EstadosParteDiario)Convert.ToInt32(dr["idEstado"]);
                    TiposLicencia tipoLicencia = TiposLicencia.SinGoceHaberes;
                    EstadosLicencia estadoLicencia = EstadosLicencia.RechazadaRRHH;
                    if (idEstado == EstadosParteDiario.Licencia)
                    {
                        tipoLicencia = (TiposLicencia)Convert.ToInt32(dr["idTipo"]);
                        estadoLicencia = (EstadosLicencia)Convert.ToInt32(dr["idEstadoAutoriz"]);
                    }
                    datosPD = new object[] { idParteDiario, idEstado.ToString(), (int)tipoLicencia, (int)estadoLicencia };

                    partesDiarios.Add(fecha, datosPD);
                }
                fila = new FilaPCParteDiario(idPersona, personas[idPersona], partesDiarios);
                result.Add(fila);

                dr.Close();
            }
        }
        catch
        {
            result = new List<FilaPCParteDiario>();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene un panel de control.
    /// </summary>
    public static PanelControlPD GetPanelControlPD()
    {
        PanelControlPD result = null;

        result = new PanelControlPD(GPersonal.GetPersonas(Constantes.Usuario.ID, true, true));

        return result;
    }
    /// <summary>
    /// Obtiene un panel de control para las fechas indicadas.
    /// </summary>
    public static PanelControlPD GetPanelControlPD(DateTime fechaInicio, DateTime fechaFin)
    {
        PanelControlPD result;

        result = GetPanelControlPD();
        if (result != null)
        {
            result.CargarFilas(fechaInicio, fechaFin);
        }

        return result;
    }
}