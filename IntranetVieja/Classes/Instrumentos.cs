using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;


public class Instrumento
{
    // Variables.
    private int idInstrumento;
    private int numero;
    private string tipo;
    private string descripcion;
    private string marca;
    private string modelo;
    private string numSerie;
    private string rango;
    private string resolucion;
    private string clase;
    private string incertidumbre;
    private FrecuenciaCalibracion frecCalibracion;
    private InstrumentoRegistro ultRegistro;
    private InstrumentoCalib ultCalib;
    
    //german 
    private FrecuenciaMantenimiento frecMantenimiento;
    private FrecuenciaComprobacion frecComprobacion;
    private DateTime fechaMantenimiento; 
    private DateTime fechaComprobacion;
    //fin german

    // Propiedades.
    public int ID
    {
        get { return this.idInstrumento; }
    }
    public int Numero
    {
        get { return this.numero; }
    }
    public string Tipo
    {
        get { return this.tipo; }
    }
    public string Descripcion
    {
        get { return this.descripcion; }
    }
    public string Marca
    {
        get { return this.marca; }
    }
    public string Modelo
    {
        get { return this.modelo; }
    }
    public string NumeroSerie
    {
        get { return this.numSerie; }
    }
    public string Rango
    {
        get { return this.rango; }
    }
    public string Resolucion
    {
        get { return this.resolucion;}
    }
    public string Clase
    {
        get { return this.clase;}
    }
    public string Incertidumbre
    {
        get { return this.incertidumbre; }
    }
    public InstrumentoCalib UltCalibracion
    {
        get { return this.ultCalib; }
    }
    public DateTime ProxCalibracion
    {
        get { return Instrumentos.GetProxCalibracion(ultCalib.Fecha, CalibFrec); }
    }
    public bool Calibrado
    {
        get { return this.ProxCalibracion < DateTime.Now; }
    }
    public bool CalibProxAVencer
    {
        get 
        {
            bool result;

            result = (ProxCalibracion - DateTime.Now).TotalDays < 60;

            return result;
        }
    }
    public bool CalibVencida
    {
        get
        {
            bool result;

            result = ProxCalibracion < DateTime.Now;

            return result;
        }
    }
    public InstrumentoRegistro UltRegistro
    {
        get { return ultRegistro; }
    }
    public FrecuenciaCalibracion CalibFrec
    {
        get { return frecCalibracion; }
    }
    
    //german 
    
    public FrecuenciaMantenimiento MtoFrec
    {
        get { return frecMantenimiento; }
    }

     public FrecuenciaComprobacion ComprobFrec
    {
        get { return frecComprobacion; }
    }

    public DateTime FechaComprobacion
    {
        get { return this.fechaComprobacion; }
    }

    public DateTime FechaMantenimiento
    {
        get { return this.fechaMantenimiento; }
    }

    public DateTime ProxMantenimiento
    {
        get { return Instrumentos.GetProxMantenimiento(FechaMantenimiento, MtoFrec); }
    }
    public DateTime ProxComprobacion
    {
        get { return Instrumentos.GetProxComprobacion(FechaComprobacion, ComprobFrec); }
    }
    //fin german

    internal Instrumento(int idInstrumento, int numero, string tipo, string descripcion, string marca, string modelo,
                         string numSerie, string rango, string resolucion, string clase, string incertidumbre, FrecuenciaCalibracion frecCalibracion,
                         InstrumentoRegistro ultRegistro, InstrumentoCalib ultCalib, FrecuenciaComprobacion frecComprobacion, FrecuenciaMantenimiento frecMantenimiento,
                         DateTime fechaComprobacion, DateTime fechaMantenimiento)
    {
        this.idInstrumento = idInstrumento;
        this.numero = numero;
        this.tipo = tipo;
        this.descripcion = descripcion;
        this.marca = marca;
        this.modelo = modelo;
        this.numSerie = numSerie;
        this.rango = rango;
        this.resolucion = resolucion;
        this.clase = clase;
        this.incertidumbre = incertidumbre; 
        this.ultCalib = ultCalib;
        this.frecCalibracion = frecCalibracion;
        this.ultRegistro = ultRegistro;
        this.frecComprobacion = frecComprobacion; //G
        this.frecMantenimiento = frecMantenimiento; //G
        this.fechaComprobacion = fechaComprobacion; //G
        this.fechaMantenimiento = fechaMantenimiento; //G
    }
}
public class GrupoInstrumentos
{
    // Variables.
    private int idGrupo;
    private string nombre;
    private List<Persona> responsables;

