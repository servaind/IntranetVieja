/*
 * Historial:
 * ===================================================================================
 * [20/10/2011]
 * - Versión inicial.
 */

using System;
using System.Collections.Generic;
using System.Data;

public class SitioSSM
{
    // Variables.
    private int idSitio;
    private string nombre;
    private int[] idResponsables;
    private List<Persona> responsables;

    // Propiedades.
    public int IdSitio
    {
        get { return this.idSitio; }
    }
    public string Nombre
    {
        get { return this.nombre; } 
    }
    public int[] IdResponsables
    {
        get { return this.idResponsables; }
    }
    public List<Persona> Responsables
    {
        get
        {
            if (this.responsables == null)
            {
                this.responsables = new List<Persona>();
                foreach (int idResponsable in this.idResponsables)
                {
                    this.responsables.Add(GPersonal.GetPersona(idResponsable));
                }
            }

            return this.responsables;
        }
    }


    internal SitioSSM(int idSitio, string nombre, int[] idResponsables)
    {
        this.idSitio = idSitio;
        this.nombre = nombre;
        this.idResponsables = idResponsables;
    }
}

public class ItemSSM
{
    // Variables.
    private int idItem;
    private string nombre;
    private FrecuenciasSitio frecuencia;

    // Propiedades.
    public int IdItem
    {
        get { return this.idItem; }
    }
    public string Nombre
    {
        get { return this.nombre; }
    }
    public FrecuenciasSitio Frecuencia
    {
        get { return this.frecuencia; }
    }


    internal ItemSSM(int idItem, string nombre, FrecuenciasSitio frecuencia)
    {
        this.idItem = idItem;
        this.nombre = nombre;
        this.frecuencia = frecuencia;
    }
}

