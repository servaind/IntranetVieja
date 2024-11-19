using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using EX = Microsoft.Office.Interop.Excel;
using ExcelApp = Microsoft.Office.Interop.Excel.Application;
using System.Reflection;


public class SolEnvMat
{
    // Variables.
    private int idSEM;
    private int numero;
    private DateTime fecha;
    private string destino;
    private string telefono;
    private PrioridadSolEnvMat prioridad;
    private List<Persona> destinatarios;
    private Persona destinatarioAdic;
    private string retiraDe;
    private TIPO_TRANSP_SEM transporte;
    private short cantBultos;
    private float valorDeclarado;
    private ESTADO_SEM estado;
    private List<ItemSolEnvMat> lstItems;

    // Propiedades.
    public int ID
    {
        get { return idSEM; }
    }
    public int Numero
    {
        get { return numero; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
    }
    public string Destino
    {
        get { return destino; }
    }
    public string Telefono
    {
        get { return telefono; }
    }
    public PrioridadSolEnvMat Prioridad
    {
        get { return prioridad; }
    }
    public List<Persona> Destinatarios
    {
        get { return destinatarios; }
    }
    public Persona DestinatarioAdicional
    {
        get { return destinatarioAdic; }
    }
    public string RetiraDe
    {
        get { return retiraDe; }
    }
    public TIPO_TRANSP_SEM Transporte
    {
        get { return transporte; }
    }
    public short CantidadBultos
    {
        get { return cantBultos; }
    }
    public float ValorDeclarado
    {
        get { return valorDeclarado; }
    }
    public ESTADO_SEM Estado
    {
        get { return estado; }
    }
    public List<ItemSolEnvMat> Items
    {
        get { return lstItems; }
    }


    internal SolEnvMat(int idSEM, int numero, DateTime fecha, string destino, string telefono, 
        PrioridadSolEnvMat prioridad, List<Persona> destinatarios, Persona destinatarioAdic, 
        string retiraDe, TIPO_TRANSP_SEM transporte, short cantBultos, float valorDeclarado, 
        ESTADO_SEM estado)
    {
        this.idSEM = idSEM;
        this.numero = numero;
        this.fecha = fecha;
        this.destino = destino;
        this.telefono = telefono;
        this.prioridad = prioridad;
        this.destinatarios = destinatarios;
        this.destinatarioAdic = destinatarioAdic;
        this.retiraDe = retiraDe;
        this.transporte = transporte;
        this.cantBultos = cantBultos;
        this.valorDeclarado = valorDeclarado;
        this.estado = estado;
    }

    public bool CargarItems()
    {
        this.lstItems = GSolEnvMat.ObtenerItemsSEM(this.idSEM);

        return this.lstItems.Count > 0;
    }
}
public class PrioridadSolEnvMat
{
    // Variables.
    private int idPrioridad;
    private string descripcion;

    // Propiedades.
    public int ID
    {
        get { return idPrioridad; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }


    internal PrioridadSolEnvMat(int idPrioridad, string descripcion)
    {
        this.idPrioridad = idPrioridad;
        this.descripcion = descripcion;
    }
}
public class ItemSolEnvMat
{
    // Variables.
    private int idItem;
    private string descripcion;
    private Imputacion imputacion;
    private ORIGEN_ITEM_SEM origenDatos;
    private int numOrigenDatos;
    private string destino;
    private string codigo;
    private string codigoIAT;
    private bool retorna;
    private TIPO_CAUSA_SEM causa;
    private string ns;
    private string motivo;
    private float cantidad;
    private int numeroJT;