    // Propiedades.
    public int ID
    {
        get { return this.idGrupo; }
    }
    public string Nombre
    {
        get { return this.nombre; }
    }
    public List<Persona> Responsables
    {
        get { return this.responsables; }
    }


    internal GrupoInstrumentos(int idGrupo, string nombre, List<Persona> responsables)
    {
        this.idGrupo = idGrupo;
        this.nombre = nombre;
        this.responsables = responsables;
    }
    /// <summary>
    /// Obtiene una lista con los emails de los responsables.
    /// </summary>
    public string GetEmailsResponsables()
    {
        string result = "";

        this.responsables.ForEach(p => result += p.Email + ",");
        result = result.TrimEnd(',');

        return result;
    }
}
public class InstrumentoRegistro
{
    // Variables.
    private string grupo;
    private string ubicacion;
    private string responsable;

    // Propiedades.
    public string Grupo
    {
        get { return this.grupo; }
    }
    public string Ubicacion
    {
        get { return this.ubicacion; }
    }
    public string Responsable
    {
        get { return responsable; }
    }


    internal InstrumentoRegistro(string grupo, string ubicacion, string responsable)
    {
        this.grupo = grupo;
        this.ubicacion = ubicacion;
        this.responsable = responsable;
    }
}
public class InstrumentoCalib
{
    // Variables.
    private DateTime fecha;

    // Propiedades.
    public DateTime Fecha
    {
        get { return fecha; }
    }


    internal InstrumentoCalib(DateTime fecha)
    {
        this.fecha = fecha;
    }
}

/// <summary>
/// Descripción breve de Instrumentos
/// </summary>
public static class Instrumentos
{
    // Constantes.
    private const int MaxRegistrosPagina = 20;
    private const string PathImagenesInst = @"\\10.0.0.4\Usuarios\Liliana.villamil\SGI MULTISITIO\03 Listas de control\FA\FA-020 Lista de control de equipos\EQUIPOS SGI\@NUMERO\01 IMAGENES\";
    private const string PathCertifInst = @"\\10.0.0.4\Usuarios\Liliana.villamil\SGI MULTISITIO\03 Listas de control\FA\FA-020 Lista de control de equipos\EQUIPOS SGI\@NUMERO\02 CERTIFICADOS\CERTIF INST @NUMERO.pdf";
    private const string PathManualesInst = @"\\10.0.0.4\Usuarios\Liliana.villamil\SGI MULTISITIO\03 Listas de control\FA\FA-020 Lista de control de equipos\EQUIPOS SGI\@NUMERO\03 MANUALES\";
    private const string PathEAC = @"\\10.0.0.4\Usuarios\Liliana.villamil\SGI MULTISITIO\03 Listas de control\FA\FA-020 Lista de control de equipos\EQUIPOS SGI\@NUMERO\04 EAC\EAC @NUMERO.pdf";
    private const int DiasVencimiento = 60;


