/*
 * Historial:
 * ===================================================================================
 * [01/06/2011]
 * - Agregado el campo Usuario.
 * - La propiedad Email ahora contiene el email completo de la persona.
 * [30/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de Personal
/// </summary>
public class Persona : IComparable, IEquatable<Persona>
{
    // Variables.
    private int idPersonal;
    private string nombre;
    private string nombreDominio;
    private string email;
    private string usuario;
    private int idAutoriza;
    private Persona autoriza;
    private bool enPanelControl;
    private bool activo;
    private List<PermisoPersonal> permisos;
    private Legajo legajo;
    private string cuil;
    private DateTime horaEntrada;
    private DateTime horaSalida;
    private int baseID;

    // Propiedades.
    public string Nombre
    {
        get
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombre.ToLower());
        }
    }
    public string NombreDominio
    {
        get
        {
            return this.nombreDominio;
        }
    }
    public string Email
    {
        get
        {
            return email;
        }
    }
    public string Usuario
    {
        get { return this.usuario; }
    }
    public int ID
    {
        get
        {
            return idPersonal;
        }
    }
    public int IdAutoriza
    {
        get
        {
            return idAutoriza;
        }
    }
    public bool EnPanelControl
    {
        get { return this.enPanelControl; }
    }
    public bool Activo
    {
        get
        {
            return activo;
        }
    }
    public Persona Autoriza
    {
        get
        {
            if (this.autoriza == null)
            {
                this.autoriza = GPersonal.GetPersona(this.idAutoriza);
            }

            return this.autoriza;
        }
    }
    public List<PermisoPersonal> Permisos
    {
        get
        {
            if (this.permisos == null)
            {
                this.permisos = GPermisosPersonal.GetPermisosPersonal();
            }

            return this.permisos;
        }
    }
    public string Cuil
    {
        get { return cuil; }
    }
    public DateTime HoraEntrada
    {
        get { return horaEntrada; }
    }
    public DateTime HoraSalida
    {
        get { return horaSalida; }
    }
    public Legajo Legajo
    {
        get { return legajo; }
    }
    public int BaseID
    {
        get { return baseID; }
    }


    /// <summary>
    /// Almacena un Personal.
    /// </summary>
    internal Persona(int idPersonal, string nombre, string email, string usuario, int idAutoriza, bool enPanelControl,
        bool activo, Legajo legajo, string cuil, DateTime horaEntrada, DateTime horaSalida, int baseID)
    {
        this.idPersonal = idPersonal;
        this.nombre = nombre;
        this.email = email;
        this.usuario = usuario;
        this.idAutoriza = idAutoriza;
        this.enPanelControl = enPanelControl;
        this.activo = activo;
        this.nombreDominio = (Constantes.DominioNombre + "\\" + usuario).ToLower();
        this.legajo = legajo;
        this.cuil = cuil;
        this.horaEntrada = horaEntrada;
        this.horaSalida = horaSalida;
        this.baseID = baseID;
    }

    public int CompareTo(object obj)
    {
        Persona p = (Persona)obj;

        return this.Nombre.CompareTo(p.Nombre);
    }

    public bool Equals(Persona other)
    {
        return this.ID == other.ID;
    }
}

/// <summary>
/// Descripción breve de GPersonal
/// </summary>
public class GPersonal
{
    // Constantes.
    private const string NombreDefault = "No encontrado";
    private const string EmailDefault = "NoEncontrado";
    private const string UsuarioDefault = "NoEncontrado";
    private const int MaxRegistrosPagina = 30;


    /// <summary>
    /// Obtiene una persona.
    /// </summary>
    internal static Persona GetPersona(IDataReader dr)
    {
        Persona result;
        DateTime now = DateTime.Now;

        try
        {
            int hEntrada = Convert.ToInt32(dr["HorarioEntrada"]);
            int hSalida = Convert.ToInt32(dr["HorarioSalida"]);

            DateTime horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada/100,
                                                hEntrada - ((int) hEntrada/100)*100, 0);
            DateTime horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida / 100,
                                                hSalida - ((int)hSalida / 100) * 100, 0);

            result = new Persona(Convert.ToInt32(dr["idPersonal"]), dr["Nombre"].ToString(), dr["Email"].ToString(),
                                 dr["Usuario"].ToString(), Convert.ToInt32(dr["idAutoriza"]),
                                 Convert.ToBoolean(dr["EnPanelControl"]),
                                 Convert.ToBoolean(dr["Activo"]), LegajoFac.GetLegajo(dr), dr["CUIL"].ToString(),
                                 horaEntrada, horaSalida, Convert.ToInt32(dr["BaseID"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una persona.
    /// </summary>
    private static Persona GetPersona(DataRow dr)
    {
        Persona result;
        DateTime now = DateTime.Now;

        try
        {
            int hEntrada = Convert.ToInt32(dr["HorarioEntrada"]);
            int hSalida = Convert.ToInt32(dr["HorarioSalida"]);

            DateTime horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada / 100,
                                                hEntrada - ((int)hEntrada / 100) * 100, 0);
            DateTime horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida / 100,
                                                hSalida - ((int)hSalida / 100) * 100, 0);

            result = new Persona(Convert.ToInt32(dr["idPersonal"]), dr["Nombre"].ToString(), dr["Email"].ToString(),
                                 dr["Usuario"].ToString(), Convert.ToInt32(dr["idAutoriza"]),
                                 Convert.ToBoolean(dr["EnPanelControl"]),
                                 Convert.ToBoolean(dr["Activo"]), LegajoFac.GetLegajo(dr), dr["CUIL"].ToString(),
                                 horaEntrada, horaSalida, Convert.ToInt32(dr["BaseID"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una cadena con el nombre del personal separado por 'separador'.
    /// </summary>
    internal static string PersonalToString(List<Persona> lstPersonal, char separador)
    {
        string s = "";

        foreach (Persona p in lstPersonal)
        {
            s += p.Nombre + separador;
        }
        s = s.TrimEnd(separador);

        return s;
    }
    /// <summary>
    /// Obtiene un listado con Todas las personas activas.
    /// </summary>
    public static List<Persona> GetPersonasActivas()
    {
        return GetPersonas(true);
    }
    /// <summary>
    /// Obtiene un listado con Todas las personas activas.
    /// </summary>
    public static List<Persona> GetPersonas(bool soloActivas)
    {
        return GetPersonas(null, soloActivas);
    }
    /// <summary>
    /// Obtiene un listado de personal.
    /// </summary>
    public static List<Persona> GetPersonas(int[] idsPersonal)
    {
        return GetPersonas(idsPersonal, false);
    }
    /// <summary>
    /// Obtiene un listado de personal.
    /// </summary>
    private static List<Persona> GetPersonas(int[] idsPersonal, bool soloActivas)
    {
        List<Persona> result = new List<Persona>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, ISNULL(b.BaseID, @ValorInvalido) AS BaseID FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "LEFT JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE l.Activo = @Activo ";
            cmd.CommandText += (soloActivas ? "AND p.Activo = @Activo " : "");
            if (idsPersonal != null)
            {
                cmd.CommandText += "AND p.idPersonal IN (";
                foreach(int id in idsPersonal)
                {
                    cmd.CommandText += id + ",";
                }
                cmd.CommandText = cmd.CommandText.TrimEnd(',');

                cmd.CommandText += ") ";
            }
            cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constantes.ValorInvalido));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Persona persona = GetPersona(dr);
                if (persona != null)
                {
                    result.Add(persona);
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
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
    /// <summary>
    /// Comprueba si la persona existe.
    /// </summary>
    public static bool ExistePersona(int idPersonal)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        bool resultado;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Count(idPersona) FROM tbl_Personal WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));

            resultado = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
        catch
        {
            resultado = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return resultado;
    }
    /// <summary>
    /// Comprueba si la persona existe.
    /// </summary>
    public static bool ExistePersona(string usuario)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        bool resultado;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Count(idPersona) FROM tbl_Personal WHERE Usuario = @Usuario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));

            resultado = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
        catch
        {
            resultado = false;
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return resultado;
    }
    /// <summary>
    /// Obtiene una persona.
    /// </summary>
    public static Persona GetPersona(int idPersonal)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        Persona personal;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, ISNULL(b.BaseID, @ValorInvalido) AS BaseID, l.* FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "LEFT JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE l.Activo = @Activo AND p.idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constantes.ValorInvalido));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                // La persona no existe.
                throw new ElementoInexistenteException();
            }

            personal = GetPersona(dr);
            
            dr.Close();
        }
        catch
        {
            personal = new Persona(1, NombreDefault, EmailDefault, UsuarioDefault, 1, false, false, null, "",
                                   DateTime.Now, DateTime.Now, Constantes.ValorInvalido);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return personal;
    }
    /// <summary>
    /// Obtiene una persona.
    /// </summary>
    public static Persona GetPersona(string usuario)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        Persona personal;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, ISNULL(b.BaseID, @ValorInvalido) AS BaseID FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "LEFT JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE l.Activo = @Activo AND p.Usuario = @Usuario";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constantes.ValorInvalido));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                //La persona no existe.
                throw new ElementoInexistenteException();
            }

            personal = GetPersona(dr);
            
            dr.Close();
        }
        catch
        {
            personal = new Persona(1, NombreDefault, EmailDefault, UsuarioDefault, 1, false, false, null, "",
                                   DateTime.Now, DateTime.Now, Constantes.ValorInvalido);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return personal;
    }
    /// <summary>
    /// Devuelve el Nombre completo de una persona a partir de su ID.
    /// </summary>
    public static string GetNombrePersona(int idPersonal)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        string result = "";

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Nombre FROM tbl_Personal WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));

            result = cmd.ExecuteScalar().ToString();
        }
        catch
        {
            result = NombreDefault;
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
    /// Devuelve el Email de una persona a partir de su ID.
    /// </summary>
    public static string GetEmailPersona(int idPersonal)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        string result = "";

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Email FROM tbl_Personal WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));

            result = cmd.ExecuteScalar().ToString();
        }
        catch
        {
            result = EmailDefault;
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
    /// Guarda la Persona.
    /// </summary>
    public static void AltaPersonal(string nombre, string email, string usuario, int idAutoriza, bool enPanelControl,
        bool activo, string legajo, string cuil, int horaEntrada, int horaSalida, int baseID)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(nombre) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(usuario) ||
            idAutoriza == Constantes.IdPersonaInvalido)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Personal (Nombre, Email, Usuario, idAutoriza, EnPanelControl, ";
            cmd.CommandText += "Activo, Cuil, HorarioEntrada, HorarioSalida) VALUES (@Nombre, @Email, @Usuario, ";
            cmd.CommandText += "@idAutoriza, @EnPanelControl, @Activo, @Cuil, @HorarioEntrada, @HorarioSalida); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Personal;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutoriza", idAutoriza));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Email", email));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", activo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Cuil", cuil));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioEntrada", horaEntrada));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioSalida", horaSalida));
            int personalID = Convert.ToInt32(cmd.ExecuteScalar());
            
            // Agrego el legajo.
            LegajoFac.AddLegajo(personalID, legajo);

            // Lo asigno a la base.
            BaseFac.AddBasePersonal(baseID, personalID, trans);

            trans.Commit();
        }
        catch
        {
            if(trans != null) trans.Rollback();
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
    /// Actualiza los datos de la persona.
    /// </summary>
    public static void ActualizarPersonal(int idPersonal, string nombre, string email, string usuario, int idAutoriza,
        bool enPanelControl, bool activo, int horaEntrada, int horaSalida, int baseID)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(nombre) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(usuario) ||
            idAutoriza == Constantes.IdPersonaInvalido)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE tbl_Personal SET Nombre = @Nombre, Email = @email, ";
            cmd.CommandText += "Usuario = @Usuario, idAutoriza = @idAutoriza, EnPanelControl = @EnPanelControl, ";
            cmd.CommandText += "Activo = @Activo, HorarioEntrada = @HorarioEntrada, HorarioSalida = @HorarioSalida ";
            cmd.CommandText += "WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Nombre", nombre));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutoriza", idAutoriza));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Email", email));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Usuario", usuario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", activo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioEntrada", horaEntrada));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioSalida", horaSalida));
            cmd.ExecuteNonQuery();

            // Lo asigno a la base.
            BaseFac.DeletePersonalBase(idPersonal, trans);
            BaseFac.AddBasePersonal(baseID, idPersonal, trans);

            trans.Commit();
        }
        catch
        {
            if(trans != null) trans.Rollback();
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
    /// Obtiene las personas que coincidan con el filtro.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<Persona> GetPersonas(int pagina, List<Filtro> filtros)
    {
        List<Persona> result;

        result = DataAccess.GetDataList<Persona>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetPersona);

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetCantidadPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroJoin = "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
        string filtroWhere = "AND l.Activo = 1 ";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosPersona.Id:
                    filtroWhere += "AND p.idPersonal = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosPersona.Nombre:
                    filtroWhere += "AND p.Nombre LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosPersona.Usuario:
                    filtroWhere += "AND p.Usuario LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosPersona.Autoriza:
                    filtroWhere += "AND p.idAutoriza = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosPersona.EnPanelControl:
                    filtroWhere += "AND p.EnPanelControl = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosPersona.Estado:
                    filtroWhere += "AND p.Activo = " + filtro.Valor + " ";
                    break;
                default:
                    filtroWhere += "";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT COUNT(p.idPersonal) as TotalRegistros";
        }
        else
        {
            consulta = "SELECT p.*, ISNULL(b.BaseID, " + Constantes.ValorInvalido + ") AS BaseID";
            filtroJoin += "LEFT JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
        }

        if (filtroWhere.Length > 0)
        {
            filtroWhere = "WHERE " + filtroWhere;
        }
        consulta += " FROM tbl_Personal p " + filtroJoin + " " + filtroWhere;

        if (!cantidad)
        {
            consulta += " ORDER BY p.Nombre";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene los tipos de estado para las personas.
    /// </summary>
    public static Dictionary<int, string> GetEstadosPersona()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosPersona.Activa, "Activa");
        result.Add((int)EstadosPersona.Inactiva, "Inactiva");

        return result;
    }
    /// <summary>
    /// Obtiene las personas que son autorizadas por la persona.
    /// </summary>
    internal static Dictionary<int, string> GetPersonas(int idAutoriza, bool enPanelControl, bool soloActivas)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idPersonal, UPPER(Nombre) AS Nombre FROM tbl_Personal WHERE ";
            cmd.CommandText += "EnPanelControl = @EnPanelControl ";
            if (idAutoriza != Constantes.ValorInvalido && !GPermisosPersonal.TieneAcceso(PermisosPersona.RolDireccion) 
                && !GPermisosPersonal.TieneAcceso(PermisosPersona.PCVerGeneral))
            {
                cmd.CommandText += "AND idPersonal = @idAutoriza OR idAutoriza = @idAutoriza ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutoriza", idAutoriza));
            }
            // Elio Zapata tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
            if (idAutoriza == 125)
            {
                cmd.CommandText += "OR idPersonal IN (21, 97, 108, 113, 126, 127)";
            }
            // José Garcés tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
            if (idAutoriza == 188)
            {
                cmd.CommandText += "OR idPersonal IN (29, 195, 196)";
            }
            // Paulo Velardes tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
            if (idAutoriza == 56)
            {
                cmd.CommandText += "OR idPersonal IN (56,1468, 1471)";
            }
            // Mariano Arévalo tiene que poder ver los partes diarios de estas personas (las cuales no autoriza ¬¬).
            if (idAutoriza == 276)
            {
                cmd.CommandText += "OR idPersonal IN (161,153,232,156,157,183)";
            }
            if (soloActivas)
            {
                cmd.CommandText += "AND Activo = @Activo ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", soloActivas));
            }
            cmd.CommandText += "ORDER BY Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EnPanelControl", enPanelControl));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(Convert.ToInt32(dr["idPersonal"]), dr["Nombre"].ToString());
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
        
        List<int> customOrder = new List<int>();
        // Paulo Velardes no quiere el personal ordenado.

        if (idAutoriza == 56)
        {
            customOrder = new List<int>
            {
                56,
                1468,
                1471
            };
        }
        if (customOrder.Count > 0)
        {
            Dictionary<int, string> newOrder = new Dictionary<int, string>();
            customOrder.ForEach(id =>
            {
                if (result.ContainsKey(id)) newOrder.Add(id, result[id]);
            });
            result = newOrder;
        }

        return result;
    }
    /// <summary>
    /// Obtiene las personas asociadas a una obra.
    /// </summary>
    internal static List<Persona> GetPersonasInfObra(int idObraHistorico, string tabla)
    {
        List<Persona> result = new List<Persona>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, ISNULL(b.BaseID, @ValorInvalido) AS BaseID FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN " + tabla + " t ON p.idPersonal = t.idPersona ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "LEFT JOIN tbl_BasesPersonal b ON b.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE l.Activo = @Activo AND idObraHistorico = @idObraHistorico ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObraHistorico", idObraHistorico));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ValorInvalido", Constantes.ValorInvalido));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Persona persona = GetPersona(dr);
                if (persona != null)
                {
                    result.Add(persona);
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
            if (conn != null)
            {
                conn.Close();
            }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los emails de las personas asociadas a un rol.
    /// </summary>
    public static string GetEmails(PermisosPersona permiso)
    {
        string result = (permiso == PermisosPersona.SNV_NotifProducto) ? Constantes.EmailProveedores + "," : "";

        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.Email FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PermisosPersonal pe ON pe.idPersonal = p.idPersonal ";
            cmd.CommandText += "WHERE pe.idPermiso = @PermisoID AND p.Activo = @Activo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoID", (int)permiso));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result += dr["Email"] + ",";
            }
            
			result = result.TrimEnd(',');

            dr.Close();
        }
        catch
        {
            result = String.Empty;
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene un listado de personas asociadas a un rol.
    /// </summary>
    public static List<DataSourceItem> GetPersonas(PermisosPersona permiso)
    {
        List<DataSourceItem> result = new List<DataSourceItem>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.idPersonal, p.Nombre FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PermisosPersonal pe ON pe.idPersonal = p.idPersonal ";
            cmd.CommandText += "WHERE pe.idPermiso = @PermisoID AND p.Activo = @Activo ";
            cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PermisoID", (int)permiso));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(new DataSourceItem(Funciones.ToTitleCase(dr["Nombre"].ToString()), dr["idPersonal"]));
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
}
