using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class GSParticipante
{
    // Variables.
    private int idParticipante;
    private string nombre;

    // Propiedades.
    public int IdParticipante
    {
        get { return this.idParticipante; }
    }
    public string Nombre
    {
        get { return this.nombre; }
    }


    internal GSParticipante(int idParticipante, string nombre)
    {
        this.idParticipante = idParticipante;
        this.nombre = nombre;
    }
}

public class GSVotosParticipante
{
    // Variables.
    private GSParticipante participante;
    private int votos;

    // Propiedades.
    public GSParticipante Participante
    {
        get { return this.participante; }
    }
    public int Votos
    {
        get { return this.votos; }
    }


    internal GSVotosParticipante(GSParticipante participante, int votos)
    {
        this.participante = participante;
        this.votos = votos;
    }
}

public class GSResultadoVotacion
{
    // Variables.
    private DateTime semana;
    private List<GSVotosParticipante> votos;

    // Propiedades.
    public DateTime Semana
    {
        get { return this.semana; }
    }
    public List<GSVotosParticipante> Votos
    {
        get { return this.votos; }
    }
    public GSVotosParticipante Ganador
    {
        get
        {
            GSVotosParticipante result;

            result = this.votos[0];
            foreach (GSVotosParticipante voto in this.votos)
            {
                if (voto.Votos > result.Votos)
                {
                    result = voto;
                }
            }

            return result;
        }
    }


    internal GSResultadoVotacion(DateTime semana, List<GSVotosParticipante> votos)
    {
        this.semana = semana;
        this.votos = votos;
    }
}
/// <summary>
/// Summary description for GranServaind
/// </summary>
public static class GranServaind
{
    /// <summary>
    /// Obtiene un participante.
    /// </summary>
    private static GSParticipante GetParticipante(IDataReader dr)
    {
        GSParticipante result;

        try
        {
            result = new GSParticipante(Convert.ToInt32(dr["idParticipante"]), dr["Nombre"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los participantes.
    /// </summary>
    public static List<GSParticipante> GetParticipantes()
    {
        List<GSParticipante> result = new List<GSParticipante>();
        IDbConnection conn = null;
        IDataReader dr;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_GS_Participantes ORDER BY Nombre";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                GSParticipante participante = GetParticipante(dr);
                if (participante != null)
                {
                    result.Add(participante);
                }
            }

            dr.Close();
        }
        catch
        {

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }


        return result;
    }
    /// <summary>
    /// Obtiene un participante.
    /// </summary>
    public static GSParticipante GetParticipante(int idParticipante)
    {
        GSParticipante result = null;
        IDbConnection conn = null;
        IDataReader dr;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 * FROM tbl_GS_Participantes WHERE idParticipante = @idParticipante";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParticipante", idParticipante));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                GSParticipante participante = GetParticipante(dr);
                if (participante != null)
                {
                    result = participante;
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
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
    /// <summary>
    /// Vota a un participante.
    /// </summary>
    public static void VotarParticipante(int idParticipante)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        DateTime fecha = DateTime.Now;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_GS_Votos(Anio, NumSemana, idPersonal, idParticipante) VALUES ";
            cmd.CommandText += "(@Anio, @NumSemana, @idPersonal, @idParticipante)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Anio", fecha.Year));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NumSemana", Funciones.GetNumeroSemana(fecha)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idParticipante", idParticipante));
            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
    /// <summary>
    /// Obtiene si el usuario actual puede votar.
    /// </summary>
    public static bool PuedeVotar()
    {
        bool result = false;
        IDbConnection conn = null;
        IDbCommand cmd;

        DateTime fecha = DateTime.Now;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT COUNT(idPersonal) FROM tbl_GS_Votos WHERE Anio = @Anio AND NumSemana = @NumSemana AND ";
            cmd.CommandText += "idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Anio", fecha.Year));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NumSemana", Funciones.GetNumeroSemana(fecha)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", Constantes.Usuario.ID));

            result = Convert.ToInt32(cmd.ExecuteScalar()) == 0;
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
    /// <summary>
    /// Obtiene el resultado de la votación de una semana.
    /// </summary>
    public static GSResultadoVotacion GetResultadoVotacion(DateTime fecha)
    {
        GSResultadoVotacion result = null;
        IDbConnection conn = null;
        IDataReader dr;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idParticipante, Nombre, ISNULL((SELECT COUNT(idPersonal) FROM tbl_GS_Votos WHERE ";
            cmd.CommandText += "idParticipante = p.idParticipante AND Anio = @Anio AND NumSemana = @NumSemana ";
            cmd.CommandText += "GROUP BY idParticipante), 0) AS Votos FROM tbl_GS_Participantes p ORDER BY Votos DESC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Anio", fecha.Year));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NumSemana", Funciones.GetNumeroSemana(fecha)));
            dr = cmd.ExecuteReader();

            List<GSVotosParticipante> votos = new List<GSVotosParticipante>();
            while (dr.Read())
            {
                GSVotosParticipante voto = new GSVotosParticipante(
                    new GSParticipante(Convert.ToInt32(dr["idParticipante"]), dr["Nombre"].ToString()),
                    Convert.ToInt32(dr["Votos"]));
                votos.Add(voto);
            }
            
            dr.Close();

            result = new GSResultadoVotacion(fecha , votos);
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
}