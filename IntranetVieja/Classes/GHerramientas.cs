/*
 * Historial:
 * ===================================================================================
 * [07/03/2012]
 * - Se le agregó una clasificación a las herramientas (Herramienta / Instrumento).
 * - Se le agregó número de instrumento a las herramientas.
 * [13/06/2011]
 * - Agregado seguimiento de calibración a las herramientas.
 * - Las herramientas poseen un número de EAC.
 * [12/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


public class Herramienta : IComparable
{
    // Variables.
    private int idHerramienta;
    private int numeroHerr;
    private string numeroInst;
    private TipoHerramienta tipoHerramienta;
    private string marca;
    private string descripcion;
    private string numSerie;
    private Persona personaACargo;
    private int numEAC;
    private bool activo;
    private List<ItemHerramienta> lstItems;
    private List<RegHistHerramienta> lstRegHistoricos;
    private List<EventoHerramienta> lstEventos;
    private CalibracionHerramienta calibracion;
    private ClasifHerramienta clasifHerramienta;

    // Propiedades.
    public int ID
    {
        get { return idHerramienta; }
    }
    public object NumeroHerramienta
    {
        get { return this.clasifHerramienta == ClasifHerramienta.Herramienta ? (object)this.numeroHerr : (object)this.numeroInst; }
    }
    public TipoHerramienta TipoHerramienta
    {
        get { return tipoHerramienta; }
    }
    public string Marca
    {
        get { return marca; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public string NumeroSerie
    {
        get { return numSerie; }
    }
    public List<ItemHerramienta> Items
    {
        get { return lstItems; }
    }
    public List<RegHistHerramienta> Historicos
    {
        get { return lstRegHistoricos; }
    }
    public List<EventoHerramienta> Eventos
    {
        get { return lstEventos; }
    }
    public Persona PersonaACargo
    {
        get { return personaACargo; }
    }
    public bool Activo
    {
        get { return activo; }
    }
    public int NumeroEAC
    {
        get { return this.numEAC; }
    }
    public CalibracionHerramienta Calibracion
    {
        get
        {
            if (this.calibracion == null)
            {
                this.calibracion = GHerramientas.GetCalibracionHerramienta(this.idHerramienta);
            }

            return this.calibracion;
        }
    }
    public ClasifHerramienta Clasificacion
    {
        get { return this.clasifHerramienta; }
    }


    internal Herramienta(int idHerramienta, TipoHerramienta tipoHerramienta, string marca, string descripcion, 
        string numSerie, Persona personaACargo, int numEAC, bool activo, ClasifHerramienta clasifHerramienta,
        int numeroHerr, string numeroInst)
    {
        this.idHerramienta = idHerramienta;
        this.tipoHerramienta = tipoHerramienta;
        this.marca = marca;
        this.descripcion = descripcion;
        this.numSerie = numSerie;
        this.activo = activo;
        this.personaACargo = personaACargo;
        this.lstItems = new List<ItemHerramienta>();
        this.lstRegHistoricos = new List<RegHistHerramienta>();
        this.lstEventos = new List<EventoHerramienta>();
        this.numEAC = numEAC;
        this.clasifHerramienta = clasifHerramienta;
        this.numeroHerr = numeroHerr;
        this.numeroInst = numeroInst;
    }
    /// <summary>
    /// Carga los ítems del grupo.
    /// </summary>
    public void CargarItems()
    {
        this.lstItems = GHerramientas.GetItemsHerramienta(this.idHerramienta, true);
    }
    /// <summary>
    /// Carga los registros históricos.
    /// </summary>
    public void CargarRegHistoricos()
    {
        this.lstRegHistoricos = GHerramientas.GetRegHistHerramienta(this.idHerramienta);
    }
    /// <summary>
    /// Carga los eventos asociados.
    /// </summary>
    public void CargarEventos()
    {
        this.lstEventos = GHerramientas.GetEventosHerramienta(this.idHerramienta, true);
    }
    public override string ToString()
    {
        return this.tipoHerramienta.Descripcion + 
            (this.marca.Length > 0 ? " " + this.marca : "") + 
            (this.descripcion.Length > 0 ? " " + this.descripcion : "");
    }
    public int CompareTo(Object o)
    {
        int result;
        Herramienta g = (Herramienta)o;

        result = this.TipoHerramienta.Descripcion.CompareTo(g.TipoHerramienta.Descripcion);
        if (result == 0)
        {
            result = this.marca.CompareTo(g.Marca);
            if (result == 0)
            {
                result = this.descripcion.CompareTo(g.Descripcion);
            }
        }

        return result;
    }
}
public class TipoHerramienta
{
    // Variables.
    private short idTipo;
    private string descripcion;

    // Propiedades.
    public short ID
    {
        get { return idTipo; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }


    internal TipoHerramienta(short idTipo, string descripcion)
    {
        this.idTipo = idTipo;
        this.descripcion = descripcion;
    }
}
public class RegHistHerramienta
{
    // Variables.
    private int idRegistro;
    private int idHerramienta;
    private DateTime fecha;
    private Persona personaCargo;
    private string ubicacion;
    private string descripcion;
    private Persona personaRegistro;

    // Propiedades.
    public int ID
    {
        get { return idRegistro; }
    }
    public int IDHerramienta
    {
        get { return idHerramienta; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
    }
    public Persona PersonaACargo
    {
        get { return personaCargo; }
    }
    public string Ubicacion
    {
        get { return ubicacion; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public Persona PersonaRegistro
    {
        get { return personaRegistro; }
    }


    internal RegHistHerramienta(int idRegistro, int idHerramienta, DateTime fecha, 
        Persona personaCargo, string ubicacion, string descripcion, Persona personaRegistro)
    {
        this.idRegistro = idRegistro;
        this.idHerramienta = idHerramienta;
        this.fecha = fecha;
        this.personaCargo = personaCargo;
        this.ubicacion = ubicacion;
        this.descripcion = descripcion;
        this.personaRegistro = personaRegistro;
    }
}
public class ItemHerramienta
{
    // Variables.
    private int idItem;
    private int idHerramienta;
    private string marca;
    private string descripcion;
    private int cantidad;
    private bool activo;

    // Propiedades.
    public int ID
    {
        get { return idItem; }
    }
    public int IDHerramienta
    {
        get { return idHerramienta; }
    }
    public string Marca
    {
        get { return marca; }
        set { this.marca = value; }
    }
    public string Descripcion
    {
        get { return descripcion; }
        set { this.descripcion = value; }
    }
    public int Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
    }
    public bool Activo
    {
        get { return activo; }
    }


    internal ItemHerramienta(int idItem, int idHerramienta, string marca, string descripcion, int cantidad, 
        bool activo)
    {
        this.idItem = idItem;
        this.idHerramienta = idHerramienta;
        this.marca = marca;
        this.descripcion = descripcion;
        this.cantidad = cantidad;
        this.activo = activo;
    }
    public ItemHerramienta(int idHerramienta, string marca, string descripcion, int cantidad)
        : this(Constantes.ValorInvalido, idHerramienta, marca, descripcion, cantidad, true)
    {

    }
}
public class EventoHerramienta
{
    // Variables.
    private int idEvento;
    private int idHerramienta;
    private string descripcion;
    private DateTime fecha;
    private bool activo;

    // Propiedadaes.
    public int ID
    {
        get { return idEvento; }
    }
    public int IDHerramienta
    {
        get { return idHerramienta; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
    }
    public bool Activo
    {
        get { return activo; }
    }


    internal EventoHerramienta(int idEvento, int idHerramienta, string descripcion, DateTime fecha, 
        bool activo)
    {
        this.idEvento = idEvento;
        this.idHerramienta = idHerramienta;
        this.descripcion = descripcion;
        this.fecha = fecha;
        this.activo = activo;
    }
}
public class CalibracionHerramienta
{
    // Variables.
    private int idCalibracion;
    private FrecCalHerramienta frecuencia;
    private DateTime ultCalibracion;
    private DateTime proxCalibracion;
    private TiposCalHerramienta tipoCalibracion;
    private string observaciones;

    // Propiedades.
    public int IdCalibracion
    {
        get { return this.idCalibracion; }
    }
    public FrecCalHerramienta Frecuencia
    {
        get { return this.frecuencia; }
    }
    public DateTime UltimaCalibracion
    {
        get { return this.ultCalibracion; }
    }
    public DateTime ProximaCalibracion
    {
        get { return this.proxCalibracion; }
    }
    public TiposCalHerramienta TipoCalibracion
    {
        get { return this.tipoCalibracion; }
    }
    public string Observaciones
    {
        get { return this.observaciones; }
    }


    internal CalibracionHerramienta(int idCalibracion, FrecCalHerramienta frecuencia, DateTime ultCalibracion, 
        DateTime proxCalibracion, TiposCalHerramienta tipoCalibracion, string observaciones)
    {
        this.idCalibracion = idCalibracion;
        this.frecuencia = frecuencia;
        this.ultCalibracion = ultCalibracion;
        this.proxCalibracion = proxCalibracion;
        this.tipoCalibracion = tipoCalibracion;
        this.observaciones = observaciones;
    }
}

/// <summary>
/// Descripción breve de GGrupoHerramientas
/// </summary>
public static class GHerramientas
{
    // Constantes.
    private const int MaxRegistrosPagina = 20;
    private const int MaxRegistrosHistPagina = 5;
    private const string PathDatosHerramientas = "\\server-storage\\";
    private const int DiasPreavisoEventos = 30;


    /// <summary>
    /// Obtiene una herramienta.
    /// </summary>
    private static Herramienta GetHerramienta(IDataReader dr)
    {
        Herramienta result = null;

        try
        {
            result = new Herramienta(
                Convert.ToInt32(dr["idHerramienta"]),
                GetTipoHerramienta(Convert.ToInt16(dr["idTipoHerramienta"])),
                dr["Marca"].ToString(),
                dr["Descripcion"].ToString(),
                dr["NumSerie"].ToString(),
                GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaCargo"])),
                Convert.ToInt32(dr["NumEAC"]),
                Convert.ToBoolean(dr["Activo"]),
                (ClasifHerramienta)Convert.ToInt32(dr["Clasificacion"]),
                Convert.ToInt32(dr["NumeroHerr"]),
                dr["NumeroInst"].ToString()
                );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una herramienta.
    /// </summary>
    private static Herramienta GetHerramienta(DataRow dr)
    {
        Herramienta result = null;

        try
        {
            result = new Herramienta(
                Convert.ToInt32(dr["idHerramienta"]),
                GetTipoHerramienta(Convert.ToInt16(dr["idTipoHerramienta"])),
                dr["Marca"].ToString(),
                dr["Descripcion"].ToString(),
                dr["NumSerie"].ToString(),
                GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaCargo"])),
                Convert.ToInt32(dr["NumEAC"]),
                Convert.ToBoolean(dr["Activo"]),
                (ClasifHerramienta)Convert.ToInt32(dr["Clasificacion"]),
                Convert.ToInt32(dr["NumeroHerr"]),
                dr["NumeroInst"].ToString()
                );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un ítem de la herramienta.
    /// </summary>
    private static ItemHerramienta GetItemHerramienta(SqlDataReader dr)
    {
        ItemHerramienta result = null;

        try
        {
            result = new ItemHerramienta(
                    Convert.ToInt32(dr["idItem"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    dr["Marca"].ToString(),
                    dr["Descripcion"].ToString(),
                    Convert.ToInt32(dr["Cantidad"]),
                    Convert.ToBoolean(dr["Activo"])
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un ítem de la herramienta.
    /// </summary>
    private static ItemHerramienta GetItemHerramienta(DataRow dr)
    {
        ItemHerramienta result = null;

        try
        {
            result = new ItemHerramienta(
                    Convert.ToInt32(dr["idItem"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    dr["Marca"].ToString(),
                    dr["Descripcion"].ToString(),
                    Convert.ToInt32(dr["Cantidad"]),
                    Convert.ToBoolean(dr["Activo"])
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un evento de la herramienta.
    /// </summary>
    private static EventoHerramienta GetEventoHerramienta(SqlDataReader dr)
    {
        EventoHerramienta result = null;

        try
        {
            result = new EventoHerramienta(
                    Convert.ToInt32(dr["idEvento"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    dr["Descripcion"].ToString(),
                    Convert.ToDateTime(dr["Fecha"]),
                    Convert.ToBoolean(dr["Activo"])
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un evento de la herramienta.
    /// </summary>
    private static EventoHerramienta GetEventoHerramienta(DataRow dr)
    {
        EventoHerramienta result = null;

        try
        {
            result = new EventoHerramienta(
                    Convert.ToInt32(dr["idEvento"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    dr["Descripcion"].ToString(),
                    Convert.ToDateTime(dr["Fecha"]),
                    Convert.ToBoolean(dr["Activo"])
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un registro histórico de la herramienta.
    /// </summary>
    private static RegHistHerramienta GetRegHistHerramienta(SqlDataReader dr)
    {
        RegHistHerramienta result = null;

        try
        {
            result = new RegHistHerramienta(
                    Convert.ToInt32(dr["idRegistro"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    Convert.ToDateTime(dr["Fecha"]),
                    GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaCargo"])),
                    dr["Ubicacion"].ToString(),
                    dr["Descripcion"] as string,
                    GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaRegistro"]))
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene un registro histórico de la herramienta.
    /// </summary>
    private static RegHistHerramienta GetRegHistHerramienta(DataRow dr)
    {
        RegHistHerramienta result = null;

        try
        {
            result = new RegHistHerramienta(
                    Convert.ToInt32(dr["idRegistro"]),
                    Convert.ToInt32(dr["idHerramienta"]),
                    Convert.ToDateTime(dr["Fecha"]),
                    GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaCargo"])),
                    dr["Ubicacion"].ToString(),
                    dr["Descripcion"] as string,
                    GPersonal.GetPersona(Convert.ToInt32(dr["idPersonaRegistro"]))
                    );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene la calibración para la herramienta.
    /// </summary>
    private static CalibracionHerramienta GetCalibracionHerramienta(IDataReader dr)
    {
        CalibracionHerramienta result;

        try
        {
            result = new CalibracionHerramienta(Convert.ToInt32(dr["idCalibracion"]),
                (FrecCalHerramienta)Convert.ToInt32(dr["FrecCalibracion"]),
                Convert.ToDateTime(dr["UltCalibracion"]), Convert.ToDateTime(dr["ProxCalibracion"]),
                (TiposCalHerramienta)Convert.ToInt32(dr["TipoCalibracion"]),
                dr["Observaciones"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene la calibración para la herramienta.
    /// </summary>
    private static CalibracionHerramienta GetCalibracionHerramienta(DataRow dr)
    {
        CalibracionHerramienta result;

        try
        {
            result = new CalibracionHerramienta(Convert.ToInt32(dr["idCalibracion"]),
                (FrecCalHerramienta)Convert.ToInt32("FrecCalibracion"),
                Convert.ToDateTime(dr["UltCalibracion"]), Convert.ToDateTime(dr["ProxCalibracion"]),
                (TiposCalHerramienta)Convert.ToInt32(dr["TipoCalibracion"]),
                dr["Observaciones"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una fila para el panel de calibración.
    /// </summary>
    private static object[] GetFilaPanelCalibracion(DataRow dr)
    {
        object[] result;

        try
        {
            result = new object[] { 
                dr["idHerramienta"], dr["TipoInstrumento"], dr["Descripcion"], dr["Marca"], dr["NumSerie"], 
                dr["FrecCalibracion"], dr["UltCalibracion"], dr["ProxCalibracion"], dr["Ubicacion"], dr["TipoCalibracion"], 
                dr["Observaciones"], dr["NumeroInst"], dr["PersonaCargo"]
            };
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una herramienta.
    /// </summary>
    public static Herramienta GetHerramienta(int idHerramienta)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlDataReader dr;
        Herramienta result = null;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT *, (SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos ";
            cmd.CommandText += "WHERE idHerramienta = h.idHerramienta ORDER BY Fecha DESC) AS idPersonaCargo ";
            cmd.CommandText += "FROM tbl_Herramientas h WHERE h.idHerramienta = @idHerramienta";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                dr.Close();
                throw new Exception("La herramienta no existe.");
            }

            result = GetHerramienta(dr);

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene una herramienta.
    /// </summary>
    public static Herramienta GetHerramienta(SqlConnection conn, int idHerramienta)
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        Herramienta result = null;

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = "SELECT *, (SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos ";
            cmd.CommandText += "WHERE idHerramienta = h.idHerramienta ORDER BY Fecha DESC) AS idPersonaCargo ";
            cmd.CommandText += "FROM tbl_Herramientas h WHERE h.idHerramienta = @idHerramienta";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                dr.Close();
                throw new Exception("La herramienta no existe.");
            }

            result = GetHerramienta(dr);

            dr.Close();
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene las herramientas.
    /// </summary>
    public static Hashtable GetHerramientas(int[] idHerramientas)
    {
        SqlConnection conn = new SqlConnection();
        Hashtable result = new Hashtable(idHerramientas.Length);

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
        }
        catch
        {

        }

        foreach (int idHerramienta in idHerramientas)
        {
            result.Add(idHerramienta, GetHerramienta(conn, idHerramienta));
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Obtiene el tipo de herramienta.
    /// </summary>
    public static TipoHerramienta GetTipoHerramienta(short idTipoHerramienta)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        TipoHerramienta result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_TipoHerramienta WHERE idTipoHerramienta = @idTipo";
            cmd.Parameters.Add("@idTipo", SqlDbType.SmallInt, 2).Value = idTipoHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                // No existe el tipo.
                throw new Exception("El tipo de herramienta no existe.");
            }

            result = new TipoHerramienta(idTipoHerramienta, dr["Descripcion"].ToString());

            dr.Close();
        }
        catch
        {
            result = new TipoHerramienta(Constantes.ValorInvalido, "");
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene una lista de las herramientas activas.
    /// </summary>
    public static List<int> GetHerramientas()
    {
        IDbConnection conn = null;
        List<int> result = new List<int>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idHerramienta FROM tbl_Herramientas WHERE Activo = @Activo ORDER BY idHerramienta";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            IDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idHerramienta"]));
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
    /// Obtiene el listado de herramientas que apliquen a los filtros.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<Herramienta> GetHerramientas(int pagina, List<Filtro> filtros, bool activas)
    {
        List<Herramienta> result = new List<Herramienta>();

        result = DataAccess.GetDataList<Herramienta>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetHerramienta);

        return result;
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        return GetConsultaFiltro(filtros, cantidad, true);
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad, bool soloActivas)
    {
        // Cuando quiero traer las herramientas sin filtrar las por persona a cargo, adjunto esta subconsulta.
        // En cambio, si quiero filtrar por persona a cargo, tengo que cambiar el auxConsulta = idPersonaCargo
        // porque sino se ejecuta la misma consulta 2 veces por cada fila de herramientas :/
        string auxConsulta = "(SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos WHERE "
                             + "idHerramienta = h.idHerramienta ORDER BY Fecha DESC, idRegistro DESC) ";
        string filtroJoin = "";
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosHerramienta.Marca:
                    filtroWhere += "AND h.Marca LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.Descripcion:
                    filtroWhere += "AND h.Descripcion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.PersonaCargo:
                    auxConsulta = "'" + filtro.Valor + "'";
                    filtroWhere += "AND (SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos ";
                    filtroWhere += "WHERE idHerramienta = h.idHerramienta ORDER BY Fecha DESC, idRegistro DESC) = ";
                    filtroWhere += filtro.Valor + " ";
                    break;
                case (int)FiltrosHerramienta.Tipo:
                    filtroJoin = "INNER JOIN tbl_TipoHerramienta t ON h.idTipoHerramienta = t.idTipoHerramienta";
                    filtroWhere = "AND t.Descripcion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.NumeroHerramienta:
                    filtroWhere += "AND h.NumeroHerr = " + filtro.Valor + " ";
                    break;
                default:
                    filtroJoin = "";
                    filtroWhere = "";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idHerramienta) as TotalRegistros FROM tbl_Herramientas h " + filtroJoin + " ";
            consulta += "WHERE Activo = " + (soloActivas ? "1" : "0");
            if (filtroWhere.Length > 0)
            {
                consulta += " AND " + filtroWhere;
            }
            consulta += " AND h.Clasificacion = " + (int)ClasifHerramienta.Herramienta;
        }
        else
        {
            consulta = "SELECT *, " + auxConsulta;
            consulta += "AS idPersonaCargo FROM tbl_Herramientas h " + filtroJoin + " ";
            consulta += "WHERE Activo = " + (soloActivas ? "1" : "0");
            if (filtroWhere.Length > 0)
            {
                consulta += " AND " + filtroWhere;
            }
            consulta += " AND h.Clasificacion = " + (int)ClasifHerramienta.Herramienta;
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas de herramientas que apliquen al filtro.
    /// </summary>
    public static int GetCantidadPaginasHerramientas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
    }
    /// <summary>
    /// Obtiene los tipos de herramientas.
    /// </summary>
    public static List<TipoHerramienta> GetTiposHerramientas()
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlDataReader dr;
        List<TipoHerramienta> result = new List<TipoHerramienta>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_TipoHerramienta ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                dr.Close();
                throw new Exception("No hay tipos de herramientas.");
            }

            while (dr.Read())
            {
                TipoHerramienta aux = new TipoHerramienta(
                    Convert.ToInt16(dr["idTipoHerramienta"]),
                    dr["Descripcion"].ToString()
                    );

                result.Add(aux);
            }

            dr.Close();
        }
        catch
        {

        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene la lista de ítems para la herramienta.
    /// </summary>
    internal static List<ItemHerramienta> GetItemsHerramienta(int idHerramienta, bool activos)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlDataReader dr;
        List<ItemHerramienta> result = new List<ItemHerramienta>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_HerramientasItems WHERE idHerramienta = @idHerramienta ";
            cmd.CommandText += activos ? "AND Activo = 1 " : "";
            cmd.CommandText += "ORDER BY Descripcion";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                dr.Close();
                throw new Exception("No hay items para la herramienta.");
            }

            while (dr.Read())
            {
                ItemHerramienta aux = GetItemHerramienta(dr);

                if (aux != null)
                {
                    result.Add(aux);
                }
            }

            dr.Close();
        }
        catch
        {

        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene la lista de registros históricos para la herramienta.
    /// </summary>    
    public static List<RegHistHerramienta> GetRegHistHerramienta(int idHerramienta)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlDataReader dr;
        List<RegHistHerramienta> result = new List<RegHistHerramienta>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_HerramientasHistoricos WHERE idHerramienta = @idHerramienta ";
            cmd.CommandText += "ORDER BY Fecha DESC, idRegistro DESC";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                dr.Close();
                throw new Exception("No hay items para la herramienta.");
            }

            while (dr.Read())
            {
                RegHistHerramienta aux = GetRegHistHerramienta(dr);
                if (aux != null)
                {
                    result.Add(aux);
                }
            }

            dr.Close();
        }
        catch
        {
            
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene la lista de registros históricos para la herramienta.
    /// </summary>
    public static List<RegHistHerramienta> GetRegHistHerramienta(int pagina, int idHerramienta)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbDataAdapter adap;
        DataSet ds = new DataSet();
        List<RegHistHerramienta> result = new List<RegHistHerramienta>();

        pagina = pagina - 1;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_HerramientasHistoricos WHERE idHerramienta = @idHerramienta ";
            cmd.CommandText += "ORDER BY Fecha DESC, idRegistro DESC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idHerramienta", idHerramienta));
            adap = DataAccess.GetDataAdapter(cmd);
            ((System.Data.Common.DbDataAdapter)adap).Fill(ds, pagina * MaxRegistrosHistPagina, MaxRegistrosHistPagina, "RegHist");

            if (ds.Tables["RegHist"].Rows.Count > 0)
            {
                foreach (DataRow fila in ds.Tables["RegHist"].Rows)
                {
                    RegHistHerramienta aux = GetRegHistHerramienta(fila);
                    if (aux != null)
                    {
                        result.Add(aux);
                    }
                }
            }
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
    /// Obtiene la cantidad de páginas de registros históricos que apliquen al filtro.
    /// </summary>
    public static int GetCantidadPaginasRegHistHerramienta(int idHerramienta)
    {
        string query = "SELECT COUNT(idHerramienta) AS TotalRegistros FROM tbl_HerramientasHistoricos WHERE idHerramienta = " + idHerramienta;

        return Funciones.CantidadPaginas(query, MaxRegistrosHistPagina);
    }
    /// <summary>
    /// Obtiene la lista de eventos para la herramienta.
    /// </summary>    
    public static List<EventoHerramienta> GetEventosHerramienta(int idHerramienta, bool activos)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        List<EventoHerramienta> result = new List<EventoHerramienta>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_HerramientasEventos WHERE idHerramienta = @idHerramienta ";
            cmd.CommandText += activos ? "AND Activo = 1 " : "";
            cmd.CommandText += "ORDER BY Fecha DESC";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                // No existe el tipo.
                throw new Exception("No hay eventos para la herramienta.");
            }

            while (dr.Read())
            {
                EventoHerramienta aux = GetEventoHerramienta(dr);
                if (aux != null)
                {
                    result.Add(aux);
                }
            }

            dr.Close();
        }
        catch
        {
            
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene la lista de eventos para la herramienta.
    /// </summary>
    public static List<EventoHerramienta> GetEventosHerramienta(int pagina, int idHerramienta)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbDataAdapter adap;
        DataSet ds = new DataSet();
        List<EventoHerramienta> result = new List<EventoHerramienta>();

        pagina = pagina - 1;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_HerramientasEventos WHERE idHerramienta = @idHerramienta ";
            cmd.CommandText += "AND Activo = 1 ORDER BY Fecha DESC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idHerramienta", idHerramienta));
            adap = DataAccess.GetDataAdapter(cmd);
            ((System.Data.Common.DbDataAdapter)adap).Fill(ds, pagina * MaxRegistrosHistPagina, MaxRegistrosHistPagina, "Eventos");

            if (ds.Tables["Eventos"].Rows.Count > 0)
            {
                foreach (DataRow fila in ds.Tables["Eventos"].Rows)
                {
                    EventoHerramienta aux = GetEventoHerramienta(fila);
                    if (aux != null)
                    {
                        result.Add(aux);
                    }
                }
            }
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
    /// Obtiene la cantidad de páginas de eventos que apliquen al filtro.
    /// </summary>
    public static int GetCantidadPaginasEventosHerramienta(int idHerramienta)
    {
        string query = "SELECT * FROM tbl_HerramientasEventos WHERE idHerramienta = " + idHerramienta + " ";
        query += "AND Activo = 1"; ;

        return Funciones.CantidadPaginas(query, MaxRegistrosHistPagina);
    }
    /// <summary>
    /// Obtiene un evento de la herramienta.
    /// </summary>
    public static EventoHerramienta GetEventoHerramienta(int idEvento)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        EventoHerramienta result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_HerramientasEventos WHERE idEvento = @idEvento";
            cmd.Parameters.Add("@idEvento", SqlDbType.Int, 4).Value = idEvento;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                // No existe el evento.
                throw new Exception("El evento no existe.");
            }

            result = GetEventoHerramienta(dr);

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene las descripciones de las herramientas.
    /// </summary>
    public static Hashtable GetDescripcionHerramientas(int[] idHerramientas)
    {
        SqlConnection conn = new SqlConnection();
        Hashtable result = new Hashtable(idHerramientas.Length);

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
        }
        catch
        {
            
        }

        foreach (int idHerramienta in idHerramientas)
        {
            result.Add(idHerramienta, GetDescripcionHerramienta(conn, idHerramienta));
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Obtiene la descripción de una herramienta.
    /// </summary>
    public static string GetDescripcionHerramienta(SqlConnection conn, int idHerramienta)
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        string result = "<no disponible>";

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT Marca, Descripcion, NumSerie ";
            cmd.CommandText += "FROM tbl_Herramientas h WHERE h.idHerramienta = @idHerramienta";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                dr.Close();
                throw new Exception("La herramienta no existe.");
            }

            result = "[" + idHerramienta.ToString("0000") + "]" + dr["Marca"].ToString() + " " + dr["Descripcion"].ToString() 
                + " " + dr["NumSerie"].ToString();

            dr.Close();
        }
        catch
        {
            result = "<no disponible>";
        }

        return result;
    }
    /// <summary>
    /// Obtiene los eventos disponibles para la fecha indicada.
    /// </summary>
    public static List<EventoHerramienta> GetEventosHerramientas(DateTime fecha)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        List<EventoHerramienta> result = new List<EventoHerramienta>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_HerramientasEventos WHERE Fecha = @Fecha ";
            cmd.CommandText += "AND Activo = 1 ORDER BY Fecha DESC";
            cmd.Parameters.Add("@Fecha", SqlDbType.SmallDateTime, 4).Value = new DateTime(fecha.Year, fecha.Month, fecha.Day);
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                throw new Exception("No hay eventos para la fecha indicada.");
            }

            while (dr.Read())
            {
                EventoHerramienta aux = GetEventoHerramienta(dr);
                if (aux != null)
                {
                    result.Add(aux);
                }
            }

            dr.Close();
        }
        catch
        {

        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene el último registro histórico de la herramienta.
    /// </summary>
    public static RegHistHerramienta GetUltimoRegHistHerramienta(int idHerramienta)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection conn = new SqlConnection();
        SqlDataReader dr;
        RegHistHerramienta result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT TOP 1 * FROM tbl_HerramientasHistoricos WHERE ";
            cmd.CommandText += "idHerramienta = @idHerramienta ORDER BY Fecha DESC, idRegistro DESC";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                dr.Close();
                throw new Exception("No hay items para la herramienta.");
            }

            result = GetRegHistHerramienta(dr);

            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Agrega una nueva herramienta.
    /// </summary>
    public static bool NuevaHerramienta(int idTipoHerramienta, string marca,
        string descripcion, string numSerie, int idPersonaACargo, string ubicacion, int numEAC, int idPersonaRegistro, 
        ClasifHerramienta clasificacion, string numeroInst, out int idHerramienta)
    {
        return NuevaHerramienta(idTipoHerramienta, marca, descripcion, numSerie, idPersonaACargo,
            ubicacion, numEAC, clasificacion, numeroInst, null, idPersonaRegistro, out idHerramienta);
    }
    /// <summary>
    /// Agrega una nueva herramienta.
    /// </summary>
    public static bool NuevaHerramienta(int idTipoHerramienta, string marca,
        string descripcion, string numSerie, int idPersonaACargo, string ubicacion, int numEAC,
        ClasifHerramienta clasificacion, string numeroInst, List<ItemHerramienta> lstItems, int idPersonaRegistro, 
        out int idHerramienta)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        idHerramienta = Constantes.ValorInvalido;

        if (clasificacion == ClasifHerramienta.Instrumento && String.IsNullOrEmpty(numeroInst))
        {
            return false;
        }

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if ((idHerramienta = InsertarHerramienta(conn, trans, idTipoHerramienta, marca, 
                descripcion, numSerie, numEAC, clasificacion, numeroInst)) == Constantes.ValorInvalido)
            {
                throw new Exception("No se pudo insertar la herramienta.");
            }

            if (lstItems != null && lstItems.Count > 0)
            {
                foreach (ItemHerramienta item in lstItems)
                {
                    if (!InsertarItemHerramienta(conn, trans, idHerramienta, item))
                    {
                        throw new Exception("No se pudo insertar el item para la herramienta.");
                    }
                }
            }

            if (!NuevoRegHistHerramienta(conn, trans, idHerramienta, DateTime.Now, idPersonaACargo,
                ubicacion, "", idPersonaRegistro))
            {
                throw new Exception("No se pudo insertar el registro histórico para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Inserta una herramienta.
    /// </summary>
    private static int InsertarHerramienta(SqlConnection conn, SqlTransaction trans, 
        int idTipoHerramienta, string marca, string descripcion, string numSerie, int numEAC,
        ClasifHerramienta clasificacion, string numeroInst)
    {
        SqlCommand cmd = new SqlCommand();
        int result = Constantes.ValorInvalido;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "INSERT INTO tbl_Herramientas (idTipoHerramienta, Marca, Descripcion, ";
            cmd.CommandText += "NumSerie, NumEAC, Clasificacion, NumeroHerr, NumeroInst) VALUES (@idTipoHerramienta, @Marca, ";
            cmd.CommandText += "@Descripcion, @NumSerie, @NumEAC, @Clasificacion, @NumeroHerr, @NumeroInst);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Herramientas;";
            cmd.Parameters.Add("@idTipoHerramienta", SqlDbType.SmallInt, 2).Value = idTipoHerramienta;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 20).Value = marca;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = descripcion;
            cmd.Parameters.Add("@NumSerie", SqlDbType.VarChar, 15).Value = numSerie;
            cmd.Parameters.Add("@NumEAC", SqlDbType.Int, 4).Value = numEAC;
            cmd.Parameters.Add("@Clasificacion", SqlDbType.TinyInt, 1).Value = (int)clasificacion;
            cmd.Parameters.Add("@NumeroHerr", SqlDbType.Int, 4).Value = clasificacion == ClasifHerramienta.Herramienta ?
                (GetUltimoNumeroHerramienta(trans) + 1) : Constantes.ValorInvalido;
            cmd.Parameters.Add("@NumeroInst", SqlDbType.VarChar, 10).Value = clasificacion == ClasifHerramienta.Instrumento ?
                numeroInst : "";

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            result = Constantes.ValorInvalido;
        }

        return result;
    }
    /// <summary>
    /// Obtiene el último número de herramienta.
    /// </summary>
    private static int GetUltimoNumeroHerramienta(SqlTransaction trans)
    {
        int result;

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = trans.Connection;
        cmd.Transaction = trans;
        cmd.CommandText = "SELECT TOP 1 NumeroHerr FROM tbl_Herramientas WHERE Clasificacion = @Clasificacion ORDER BY idHerramienta DESC";
        cmd.Parameters.Add("@Clasificacion", SqlDbType.TinyInt, 1).Value = (int)ClasifHerramienta.Herramienta;
        result = Convert.ToInt32(cmd.ExecuteScalar());

        return result;
    }
    /// <summary>
    /// Inserta un item para una herramienta.
    /// </summary>
    private static bool InsertarItemHerramienta(SqlConnection conn, SqlTransaction trans, 
        int idHerramienta, ItemHerramienta item)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "INSERT INTO tbl_HerramientasItems (idHerramienta, Marca, Descripcion, ";
            cmd.CommandText += "Cantidad) VALUES (@idHerramienta, @Marca, @Descripcion, @Cantidad);";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 2).Value = idHerramienta;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 20).Value = item.Marca;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = item.Descripcion;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Int, 4).Value = item.Cantidad;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Actualiza una herramienta.
    /// </summary>
    public static bool ActualizarHerramienta(int idHerramienta, int idTipoHerramienta, string marca, string descripcion, 
        string numSerie, int numEAC, ClasifHerramienta clasificacion, string numeroInst, List<ItemHerramienta> lstItems, string motivo)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        if (clasificacion == ClasifHerramienta.Instrumento && String.IsNullOrEmpty(numeroInst))
        {
            return false;
        }

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!ActualizarHerramienta(conn, trans, idHerramienta, idTipoHerramienta, marca, descripcion, numSerie, numEAC,
                clasificacion, numeroInst))
            {
                throw new Exception("No se pudo actualizar la herramienta. [Actualizar herramienta]");
            }

            if (!EliminarItemsHerramienta(conn, trans, idHerramienta, lstItems))
            {
                throw new Exception("No se pudo actualizar la herramienta. [Borrar items]");
            }

            foreach (ItemHerramienta item in lstItems)
            {
                if (item.ID != Constantes.ValorInvalido)
                {
                    if (!ActualizarItemHerramienta(conn, trans, item))
                    {
                        throw new Exception("No se pudo actualizar la herramienta. [Actualizar item]");
                    }
                }
                else
                {
                    if (!InsertarItemHerramienta(conn, trans, idHerramienta, item))
                    {
                        throw new Exception("No se pudo actualizar la herramienta. [Insertar item]");
                    }
                }
            }

            if (!NuevoRegActividad(conn, trans, idHerramienta, motivo, Constantes.Usuario.ID))
            {
                throw new Exception("No se pudo actualizar la herramienta. [Registro de actividad]");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Actualiza un item para una herramienta.
    /// </summary>
    private static bool ActualizarHerramienta(SqlConnection conn, SqlTransaction trans, int idHerramienta, int idTipoHerramienta, 
        string marca, string descripcion, string numSerie, int numEAC, ClasifHerramienta clasificacion, string numeroInst)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_Herramientas SET idTipoHerramienta = @idTipoHerramienta, Marca = @Marca, ";
            cmd.CommandText += "Descripcion = @Descripcion, NumSerie = @NumSerie, NumEAC = @NumEAC, ";
            cmd.CommandText += "Clasificacion = @Clasificacion, NumeroInst = @NumeroInst WHERE idHerramienta = @idHerramienta";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            cmd.Parameters.Add("@idTipoHerramienta", SqlDbType.SmallInt, 2).Value = idTipoHerramienta;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 20).Value = marca;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = descripcion;
            cmd.Parameters.Add("@NumSerie", SqlDbType.VarChar, 15).Value = numSerie;
            cmd.Parameters.Add("@NumEAC", SqlDbType.Int, 4).Value = numEAC;
            cmd.Parameters.Add("@NumeroInst", SqlDbType.VarChar, 10).Value = clasificacion == ClasifHerramienta.Instrumento ?
                numeroInst : "";
            cmd.Parameters.Add("@Clasificacion", SqlDbType.TinyInt, 1).Value = (int)clasificacion;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Actualiza un item para una herramienta.
    /// </summary>
    private static bool ActualizarItemHerramienta(ItemHerramienta item)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!ActualizarItemHerramienta(conn, trans, item))
            {
                throw new Exception("No se pudo actualizar el item.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Actualiza un item para una herramienta.
    /// </summary>
    private static bool ActualizarItemHerramienta(SqlConnection conn, SqlTransaction trans, ItemHerramienta item)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_HerramientasItems SET Marca = @Marca, Descripcion = @Descripcion, Cantidad = @Cantidad ";
            cmd.CommandText += "WHERE idItem = @idItem";
            cmd.Parameters.Add("@idItem", SqlDbType.Int, 4).Value = item.ID;
            cmd.Parameters.Add("@Marca", SqlDbType.VarChar, 20).Value = item.Marca;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = item.Descripcion;
            cmd.Parameters.Add("@Cantidad", SqlDbType.Int, 4).Value = item.Cantidad;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Inserta un item para una herramienta.
    /// </summary>
    private static bool InsertarItemHerramienta(int idHerramienta, ItemHerramienta item)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!InsertarItemHerramienta(conn, trans, idHerramienta, item))
            {
                throw new Exception("No se pudo insertar el item para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Da de baja un item para una herramienta.
    /// </summary>
    private static bool EliminarItemHerramienta(int idItem)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE tbl_HerramientasItems SET Activo = @Activo WHERE ";
            cmd.CommandText += "idItem = @idItem";
            cmd.Parameters.Add("@idItem", SqlDbType.Int, 4).Value = idItem;
            cmd.Parameters.Add("@Activo", SqlDbType.Bit, 1).Value = false;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Elimina los ítems de una herramienta.
    /// </summary>
    private static bool EliminarItemsHerramienta(SqlConnection conn, SqlTransaction trans,
        int idHerramienta)
    {
        return EliminarItemsHerramienta(conn, trans, idHerramienta, null);
    }
    /// <summary>
    /// Elimina los ítems de una herramienta excluyendo los indicados.
    /// </summary>
    private static bool EliminarItemsHerramienta(SqlConnection conn, SqlTransaction trans,
        int idHerramienta, List<ItemHerramienta> lstExcluir)
    {
        string adic = "";
        if (lstExcluir != null && lstExcluir.Count > 0)
        {
            adic = "AND idItem NOT IN (";
            bool hayItems = false;
            foreach (ItemHerramienta item in lstExcluir)
            {
                if (item.ID != Constantes.ValorInvalido)
                {
                    hayItems = true;
                    adic += item.ID + ",";
                }
            }
            adic = adic.TrimEnd(',') + ")";
            if (!hayItems)
            {
                adic = "";
            }
        }

        bool result;
        SqlCommand cmd = new SqlCommand();

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_HerramientasItems SET Activo = @Activo WHERE ";
            cmd.CommandText += "idHerramienta = @idHerramienta " + adic;
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            cmd.Parameters.Add("@Activo", SqlDbType.Bit, 1).Value = false;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Cambia el estado de una herramienta.
    /// </summary>
    public static bool CambiarEstadoHerramienta(int idHerramienta, bool activo, string motivo)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans = null;
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_Herramientas SET Activo = @Activo WHERE ";
            cmd.CommandText += "idHerramienta = @idHerramienta";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            cmd.Parameters.Add("@Activo", SqlDbType.Bit, 1).Value = activo;

            cmd.ExecuteNonQuery();

            if (!NuevoRegActividad(conn, trans, idHerramienta, "[Baja] " + motivo, Constantes.Usuario.ID))
            {
                throw new Exception("[NuevoRegActividad]");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            if (trans != null)
            {
                trans.Rollback();
            }

            result = false;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Agrega un registro histórico para el grupo de herramientas.
    /// </summary>
    public static bool NuevoRegHistHerramienta(int idHerramienta, DateTime fecha, int idPersonaACargo,
        string ubicacion, string descripcion, int idPersonaRegistro)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!NuevoRegHistHerramienta(conn, trans, idHerramienta, fecha, idPersonaACargo, ubicacion,
                descripcion, idPersonaRegistro))
            {
                throw new Exception("No se pudo insertar el registro histórico para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Agrega un registro histórico para el grupo de herramientas.
    /// </summary>
    private static bool NuevoRegHistHerramienta(SqlConnection conn, SqlTransaction trans,
        int idHerramienta, DateTime fecha, int idPersonaACargo, string ubicacion, string descripcion, 
        int idPersonaRegistro)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "INSERT INTO tbl_HerramientasHistoricos (idHerramienta, Fecha, ";
            cmd.CommandText += "idPersonaCargo, Ubicacion, Descripcion, idPersonaRegistro) VALUES (";
            cmd.CommandText += "@idHerramienta, @Fecha, @idPersonaCargo, @Ubicacion, @Descripcion, ";
            cmd.CommandText += "@idPersonaRegistro);";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            cmd.Parameters.Add("@Fecha", SqlDbType.SmallDateTime, 4).Value = fecha;
            cmd.Parameters.Add("@idPersonaCargo", SqlDbType.Int, 4).Value = idPersonaACargo;
            cmd.Parameters.Add("@Ubicacion", SqlDbType.VarChar, 50).Value = ubicacion;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 150).Value = descripcion;
            cmd.Parameters.Add("@idPersonaRegistro", SqlDbType.Int, 4).Value = idPersonaRegistro;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Agrega un evento para una herramienta.
    /// </summary>
    public static bool NuevoEventoHerramienta(int idHerramienta, string descripcion, DateTime fecha)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!NuevoEventoHerramienta(conn, trans, idHerramienta, descripcion, fecha))
            {
                throw new Exception("No se pudo insertar el evento para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Agrega un evento para una herramienta.
    /// </summary>
    private static bool NuevoEventoHerramienta(SqlConnection conn, SqlTransaction trans, int idHerramienta, 
        string descripcion, DateTime fecha)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "INSERT INTO tbl_HerramientasEventos (idHerramienta, Descripcion, ";
            cmd.CommandText += "Fecha) VALUES (@idHerramienta, @Descripcion, @Fecha)";
            cmd.Parameters.Add("@idHerramienta", SqlDbType.Int, 4).Value = idHerramienta;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 140).Value = descripcion;
            cmd.Parameters.Add("@Fecha", SqlDbType.SmallDateTime, 4).Value = Funciones.GetDate(fecha);

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Elimina un evento para una herramienta.
    /// </summary>
    public static bool EliminarEventoHerramienta(int idEvento)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!EliminarEventoHerramienta(conn, trans, idEvento))
            {
                throw new Exception("No se pudo eliminar el evento para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Elimina los eventos.
    /// </summary>
    public static bool EliminarEventosHerramientas(int[] idEventos)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            foreach (int idEvento in idEventos)
            {
                if (!EliminarEventoHerramienta(conn, trans, idEvento))
                {
                    throw new Exception("No se pudo eliminar el evento para la herramienta.");
                }
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Elimina un evento para una herramienta.
    /// </summary>
    public static bool EliminarEventoHerramienta(SqlConnection conn, SqlTransaction trans, int idEvento)
    {
        bool result;
        SqlCommand cmd = new SqlCommand();

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_HerramientasEventos SET Activo = @Activo WHERE ";
            cmd.CommandText += "idEvento = @idEvento";
            cmd.Parameters.Add("@idEvento", SqlDbType.Int, 4).Value = idEvento;
            cmd.Parameters.Add("@Activo", SqlDbType.Bit, 1).Value = false;

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Actualiza un evento para una herramienta.
    /// </summary>
    public static bool ActualizarEventoHerramienta(int idEvento, string descripcion, DateTime fecha)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

        // Abro la conexión y creo la transacción.
        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            trans = conn.BeginTransaction();
        }
        catch
        {
            conn.Close();

            return false;
        }

        bool result;
        try
        {
            if (!ActualizarEventoHerramienta(conn, trans, idEvento, descripcion, fecha))
            {
                throw new Exception("No se pudo actualizar el evento para la herramienta.");
            }

            trans.Commit();
            result = true;
        }
        catch
        {
            trans.Rollback();
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Actualiza un evento para una herramienta.
    /// </summary>
    public static bool ActualizarEventoHerramienta(SqlConnection conn, SqlTransaction trans, int idEvento,
        string descripcion, DateTime fecha)
    {
        bool result;
        SqlCommand cmd = new SqlCommand();

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "UPDATE tbl_HerramientasEventos SET Descripcion = @Descripcion, Fecha = @Fecha ";
            cmd.CommandText += "WHERE idEvento = @idEvento";
            cmd.Parameters.Add("@idEvento", SqlDbType.Int, 4).Value = idEvento;
            cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 140).Value = descripcion;
            cmd.Parameters.Add("@Fecha", SqlDbType.SmallDateTime, 4).Value = Funciones.GetDate(fecha);

            cmd.ExecuteNonQuery();

            result = true;
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Agrega un nuevo registro histórico para la herramienta.
    /// </summary>
    private static bool NuevoRegActividad(SqlConnection conn, SqlTransaction trans, int idHerramienta, string descripcion, 
        int idPersonaRegistro)
    {
        RegHistHerramienta ultimoHist = GetUltimoRegHistHerramienta(idHerramienta);
        int idPersonaACargo = idPersonaRegistro;
        string ubicacion = "<No disponible>";

        if (ultimoHist != null)
        {
            idPersonaACargo = ultimoHist.PersonaACargo.ID;
            ubicacion = ultimoHist.Ubicacion;
        }

        return NuevoRegHistHerramienta(conn, trans, idHerramienta, DateTime.Now, idPersonaACargo, ubicacion, descripcion,
            idPersonaRegistro);
    }
    /// <summary>
    /// Envía un E-mail de recordatorio de un evento.
    /// </summary>
    private static void EmailRecordatorioEvento(DateTime fecha, int idHerramienta, string descripcion)
    {
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_RECORDATORIO_EVENTO);
        
        Email email;
        if (plantilla != null)
        {
            plantilla = plantilla.Replace("@ENCABEZADO", "Este e-mail es para recordarle el siguiente evento:");
            plantilla = plantilla.Replace("@FECHA", fecha.ToShortDateString());
            plantilla = plantilla.Replace("@DESCRIPCION", descripcion);
            plantilla = plantilla.Replace("@TIPO_MENSAJE", "info");

            email = new Email(Constantes.EmailIntranet, Constantes.EmailResponsableHerramientas, "", "Recordatorio evento - Equipo Nº" 
                + idHerramienta, plantilla);
        }
        else
        {
            email = new Email(Constantes.EmailIntranet, Constantes.EmailResponsableHerramientas, "", "Recordatorio evento - Equipo Nº"
                + idHerramienta, descripcion);
        }

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene la calibración para la herramienta.
    /// </summary>
    public static CalibracionHerramienta GetCalibracionHerramienta(int idHerramienta)
    {
        CalibracionHerramienta result = null;
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_HerramientasCalibracion WHERE idHerramienta = @idHerramienta";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idHerramienta", idHerramienta));
            IDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetCalibracionHerramienta(dr);
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
    /// Obtiene las calibraciones que coincidan con el filtro.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<object[]> GetCalibracionesHerramientas(int pagina, List<Filtro> filtros)
    {
        List<object[]> result;

        result = DataAccess.GetDataList<object[]>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltroCalibracion, GetFilaPanelCalibracion);

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetCantidadPaginasCalibraciones(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltroCalibracion);
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltroCalibracion(List<Filtro> filtros, bool cantidad)
    {
        // Cuando quiero traer las herramientas sin filtrarlas por persona a cargo, adjunto esta subconsulta.
        // En cambio, si quiero filtrar por persona a cargo, tengo que cambiar el auxConsulta = idPersonaCargo
        // porque sino se ejecuta la misma consulta 2 veces por cada fila de herramientas :/
        string auxConsulta = "(SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos WHERE "
                             + "idHerramienta = h.idHerramienta ORDER BY Fecha DESC, idRegistro DESC) ";
        string auxPersonaCargo = "(SELECT Nombre FROM tbl_Personal WHERE idPersonal = " + auxConsulta + ") ";
        string filtroJoin = "";
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosHerramienta.Marca:
                    filtroWhere += "AND h.Marca LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.Descripcion:
                    filtroWhere += "AND h.Descripcion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.PersonaCargo:
                    auxConsulta = "'" + GPersonal.GetNombrePersona((int)filtro.Valor) + "'";
                    filtroWhere += "AND (SELECT TOP 1 idPersonaCargo FROM tbl_HerramientasHistoricos ";
                    filtroWhere += "WHERE idHerramienta = h.idHerramienta ORDER BY Fecha DESC, idRegistro DESC) = ";
                    filtroWhere += filtro.Valor + " ";
                    break;
                case (int)FiltrosHerramienta.Tipo:
                    filtroWhere = "AND th.Descripcion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosHerramienta.NumeroInstrumento:
                    filtroWhere += "AND h.NumeroInst LIKE '%" + filtro.Valor + "%' AND h.Clasificacion = " + (int)ClasifHerramienta.Instrumento + " ";
                    break;
                default:
                    filtroJoin = "";
                    filtroWhere = "";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idHerramienta) as TotalRegistros FROM tbl_Herramientas h " + filtroJoin + " ";
            consulta += "WHERE (1=1) ";
            if (filtroWhere.Length > 0)
            {
                consulta += "AND " + filtroWhere;
            }
        }
        else
        {
            consulta = "SELECT h.*, hc.FrecCalibracion, hc.UltCalibracion, hc.ProxCalibracion, hc.TipoCalibracion, hc.Observaciones, ";
            consulta += "(SELECT TOP 1 hh.Ubicacion FROM tbl_HerramientasHistoricos hh WHERE hh.idHerramienta = h.idHerramienta ";
            consulta += "ORDER BY hh.Fecha DESC) AS Ubicacion, th.Descripcion AS TipoInstrumento, ";
            consulta += auxPersonaCargo + " AS PersonaCargo FROM tbl_Herramientas h LEFT JOIN tbl_HerramientasCalibracion hc ";
            consulta += "ON h.idHerramienta = hc.idHerramienta INNER JOIN tbl_TipoHerramienta th ON ";
            consulta += "th.idTipoHerramienta = h.idTipoHerramienta ";
            consulta += filtroJoin + " ";
            consulta += "WHERE h.Clasificacion = " + (int)ClasifHerramienta.Instrumento + " ";
            if (filtroWhere.Length > 0)
            {
                consulta += "AND " + filtroWhere;
            }
        }

        return consulta;
    }
    /// <summary>
    /// Genera una nueva calibración.
    /// </summary>
    public static void NuevaCalibracion(int idHerramienta, FrecCalHerramienta frecuencia, DateTime ultCalibracion,
        DateTime proxCalibracion, TiposCalHerramienta tipoCalibracion, string observaciones)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_HerramientasCalibracion (idHerramienta, FrecCalibracion, UltCalibracion, ";
            cmd.CommandText += "ProxCalibracion, TipoCalibracion, Observaciones) VALUES (@idHerramienta, @FrecCalibracion, ";
            cmd.CommandText += "@UltCalibracion, @ProxCalibracion, @TipoCalibracion, @Observaciones)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idHerramienta", idHerramienta));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FrecCalibracion", (int)frecuencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UltCalibracion", ultCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ProxCalibracion", proxCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoCalibracion", (int)tipoCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", observaciones));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }
    }
    /// <summary>
    /// Actualiza una calibración.
    /// </summary>
    public static void ActualizarCalibracion(int idHerramienta, FrecCalHerramienta frecuencia, DateTime ultCalibracion,
        DateTime proxCalibracion, TiposCalHerramienta tipoCalibracion, string observaciones)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_HerramientasCalibracion SET FrecCalibracion = @FrecCalibracion, UltCalibracion = ";
            cmd.CommandText += "@UltCalibracion, ProxCalibracion = @ProxCalibracion, TipoCalibracion = @TipoCalibracion, ";
            cmd.CommandText += "Observaciones = @Observaciones WHERE idHerramienta = @idHerramienta";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idHerramienta", idHerramienta));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FrecCalibracion", (int)frecuencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@UltCalibracion", ultCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ProxCalibracion", proxCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoCalibracion", (int)tipoCalibracion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", observaciones));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }
    }
    /// <summary>
    /// Obtiene las frecuencias de calibración.
    /// </summary>
    public static Dictionary<int, string> GetFrecuenciasCalibracion()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)FrecCalHerramienta.Mensual, "Mensual");
        result.Add((int)FrecCalHerramienta.Bimestral, "Bimestral");
        result.Add((int)FrecCalHerramienta.UnAno, "1 año");
        result.Add((int)FrecCalHerramienta.DosAnos, "2 años");

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de calibración.
    /// </summary>
    public static Dictionary<int, string> GetTiposCalibracion()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)TiposCalHerramienta.Externa, "Externa");
        result.Add((int)TiposCalHerramienta.Interna, "Interna");

        return result;
    }
    /// <summary>
    /// Obtiene las clasificaciones de las herramientas.
    /// </summary>
    public static Dictionary<int, string> GetClasificacionHerramienta()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)ClasifHerramienta.Herramienta, "Herramienta");
        result.Add((int)ClasifHerramienta.Instrumento, "Instrumento");

        return result;
    }
    /// <summary>
    /// Obtiene el tipo de calibración.
    /// </summary>
    public static string GetTipoCalibracion(int tipo)
    {
        if (!Enum.IsDefined(typeof(TiposCalHerramienta), tipo))
        {
            return "-";
        }

        TiposCalHerramienta s = (TiposCalHerramienta)tipo;
        switch (s)
        {
            case TiposCalHerramienta.Externa:
                return "Externa";
            case TiposCalHerramienta.Interna:
                return "Interna";
            default:
                return "-";
        }
    }
    /// <summary>
    /// Obtiene la frecuencia de calibración.
    /// </summary>
    public static string GetFrecuenciaCalibracion(int frecuencia)
    {
        if (!Enum.IsDefined(typeof(FrecCalHerramienta), frecuencia))
        {
            return "-";
        }

        FrecCalHerramienta s = (FrecCalHerramienta)frecuencia;
        switch (s)
        {
            case FrecCalHerramienta.Mensual:
                return "Mensual";
            case FrecCalHerramienta.Bimestral:
                return "Bimestral";
            case FrecCalHerramienta.UnAno:
                return "1 año";
            case FrecCalHerramienta.DosAnos:
                return "2 años";
            default:
                return "-";
        }
    }
    /// <summary>
    /// Obtiene el path al archivo con los datos de la herramienta.
    /// </summary>
    public static string GetPathDatosHerramienta(int idHerramienta)
    {
        string result = PathDatosHerramientas + idHerramienta + ".zip";

        if (!File.Exists(result))
        {
            throw new ElementoInexistenteException();
        }
        
        return result;
    }
    /// <summary>
    /// Controla los eventos para las herramientas.
    /// </summary>
    public static void ControlarEventosHerramientas()
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<object[]> eventos = new List<object[]>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            
            // Eventos de las herramientas.
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idHerramienta, Fecha, Descripcion FROM tbl_HerramientasEventos WHERE ";
            cmd.CommandText += "(DATEDIFF(day, @Fecha, Fecha) = @Dias OR Fecha = @Fecha) AND Activo = @Activo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", DateTime.Now));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Dias", DiasPreavisoEventos));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            dr = cmd.ExecuteReader();
            
            while (dr.Read())
            {
                eventos.Add(new object[] { dr["idHerramienta"], dr["Fecha"], dr["Descripcion"] });
            }

            dr.Close();

            // Calibraciones.
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idHerramienta, ProxCalibracion FROM tbl_HerramientasCalibracion WHERE ";
            cmd.CommandText += "(DATEDIFF(day, @Fecha, ProxCalibracion) = @Dias OR ProxCalibracion = @Fecha)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(DateTime.Now)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Dias", DiasPreavisoEventos));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                eventos.Add(new object[] { dr["idHerramienta"], dr["ProxCalibracion"], "Próxima calibración." });
            }

            dr.Close();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        foreach (object[] evento in eventos)
        {
            try
            {
                EmailRecordatorioEvento(Convert.ToDateTime(evento[1]), Convert.ToInt32(evento[0]), evento[2].ToString());
            }
            catch
            {

            }
        }
    }
}