/// <summary>
/// Summary description for GSSM
/// </summary>
public static class GSSM
{
    /// <summary>
    /// Obtiene los items del SSM.
    /// </summary>
    public static List<ItemSSM> GetItems()
    {
        List<ItemSSM> result = new List<ItemSSM>();
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetItems(conn);
        }
        catch
        {

        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene los items del SSM.
    /// </summary>
    public static List<ItemSSM> GetItems(IDbConnection conn)
    {
        List<ItemSSM> result = new List<ItemSSM>();
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idItem, Nombre, Frecuencia FROM tbl_SitiosItems ORDER BY Nombre";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(new ItemSSM(Convert.ToInt32(dr["idItem"]), dr["Nombre"].ToString(), 
                    (FrecuenciasSitio)Convert.ToInt32(dr["Frecuencia"])));
            }
            dr.Close();
        }
        catch
        {

        }

        return result;
    }
    /// <summary>
    /// Obtiene un ítem del SSM.
    /// </summary>
    public static ItemSSM GetItem(int idItem)
    {
        ItemSSM result;
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetItem(idItem, conn);
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene un ítem del SSM.
    /// </summary>
    public static ItemSSM GetItem(int idItem, IDbConnection conn)
    {
        ItemSSM result = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Nombre, Frecuencia FROM tbl_SitiosItems WHERE idItem = @idItem";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idItem", idItem));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = new ItemSSM(idItem, dr["Nombre"].ToString(), (FrecuenciasSitio)Convert.ToInt32(dr["Frecuencia"]));
            }
            dr.Close();
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los sitios del SSM.
    /// </summary>
    public static List<SitioSSM> GetSitios()
    {
        List<SitioSSM> result = new List<SitioSSM>();
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetSitios(conn);
        }
        catch
        {

        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene los sitios del SSM.
    /// </summary>
    public static List<SitioSSM> GetSitios(IDbConnection conn)
    {
        List<SitioSSM> result = new List<SitioSSM>();
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idSitio, Nombre FROM tbl_Sitios ORDER BY Nombre";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int idSitio = Convert.ToInt32(dr["idSitio"]);
                string nombre = dr["Nombre"].ToString();
                result.Add(new SitioSSM(idSitio, nombre, GetResponsablesSitio(idSitio)));
            }
            dr.Close();
        }
        catch
        {

        }

        return result;
    }
    /// <summary>
    /// Obtien un sitio del SSM.
    /// </summary>
    public static SitioSSM GetSitio(int idSitio)
    {
        SitioSSM result;
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetSitio(idSitio, conn);
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtien un sitio del SSM.
    /// </summary>
    public static SitioSSM GetSitio(int idSitio, IDbConnection conn)
    {
        SitioSSM result = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Nombre FROM tbl_Sitios WHERE idSitio = @idSitio";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSitio", idSitio));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string nombre = dr["Nombre"].ToString();
                result = new SitioSSM(idSitio, nombre, GetResponsablesSitio(idSitio));
            }
            dr.Close();
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los responsables para el sitio.
    /// </summary>
    private static int[] GetResponsablesSitio(int idSitio)
    {
        List<int> result = new List<int>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idPersona FROM tbl_SitiosResponsables WHERE idSitio = @idSitio";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSitio", idSitio));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idPersona"]));
            }
            dr.Close();
        }
        catch
        {

        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene los estados de los sitios para el período seleccionado.
    /// </summary>
    public static Dictionary<ItemSSM, Dictionary<int, EstadosSitio>> GetEstadosSitios(int mes, int anio, List<SitioSSM> sitios)
    {
        Dictionary<ItemSSM, Dictionary<int, EstadosSitio>> result = new Dictionary<ItemSSM, Dictionary<int, EstadosSitio>>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            if (sitios.Count == 0)
            {
                throw new NoHayItemsException();
            }

            // Obtengo los ítems.
            List<ItemSSM> items = GetItems(conn);
            if (items.Count == 0)
            {
                throw new NoHayItemsException();
            }

            string sitiosItem = "";
            foreach (SitioSSM sitio in sitios)
            {
                sitiosItem += sitio.IdSitio + ",";
            }
            sitiosItem = sitiosItem.TrimEnd(',');

            // Por cada Sitio-Item obtengo el estado.
            foreach (ItemSSM item in items)
            {
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "SELECT idSitio, idItem, idEstado FROM tbl_SitiosEstados WHERE Periodo = @Periodo AND ";
                cmd.CommandText += "idItem = @idItem AND idSitio IN (" + sitiosItem + ")";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idItem", item.IdItem));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Periodo", String.Format("{0:00}/{1:0000}", mes, anio)));
                dr = cmd.ExecuteReader();

                Dictionary<int, EstadosSitio> estadosSitio = new Dictionary<int, EstadosSitio>();
                while (dr.Read())
                {
                    estadosSitio.Add(Convert.ToInt32(dr["idSitio"]), (EstadosSitio)Convert.ToInt32(dr["idEstado"]));
                }
                dr.Close();

                result.Add(item, estadosSitio);
            }
        }
        catch
        {

        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Agrega un estado a un sitio. Si el estado existe, lo actualiza.
    /// </summary>
    public static void AddEstadoSitio(int idSitio, int idItem, int mes, int anio, EstadosSitio estado)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbTransaction trans = null;

        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.SSM_Admin, Constantes.Usuario))
        {
            string msg = "Se produjo un error al intentar aplicar los cambios. Verifique que los datos ingresados "
                       + "sean válidos y que posee los permisos necesarios e intente nuevamente.<br /><br />Si el problema "
                       + "persiste, contáctese con el Área de Sistemas.";

            throw new Exception(msg);
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            ItemSSM item = GetItem(idItem, conn);
            if (item == null)
            {
                throw new ElementoInexistenteException();
            }

            trans = DataAccess.GetTransaction(conn);

            cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "SELECT COUNT(idSitio) FROM tbl_SitiosEstados WHERE Periodo = @Periodo AND idSitio = @idSitio AND ";
            cmd.CommandText += "idItem = @idItem";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSitio", idSitio));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Periodo", String.Format("{0:00}/{1:0000}", mes, anio)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idItem", idItem));
            bool insertar = Convert.ToInt32(cmd.ExecuteScalar()) == 0;

            if (insertar)
            {
                // Agregar el estado.
                int c;
                if (item.Frecuencia == FrecuenciasSitio.Anual)
                {
                    c = 12;
                    mes = 1;
                }
                else
                {
                    c = 1;
                }

                for (int i = 1; i <= c; i++)
                {
                    cmd = DataAccess.GetCommand(conn, trans);
                    cmd.CommandText = "INSERT INTO tbl_SitiosEstados (idSitio, Periodo, idItem, idEstado) VALUES ";
                    cmd.CommandText += "(@idSitio, @Periodo, @idItem, @idEstado)";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@idSitio", idSitio));
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@idItem", idItem));
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@Periodo", String.Format("{0:00}/{1:0000}", mes++, anio)));
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)estado));
                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                // Actualizar el estado.
                cmd = DataAccess.GetCommand(conn, trans);
                cmd.CommandText = "UPDATE tbl_SitiosEstados SET idEstado = @idEstado WHERE ";
                if (item.Frecuencia == FrecuenciasSitio.Anual)
                {
                    cmd.CommandText += "Periodo LIKE '%/" + String.Format("{0:0000}", anio) + "' ";
                }
                else
                {
                    cmd.CommandText += "Periodo = @Periodo ";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@Periodo", String.Format("{0:00}/{1:0000}", mes, anio)));
                }
                cmd.CommandText += "AND idSitio = @idSitio AND idItem = @idItem";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idSitio", idSitio));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idItem", idItem));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)estado));
                cmd.ExecuteNonQuery();
            }

            trans.Commit();
        }
        catch
        {
            if (trans != null)
            {
                trans.Rollback();
            }
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }
    /// <summary>
    /// Obtiene una cadena que representa el estado del sitio.
    /// </summary>
    /// <returns></returns>
    public static string EstadoSitioToString(EstadosSitio estado)
    {
        switch (estado)
        {
            case EstadosSitio.Cumplido:
                return "Cumplido";
            case EstadosSitio.NoAplica:
                return "No aplica";
            case EstadosSitio.NoCumplido:
                return "No cumplido";
        }

        return "-";
    }
    /// <summary>
    /// Envía un e-mail de recordatorio del cumplimiento del ítem.
    /// </summary>
    public static void EnviarEmailRecordatorio(int idSitio, int idItem)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_SSM_RECORDATORIO);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        ItemSSM item = GetItem(idItem);
        if (item == null)
        {
            throw new EmailException();
        }

        SitioSSM sitio = GetSitio(idSitio);
        if (sitio == null)
        {
            throw new EmailException();
        }

        string para = Constantes.EmailCalidad;
        string cc = "";
        sitio.Responsables.ForEach(r => cc += r.Email + ";");
        cc = cc.TrimEnd(';');

        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", "error");
        strPlantilla = strPlantilla.Replace("@ITEM", item.Nombre);

        string de = Constantes.EmailIntranet;
        if (Constantes.Usuario != null)
        {
            de = Constantes.Usuario.Email;
        }

        Email email = new Email(de, para, cc, "Recordatorio: " + item.Nombre, strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene los tipos de estado para los sitios.
    /// </summary>
    public static Dictionary<int, string> GetEstadosSitios()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosSitio.Cumplido, EstadoSitioToString(EstadosSitio.Cumplido));
        result.Add((int)EstadosSitio.NoAplica, EstadoSitioToString(EstadosSitio.NoAplica));
        result.Add((int)EstadosSitio.NoCumplido, EstadoSitioToString(EstadosSitio.NoCumplido));

        return result;
    }
}