/*
 * Historial:
 * ===================================================================================
 * [06/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de ParteDiario
/// </summary>
public class ImputacionParteDiario
{
    // Variables.
    private int idImputacionPD;
    private short cantHoras;
    private Imputacion imputacion;
    private string tareasRealizadas = "";
    private string tareasProgramadas = "";
    private string novedadesHerramienta = "";
    private string novedadesVehiculo = "";
    private bool hayITR;
    private List<PersonaInterviene> lstPersonalIntervinieron = new List<PersonaInterviene>();

    // Propiedades.
    public int ID
    {
        get
        {
            return idImputacionPD;
        }
    }
    public short Horas
    {
        get { return cantHoras; }
        set { cantHoras = value; }
    }
    public Imputacion Imputacion
    {
        get { return imputacion; }
        set { imputacion = value; }
    }
    public string TareasRealizadas
    {
        get { return tareasRealizadas; }
        set { tareasRealizadas = value; }
    }
    public string TareasProgramadas
    {
        get { return tareasProgramadas; }
        set { tareasProgramadas = value; }
    }
    public string NovedadesHerramienta
    {
        get { return novedadesHerramienta; }
        set { novedadesHerramienta = value; }
    }
    public string NovedadesVehiculo
    {
        get { return novedadesVehiculo; }
        set { novedadesVehiculo = value; }
    }
    public List<PersonaInterviene> PersonalIntervinieron
    {
        get { return lstPersonalIntervinieron; }
    }
    public bool HayItr
    {
        get { return hayITR; }
        set { hayITR = value; }
    }


    public ImputacionParteDiario(short cantHoras, Imputacion imputacion,
        string tareasRealizadas, string tareasProgramadas, string novedadesHerramienta,
        string novedadesVehiculo, bool hayITR, List<PersonaInterviene> lstPersonalIntervinieron)
        : this(Constantes.ValorInvalido, cantHoras, imputacion, tareasRealizadas, tareasProgramadas, novedadesVehiculo,
          novedadesHerramienta, hayITR, lstPersonalIntervinieron)
    {

    }
    /// <summary>
    /// Almacena una Imputación de un Parte Diario.
    /// </summary>
    internal ImputacionParteDiario(int idImputacionPD, short cantHoras, Imputacion imputacion,
        string tareasRealizadas, string tareasProgramadas, string novedadesHerramienta,
        string novedadesVehiculo, bool hayITR, List<PersonaInterviene> lstPersonalIntervinieron)
    {
        this.idImputacionPD = idImputacionPD;
        this.cantHoras = cantHoras;
        this.imputacion = imputacion;
        this.tareasRealizadas = tareasRealizadas;
        this.tareasProgramadas = tareasProgramadas;
        this.novedadesHerramienta = novedadesHerramienta;
        this.novedadesVehiculo = novedadesVehiculo;
        this.hayITR = hayITR;
        this.lstPersonalIntervinieron = lstPersonalIntervinieron;
    }
    /// <summary>
    /// Agrega una Persona que intervino.
    /// </summary>
    public bool AgregarPersonaInterviene(int idPersona, short cantHoras)
    {
        bool result;

        if (this.lstPersonalIntervinieron.Find(p => p.Persona.ID == idPersona) == null)
        {
            this.lstPersonalIntervinieron.Add(new PersonaInterviene(idPersona, cantHoras));

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Quita una Persona que intervino.
    /// </summary>
    public void QuitarPersonaInterviene(int idPersona)
    {
        PersonaInterviene persona = this.lstPersonalIntervinieron.Find(p => p.Persona.ID == idPersona);

        if (persona != null)
        {
            this.lstPersonalIntervinieron.Remove(persona);
        }
    }
    /// <summary>
    /// Edita una Persona que intervino.
    /// </summary>
    public void EditarPersonaInterviene(int idPersona, short horas)
    {
        PersonaInterviene persona = this.lstPersonalIntervinieron.Find(p => p.Persona.ID == idPersona);

        if (persona != null)
        {
            persona.Horas = horas;
        }
    }
}

public static class GImputacionParteDiario
{
    /// <summary>
    /// Obtiene la lista de imputaciones asociadas a un parte diario.
    /// </summary>
    internal static List<ImputacionParteDiario> GetImputacionesPD(int idParteDiario)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<ImputacionParteDiario> result = new List<ImputacionParteDiario>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_PDImputaciones WHERE idParteDiario = @idParteDiario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ImputacionParteDiario imputacion = new ImputacionParteDiario(
                    Convert.ToInt32(dr["idImputacionPD"]),
                    Convert.ToInt16(dr["CantHoras"]),
                    GImputaciones.GetImputacion(Convert.ToInt32(dr["idImputacion"])),
                    dr["TareasR"].ToString().Trim(),
                    dr["TareasP"].ToString().Trim(),
                    dr["NovedadesH"].ToString().Trim(),
                    dr["NovedadesV"].ToString().Trim(),
                    Convert.ToBoolean(dr["HayITR"]),
                    GPersonasIntervienen.GetPersonasIntervienen(
                    Convert.ToInt32(dr["idImputacionPD"]))
                );

                // Agrego la imputación a la lista.
                result.Add(imputacion);
            }

            dr.Close();
        }
        catch
        {
            
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Inserta una imputación de un parte diario.
    /// </summary>
    internal static void InsertarImputacionPD(IDbConnection conn, IDbTransaction trans, int idParteDiario, int idImputacion,
        int cantHoras, string tareasR, string tareasP, string novedadesV, string novedadesH, bool hayITR,
        List<PersonaInterviene> personasIntervienen)
    {
        try
        {
            int idImputacionPD = InsertarImputacionPD(conn, trans, idParteDiario, idImputacion, cantHoras, tareasR, tareasP,
                novedadesV, novedadesH, hayITR);

            foreach (PersonaInterviene persona in personasIntervienen)
            {
                GPersonasIntervienen.InsertarPersonaIntervino(conn, trans, idImputacionPD, persona.IdPersona, persona.Horas);
            }
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Inserta una imputación de un parte diario.
    /// </summary>
    private static int InsertarImputacionPD(IDbConnection conn, IDbTransaction trans, int idParteDiario, int idImputacion,
        int cantHoras, string tareasR, string tareasP, string novedadesV, string novedadesH, bool hayITR)
    {
        int result;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "INSERT INTO tbl_PDImputaciones (idParteDiario, idImputacion, CantHoras, TareasR, ";
            cmd.CommandText += "TareasP, NovedadesV, NovedadesH, HayITR) VALUES (@idParteDiario, @idImputacion, @CantHoras, ";
            cmd.CommandText += "@TareasR, @TareasP, @NovedadesV, @NovedadesH, @HayITR);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_PDImputaciones;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacion", idImputacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CantHoras", cantHoras));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TareasR", Funciones.ReemplazarEnters(tareasR)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TareasP", Funciones.ReemplazarEnters(tareasP)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NovedadesV", Funciones.ReemplazarEnters(novedadesV)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NovedadesH", Funciones.ReemplazarEnters(novedadesH)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HayITR", hayITR));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            throw new ErrorOperacionException();
        }

        return result;
    }
    /// <summary>
    /// Borra las imputaciones de un Parte Diario.
    /// </summary>
    internal static void BorrarImputacionesPD(IDbConnection conn, IDbTransaction trans, int idParteDiario)
    {
        try
        {
            // Borro las personas que intervinieron.
            // Las personas que intervinieron se borran en cascada al borrar las imputaciones.

            // Borro las imputaciones.
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "DELETE FROM tbl_PDImputaciones WHERE idParteDiario = @idParteDiario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParteDiario", idParteDiario));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
}