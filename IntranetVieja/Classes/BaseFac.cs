using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class Base
{
    // Variables.
    private int baseID;
    private string nombre;
    private int responsableID;
    private string responsable;
    private int alternateID;
    private string alternate;
    private List<Persona> integrantes;
    private bool activa;

    // Properties.
    public int ID
    {
        get { return baseID; }
    }
    public string Nombre
    {
        get { return nombre; }
    }
    public int ResponsableID
    {
        get { return responsableID; }
    }
    public string Responsable
    {
        get { return responsable; }
    }
    public int AlternateID
    {
        get { return alternateID; }
    }
    public string Alternate
    {
        get { return alternate; }
    }
    public List<Persona> Integrantes
    {
        get { return integrantes; }
    }
    public bool Activa
    {
        get { return activa; }
    }


    internal Base(int baseID, string nombre, int responsableID, string responsable, int alternateID, string alternate, 
        bool activa, List<Persona> integrantes)
    {
        this.baseID = baseID;
        this.nombre = nombre;
        this.responsableID = responsableID;
        this.responsable = responsable;
        this.alternateID = alternateID;
        this.alternate = alternate;
        this.activa = activa;
        this.integrantes = integrantes;
    }

    internal void SetIntegrantes(List<Persona> integrantes)
    {
        this.integrantes = integrantes;
    }
}

/// <summary>
/// Descripción breve de BaseFac
/// </summary>
public class BaseFac
{
    public static void AddBase(string nombre, int responsableID, int alternateID)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(nombre))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Agrego la base.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Bases(Nombre, ResponsableID, AlternateID) VALUES (@Nombre, @ResponsableID, ";
            cmd.CommandText += "@AlternateID); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Bases;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableID", responsableID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AlternateID", alternateID));
            int baseID = Convert.ToInt32(cmd.ExecuteScalar());

            trans.Commit();
        }
        catch
        {
            if (trans != null) trans.Rollback();
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    public static void DeleteBase(int baseID)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Borro la base.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "DELETE FROM tbl_Bases WHERE BaseID = @BaseID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
            cmd.ExecuteNonQuery();

            // Borro los integrantes.
            DeleteBasePersonal(baseID, trans);
            
            trans.Commit();
        }
        catch
        {
            if (trans != null) trans.Rollback();
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    private static void DeleteBasePersonal(int baseID, IDbTransaction trans)
    {
        // Borro los integrantes.
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "DELETE FROM tbl_BasesPersonal WHERE BaseID = @BaseID";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
        cmd.ExecuteNonQuery();
    }

    internal static void AddBasePersonal(int baseID, int personalID, IDbTransaction trans)
    {
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "INSERT INTO tbl_BasesPersonal(BaseID, PersonalID) VALUES (@BaseID, @PersonalID)";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.ExecuteNonQuery();
    }

    internal static void DeletePersonalBase(int personalID, IDbTransaction trans)
    {
        // Borro los integrantes.
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "DELETE FROM tbl_BasesPersonal WHERE PersonalID = @PersonalID";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.ExecuteNonQuery();
    }

    public static void UpdateBase(int baseID, string nombre, int responsableID, int alternateID, bool activa)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(nombre))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Agrego la base.
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE tbl_Bases SET Nombre = @Nombre, ResponsableID = @ResponsableID, Activa = @Activa, ";
            cmd.CommandText += "AlternateID = @AlternateID WHERE BaseID = @BaseID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableID", responsableID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AlternateID", alternateID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", activa));
            cmd.ExecuteNonQuery();

            trans.Commit();
        }
        catch
        {
            if (trans != null) trans.Rollback();
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    public static Dictionary<int, string> GetBasesLista()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT BaseID, Nombre FROM tbl_Bases ORDER BY Nombre";
            dr = cmd.ExecuteReader();

            while (dr.Read()) result.Add(Convert.ToInt32(dr["BaseID"]), dr["Nombre"].ToString());

            dr.Close();
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }

    public static List<Base> GetBases()
    {
        return GetBases(false);
    }

    public static List<Base> GetBasesActivas()
    {
        return GetBases(true);
    }

    private static List<Base> GetBases(bool soloActivas)
    {
        List<Base> result = new List<Base>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT b.BaseID, b.Nombre AS BaseNombre, b.Activa, p.idPersonal AS ResponsableID, ";
            cmd.CommandText += "p.Nombre AS Responsable, p1.idPersonal AS AlternateID, p1.Nombre AS Alternate FROM tbl_Bases b ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = b.ResponsableID ";
            cmd.CommandText += "INNER JOIN tbl_Personal p1 ON p1.idPersonal = b.AlternateID ";
            if (soloActivas)
            {
                cmd.CommandText += "WHERE b.Activa = @Activa ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", soloActivas));
            }
            cmd.CommandText += "ORDER BY b.Nombre";
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                result.Add(new Base(Convert.ToInt32(dr["BaseID"]), dr["BaseNombre"].ToString(),
                                    Convert.ToInt32(dr["ResponsableID"]), dr["Responsable"].ToString(),
                                    Convert.ToInt32(dr["AlternateID"]),
                                    dr["Alternate"].ToString(), Convert.ToBoolean(dr["Activa"]), null));
            }
            dr.Close();

            result.ForEach(b => b.SetIntegrantes(GetBasePersonas(b.ID, conn)));
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }

    private static List<Persona> GetBasePersonas(int baseID, IDbConnection conn)
    {
        List<Persona> result = new List<Persona>();
        IDataReader dr = null;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, b.BaseID FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "INNER JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE b.BaseID = @BaseID AND l.Activo = @Activo AND p.Activo = @Activo ";
            cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Persona persona = GPersonal.GetPersona(dr);
                if (persona != null) result.Add(persona);
            }

            dr.Close();
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
        }

        return result;
    }

    public static Base GetBase(int baseID)
    {
        Base result;
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT b.BaseID, b.Nombre AS BaseNombre, b.Activa, p.idPersonal AS ResponsableID, ";
            cmd.CommandText += "p.Nombre AS Responsable, p1.idPersonal AS AlternateID, p1.Nombre AS Alternate FROM tbl_Bases b ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = b.ResponsableID ";
            cmd.CommandText += "INNER JOIN tbl_Personal p1 ON p1.idPersonal = b.AlternateID ";
            cmd.CommandText += "WHERE b.BaseID = @BaseID ORDER BY b.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@BaseID", baseID));
            dr = cmd.ExecuteReader();

            dr.Read();
            result = new Base(Convert.ToInt32(dr["BaseID"]), dr["BaseNombre"].ToString(),
                                    Convert.ToInt32(dr["ResponsableID"]), dr["Responsable"].ToString(),
                                    Convert.ToInt32(dr["AlternateID"]),
                                    dr["Alternate"].ToString(), Convert.ToBoolean(dr["Activa"]), null);
            dr.Close();

            result.SetIntegrantes(GetBasePersonas(result.ID, conn));
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
}