    // Propiedades.
    public int ID
    {
        get { return idItem; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public Imputacion Imputacion
    {
        get { return imputacion; }
    }
    public ORIGEN_ITEM_SEM OrigenDatos
    {
        get { return origenDatos; }
    }
    public int NumeroOrigenDatos
    {
        get { return numOrigenDatos; }
    }
    public string Destino
    {
        get { return destino; }
    }
    public string Codigo
    {
        get { return codigo; }
    }
    public string CodigoIAT
    {
        get { return codigoIAT; }
    }
    public bool Retorna
    {
        get { return retorna; }
    }
    public TIPO_CAUSA_SEM Causa
    {
        get { return causa; }
    }
    public string NS
    {
        get { return ns; }
    }
    public string Motivo
    {
        get { return motivo; }
    }
    public float Cantidad
    {
        get { return cantidad; }
    }
    public int NumeroJT
    {
        get { return numeroJT; }
    }


    public ItemSolEnvMat(string descripcion, Imputacion imputacion, ORIGEN_ITEM_SEM origenDatos,
        int numOrigenDatos, string destino, string codigo, string codigoIAT, bool retorna, TIPO_CAUSA_SEM causa, 
        string ns, string motivo, float cantidad, int numeroJT) : this(Constantes.ValorInvalido, descripcion, 
        imputacion, origenDatos, numOrigenDatos, destino, codigo, codigoIAT, retorna, causa, ns, motivo, 
        cantidad, numeroJT)
    {

    }
    internal ItemSolEnvMat(int idItem, string descripcion, Imputacion imputacion,
        ORIGEN_ITEM_SEM origenDatos, int numOrigenDatos, string destino, string codigo, string codigoIAT,
        bool retorna, TIPO_CAUSA_SEM causa, string ns, string motivo, float cantidad, int numeroJT)
    {
        this.idItem = idItem;
        this.descripcion = descripcion;
        this.imputacion = imputacion;
        this.origenDatos = origenDatos;
        this.numOrigenDatos = numOrigenDatos;
        this.destino = destino;
        this.codigo = codigo;
        this.codigoIAT = codigoIAT;
        this.retorna = retorna;
        this.causa = causa;
        this.ns = ns;
        this.motivo = motivo;
        this.cantidad = cantidad;
        this.numeroJT = numeroJT;
    }
}
public abstract class SolEnvMatRemito
{
    // Constantes.

    // Variables.
    protected SolEnvMat sem;
    protected string pathArchivo;

    // Propiedades.
    public string PathArchivo
    {
        get { return pathArchivo; }
    }


    internal SolEnvMatRemito(SolEnvMat sem)
    {
        this.sem = sem;
    }
}
public class SolEnvMatRemitoNoOficial : SolEnvMatRemito
{
    // Variables.
    private ExcelApp excel;


    internal SolEnvMatRemitoNoOficial(SolEnvMat sem) 
        : base(sem)
    {

    }
    /// <summary>
    /// Guarda un archivo con formato Excel 97/2003 y obtiene el path del archivo.
    /// </summary>
    /// <returns></returns>
    public string ExportarAExcel()
    {
        if (this.sem == null)
        {
            throw new NullReferenceException("La Solicitud de Envío de Materiales no puede ser null.");
        }

        Random r = new Random(DateTime.Now.Millisecond);
        string path = Constantes.PATH_TEMP + "archivo" + r.Next() + ".xls";

        // Abro el Excel.
        excel = new ExcelApp();
        excel.Visible = false;

        // Abro el libro.
        Workbook libro = excel.Workbooks.Open(Constantes.PATH_PLANTILLA_SEM_REM_NO_OF_XLS, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

        try
        {
            // Abro la hoja.
            Worksheet hojaRemitoNoOficial = (Worksheet)libro.Worksheets["RemitoNoOficial"];

            // Número.  (G:3)
            hojaRemitoNoOficial.Cells[3, 7] = this.sem.Numero.ToString("0000");

            // Fecha.   (G:4)
            hojaRemitoNoOficial.Cells[4, 7] = this.sem.Fecha.ToShortDateString();

            // Destinatarios.   (B:12, B:13)
            hojaRemitoNoOficial.Cells[12, 2] = GSolEnvMat.DestinatariosToString(this.sem.Destinatarios);
            string destinatarios = String.Format("{0} {1}", this.sem.DestinatarioAdicional == null ? "" : 
                this.sem.DestinatarioAdicional.Nombre + " ", this.sem.Telefono.Length > 0 ? "[ " + this.sem.Telefono + " ]" 
                : "");
            hojaRemitoNoOficial.Cells[13, 2] = destinatarios.Trim();

            // Domicilio.       (B:14)
            hojaRemitoNoOficial.Cells[14, 2] = this.sem.Destino;

            // Localidad.       (F:14)
            hojaRemitoNoOficial.Cells[14, 6] = this.sem.RetiraDe;

            // Items.
            if (this.sem.Items == null)
            {
                throw new NoHayItemsException();
            }

            // Cada ítem ocupa 2 filas.
            int idFila = 19;
            foreach (ItemSolEnvMat item in this.sem.Items)
            {
                // Cantidad.    (A:idFila)
                hojaRemitoNoOficial.Cells[idFila, 1] = item.Cantidad.ToString();

                // Detalle.     (B:idFila)
                hojaRemitoNoOficial.Cells[idFila, 2] = item.Descripcion;

                // Imputación.  (G:idFila)
                hojaRemitoNoOficial.Cells[idFila, 7] = "IMP " + item.Imputacion;

                // Motivo       (B:idFila + 1)
                hojaRemitoNoOficial.Cells[idFila + 1, 2] = item.Motivo;

                // Destino      (G:idFila + 1)
                hojaRemitoNoOficial.Cells[idFila + 1, 7] = item.Destino;

                idFila += 2;
            }

            // At.Sr.           (B:52)
            hojaRemitoNoOficial.Cells[52, 2] = GSolEnvMat.DestinatariosToString(this.sem.Destinatarios);

            // Cantidad de bultos.  (C:53)
            hojaRemitoNoOficial.Cells[53, 3] = this.sem.CantidadBultos.ToString();

            // Valor declarado.     (C:54)
            hojaRemitoNoOficial.Cells[54, 3] = this.sem.ValorDeclarado.ToString("$ 0");

            // Guardo el libro.
            libro.SaveAs(path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            // Cierro el Excel.
            libro.Close(false, Missing.Value, Missing.Value);
            excel.Quit();

            // Libero los recursos.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(hojaRemitoNoOficial);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(libro);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        this.pathArchivo = path;

        return path;
    }
}
public class SolEnvMatRemitoOficial : SolEnvMatRemito
{
    // Variables.
    private ExcelApp excel;


    internal SolEnvMatRemitoOficial(SolEnvMat sem)
        : base(sem)
    {

    }
    /// <summary>
    /// Guarda un archivo con formato Excel 97/2003 y obtiene el path del archivo.
    /// </summary>
    /// <returns></returns>
    public string ExportarAExcel()
    {
        if (this.sem == null)
        {
            throw new NullReferenceException("La Solicitud de Envío de Materiales no puede ser null.");
        }

        Random r = new Random(DateTime.Now.Millisecond);
        string path = Constantes.PATH_TEMP + "archivo" + r.Next() + ".xls";

        // Abro el Excel.
        excel = new ExcelApp();
        excel.Visible = false;

        // Abro el libro.
        Workbook libro = excel.Workbooks.Open(Constantes.PATH_PLANTILLA_SEM_REM_OF_XLS, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

        try
        {
            // Abro la hoja.
            Worksheet hojaRemitoOficial = (Worksheet)libro.Worksheets["RemitoOficial"];

            // Número.  (G:3)
            hojaRemitoOficial.Cells[3, 7] = this.sem.Numero.ToString("0000");

            // Fecha.   (G:4)
            hojaRemitoOficial.Cells[4, 7] = this.sem.Fecha.ToShortDateString();

            // Destinatarios.   (B:10)
            string destinatarios = String.Format("{0} {1}", GSolEnvMat.DestinatariosToString(this.sem.Destinatarios), 
                this.sem.DestinatarioAdicional == null ? "" : " / " + this.sem.DestinatarioAdicional.Nombre + " ");
            hojaRemitoOficial.Cells[10, 2] = destinatarios;

            // Domicilio.       (B:12)
            hojaRemitoOficial.Cells[12, 2] = this.sem.Destino;

            // Localidad.       (G:14)
            hojaRemitoOficial.Cells[12, 7] = this.sem.RetiraDe;

            // C.U.I.T.         (G:14)
            hojaRemitoOficial.Cells[14, 7] = "";

            // Proveedor.       (B:16)
            hojaRemitoOficial.Cells[16, 2] = "";

            // Items.
            if (this.sem.Items == null)
            {
                throw new NoHayItemsException();
            }

            // Cada ítem ocupa 2 filas.
            int idFila = 19;
            foreach (ItemSolEnvMat item in this.sem.Items)
            {
                // Cantidad.    (A:idFila)
                hojaRemitoOficial.Cells[idFila, 1] = item.Cantidad.ToString();

                // Detalle.     (B:idFila)
                hojaRemitoOficial.Cells[idFila, 2] = item.Descripcion;

                // Nº Serie     (B:idFila + 1)
                hojaRemitoOficial.Cells[idFila + 1, 2] = item.NS;

                idFila += 2;
            }

            // At.Sr.           (B:52)
            hojaRemitoOficial.Cells[52, 2] = GSolEnvMat.DestinatariosToString(this.sem.Destinatarios);

            // Cantidad de bultos.  (C:53)
            hojaRemitoOficial.Cells[53, 3] = this.sem.CantidadBultos.ToString();

            // Valor declarado.     (C:54)
            hojaRemitoOficial.Cells[54, 3] = this.sem.ValorDeclarado.ToString("$ 0");

            // Guardo el libro.
            libro.SaveAs(path, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            // Cierro el Excel.
            libro.Close(false, Missing.Value, Missing.Value);
            excel.Quit();

            // Libero los recursos.
            System.Runtime.InteropServices.Marshal.ReleaseComObject(hojaRemitoOficial);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(libro);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        this.pathArchivo = path;

        return path;
    }
}



/// <summary>
/// Descripción breve de GSolEnvMat
/// </summary>
public class GSolEnvMat
{
    private static List<Persona> ObtenerDestinatariosSEM(int idSEM)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        List<Persona> result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();

            result = ObtenerDestinatariosSEM(conn, idSEM);
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
    /// Obtiene los destinatarios de una SEM.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="idSEM"></param>
    /// <returns></returns>
    private static List<Persona> ObtenerDestinatariosSEM(SqlConnection conn, int idSEM)
    {
        List<Persona> lstDestinatarios = new List<Persona>();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr = null;

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = "SELECT idPersonal FROM tbl_SEMDestinatarios WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                throw new Exception("No hay destinatarios.");
            }

            while (dr.Read())
            {
                Persona p = GPersonal.GetPersona(Convert.ToInt32(dr["idPersonal"]));
                lstDestinatarios.Add(p);
            }

            if (lstDestinatarios.Count == 0)
            {
                throw new Exception("No hay destinatarios.");
            }
        }
        catch
        {
            throw new Exception("Error al cargar los destinatarios.");
        }
        finally
        {
            if (dr != null)
            {
                dr.Close();
            }
        }

        return lstDestinatarios;
    }
    /// <summary>
    /// Obtiene una Prioridad.
    /// </summary>
    /// <param name="idPrioridad"></param>
    /// <returns></returns>
    public static PrioridadSolEnvMat ObtenerPrioridadSEM(int idPrioridad)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        PrioridadSolEnvMat result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();

            result = ObtenerPrioridadSEM(conn, idPrioridad);
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
    /// Obtiene una Prioridad.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="idPrioridad"></param>
    /// <returns></returns>
    private static PrioridadSolEnvMat ObtenerPrioridadSEM(SqlConnection conn, int idPrioridad)
    {
        PrioridadSolEnvMat prioridad = null;
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr = null;

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = "SELECT Descripcion FROM tbl_SEMPrioridades WHERE ";
            cmd.CommandText += "idPrioridad = @idPrioridad";
            cmd.Parameters.Add("@idPrioridad", SqlDbType.TinyInt, 1).Value = idPrioridad;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                throw new Exception("No hay prioridad.");
            }

            prioridad = new PrioridadSolEnvMat(
                idPrioridad,
                dr["Descripcion"].ToString()
            );
        }
        catch
        {
            throw new Exception("Error al cargar la prioridad.");
        }
        finally
        {
            if (dr != null)
            {
                dr.Close();
            }
        }

        return prioridad;
    }
    /// <summary>
    /// Obtiene las Prioridades disponibles.
    /// </summary>
    /// <returns></returns>
    public static List<PrioridadSolEnvMat> ObtenerPrioridadesSEM()
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        List<PrioridadSolEnvMat> lstPrioridades = new List<PrioridadSolEnvMat>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT idPrioridad, Descripcion FROM tbl_SEMPrioridades ORDER BY ";
            cmd.CommandText += "idPrioridad";
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                throw new Exception("No hay prioridades.");
            }

            while (dr.Read())
            {
                PrioridadSolEnvMat prioridad = new PrioridadSolEnvMat(
                    Convert.ToInt32(dr["idPrioridad"]),
                    dr["Descripcion"].ToString()
                );

                lstPrioridades.Add(prioridad);
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

        return lstPrioridades;
    }
    /// <summary>
    /// Obtiene el ID del número de SEM.
    /// </summary>
    /// <param name="numero"></param>
    /// <returns></returns>
    public static int ObtenerIDSEM(int numero)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        int result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT idSEM FROM tbl_SEM WHERE Numero = @Numero";
            cmd.Parameters.Add("@Numero", SqlDbType.Int, 4).Value = numero;

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            result = Constantes.ValorInvalido;
        }
        finally
        {
            conn.Close();
        }

        return result;
    }
    /// <summary>
    /// Obtiene una Solicitud de Envío de Materiales en base a un Data Reader.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="dr">Data Reader de la consulta. Ya se debe haber ejecutado el método Read().</param>
    /// <returns></returns>
    private static SolEnvMat ObtenerSEM(SqlConnection conn, SqlDataReader dr)
    {
        SolEnvMat sev;

        try
        {
            sev = new SolEnvMat(
                Convert.ToInt32(dr["idSEM"]),
                Convert.ToInt32(dr["Numero"]),
                Convert.ToDateTime(dr["Fecha"]),
                dr["Destino"].ToString(),
                dr["Telefono"].ToString(),
                ObtenerPrioridadSEM(Convert.ToInt32(dr["idPrioridad"])),
                ObtenerDestinatariosSEM(Convert.ToInt32(dr["idSEM"])),
                Convert.ToInt32(dr["idDestinatarioAdic"]) != Constantes.IdPersonaInvalido ? 
                GPersonal.GetPersona(Convert.ToInt32(dr["idDestinatarioAdic"])) : null,
                dr["RetiraDe"].ToString(),
                (TIPO_TRANSP_SEM)Convert.ToInt16(dr["idTipoTransporte"]),
                Convert.ToInt16(dr["CantBultos"]),
                Convert.ToSingle(dr["ValorDeclarado"]),
                (ESTADO_SEM)Convert.ToInt16(dr["idEstado"])
            );
        }
        catch
        {
            sev = null;
        }

        return sev;
    }
    /// <summary>
    /// Obtiene una Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="idSEM"></param>
    /// <returns></returns>
    public static SolEnvMat ObtenerSEM(int idSEM)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        SolEnvMat sev = null;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_SEM WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows || !dr.Read())
            {
                throw new Exception("La Solicitud de Envío de Materiales no existe.");
            }

