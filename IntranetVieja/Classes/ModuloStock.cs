/*
 * Historial:
 * ===================================================================================
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Web;


public class Equipo
{
    // Variables.
    private int idEquipo;
    private ArticuloStock articuloAsociado;
    private List<ArticuloEquipo> articulos;

    // Propiedades.
    public int ID
    {
        get { return this.idEquipo; }
    }
    public ArticuloStock ArticuloAsociado
    {
        get { return this.articuloAsociado; }
    }
    public List<ArticuloEquipo> Articulos
    {
        get { return this.articulos; }
    }


    internal Equipo(int idEquipo, ArticuloStock articuloAsociado, List<ArticuloEquipo> articulos)
    {
        this.idEquipo = idEquipo;
        this.articuloAsociado = articuloAsociado;
        this.articulos = articulos;
    }
}

public class EquipoProduccion
{
    // Variables.
    private Equipo equipo;
    private int cantidad;

    // Propiedades.
    public Equipo Equipo
    {
        get { return this.equipo; }
    }
    public int Cantidad
    {
        get { return this.cantidad; }
    }


    internal EquipoProduccion(Equipo equipo, int cantidad)
    {
        this.equipo = equipo;
        this.cantidad = cantidad;
    }
}

public class ArticuloEquipo
{
    // Variables.
    private ArticuloStock articulo;
    private float cantidad;

    // Propiedades.
    public ArticuloStock Articulo
    {
        get { return this.articulo; }
    }
    public float Cantidad
    {
        get { return this.cantidad; }
    }


    internal ArticuloEquipo(ArticuloStock articulo, float cantidad)
    {
        this.articulo = articulo;
        this.cantidad = cantidad;
    }
}

public class ArticuloStock
{
    // Variables.
    private int idArticulo;
    private string codigo;
    private string descripcion;
    private float cantidad;
    private int ptoPedido;
    private bool esEquipo;

    // Propiedades.
    public int ID
    {
        get { return this.idArticulo; }
    }
    public string Codigo
    {
        get { return this.codigo; }
    }
    public string Descripcion
    {
        get { return this.descripcion; }
    }
    public float Cantidad
    {
        get { return this.cantidad; }
    }
    public int PuntoPedido
    {
        get { return this.ptoPedido; }
    }
    public bool EsEquipo
    {
        get { return this.esEquipo; }
    }


    internal ArticuloStock(int idArticulo, string codigo, string descripcion, float cantidad, int ptoPedido, bool esEquipo)
    {
        this.idArticulo = idArticulo;
        this.codigo = codigo;
        this.descripcion = descripcion;
        this.cantidad = cantidad;
        this.ptoPedido = ptoPedido;
        this.esEquipo = esEquipo;
    }
}

public static class ModuloStock
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;


    /// <summary>
    /// Genera el ingreso de un artículo.
    /// </summary>
    public static void IngresoArticulo(int idArticulo, float cantidad, string descripcion)
    {
        try
        {
            UpdateArticulo(idArticulo, StockOperaciones.Ingreso, cantidad, descripcion);
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Genera el egreso de un artículo.
    /// </summary>
    public static void EgresoArticulo(int idArticulo, float cantidad, string descripcion)
    {
        try
        {
            UpdateArticulo(idArticulo, StockOperaciones.Egreso, cantidad, descripcion);
        }
        catch
        {
            throw new ErrorOperacionException();
        }
    }
    /// <summary>
    /// Genera el ingreso/egreso de un artículo.
    /// </summary>
    private static void UpdateArticulo(int idArticulo, StockOperaciones operacion, float cantidad, string descripcion)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        
        if (cantidad <= 0 || (operacion != StockOperaciones.Ingreso && operacion != StockOperaciones.Egreso))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            UpdateArticulo(idArticulo, operacion, cantidad, descripcion, trans);

            // Finalizo la transacción.
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
            if (conn != null)
            {
                conn.Close();
            }
        }
    }   
    /// <summary>
    /// Genera el ingreso/egreso de un artículo.
    /// </summary>
    private static void UpdateArticulo(int idArticulo, StockOperaciones operacion, float cantidad, string descripcion,
        IDbTransaction trans)
    {
        if (operacion == StockOperaciones.Ingreso && !GPermisosPersonal.TieneAcceso(PermisosPersona.StockIngreso))
        {
            throw new ErrorOperacionException();
        }
        if (operacion == StockOperaciones.Egreso && !GPermisosPersonal.TieneAcceso(PermisosPersona.StockEgreso))
        {
            throw new ErrorOperacionException();
        }

        if (cantidad <= 0 || (operacion != StockOperaciones.Ingreso && operacion != StockOperaciones.Egreso))
        {
            throw new DatosInvalidosException();
        }

        IDbCommand cmd;

        // Verifico que haya stock.
        if (operacion == StockOperaciones.Egreso)
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "SELECT cantidad FROM tbl_StockArticulos WHERE idArticulo = @idArticulo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idArticulo", idArticulo));

            float cantActual = Convert.ToSingle(cmd.ExecuteScalar());

            if (cantActual - cantidad < 0)
            {
                throw new ErrorOperacionException();
            }
        }

        // Actualizo la tabla de stock.
        cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "UPDATE tbl_StockArticulos SET cantidad = cantidad + @cantidad WHERE idArticulo = @idArticulo;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idArticulo", idArticulo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@cantidad", cantidad *
            (operacion == StockOperaciones.Egreso ? -1 : 1)));
        cmd.ExecuteNonQuery();

        // Agrego el registro histórico de la operación.
        AddHistorico(idArticulo, operacion, cantidad, descripcion, trans);
    }    
    /// <summary>
    /// Agrega un registro histórico.
    /// </summary>
    private static void AddHistorico(int idArticulo, StockOperaciones operacion, float cantidad, string descripcion, 
        IDbTransaction trans)
    {
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "INSERT INTO tbl_StockHistoricos (idArticulo, tipoOperacion, cantidad, descripcion, idPersonal) VALUES";
        cmd.CommandText += "(@idArticulo, @tipoOperacion, @cantidad, @descripcion, @idPersonal);";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idArticulo", idArticulo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@tipoOperacion", (int)operacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@cantidad", cantidad));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@descripcion", descripcion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", Constantes.Usuario.ID));

        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Agrega un equipo a producción. Si ya se encuentra, actualiza la cantidad.
    /// </summary>
    private static void AddEquipoProduccion(int idEquipo, int cantidad, IDbTransaction trans)
    {
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "SELECT COUNT(idEquipo) FROM tbl_EquiposProduccion WHERE idEquipo = @idEquipo;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));

        bool exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

        cmd = DataAccess.GetCommand(trans);
        if (exists)
        {
            cmd.CommandText = "UPDATE tbl_EquiposProduccion SET cantidad = cantidad + @cantidad WHERE idEquipo = @idEquipo;";
        }
        else
        {
            cmd.CommandText = "INSERT INTO tbl_EquiposProduccion (idEquipo, cantidad) VALUES (@idEquipo, @cantidad);";
        }
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@cantidad", cantidad));
        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Agrega un equipo terminado.
    /// </summary>
    public static void AddEquipoTerminado(int idEquipo, int cantidad)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        if (cantidad <= 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            Equipo equipo = GetEquipo(idEquipo, conn);
            if (equipo == null)
            {
                throw new ElementoInexistenteException();
            }

            trans = DataAccess.GetTransaction(conn);

            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "SELECT cantidad FROM tbl_EquiposProduccion WHERE idEquipo = @idEquipo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));
            int cantEquipo = Convert.ToInt32(cmd.ExecuteScalar());

            if (cantEquipo < cantidad)
            {
                throw new ErrorOperacionException();
            }
            
            cmd = DataAccess.GetCommand(trans);
            if (cantEquipo == cantidad)
            {
                cmd.CommandText = "DELETE FROM tbl_EquiposProduccion WHERE idEquipo = @idEquipo;";
            }
            else
            {
                cmd.CommandText = "UPDATE tbl_EquiposProduccion SET cantidad = cantidad - @cantidad WHERE idEquipo = @idEquipo;";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@cantidad", cantidad));
            }
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));
            cmd.ExecuteNonQuery();

            // Hago un ingreso del equipo terminado.
            IngresoArticulo(equipo.ArticuloAsociado.ID, cantidad, "Equipo terminado.");

            // Finalizo la transacción.
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
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
    /// <summary>
    /// Agrega un artículo.
    /// </summary>
    public static void AddArticulo(string codigo, int ptoPedido)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        if (String.IsNullOrEmpty(codigo) || ptoPedido <= 0)
        {
            throw new DatosInvalidosException();
        }

        ArticuloTango at = GArticuloTango.GetArticuloTango(codigo);
        if (at == null)
        {
            throw new ElementoInexistenteException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_StockArticulos (codigo, cantidad, ptoPedido) VALUES (@codigo, @cantidad, @ptoPedido);";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@codigo", codigo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@cantidad", 0));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ptoPedido", ptoPedido));

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
    /// Obtiene un artículo.
    /// </summary>
    private static ArticuloStock GetArticulo(int idArticulo, IDbConnection conn)
    {
        ArticuloStock result;
        IDataReader dr = null;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 sa.codigo, sa.cantidad, sa.ptoPedido, ";
            cmd.CommandText += "CASE ISNULL(eq.idArticulo, -1) WHEN -1 THEN 0 ELSE 1 END AS EsEquipo ";
            cmd.CommandText += "FROM tbl_StockArticulos sa LEFT JOIN tbl_Equipos eq ON sa.idArticulo = eq.idArticulo ";
            cmd.CommandText += "WHERE sa.idArticulo = @idArticulo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idArticulo", idArticulo));
            dr = cmd.ExecuteReader();

            dr.Read();
            ArticuloTango at = GArticuloTango.GetArticuloTango(dr["codigo"].ToString());
            if (at == null)
            {
                throw new ElementoInexistenteException();
            }

            result = new ArticuloStock(idArticulo, dr["codigo"].ToString(), at.Descripcion, Convert.ToSingle(dr["cantidad"]),
                Convert.ToInt32(dr["ptoPedido"]), Convert.ToInt32(dr["EsEquipo"]) == 1);

            dr.Close();
        }
        catch
        {
            if(dr != null && !dr.IsClosed)
            {
                dr.Close();
            }

            throw new ElementoInexistenteException();
        }

        return result;
    }
    /// <summary>
    /// Obtiene un equipo.
    /// </summary>
    public static Equipo GetEquipo(int idEquipo)
    {
        Equipo result;
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetEquipo(idEquipo, conn);
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
    /// Obtiene un equipo.
    /// </summary>
    private static Equipo GetEquipo(int idEquipo, IDbConnection conn)
    {
        Equipo result = null;
        IDataReader dr = null;

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 idArticulo FROM tbl_Equipos WHERE idEquipo = @idEquipo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));
            int idArticuloAsociado = Convert.ToInt32(cmd.ExecuteScalar());

            ArticuloStock articuloAsociado = GetArticulo(idArticuloAsociado, conn);

            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT sa.idArticulo, ea.cantidad FROM tbl_StockArticulos sa ";
            cmd.CommandText += "INNER JOIN tbl_EquiposArticulos ea ON sa.idArticulo = ea.idArticulo WHERE ";
            cmd.CommandText += "ea.idEquipo = @idEquipo";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idEquipo", idEquipo));
            dr = cmd.ExecuteReader();

            Dictionary<int, float> idArticulos = new Dictionary<int, float>();
            while (dr.Read())
            {
                int idArticulo = Convert.ToInt32(dr["idArticulo"]);
                idArticulos.Add(idArticulo, Convert.ToSingle(dr["cantidad"]));
            }

            dr.Close();

            List<ArticuloEquipo> articulos = new List<ArticuloEquipo>();
            foreach (int idArticulo in idArticulos.Keys)
            {
                ArticuloStock articulo = GetArticulo(idArticulo, conn);
                articulos.Add(new ArticuloEquipo(articulo, idArticulos[idArticulo]));
            }

            result = new Equipo(idEquipo, articuloAsociado, articulos);
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }

            throw new ElementoInexistenteException();
        }

        return result;
    }
    /// <summary>
    /// Obtiene los artículos que necesitan reposición.
    /// </summary>
    public static List<ArticuloStock> GetArticulosReposicion()
    {
        List<ArticuloStock> result = new List<ArticuloStock>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idArticulo FROM tbl_StockArticulos WHERE cantidad <= ptoPedido;";
            dr = cmd.ExecuteReader();

            List<int> idArticulos = new List<int>();
            while (dr.Read())
            {
                int idArticulo = Convert.ToInt32(dr["idArticulo"]);
                idArticulos.Add(idArticulo);
            }

            dr.Close();

            idArticulos.ForEach(idArticulo => result.Add(GetArticulo(idArticulo, conn)));
        }
        catch
        {
            if (dr != null && !dr.IsClosed)
            {
                dr.Close();
            }

            throw new ErrorOperacionException();
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
    /// Genera el egreso de un equipo.
    /// </summary>
    public static void EgresoEquipo(int idEquipo, int cantidad)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        if (cantidad <= 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            Equipo equipo = GetEquipo(idEquipo, conn);

            trans = DataAccess.GetTransaction(conn);

            // Genero el egreso de cada artículo.
            equipo.Articulos.ForEach(articulo => EgresoArticulo(articulo.Articulo.ID, articulo.Cantidad * cantidad,
                "Armado de equipo: " + equipo.ArticuloAsociado.Descripcion + "."));

            // Agrego a producción.
            AddEquipoProduccion(idEquipo, cantidad, trans);

            // Finalizo la transacción.
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
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
    /// <summary>
    /// Envía una alerta sobre la reposición de un artículo.
    /// </summary>
    private static void EnviarAlertaReposicion(ArticuloStock articulo)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_ALERTA_STOCK);

        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        if (articulo == null)
        {
            throw new EmailException();
        }

        string asunto = "Alerta de reposición: " + articulo.Descripcion;
        string de = Constantes.EmailIntranet;
        string para = Constantes.EmailAlertaStock;
        string cc = "";

        strPlantilla = strPlantilla.Replace("@COD_ARTICU", articulo.Codigo);
        strPlantilla = strPlantilla.Replace("@DESC_ARTICULO", articulo.Descripcion);
        strPlantilla = strPlantilla.Replace("@CANTIDAD", articulo.Cantidad.ToString());
        strPlantilla = strPlantilla.Replace("@PTO_PEDIDO", articulo.PuntoPedido.ToString());

        Email email = new Email(de, para, cc, asunto, strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene un artículo.
    /// </summary>
    private static int GetArticulo(DataRow dr)
    {
        int result;

        try
        {
            result = Convert.ToInt32(dr["idArticulo"]);
        }
        catch
        {
            result = Constantes.ValorInvalido;
        }

        return result;
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string filtroJoin = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            if (Enum.IsDefined(typeof(FiltrosArticuloStock), filtro.Tipo))
            {
                FiltrosArticuloStock f = (FiltrosArticuloStock)filtro.Tipo;
                switch (f)
                {

                }
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idArticulo) as TotalRegistros FROM tbl_StockArticulos sa "
                     + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "");
        }
        else
        {
            consulta = "SELECT idArticulo FROM tbl_StockArticulos " + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "");
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetArticulosPaginas(List<Filtro> filtros)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.StockVer))
        {
            throw new ErrorOperacionException();
        }

        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
    }
    /// <summary>
    /// Obtiene una lista con los artículos.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<ArticuloStock> GetArticulos(int pagina, List<Filtro> filtros)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.StockVer))
        {
            throw new ErrorOperacionException();
        }

        List<ArticuloStock> result = new List<ArticuloStock>();

        List<int> search;
        search = DataAccess.GetDataList<int>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetArticulo);

        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            foreach (int idArticulo in search)
            {
                try
                {
                    ArticuloStock articulo = GetArticulo(idArticulo, conn);
                    result.Add(articulo);
                }
                catch
                {

                }
            }
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los equipos que se encuentran en producción.
    /// </summary>
    public static List<EquipoProduccion> GetEquiposProduccion()
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.StockVer))
        {
            throw new ErrorOperacionException();
        }

        List<EquipoProduccion> result = new List<EquipoProduccion>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);

            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idEquipo, cantidad FROM tbl_EquiposProduccion";
            dr = cmd.ExecuteReader();

            Dictionary<int, int> idEquipos = new Dictionary<int, int>();
            while (dr.Read())
            {
                int idEquipo = Convert.ToInt32(dr["idEquipo"]);
                idEquipos.Add(idEquipo, Convert.ToInt32(dr["cantidad"]));
            }

            dr.Close();

            foreach (int idEquipo in idEquipos.Keys)
            {
                Equipo equipo = GetEquipo(idEquipo, conn);
                result.Add(new EquipoProduccion(equipo, idEquipos[idEquipo]));
            }
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
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene los equipos.
    /// </summary>
    public static Dictionary<int, string> GetEquipos()
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.StockVer))
        {
            throw new ErrorOperacionException();
        }

        Dictionary<int, string> result = new Dictionary<int, string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);

            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT e.idEquipo, sa.codigo FROM tbl_StockArticulos sa ";
            cmd.CommandText += "INNER JOIN tbl_Equipos e ON sa.idArticulo = e.idArticulo;";
            dr = cmd.ExecuteReader();

            Dictionary<int, string> codigosEquipos = new Dictionary<int, string>();
            while (dr.Read())
            {
                codigosEquipos.Add(Convert.ToInt32(dr["idEquipo"]), dr["codigo"].ToString());
            }

            dr.Close();

            foreach (int idEquipo in codigosEquipos.Keys)
            {
                ArticuloTango at = GArticuloTango.GetArticuloTango(codigosEquipos[idEquipo]);
                if (at != null)
                {
                    result.Add(idEquipo, at.Descripcion);
                }
            }
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
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Obtiene si un equipo se puede armar. En caso contrario, devuelve los artículos que faltan.
    /// </summary>
    public static bool GetDisponibilidadEquipo(int idEquipo, int cantidad, out List<ArticuloEquipo> faltantes)
    {
        bool result = true;
        faltantes = new List<ArticuloEquipo>();

        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            Equipo equipo = GetEquipo(idEquipo, conn);
            if (equipo == null)
            {
                throw new ElementoInexistenteException();
            }

            foreach (ArticuloEquipo articulo in equipo.Articulos)
            {
                if (articulo.Articulo.Cantidad - articulo.Cantidad * cantidad < 0)
                {
                    faltantes.Add(articulo);
                    result = false;
                }
            }
        }
        catch
        {
            result = false;
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        return result;
    }
    /// <summary>
    /// Actualiza el punto de pedido para un artículo.
    /// </summary>
    public static void UpdatePtoPedido(int idArticulo, int ptoPedido)
    {
        IDbConnection conn = null;

        if (ptoPedido <= 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_StockArticulos SET ptoPedido = @ptoPedido WHERE idArticulo = @idArticulo;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idArticulo", idArticulo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ptoPedido", ptoPedido));

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
}