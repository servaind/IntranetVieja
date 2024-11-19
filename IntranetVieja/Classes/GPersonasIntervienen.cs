/*
 * Historial:
 * ===================================================================================
 * [06/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

public class PersonaInterviene
{
    // Variables.
    private int idPersona;
    private Persona persona;
    private short horas;

    // Propiedades.
    public int IdPersona
    {
        get { return this.idPersona; }
    }
    /// <summary>
    /// Obtiene la persona que intervino.
    /// </summary>
    public Persona Persona
    {
        get
        {
            if (this.persona == null)
            {
                this.persona = GPersonal.GetPersona(this.idPersona);
            }

            return this.persona;
        }
    }
    /// <summary>
    /// Obtiene la cantidad de horas.
    /// </summary>
    public short Horas
    {
        get
        {
            return horas;
        }
        set
        {
            horas = value;
        }
    }


    /// <summary>
    /// Agrega una nueva persona que intervino en una imputación.
    /// </summary>
    public PersonaInterviene(int idPersona, short horas)
    {
        this.idPersona = idPersona;
        this.horas = horas;
    }
}

/// <summary>
/// Descripción breve de GPersonalInterviene
/// </summary>
public class GPersonasIntervienen
{
    /// <summary>
    /// Carga las Personas que intervinieron.
    /// </summary>
    public static List<PersonaInterviene> GetPersonasIntervienen(int idImputacionPD)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<PersonaInterviene> result = new List<PersonaInterviene>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idPersona, CantHoras FROM tbl_PDImpPersonas ";
            cmd.CommandText += "WHERE idImputacionPD = @idImputacionPD";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacionPD", idImputacionPD));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                PersonaInterviene persona = new PersonaInterviene(
                    Convert.ToInt32(dr["idPersona"]), Convert.ToInt16(dr["CantHoras"]));

                // Agrego la persona a la lista.
                result.Add(persona);
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
    /// Inserta una persona que intervino en una imputación.
    /// </summary>
    public static void InsertarPersonaIntervino(IDbConnection conn, IDbTransaction trans, int idImputacionPD, int idPersona,
        int cantHoras)
    {
        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "INSERT INTO tbl_PDImpPersonas (idImputacionPD, idPersona, cantHoras) VALUES ";
            cmd.CommandText += "(@idImputacionPD, @idPersona, @cantHoras);";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacionPD", idImputacionPD));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@cantHoras", cantHoras));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();   
        }
    }
    /// <summary>
    /// Borra las personas que intervinieron en una imputación.
    /// </summary>
    public static void BorrarPersonasIntervienen(IDbConnection conn, IDbTransaction trans, int idImputacionPD)
    {
        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "DELETE FROM tbl_PDImpPersonas WHERE idImputacionPD = @idImputacionPD";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacionPD", idImputacionPD));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
}