            sev = ObtenerSEM(conn, dr);

            dr.Close();
        }
        catch
        {
            sev = null;
        }
        finally
        {
            conn.Close();
        }

        return sev;
    }
    /// <summary>
    /// Obtiene un Ítem de Solicitud de Envío de Materiales en base a un Data Reader.
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    private static ItemSolEnvMat ObtenerItemSEM(SqlDataReader dr)
    {
        ItemSolEnvMat item;

        try
        {
            item = new ItemSolEnvMat(
                Convert.ToInt32(dr["idItem"]),
                dr["Descripcion"].ToString(),
                GImputaciones.GetImputacion(Convert.ToInt32(dr["idImputacion"])),
                (ORIGEN_ITEM_SEM)Convert.ToInt32(dr["OrigenDatos"]),
                Convert.ToInt32(dr["NumOrigenDatos"]),
                dr["Destino"].ToString(),
                dr["Codigo"].ToString(),
                dr["CodigoIAT"].ToString(),
                Convert.ToInt32(dr["Retorna"]) == 1,
                (TIPO_CAUSA_SEM)Convert.ToInt32(dr["idCausa"]),
                dr["NS"].ToString(),
                dr["Motivo"].ToString(),
                Convert.ToSingle(dr["Cantidad"]),
                Convert.ToInt32(dr["NumeroJT"])
            );
        }
        catch
        {
            item = null;
        }

        return item;
    }
    /// <summary>
    /// Obtiene los ítems de una Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="idSEM"></param>
    /// <returns></returns>
    internal static List<ItemSolEnvMat> ObtenerItemsSEM(int idSEM)
    {
        List<ItemSolEnvMat> lstItems = new List<ItemSolEnvMat>();
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM tbl_SEMItems WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
            {
                throw new Exception("No hay prioridades.");
            }

            while (dr.Read())
            {
                lstItems.Add(ObtenerItemSEM(dr));
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

        return lstItems;
    }
    /// <summary>
    /// Inserta una nueva Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="destino"></param>
    /// <param name="telefono"></param>
    /// <param name="prioridad"></param>
    /// <param name="destinatarios"></param>
    /// <param name="destinatarioAdic"></param>
    /// <param name="retiraDe"></param>
    /// <param name="numero"></param>
    /// <returns></returns>
    public static bool NuevaSEM(string destino, string telefono, PrioridadSolEnvMat prioridad,
        List<Persona> destinatarios, Persona destinatarioAdic, string retiraDe,
        List<ItemSolEnvMat> items, out int numero)
    {
        List<int> lstDestinatarios = new List<int>();
        int destAdic;
        foreach (Persona p in destinatarios)
        {
            lstDestinatarios.Add(p.ID);
        }
        destAdic = destinatarioAdic != null ? destinatarioAdic.ID : Constantes.IdPersonaInvalido;

        return NuevaSEM(destino, telefono, prioridad.ID, lstDestinatarios, destAdic, retiraDe, 
            items, out numero);
    }
    /// <summary>
    /// Inserta una nueva Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="destino"></param>
    /// <param name="telefono"></param>
    /// <param name="prioridad"></param>
    /// <param name="destinatarios"></param>
    /// <param name="destinatarioAdic"></param>
    /// <param name="retiraDe"></param>
    /// <param name="items"></param>
    /// <param name="numero"></param>
    /// <returns></returns>
    public static bool NuevaSEM(string destino, string telefono, int idPrioridad,
        List<int> idDestinatarios, int idDestinatarioAdic, string retiraDe,
        List<ItemSolEnvMat> items, out int numero)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;
        numero = Constantes.ValorInvalido;

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
            result = InsertarSEM(conn, trans, destino, telefono, idPrioridad, idDestinatarios,
                idDestinatarioAdic, retiraDe, items, out numero);

            if (result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
        }
        catch
        {
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Inserta una nueva Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="destino"></param>
    /// <param name="telefono"></param>
    /// <param name="prioridad"></param>
    /// <param name="destinatarios"></param>
    /// <param name="destinatarioAdic"></param>
    /// <param name="retiraDe"></param>
    /// <param name="items"></param>
    /// <param name="numero"></param>
    /// <returns></returns>
    private static bool InsertarSEM(SqlConnection conn, SqlTransaction trans, string destino, 
        string telefono, int idPrioridad, List<int> idDestinatarios, 
        int idDestinatarioAdic, string retiraDe, List<ItemSolEnvMat> items, 
        out int numero)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.Transaction = trans;

        try
        {
            cmd.CommandText = "INSERT INTO tbl_SEM (destino, telefono, idPrioridad, idDestinatarioAdic, ";
            cmd.CommandText += "retiraDe) VALUES (@destino, @telefono, @idPrioridad, ";
            cmd.CommandText += "@idDestinatarioAdic, @retiraDe);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_SEM";
            cmd.Parameters.Add("@destino", SqlDbType.VarChar, 25).Value = destino;
            cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 16).Value = telefono;
            cmd.Parameters.Add("@idPrioridad", SqlDbType.TinyInt, 1).Value = idPrioridad;
            cmd.Parameters.Add("@idDestinatarioAdic", SqlDbType.Int, 4).Value = idDestinatarioAdic;
            cmd.Parameters.Add("@retiraDe", SqlDbType.VarChar, 25).Value = retiraDe;

            int idSEM;
            idSEM = Convert.ToInt32(cmd.ExecuteScalar());

            if (!InsertarDestinatariosSEM(conn, trans, idSEM, idDestinatarios))
            {
                throw new Exception("No se pudieron insertar los destinatarios.");
            }

            if (!InsertarItemsSEM(conn, trans, idSEM, items))
            {
                throw new Exception("No se pudieron insertar los ítems.");
            }

            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText += "SELECT Numero FROM tbl_SEM WHERE idSEM = @idSEM;";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;

            numero = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            numero = Constantes.ValorInvalido;
            return false;
        }

        return true;
    }
    /// <summary>
    /// Inserta los destinatarios de una SEM.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="trans"></param>
    /// <param name="idSEM"></param>
    /// <param name="destinatarios"></param>
    /// <returns></returns>
    private static bool InsertarDestinatariosSEM(SqlConnection conn, SqlTransaction trans, int idSEM,
        List<int> destinatarios)
    {
        try
        {
            foreach (int idPersonal in destinatarios)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandText = "INSERT INTO tbl_SEMDestinatarios (idSEM, idPersonal) VALUES";
                cmd.CommandText += "(@idSEM, @idPersonal)";
                cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
                cmd.Parameters.Add("@idPersonal", SqlDbType.Int, 4).Value = idPersonal;

                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Inserta los ítems de una SEM.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="trans"></param>
    /// <param name="idSEM"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    private static bool InsertarItemsSEM(SqlConnection conn, SqlTransaction trans, int idSEM,
        List<ItemSolEnvMat> items)
    {
        try
        {
            foreach (ItemSolEnvMat i in items)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandText = "INSERT INTO tbl_SEMItems (idSEM, Descripcion, idImputacion, ";
                cmd.CommandText += "OrigenDatos, NumOrigenDatos, Destino, Codigo, CodigoIAT, Retorna, ";
                cmd.CommandText += "idCausa, NS, Motivo, Cantidad, NumeroJT) VALUES (@idSEM, @Descripcion, ";
                cmd.CommandText += "@idImputacion, @OrigenDatos, @NumOrigenDatos, @Destino, @Codigo, ";
                cmd.CommandText += "@CodigoIAT, @Retorna, @idCausa, @NS, @Motivo, @Cantidad, @NumeroJT)";
                cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = i.Descripcion;
                cmd.Parameters.Add("@idImputacion", SqlDbType.Int, 4).Value = i.Imputacion.ID;
                cmd.Parameters.Add("@OrigenDatos", SqlDbType.TinyInt, 1).Value = (int)i.OrigenDatos;
                cmd.Parameters.Add("@NumOrigenDatos", SqlDbType.Int, 4).Value = i.NumeroOrigenDatos;
                cmd.Parameters.Add("@Destino", SqlDbType.VarChar, 25).Value = i.Destino;
                cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 15).Value = i.Codigo;
                cmd.Parameters.Add("@CodigoIAT", SqlDbType.VarChar, 13).Value = i.CodigoIAT;
                cmd.Parameters.Add("@Retorna", SqlDbType.Bit, 1).Value = i.Retorna ? 1 : 0;
                cmd.Parameters.Add("@idCausa", SqlDbType.TinyInt, 1).Value = (int)i.Causa;
                cmd.Parameters.Add("@NS", SqlDbType.VarChar, 50).Value = i.NS;
                cmd.Parameters.Add("@Motivo", SqlDbType.VarChar, 55).Value = i.Motivo;
                cmd.Parameters.Add("@Cantidad", SqlDbType.Real, 4).Value = i.Cantidad;
                cmd.Parameters.Add("@NumeroJT", SqlDbType.Int, 4).Value = i.NumeroJT;

                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Borra los ítems de una SEM.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="trans"></param>
    /// <param name="idSEM"></param>
    /// <returns></returns>
    private static bool BorrarItemsSEM(SqlConnection conn, SqlTransaction trans, int idSEM)
    {
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            cmd.Connection = conn;
            cmd.Transaction = trans;
            cmd.CommandText = "DELETE FROM tbl_SEMItems WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;

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
    /// Actualiza una SEM.
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="trans"></param>
    /// <param name="idSEM"></param>
    /// <param name="items"></param>
    /// <param name="cantBultos"></param>
    /// <param name="valorDeclarado"></param>
    /// <param name="transporte"></param>
    /// <param name="numTrack"></param>
    /// <returns></returns>
    private static bool ActualizarSEM(SqlConnection conn, SqlTransaction trans, int idSEM,
        List<ItemSolEnvMat> items, int cantBultos, float valorDeclarado, TIPO_TRANSP_SEM transporte, 
        int numTrack)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.Transaction = trans;

        try
        {
            cmd.CommandText = "UPDATE tbl_SEM SET idTipoTransporte = @idTipoTransporte, ";
            cmd.CommandText += "CantBultos = @CantBultos, ValorDeclarado = @ValorDeclarado, ";
            cmd.CommandText += "TrackID = @TrackID ";
            cmd.CommandText += "WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idTipoTransporte", SqlDbType.TinyInt, 1).Value = (int)transporte;
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
            cmd.Parameters.Add("@CantBultos", SqlDbType.SmallInt, 2).Value = cantBultos;
            cmd.Parameters.Add("@ValorDeclarado", SqlDbType.Real, 4).Value = valorDeclarado;
            cmd.Parameters.Add("@TrackID", SqlDbType.Int, 4).Value = numTrack;

            cmd.ExecuteNonQuery();

            if (!BorrarItemsSEM(conn, trans, idSEM))
            {
                throw new Exception("No se pudieron borrar los ítems.");
            }

            if (!InsertarItemsSEM(conn, trans, idSEM, items))
            {
                throw new Exception("No se pudieron insertar los ítems.");
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Actualiza una Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="idSEM"></param>
    /// <param name="items"></param>
    /// <param name="cantBultos"></param>
    /// <param name="valorDeclarado"></param>
    /// <param name="transporte"></param>
    /// <param name="numTrack"></param>
    /// <returns></returns>
    public static bool ActualizarSEM(int idSEM, List<ItemSolEnvMat> items, int cantBultos, 
        float valorDeclarado, TIPO_TRANSP_SEM transporte, int numTrack)
    {
        SqlConnection conn = new SqlConnection();
        SqlTransaction trans;

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
            result = ActualizarSEM(conn, trans, idSEM, items, cantBultos, valorDeclarado, transporte, 
                numTrack);

            if (result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
        }
        catch
        {
            result = false;
        }

        conn.Close();

        return result;
    }
    /// <summary>
    /// Confirma el estado actual de la SEM.
    /// </summary>
    /// <param name="sem"></param>
    /// <returns></returns>
    public static bool ConfirmarEstadoSEM(SolEnvMat sem)
    {
        ESTADO_SEM estado = sem.Estado;

        if (estado == ESTADO_SEM.Cerrada || estado == ESTADO_SEM.Rechazada)
        {
            return false;
        }

        return CambiarEstadoSEM(sem.ID, ++estado);
    }
    /// <summary>
    /// Rechaza el estado actual de la SEM.
    /// </summary>
    /// <param name="sem"></param>
    /// <returns></returns>
    public static bool RechazarEstadoSEM(SolEnvMat sem)
    {
        ESTADO_SEM estado = sem.Estado;

        if (estado == ESTADO_SEM.Cerrada || estado == ESTADO_SEM.Rechazada)
        {
            return false;
        }

        return CambiarEstadoSEM(sem.ID, ESTADO_SEM.Rechazada);
    }
    /// <summary>
    /// Cambia el estado actual de la SEM.
    /// </summary>
    /// <param name="idSEM"></param>
    /// <param name="nuevoEstado"></param>
    /// <returns></returns>
    private static bool CambiarEstadoSEM(int idSEM, ESTADO_SEM nuevoEstado)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        bool result;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE tbl_SEM SET idEstado = @idEstado WHERE idSEM = @idSEM";
            cmd.Parameters.Add("@idSEM", SqlDbType.Int, 4).Value = idSEM;
            cmd.Parameters.Add("@idEstado", SqlDbType.TinyInt, 1).Value = (int)nuevoEstado;

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
    /// Envía una Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="sem"></param>
    /// <param name="de"></param>
    /// <param name="para"></param>
    /// <param name="cc"></param>
    /// <param name="asunto"></param>
    /// <returns></returns>
    public static bool EnviarSEM(SolEnvMat sem, string de, string para, string cc, string asunto)
    {
        bool resultado = true;

        //Cargo la Plantilla.
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_SEM_EMAIL);
        string plantilla_item = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_SEM_EMAIL_ITEM);
        if (plantilla == null || plantilla_item == null)
        {
            return false;
        }

        //Reemplazo las variables.
        plantilla = ReemplazarVariables(sem, plantilla, plantilla_item);

        //Envío el E-mail.
        Email email = new Email(de, para, cc, asunto, plantilla);
        resultado = email.Enviar();

        return resultado;
    }
    /// <summary>
    /// Reemplaza las variables de la Plantilla.
    /// </summary>
    /// <param name="sem"></param>
    /// <param name="cuerpoPlantilla"></param>
    /// <param name="cuerpoPlantillaItem"></param>
    /// <returns></returns>
    private static string ReemplazarVariables(SolEnvMat sem, string cuerpoPlantilla, 
        string cuerpoPlantillaItem)
    {
        string info = "";
        switch (sem.Estado)
        {
            case ESTADO_SEM.EsperandoConfirmacion:
                info = "Carlos,<br>Necesitaría me autorice el siguiente envio.<br><br>Saludos<br>Jorge";
                break;
        }

        cuerpoPlantilla = cuerpoPlantilla.Replace("@INFO", info);
        cuerpoPlantilla = cuerpoPlantilla.Replace("@NUMERO", "0001" + sem.Numero.ToString("00000000"));
        cuerpoPlantilla = cuerpoPlantilla.Replace("@FECHA", sem.Fecha.ToShortDateString());
        cuerpoPlantilla = cuerpoPlantilla.Replace("@DESTINO", sem.Destino);
        cuerpoPlantilla = cuerpoPlantilla.Replace("@PRIORIDAD", sem.Prioridad.Descripcion);
        cuerpoPlantilla = cuerpoPlantilla.Replace("@DESTINATARIOS", 
            GPersonal.PersonalToString(sem.Destinatarios, '/') + 
            ( sem.DestinatarioAdicional != null ? '/' + sem.DestinatarioAdicional.Nombre : "" ));
        cuerpoPlantilla = cuerpoPlantilla.Replace("@TELEFONO", sem.Telefono);
        cuerpoPlantilla = cuerpoPlantilla.Replace("@RETIRADE", sem.RetiraDe);

        // Ítems.
        string items = "";
        foreach (ItemSolEnvMat item in sem.Items)
        {
            items += cuerpoPlantillaItem;
            items = items.Replace("@CANT", item.Cantidad.ToString());
            items = items.Replace("@DESCRIPCION", item.Descripcion);
            items = items.Replace("@IMPUTACION", item.Imputacion.Descripcion);
        }

        cuerpoPlantilla = cuerpoPlantilla.Replace("@ITEMS", items);
        cuerpoPlantilla = cuerpoPlantilla.Replace("@BULTOS", sem.CantidadBultos.ToString());
        cuerpoPlantilla = cuerpoPlantilla.Replace("@VALOR_DECLARADO", 
            sem.ValorDeclarado.ToString("$#0.00"));

        return cuerpoPlantilla;
    }
    /// <summary>
    /// Obtiene una cadena que representa el Tipo de Causa.
    /// </summary>
    /// <param name="tipoCausa"></param>
    /// <returns></returns>
    public static string CausaToString(TIPO_CAUSA_SEM tipoCausa)
    {
        switch (tipoCausa)
        { 
            case TIPO_CAUSA_SEM.Falla:
                return "Falla";
            case TIPO_CAUSA_SEM.Otros:
                return "Otros";
            case TIPO_CAUSA_SEM.Reemplazo:
                return "Reemplazo";
        }

        return "N / A";
    }
    /// <summary>
    /// Obtiene una lista de Solicitudes de Envío de Materiales.
    /// </summary>
    /// <param name="pagina"></param>
    /// <param name="tipoFiltro"></param>
    /// <param name="valorFiltro"></param>
    /// <param name="tamPagina"></param>
    /// <returns></returns>
    public static List<SolEnvMat> ObtenerSEMs(int pagina, TIPO_FILTRO_SEM tipoFiltro, string valorFiltro, 
        int tamPagina)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adap = new SqlDataAdapter();
        DataSet ds = new DataSet();
        List<SolEnvMat> lstSEM = new List<SolEnvMat>();

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = ObtenerConsultaString(tipoFiltro, valorFiltro, false);
            adap.SelectCommand = cmd;
            adap.Fill(ds, pagina * tamPagina, tamPagina, "SolEnvMat");

            if (ds.Tables["SolEnvMat"].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables["SolEnvMat"].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables["SolEnvMat"].Rows[i];
                    SolEnvMat nnc = new SolEnvMat(
                        Convert.ToInt32(dr["idSEM"]),
                        Convert.ToInt32(dr["Numero"]),
                        DateTime.Parse(dr["Fecha"].ToString()),
                        dr["Destino"].ToString(),
                        "", null, null,null, "", TIPO_TRANSP_SEM.Interno,
                        Constantes.ValorInvalido,
                        Constantes.ValorInvalido, 
                        (ESTADO_SEM)Convert.ToInt32(dr["idEstado"])
                    );

                    lstSEM.Add(nnc);
                }
            }
        }
        catch
        {

        }
        finally
        {
            conn.Close();
        }

