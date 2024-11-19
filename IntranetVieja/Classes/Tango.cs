using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de Tango
/// </summary>
public static class Tango
{
    public static bool ExisteCliente(string cliente)
    {
        bool result;
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT COUNT(ID_GVA14) FROM GVA14 WHERE NOM_COM = @Cliente;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Cliente", cliente));

            result = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
        catch
        {
            result = false;
        }
        finally
        {
            if(conn != null) conn.Close();
        }

        return result;
    }

    public static List<string> GetClientes(string filtro)
    {
        List<string> result = new List<string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        if (filtro == null || filtro.Length < 3) return result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Tango);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 10 NOM_COM FROM GVA14 WHERE NOM_COM LIKE @Filtro ORDER BY NOM_COM;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Filtro", '%' + filtro.ToUpper() + '%'));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(dr["NOM_COM"].ToString());
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
            if (conn != null) conn.Close();
        }

        return result;
    }
}