    /// <summary>
    /// Obtiene la próxima fecha de calibración.
    /// </summary>
    internal static DateTime GetProxCalibracion(DateTime ultCalibracion, FrecuenciaCalibracion frecuencia)
    {
        DateTime result;
        
        switch (frecuencia)
        {
            case FrecuenciaCalibracion.DosAnios:
                result = ultCalibracion.AddYears(2);
                break;
            case FrecuenciaCalibracion.Anual:
                result = ultCalibracion.AddYears(1);
                break;
            case FrecuenciaCalibracion.Mensual:
                result = ultCalibracion.AddMonths(1);
                break;
            default:
                throw new DatosInvalidosException();
        }

        return result;
    }
    /// <summary>
    /// Obtiene la próxima fecha de Mantenimiento.
    /// </summary>
    internal static DateTime GetProxMantenimiento(DateTime ultMant, FrecuenciaMantenimiento frecuencia)
    {
        DateTime result;

        switch (frecuencia)
        {
            case FrecuenciaMantenimiento.DosAnios:
                result = ultMant.AddYears(2);
                break;
            case FrecuenciaMantenimiento.Anual:
                result = ultMant.AddYears(1);
                break;
            case FrecuenciaMantenimiento.Mensual:
                result = ultMant.AddMonths(1);
                break;
            default:
                throw new DatosInvalidosException();
        }

        return result;
    }

    /// <summary>
    /// Obtiene la próxima fecha de Comprobacion.
    /// </summary>
    internal static DateTime GetProxComprobacion(DateTime ultComp, FrecuenciaComprobacion frecuencia)
    {
        DateTime result;

        switch (frecuencia)
        {
            case FrecuenciaComprobacion.DosAnios:
                result = ultComp.AddYears(2);
                break;
            case FrecuenciaComprobacion.Anual:
                result = ultComp.AddYears(1);
                break;
            case FrecuenciaComprobacion.Mensual:
                result = ultComp.AddMonths(1);
                break;
            default:
                throw new DatosInvalidosException();
        }

        return result;
    }