        return lstSEM;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas de Solicitud de Envio de Materiales.
    /// </summary>
    /// <returns></returns>
    public static int CantidadPaginasSEMs(TIPO_FILTRO_SEM tipoFiltro, string valorFiltro,
        int tamanoPagina)
    {
        return Funciones.CantidadPaginas(ObtenerConsultaString(tipoFiltro, valorFiltro, true), tamanoPagina);
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string ObtenerConsultaString(TIPO_FILTRO_SEM tipoFiltro, string valorFiltro, bool cantidad)
    {
        string filtro;
        string consulta;

        switch (tipoFiltro)
        {
            case TIPO_FILTRO_SEM.Todas:
                filtro = "";
                break;
            default:
                filtro = "";
                break;
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idSEM) as TotalRegistros FROM tbl_SEM " + filtro;
        }
        else
        {
            consulta = "SELECT idSEM, Numero, Fecha, Destino, idEstado FROM tbl_SEM " + filtro + 
                "ORDER BY Numero DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene la representación para el estado.
    /// </summary>
    /// <param name="estado"></param>
    /// <returns></returns>
    public static string EstadoToString(ESTADO_SEM estado)
    {
        string s;

        switch (estado)
        {
            case ESTADO_SEM.EsperandoConfirmacion:
                s = "Esperando confirmación";
                break;
            case ESTADO_SEM.Confirmada:
                s = "Confirmada";
                break;
            case ESTADO_SEM.Cerrada:
                s = "Cerrada";
                break;
            case ESTADO_SEM.Rechazada:
                s = "Rechazada";
                break;
            default:
                s = "<No disponible>";
                break;
        }

        return s;
    }
    /// <summary>
    /// Obtiene una cadena de personas separadas por '/'.
    /// </summary>
    /// <param name="lstDestinatarios"></param>
    /// <returns></returns>
    public static string DestinatariosToString(List<Persona> lstDestinatarios)
    {
        string s = "";

        foreach (Persona p in lstDestinatarios)
        {
            s += p.Nombre + "/";
        }
        s = s.TrimEnd('/');

        return s;
    }
    /// <summary>
    /// Obtiene el Remino No Oficial para la Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="idSEM"></param>
    /// <returns></returns>
    public static SolEnvMatRemitoNoOficial ObtenerRemitoNoOficial(int idSEM)
    {
        return ObtenerRemitoNoOficial(ObtenerSEM(idSEM));
    }
    /// <summary>
    /// Obtiene el Remino No Oficial para la Solicitud de Envío de Materiales.
    /// </summary>
    /// <param name="sem"></param>
    /// <returns></returns>
    public static SolEnvMatRemitoNoOficial ObtenerRemitoNoOficial(SolEnvMat sem)
    {
        SolEnvMatRemitoNoOficial remito = null;

        // Cargo los ítems.
        if (sem != null)
        {
            sem.CargarItems();
        }

        remito = new SolEnvMatRemitoNoOficial(sem);

        return remito;
    }
}
