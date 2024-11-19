using System;
using System.Collections.Generic;
using System.Data;
using System.Web;


public class TipoVehiculo
{
    // Variables.
    private int idTipoVehiculo;
    private string descripcion;

    // Propiedades.
    public int IdTipoVehiculo
    {
        get { return this.idTipoVehiculo; }
    }
    public string Descripcion
    {
        get { return this.descripcion; }
    }


    internal TipoVehiculo(int idTipoVehiculo, string descripcion)
    {
        this.idTipoVehiculo = idTipoVehiculo;
        this.descripcion = descripcion;
    }
}
/// <summary>
/// Summary description for TiposVehiculo
/// </summary>
public static class TiposVehiculo
{
    /// <summary>
    /// Obtiene un tipo de vehículo.
    /// </summary>
    private static TipoVehiculo GetTipoVehiculo(IDataReader dr)
    {
        TipoVehiculo result;

        try
        {
            result = new TipoVehiculo(Convert.ToInt32(dr["idTipoVehiculo"]), dr["Descripcion"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un tipo de vehículo.
    /// </summary>
    public static TipoVehiculo GetTipoVehiculo(int idTipoVehiculo)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        TipoVehiculo result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_TiposVehiculo WHERE idTipoVehiculo = @idTipoVehiculo";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idTipoVehiculo", idTipoVehiculo));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetTipoVehiculo(dr);
            }

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de vehículo.
    /// </summary>
    public static List<TipoVehiculo> GetTiposVehiculo()
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<TipoVehiculo> result = new List<TipoVehiculo>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_TiposVehiculo ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TipoVehiculo t = GetTipoVehiculo(dr);
                if (t != null)
                {
                    result.Add(t);
                }
            }

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
}