    /// <summary>
    /// Agrega un instrumento.
    /// </summary>
    public static void AddInstrumento(int numero, int idTipo, string descripcion, int idGrupo, string ubicacion,
        int idMarca, string modelo, string numSerie, string rango, string resolucion, string clase, string incertidumbre, 
        FrecuenciaCalibracion frecCalibracion, DateTime ultCalib, int idResponsable, FrecuenciaComprobacion frecComprob, 
        FrecuenciaMantenimiento frecMto, DateTime fechaComprob, DateTime fechaMto)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);
            
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Instrumentos (Numero, idTipo, Descripcion, idMarca, Modelo,  ";
            cmd.CommandText += "NumSerie, idFrecCalib) VALUES (@Numero, @idTipo, @Descripcion, @idMarca, ";
            cmd.CommandText += "@Modelo, @NumSerie, @Rango, @Resolucion, @Clase, @Incertidumbre, @idFrecCalib, @idFrecMto, @idFrecComprob, ";
            cmd.CommandText += "@FechaComprob, @FechaMto); "; 
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Instrumentos;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idTipo", idTipo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idMarca", idMarca));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Modelo", modelo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NumSerie", numSerie));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Rango", rango));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Resolucion", resolucion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Clase", clase));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Incertidumbre", incertidumbre)); 
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idFrecCalib", (int)frecCalibracion));
            //German
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idFrecMto", 1));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idFrecComprob", 1));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaComprob", fechaComprob));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaMto", fechaMto));
            // fin german
            int idInstrumento = Convert.ToInt32(cmd.ExecuteScalar());

            AddRegistroCalibracion(idInstrumento, ultCalib, trans);

            AddRegistroInstrumento(idInstrumento, idGrupo, ubicacion, idResponsable, DateTime.Now, trans);

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

    public static void AddRegistroCalibracion(int idInstrumento, DateTime fecha)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            AddRegistroCalibracion(idInstrumento, fecha, trans);

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

    private static int AddRegistroCalibracion(int idInstrumento, DateTime fecha, IDbTransaction trans)
    {
        int result;

        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "INSERT INTO tbl_InstrumentosCalibraciones (Fecha) ";
        cmd.CommandText += "VALUES (@Fecha); ";
        cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_InstrumentosCalibraciones;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
        result = Convert.ToInt32(cmd.ExecuteScalar());

        // Actualizo el instrumento.
        cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "UPDATE tbl_Instrumentos SET idCalibracion = @idCalibracion WHERE idInstrumento = @idInstrumento";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idInstrumento", idInstrumento));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idCalibracion", result));
        cmd.ExecuteNonQuery();

        return result;
    }

    public static void AddRegistroInstrumento(int idInstrumento, int idGrupo, string ubicacion, int idResponsable, DateTime fecha)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            AddRegistroInstrumento(idInstrumento, idGrupo, ubicacion, idResponsable, fecha, trans);

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

    private static int AddRegistroInstrumento(int idInstrumento, int idGrupo, string ubicacion, int idResponsable,
                                              DateTime fecha, IDbTransaction trans)
    {
        int result;

        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "INSERT INTO tbl_InstrumentosRegistros (idGrupo, Ubicacion, idResponsable, Fecha) ";
        cmd.CommandText += "VALUES (@idGrupo, @Ubicacion, @idResponsable, @Fecha); ";
        cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_InstrumentosRegistros;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idGrupo", idGrupo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Ubicacion", ubicacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idResponsable", idResponsable));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
        result = Convert.ToInt32(cmd.ExecuteScalar());

        // Actualizo el instrumento.
        cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "UPDATE tbl_Instrumentos SET idRegistro = @idRegistro WHERE idInstrumento = @idInstrumento";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idInstrumento", idInstrumento));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idRegistro", result));
        cmd.ExecuteNonQuery();

        return result;
    }

    private static InstrumentoRegistro GetInstrumentoRegistro(DataRow dr)
    {
        InstrumentoRegistro result = null;

        try
        {
            result = new InstrumentoRegistro(dr["Grupo"].ToString(), dr["Ubicacion"].ToString(), dr["Responsable"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static InstrumentoCalib GetInstrumentoCalib(DataRow dr)
    {
        InstrumentoCalib result = null;

        try
        {
            result = new InstrumentoCalib(Convert.ToDateTime(dr["UltCalib"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static Instrumento GetInstrumento(DataRow dr)
    {
        Instrumento result;

        try
        {
            result = new Instrumento(Convert.ToInt32(dr["idInstrumento"]), Convert.ToInt32(dr["Numero"]), dr["Tipo"].ToString(),
                dr["Descripcion"].ToString(), dr["Marca"].ToString(), dr["Modelo"].ToString(), dr["NumSerie"].ToString(),
                dr["Rango"].ToString(), dr["Resolucion"].ToString(), dr["Clase"].ToString(), dr["Incertidumbre"].ToString(), 
                (FrecuenciaCalibracion)Convert.ToInt32(dr["idFrecCalib"]), GetInstrumentoRegistro(dr), GetInstrumentoCalib(dr),
                (FrecuenciaComprobacion)Convert.ToInt32(dr["idFrecComprob"]), (FrecuenciaMantenimiento)Convert.ToInt32(dr["idFrecMto"]), 
                DateTime.Parse(dr["fechaMto"].ToString()), DateTime.Parse(dr["fechaComprob"].ToString()));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static InstrumentoRegistro GetInstrumentoRegistro(IDataReader dr)
    {
        InstrumentoRegistro result = null;

        try
        {
            result = new InstrumentoRegistro(dr["Grupo"].ToString(), dr["Ubicacion"].ToString(), dr["Responsable"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static InstrumentoCalib GetInstrumentoCalib(IDataReader dr)
    {
        InstrumentoCalib result = null;

        try
        {
            result = new InstrumentoCalib(Convert.ToDateTime(dr["UltCalib"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static Instrumento GetInstrumento(IDataReader dr)
    {
        Instrumento result;

        try
        {
            result = new Instrumento(Convert.ToInt32(dr["idInstrumento"]), Convert.ToInt32(dr["Numero"]), dr["Tipo"].ToString(),
              dr["Descripcion"].ToString(), dr["Marca"].ToString(), dr["Modelo"].ToString(), dr["NumSerie"].ToString(),
              dr["Rango"].ToString(), dr["Resolucion"].ToString(), dr["Clase"].ToString(), dr["Incertidumbre"].ToString(),
              (FrecuenciaCalibracion)Convert.ToInt32(dr["idFrecCalib"]), GetInstrumentoRegistro(dr), GetInstrumentoCalib(dr),
              (FrecuenciaComprobacion)Convert.ToInt32(dr["idFrecComprob"]), (FrecuenciaMantenimiento)Convert.ToInt32(dr["idFrecMto"]),
              DateTime.Parse(dr["fechaMto"].ToString()), DateTime.Parse(dr["fechaComprob"].ToString()));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetInstrumentosQuery(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string filtroJoin = "";
        string result;

        foreach (Filtro filtro in filtros)
        {
            if (Enum.IsDefined(typeof(FiltroInstrumento), filtro.Tipo))
            {
                FiltroInstrumento f = (FiltroInstrumento)filtro.Tipo;
                switch (f)
                {
                    case FiltroInstrumento.Descripcion:
                        filtroWhere += "AND i.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Grupo:
                        filtroWhere += "AND g.Nombre LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Marca:
                        filtroWhere += "AND m.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Modelo:
                        filtroWhere += "AND i.Modelo LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Numero:
                        filtroWhere += "AND i.Numero = " + filtro.Valor + " ";
                        break;
                    case FiltroInstrumento.NumSerie:
                        filtroWhere += "AND i.NumSerie LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Tipo:
                        filtroWhere += "AND t.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroInstrumento.Todo:
                        filtroWhere += "AND (";
                        filtroWhere += "i.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR g.Nombre LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR m.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR i.Modelo LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR p.Nombre LIKE '%" + filtro.Valor + "%' ";
                        int i;
                        if (Int32.TryParse(filtro.Valor.ToString(), out i))
                        {
                            filtroWhere += "OR i.Numero = " + filtro.Valor + " ";
                        }
                        filtroWhere += "OR i.NumSerie LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += "OR t.Descripcion LIKE '%" + filtro.Valor + "%' ";
                        filtroWhere += ")";
                        break;
                }
            }
        }

        filtroJoin += "INNER JOIN tbl_InstrumentosTipos t ON i.idTipo = t.idTipo ";
        filtroJoin += "INNER JOIN tbl_InstrumentosMarcas m ON i.idMarca = m.idMarca ";
        filtroJoin += "INNER JOIN tbl_InstrumentosRegistros r ON r.idRegistro = i.idRegistro ";
        filtroJoin += "INNER JOIN tbl_InstrumentosGrupos g ON g.idGrupo = r.idGrupo ";
        filtroJoin += "INNER JOIN tbl_Personal p ON p.idPersonal = r.idResponsable ";
        filtroJoin += "INNER JOIN tbl_InstrumentosCalibraciones c ON c.idCalibracion = i.idCalibracion ";

        if (cantidad)
        {
            result = "SELECT Count(idInstrumento) AS TotalRegistros ";
        }
        else
        {
            result = "SELECT i.idInstrumento, i.Numero, t.Descripcion AS Tipo, i.Descripcion, g.Nombre AS Grupo, ";
            result += "m.Descripcion AS Marca, i.Modelo, i.NumSerie, r.Ubicacion, c.Fecha AS UltCalib, i.idFrecCalib, i.Rango, i.Resolucion, i.Clase, i.Incertidumbre, ";
            result += "p.Nombre AS Responsable, i.idFrecMto, i.idfrecComprob, i.fechaComprob, i.fechaMto ";
        }

        result += "FROM tbl_Instrumentos i " + filtroJoin + " ";
        result += "WHERE 1=1 " + filtroWhere;

        if (!cantidad) result += "ORDER BY i.Numero";

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetInstrumentosPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetInstrumentosQuery);
    }
    /// <summary>
    /// Obtiene una lista con los instrumentos.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<Instrumento> GetInstrumentos(int pagina, List<Filtro> filtros)
    {
        List<Instrumento> result;

        result = DataAccess.GetDataList(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina, GetInstrumentosQuery,
                                        GetInstrumento);

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de instrumento.
    /// </summary>
    public static Dictionary<int, string> GetTiposInstrumentos()
    {
        Dictionary<int, string> result = new Dictionary<int,string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_InstrumentosTipos ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idTipo"]), dr["Descripcion"].ToString());
            }

            dr.Close();
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            result.Clear();
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
    /// Obtiene las marcas de instrumento.
    /// </summary>
    public static Dictionary<int, string> GetMarcasInstrumentos()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_InstrumentosMarcas ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idMarca"]), dr["Descripcion"].ToString());
            }

            dr.Close();
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            result.Clear();
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
    /// Obtiene las marcas de instrumento.
    /// </summary>
    public static Dictionary<int, string> GetGruposInstrumentosDS()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_InstrumentosGrupos ORDER BY Nombre";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idGrupo"]), dr["Nombre"].ToString());
            }

            dr.Close();
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            result.Clear();
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
    /// Obtiene las frecuencias de calibración.
    /// </summary>
    public static Dictionary<int, string> GetFrecuenciasCalibracion()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)FrecuenciaCalibracion.DosAnios, "2 años");
        result.Add((int)FrecuenciaCalibracion.Anual, "Anual");
        result.Add((int)FrecuenciaCalibracion.Mensual, "Mensual");

        return result;
    }
    /// <summary>
    /// Obtiene la frecuencia de calibración.
    /// </summary>
    public static string GetFrecuenciaCalibracion(FrecuenciaCalibracion frecuencia)
    {
        switch (frecuencia)
        {
            case FrecuenciaCalibracion.DosAnios:
                return "2 años";
            case FrecuenciaCalibracion.Anual:
                return "Anual";
            case FrecuenciaCalibracion.Mensual:
                return "Mensual";
            default:
                return "-";
        }
    }
    // German
    /// Obtiene la frecuencia de mantenimiento.
    /// </summary>
    public static string GetFrecuenciaMantenimiento(FrecuenciaMantenimiento frecuencia)
    {
        switch (frecuencia)
        {
            case FrecuenciaMantenimiento.DosAnios:
                return "2 años";
            case FrecuenciaMantenimiento.Anual:
                return "Anual";
            case FrecuenciaMantenimiento.Mensual:
                return "Mensual";
            default:
                return "-";
        }
    }
    public static string GetFrecuenciaComprobacion(FrecuenciaComprobacion frecuencia)
    {
        switch (frecuencia)
        {
            case FrecuenciaComprobacion.DosAnios:
                return "2 años";
            case FrecuenciaComprobacion.Anual:
                return "Anual";
            case FrecuenciaComprobacion.Mensual:
                return "Mensual";
            default:
                return "-";
        }
    }

    //Fin German


    /// <summary>
    /// Obtiene un path para un instrumento.
    /// </summary>
    private static string GetPathInstrumento(string path, int numero)
    {
        string result;

        result = path.Replace("@NUMERO", numero.ToString());

        return result;
    }
    /// <summary>
    /// Obtiene el path a la imagen del instrumento.
    /// </summary>
    public static string GetPathImagenInstrumento(int numero)
    {
        string result;

        result = GetPathInstrumento(PathImagenesInst, numero);

        return result;
    }
    /// <summary>
    /// Obtiene el path al certificado del instrumento.
    /// </summary>
    public static string GetPathCertifInstrumento(int numero)
    {
        string result;

        result = GetPathInstrumento(PathCertifInst, numero);

        return result;
    }
    /// <summary>
    /// Obtiene si el instrumento tiene certificado.
    /// </summary>
    public static bool HasCertifInstrumento(int numero)
    {
        bool result;

        result = File.Exists(GetPathCertifInstrumento(numero));

        return result;
    }
    /// <summary>
    /// Obtiene el path al EAC del instrumento.
    /// </summary>
    public static string GetPathEAC(int numero)
    {
        string result;

        result = GetPathInstrumento(PathEAC, numero);

        return result;
    }
    /// <summary>
    /// Obtiene si el instrumento tiene EAC.
    /// </summary>
    public static bool HasEAC(int numero)
    {
        bool result;

        result = File.Exists(GetPathEAC(numero));

        return result;
    }
    /// <summary>
    /// Obtiene si el instrumento tiene manuales.
    /// </summary>
    public static bool HasManualesInstrumento(int numero)
    {
        bool result;

        string path = GetPathInstrumento(PathManualesInst, numero);
        result = Directory.Exists(path) && Directory.GetFiles(path).Length > 0;

        return result;
    }
    /// <summary>
    /// Obtiene las imágenes de un instrumento.
    /// </summary>
    public static List<string> GetImagenesInstrumento(int numero)
    {
        List<string> result = new List<string>();

        string path = GetPathImagenInstrumento(numero);
        string[] archivos = Directory.GetFiles(path, "*.jpg");
        result = new List<string>(archivos);

        return result;
    }
    /// <summary>
    /// Obtiene los manuales de un instrumento.
    /// </summary>
    public static List<FileInfo> GetManualesInstrumento(int numero)
    {
        List<FileInfo> result = new List<FileInfo>();

        string path = GetPathInstrumento(PathManualesInst, numero);
        List<string> manuales = new List<string>(Directory.GetFiles(path, "*.pdf"));
        manuales.ForEach(m => result.Add(new FileInfo(m)));

        return result;
    }
    /// <summary>
    /// Obtiene los instrumentos próximos a vencer.
    /// </summary>
    public static List<Instrumento> GetInstrumentosProxVencer()
    {
        List<Instrumento> result = new List<Instrumento>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT i.idInstrumento, i.Numero, t.Descripcion AS Tipo, i.Descripcion, g.Nombre AS Grupo, ";
            cmd.CommandText += "m.Descripcion AS Marca, i.Modelo, i.NumSerie, r.Ubicacion, c.Fecha AS UltCalib, i.idFrecCalib, ";
            cmd.CommandText += "p.Nombre AS Responsable ";
            cmd.CommandText += "FROM tbl_Instrumentos i ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosTipos t ON i.idTipo = t.idTipo ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosMarcas m ON i.idMarca = m.idMarca ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosRegistros r ON r.idRegistro = i.idRegistro ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosGrupos g ON g.idGrupo = r.idGrupo ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = r.idResponsable ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosCalibraciones c ON c.idCalibracion = i.idCalibracion ";
            dr = cmd.ExecuteReader();

            DateTime hoy = DateTime.Now;
            while (dr.Read())
            {
                Instrumento i = GetInstrumento(dr);
                if (i != null && Math.Round((i.ProxCalibracion - hoy).TotalDays, 0) == DiasVencimiento)
                {
                    result.Add(i);
                }
            }

            dr.Close();
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            result.Clear();
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
    /// Obtiene los instrumentos próximos a vencer.
    /// </summary>
    public static List<Instrumento> GetInstrumentosProxVencer(DateTime mes)
    {
        List<Instrumento> result = new List<Instrumento>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT i.idInstrumento, i.Numero, t.Descripcion AS Tipo, i.Descripcion, g.Nombre AS Grupo, ";
            cmd.CommandText += "m.Descripcion AS Marca, i.Modelo, i.NumSerie, r.Ubicacion, c.Fecha AS UltCalib, i.idFrecCalib, ";
            cmd.CommandText += "p.Nombre AS Responsable ";
            cmd.CommandText += "FROM tbl_Instrumentos i ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosTipos t ON i.idTipo = t.idTipo ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosMarcas m ON i.idMarca = m.idMarca ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosRegistros r ON r.idRegistro = i.idRegistro ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosGrupos g ON g.idGrupo = r.idGrupo ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = r.idResponsable ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosCalibraciones c ON c.idCalibracion = i.idCalibracion ";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Instrumento i = GetInstrumento(dr);
                if (i != null && i.ProxCalibracion.Month == mes.Month && i.ProxCalibracion.Year == mes.Year)
                {
                    result.Add(i);
                }
            }

            dr.Close();
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene los grupos de instrumentos.
    /// </summary>
    public static List<GrupoInstrumentos> GetGruposInstrumentos()
    {
        List<GrupoInstrumentos> result = new List<GrupoInstrumentos>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT g.idGrupo, g.Nombre, p.* ";
            cmd.CommandText += "FROM tbl_InstrumentosGrupos g ";
            cmd.CommandText += "INNER JOIN tbl_InstrumentosGruposResponsables gp ON gp.idGrupo = g.idGrupo ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = gp.idPersonal ";
            cmd.CommandText += "ORDER BY g.Nombre";
            dr = cmd.ExecuteReader();

            int idGrupo = Constantes.ValorInvalido;
            string nombre = "";
            List<Persona> responsables = new List<Persona>();
            while (dr.Read())
            {
                int aux = Convert.ToInt32(dr["idGrupo"]);
                if (aux != idGrupo)
                {
                    if (idGrupo != Constantes.ValorInvalido)
                    {
                        GrupoInstrumentos grupo = new GrupoInstrumentos(idGrupo, nombre, responsables);
                        result.Add(grupo);
                    }
                    idGrupo = aux;
                    nombre = dr["Nombre"].ToString();
                    responsables = new List<Persona>();
                }
                responsables.Add(GPersonal.GetPersona(dr));
            }
            result.Add(new GrupoInstrumentos(idGrupo, nombre, responsables));

            dr.Close();
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }
            result.Clear();
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
    /// Envía las alertas de vencimiento de los instrumentos.
    /// </summary>
    public static void EnviarAlertasVencimientos()
    {
        List<Instrumento> instrumentos = GetInstrumentosProxVencer();
        List<GrupoInstrumentos> grupos = GetGruposInstrumentos();

        foreach(Instrumento i in instrumentos)
        {
            GrupoInstrumentos g = grupos.Find(gi => gi.Nombre.Equals(i.UltRegistro.Grupo));
            if (g != null)
            {
                EnviarAlertaInstrumento(i, g.GetEmailsResponsables());
            }
        }
    }
    /// <summary>
    /// Envía una alerta de información de obra.
    /// </summary>
    private static void EnviarAlertaInstrumento(Instrumento i, string para)
    {
        // Obtengo la plantilla.
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_ALERTA_INSTRUMENTOS);
        if (plantilla == null)
        {
            throw new EmailException();
        }

        // Reemplazo las variables.
        plantilla = plantilla.Replace("@TIPO_MENSAJE", "info");
        plantilla = plantilla.Replace("@NUMERO", i.Numero.ToString());
        plantilla = plantilla.Replace("@TIPO", i.Tipo);
        plantilla = plantilla.Replace("@MARCA", i.Marca);
        plantilla = plantilla.Replace("@MODELO", i.Modelo);
        plantilla = plantilla.Replace("@NUM_SERIE", i.NumeroSerie);
        plantilla = plantilla.Replace("@RANGO", i.Rango);
        plantilla = plantilla.Replace("@PROX_CALIB", i.ProxCalibracion.ToShortDateString());
        plantilla = plantilla.Replace("@DESCRIPCION", i.Descripcion);

        Email email = new Email(Constantes.EmailIntranet, para, Constantes.EmailCalidad, "Vencimiento de instrumento Nº" +
            i.Numero, plantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
}