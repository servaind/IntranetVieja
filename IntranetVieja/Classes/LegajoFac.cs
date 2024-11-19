using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Legajo
{
    // Variables.
    private int legajoID;
    private string numero;
    private DateTime alta;
    private DateTime baja;

    // Propiedades.
    public int ID
    {
        get { return legajoID; }
    }
    public string Numero
    {
        get { return numero; }
    }
    public DateTime Alta
    {
        get { return alta; }
    }
    public DateTime Baja
    {
        get { return baja; }
    }


    internal Legajo(int legajoID, string numero, DateTime alta, DateTime baja)
    {
        this.legajoID = legajoID;
        this.numero = numero;
        this.alta = alta;
        this.baja = baja;
    }
}

/// <summary>
/// Descripción breve de LegajoFac
/// </summary>
public static class LegajoFac
{
    internal static Legajo GetLegajo(IDataReader dr)
    {
        Legajo result;

        try
        {
            result = new Legajo(Convert.ToInt32(dr["LegajoID"]), dr["Legajo"].ToString(), Convert.ToDateTime(dr["FechaAlta"]),
                                Convert.ToDateTime(dr["FechaBaja"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    internal static Legajo GetLegajo(DataRow dr)
    {
        Legajo result;

        try
        {
            result = new Legajo(Convert.ToInt32(dr["LegajoID"]), dr["Legajo"].ToString(), Convert.ToDateTime(dr["Alta"]),
                                Convert.ToDateTime(dr["Baja"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    internal static Legajo GetLegajo(int personalID, IDbConnection conn)
    {
        Legajo result;
        IDataReader dr = null;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_PersonalLegajos WHERE PersonalID = @PersonalID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
            dr = cmd.ExecuteReader();

            result = GetLegajo(dr);

            dr.Close();
        }
        catch(Exception ex)
        {
            throw ex;
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
        }

        return result;
    }

    internal static void AddLegajo(int personalID, string numero)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(numero))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Desactivo los otros legajos.
            DeleteLegajo(personalID, trans);

            // Agrego el nuevo legajo.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_PersonalLegajos (PersonalID, Legajo, FechaBaja) VALUES ";
            cmd.CommandText += "(@PersonalID, @Legajo, @FechaBaja)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Legajo", numero));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaBaja", Constantes.FechaInvalida));
            cmd.ExecuteNonQuery();

            trans.Commit();
        }
        catch
        {
            if(trans != null) trans.Rollback();
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    internal static void DeleteLegajo(int personalID, IDbTransaction trans)
    {
        // Desactivo los otros legajos.
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "UPDATE tbl_PersonalLegajos SET Activo = @Activo WHERE PersonalID = @PersonalID";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", false));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.ExecuteNonQuery();
    }
}