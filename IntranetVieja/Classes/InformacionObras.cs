/*
 * Historial:
 * ===================================================================================
 * [17/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class RevisionObra
{
    // Variables.
    private DateTime fecha;
    private int revision;

    // Propiedades.
    public DateTime Fecha
    {
        get { return this.fecha; }
    }
    public int Revision
    {
        get { return this.revision; }
    }


    internal RevisionObra(DateTime fecha, int revision)
    {
        this.fecha = fecha;
        this.revision = revision;
    }
}

public class AlertaInformacionObra
{
    // Variables.
    private int idObra;
    private DateTime fechaInicio;
    private string emailPara;

    // Propiedades.
    public int IdObra
    {
        get { return this.idObra; }
    }
    public DateTime FechaInicio
    {
        get { return this.fechaInicio; }
    }
    public string EmailPara
    {
        get { return this.emailPara; }
    }


    internal AlertaInformacionObra(int idObra, DateTime fechaInicio, string emailPara)
    {
        this.idObra = idObra;
        this.fechaInicio = fechaInicio;
        this.emailPara = emailPara;
    }
}

public class InformacionObra
{
    // Variables.
    private int idObra;
    private DateTime fecha;
    private int idInforma;
    private Persona informa;
    private InformacionObraHist datos;
    private List<RevisionObra> revisiones;

    // Propiedades.
    public int IdObra
    {
        get { return this.idObra; }
    }
    public DateTime Fecha
    {
        get { return this.fecha; }
    }
    public int IdInforma
    {
        get { return this.idInforma; }
    }
    public Persona Informa
    {
        get
        {
            if (this.informa == null)
            {
                this.informa = GPersonal.GetPersona(this.idInforma);
            }

            return this.informa;
        }
    }
    public InformacionObraHist Datos
    {
        get { return this.datos; }
    }
    public List<RevisionObra> Revisiones
    {
        get 
        {
            if (this.revisiones == null)
            {
                this.revisiones = InformacionObras.GetRevisionesObra(this.idObra);
            }

            return this.revisiones;
        }
    }
    public bool EsUltima
    {
        get
        {
            return this.Datos.Revision == this.Revisiones[0].Revision;
        }
    }


    internal InformacionObra(int idObra, DateTime fecha, int idInforma, InformacionObraHist datos)
    {
        this.idObra = idObra;
        this.fecha = fecha;
        this.idInforma = idInforma;
        this.datos = datos;
    }
}

public class InformacionObraHist
{
    // Variables.
    private int revision;
    private int idResponsableObra;
    private Persona responsableObra;
    private TiposTrabajoObra tipoTrabajo;
    private string imputacion;
    private string cliente;
    private string ordenCompra;
    private DateTime fechaEntrega;
    private bool subcontratistas;
    private string subcontratEmpresa;
    private bool predioTerceros;
    private string predioTercEmpresa;
    private string ubicacion;
    private string provincia;
    private string respTecCliente;
    private string respTecClienteTel;
    private string respTecClienteEmail;
    private string respSegCliente;
    private string respSegClienteTel;
    private string respSegClienteEmail;
    private string contAdminCliente;
    private string contAdminClienteTel;
    private string contAdminClienteEmail;
    private DateTime fechaInicio;
    private string duracion;
    private string descripcionTareas;
    private DateTime fechaFinalizacion;
    private List<Persona> personasMant;
    private List<Persona> personasObra;
    private List<Vehiculo> vehiculos;
    private string objetivoProyecto;
    private int idGerente;
    private Persona gerente;
    private DateTime fechaModif;
    private Autorizacion autoriz;

    // Propiedades.
    public int Revision
    {
        get { return this.revision; }
    }
    public int IdResponsableObra
    {
        get { return this.idResponsableObra; }
    }
    public Persona ResponsableObra
    {
        get
        {
            if (this.responsableObra == null)
            {
                this.responsableObra = GPersonal.GetPersona(this.idResponsableObra);
            }

            return this.responsableObra;
        }
    }
    public TiposTrabajoObra TipoTrabajo
    {
        get { return this.tipoTrabajo; }
    }
    public string Imputacion
    {
        get { return this.imputacion; }
    }
    public string Cliente
    {
        get { return this.cliente; }
    }
    public string OrdenCompra
    {
        get { return this.ordenCompra; }
    }
    public DateTime FechaEntrega
    {
        get { return this.fechaEntrega; }
    }
    public bool Subcontratistas
    {
        get { return this.subcontratistas; }
    }
    public string SubcontratEmpresa
    {
        get { return this.subcontratEmpresa; }
    }
    public bool PredioTerceros
    {
        get { return this.predioTerceros; }
    }
    public string PredioTercEmpresa
    {
        get { return this.predioTercEmpresa; }
    }
    public string Ubicacion
    {
        get { return this.ubicacion; }
    }
    public string Provincia
    {
        get { return this.provincia; }
    }
    public string RespTecCliente
    {
        get { return this.respTecCliente; }
    }
    public string RespTecClienteTel
    {
        get { return this.respTecClienteTel; }
    }
    public string RespTecClienteEmail
    {
        get { return this.respTecClienteEmail; }
    }
    public string RespSegCliente
    {
        get { return this.respSegCliente; }
    }
    public string RespSegClienteTel
    {
        get { return this.respSegClienteTel; }
    }
    public string RespSegClienteEmail
    {
        get { return this.respSegClienteEmail; }
    }
    public string ContacAdminCliente
    {
        get { return this.contAdminCliente; }
    }
    public string ContacAdminClienteTel
    {
        get { return this.contAdminClienteTel; }
    }
    public string ContacAdminClienteEmail
    {
        get { return this.contAdminClienteEmail; }
    }
    public DateTime FechaInicio
    {
        get { return this.fechaInicio; }
    }
    public string Duracion
    {
        get { return this.duracion; }
    }
    public string DescripcionTareas
    {
        get { return this.descripcionTareas; }
    }
    public DateTime FechaFinalizacion
    {
        get { return this.fechaFinalizacion; }
    }
    public List<Persona> PersonasMantenimiento
    {
        get { return this.personasMant; }
    }
    public List<Persona> PersonasObra
    {
        get { return this.personasObra; }
    }
    public List<Vehiculo> Vehiculos
    {
        get { return this.vehiculos; }
    }
    public string ObjetivoProyecto
    {
        get { return this.objetivoProyecto; }
    }
    public int IdGerente
    {
        get { return this.idGerente; }
    }
    public Persona Gerente
    {
        get
        {
            if (this.gerente == null)
            {
                this.gerente = GPersonal.GetPersona(this.idGerente);
            }

            return this.gerente;
        }
    }
    public DateTime FechaModificacion
    {
        get { return this.fechaModif; }
    }
    public Autorizacion Autorizacion
    {
        get { return this.autoriz; }
    }
    public bool AutorizacionPendiente
    {
        get { return this.autoriz != null && this.autoriz.Estado == EstadoAutorizacion.Pendiente; }
    }


    internal InformacionObraHist(int revision, int idResponsableObra,
        TiposTrabajoObra tipoTrabajo, string imputacion, string cliente, string ordenCompra, DateTime fechaEntrega,
        bool subcontratistas, string subcontratEmpresa, bool predioTerceros, string predioTercEmpresa, string ubicacion, 
        string provincia, string respTecCliente, string respTecClienteTel, string respTecClienteEmail, string respSegCliente, 
        string respSegClienteTel, string respSegClienteEmail, string contAdminCliente, string contAdminClienteTel, 
        string contAdminClienteEmail, DateTime fechaInicio, string duracion, string descripcionTareas, DateTime fechaFinalizacion, 
        List<Persona> personasMant, List<Persona> personasObra, List<Vehiculo> vehiculos, string objetivoProyecto, int idGerente, 
        DateTime fechaModif, int idAutorizacion)
    {
        this.revision = revision;
        this.idResponsableObra = idResponsableObra;
        this.tipoTrabajo = tipoTrabajo;
        this.imputacion = imputacion;
        this.cliente = cliente;
        this.ordenCompra = ordenCompra;
        this.fechaEntrega = fechaEntrega;
        this.subcontratistas = subcontratistas;
        this.subcontratEmpresa = subcontratEmpresa;
        this.predioTerceros = predioTerceros;
        this.predioTercEmpresa = predioTercEmpresa;
        this.ubicacion = ubicacion;
        this.provincia = provincia;
        this.respTecCliente = respTecCliente;
        this.respTecClienteTel = respTecClienteTel;
        this.respTecClienteEmail = respTecClienteEmail;
        this.respSegCliente = respSegCliente;
        this.respSegClienteTel = respSegClienteTel;
        this.respSegClienteEmail = respSegClienteEmail;
        this.contAdminCliente = contAdminCliente;
        this.contAdminClienteTel = contAdminClienteTel;
        this.contAdminClienteEmail = contAdminClienteEmail;
        this.fechaInicio = fechaInicio;
        this.duracion = duracion;
        this.descripcionTareas = descripcionTareas;
        this.fechaFinalizacion = fechaFinalizacion;
        this.personasMant = personasMant;
        this.personasObra = personasObra;
        this.vehiculos = vehiculos;
        this.objetivoProyecto = objetivoProyecto;
        this.idGerente = idGerente;
        this.fechaModif = fechaModif;
        this.autoriz = idAutorizacion != Constantes.ValorInvalido ? Autorizaciones.GetAutorizacion(idAutorizacion) : null;
    }
}
/// <summary>
/// Summary description for InformacionObras
/// </summary>
public static class InformacionObras
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;
    private const string TablaPersMant = "tbl_InformacionObrasPersMant";
    private const string TablaPersObra = "tbl_InformacionObrasPersObra";
    public const int MinDiasAnticipo = 15;


    /// <summary>
    /// Obtiene la información de una obra.
    /// </summary>
    private static InformacionObra GetInformacionObra(IDataReader dr)
    {
        InformacionObra result;

        try
        {
            int idObra = Convert.ToInt32(dr["idObra"]);
            int idObraHist = Convert.ToInt32(dr["idObraHistorico"]);

            InformacionObraHist datos = new InformacionObraHist(
                    Convert.ToInt32(dr["Revision"]),
                    Convert.ToInt32(dr["idResponsableObra"]),
                    (TiposTrabajoObra)Convert.ToInt32(dr["TipoTrabajo"]),
                    dr["Imputacion"].ToString(),
                    dr["Cliente"].ToString(),
                    dr["OrdenCompra"].ToString(),
                    Convert.ToDateTime(dr["FechaEntrega"]),
                    Convert.ToBoolean(dr["Subcontratistas"]),
                    dr["SubcontratEmpresa"].ToString(),
                    Convert.ToBoolean(dr["PredioTerceros"]),
                    dr["PredioTercEmpresa"].ToString(),
                    dr["Ubicacion"].ToString(),
                    dr["Provincia"].ToString(),
                    dr["RespTecCliente"].ToString(),
                    dr["RespTecClienteTel"].ToString(),
                    dr["RespTecClienteEmail"].ToString(),
                    dr["RespSegCliente"].ToString(),
                    dr["RespSegClienteTel"].ToString(),
                    dr["RespSegClienteEmail"].ToString(),
                    dr["ContAdminCliente"].ToString(),
                    dr["ContAdminClienteTel"].ToString(),
                    dr["ContAdminClienteEmail"].ToString(),
                    Convert.ToDateTime(dr["FechaInicio"]),
                    dr["Duracion"].ToString(),
                    dr["DescripcionTareas"].ToString(),
                    Convert.ToDateTime(dr["FechaFinalizacion"]),
                    GPersonal.GetPersonasInfObra(idObraHist, TablaPersMant),
                    GPersonal.GetPersonasInfObra(idObraHist, TablaPersObra),
                    Vehiculos.GetVehiculosInfObra(idObraHist),
                    dr["ObjetivoProyecto"].ToString(),
                    Convert.ToInt32(dr["idGerente"]),
                    Convert.ToDateTime(dr["FechaModif"]),
                    Convert.ToInt32(dr["idAutorizacion"])
                );

            result = new InformacionObra(
                    idObra,
                    Convert.ToDateTime(dr["Fecha"]),
                    Convert.ToInt32(dr["idInforma"]),
                    datos
                );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una fila para el panel de control.
    /// </summary>
    private static object[] GetFilaInfomacionObra(DataRow dr)
    {
        object[] result;

        try
        {
            result = new object[] { dr["idObra"], dr["Cliente"], dr["ResponsableObra"], dr["Informa"], dr["OrdenCompra"],
                dr["Imputacion"] };
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene la información de una obra.
    /// </summary>
    /// <param name="revision">Si revisión == Constantes.ValorInvalido, entonces se trae la última.</param>
    public static InformacionObra GetInformacionObra(int idObra, int revision)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        InformacionObra result = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT io.Fecha, io.idInforma, hi.*, ISNULL(au.idAutorizacion, " + Constantes.ValorInvalido + ") AS ";
            cmd.CommandText += "idAutorizacion FROM tbl_InformacionObras io ";
            cmd.CommandText += "INNER JOIN tbl_InformacionObrasHistoricos hi ON io.idObra = hi.idObra ";
            if (revision == Constantes.ValorInvalido)
            {
                cmd.CommandText += "AND hi.Revision = (SELECT TOP 1 Revision FROM tbl_InformacionObrasHistoricos WHERE ";
                cmd.CommandText += "idObra = @idObra ORDER BY Revision DESC) ";
            }
            else
            {
                cmd.CommandText += "AND hi.Revision = @Revision ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Revision", revision));
            }
            cmd.CommandText += "LEFT JOIN tbl_AutorizacionesIIO au ON au.idObra = @idObra ";
            cmd.CommandText += "WHERE io.idObra = @idObra";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObra", idObra));
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                result = GetInformacionObra(dr);
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
    /// Da de alta una nueva información para una obra.
    /// </summary>
    public static void NuevaInformacionObra(int idResponsableObra, TiposTrabajoObra tipoTrabajo, string imputacion,
        string cliente, string ordenCompra, DateTime fechaEntrega, bool subcontratistas, string subcontratEmpresa,
        bool predioTerceros, string predioTercEmpresa, string ubicacion, string provincia, string respTecCliente, 
        string respTecClienteTel, string respTecClienteEmail, string respSegCliente, string respSegClienteTel, 
        string respSegClienteEmail, string respSegTerceros, string respSegTercerosTel, string respSegTercerosEmail, 
        DateTime fechaInicio, string duracion, string descripcionTareas, DateTime fechaFinalizacion, List<int> personasMant,
        List<int> personasObra, List<int> vehiculos, string objetivoProyecto, int idGerente)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;
        int idIO;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);
            cmd = DataAccess.GetCommand(trans);

            cmd.CommandText = "INSERT INTO tbl_InformacionObras (Fecha, idInforma) VALUES (@Fecha, @idInforma); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_InformacionObras;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(DateTime.Now)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idInforma", Constantes.Usuario.ID));

            idIO = Convert.ToInt32(cmd.ExecuteScalar());

            AgregarHistoricoObra(idIO, idResponsableObra, tipoTrabajo, imputacion, cliente, ordenCompra, fechaEntrega,
                subcontratistas, subcontratEmpresa, predioTerceros, predioTercEmpresa, ubicacion, provincia, respTecCliente,
                respTecClienteTel, respTecClienteEmail, respSegCliente, respSegClienteTel, respSegClienteEmail,
                respSegTerceros, respSegTercerosTel, respSegTercerosEmail, fechaInicio, duracion, descripcionTareas,
                fechaFinalizacion, personasMant, personasObra, vehiculos, objetivoProyecto, idGerente, trans);

            trans.Commit();
        }
        catch(Exception ex)
        {
            if (trans != null)
            {
                trans.Rollback();
            }
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) { conn.Close(); }
        }

        EnviarInformacionObra(idIO, false);
    }
    /// <summary>
    /// Agrega un nuevo registro histórico para la obra.
    /// </summary>
    private static void AgregarHistoricoObra(int idObra, int idResponsableObra, TiposTrabajoObra tipoTrabajo, string imputacion,
        string cliente, string ordenCompra, DateTime fechaEntrega, bool subcontratistas, string subcontratEmpresa,
        bool predioTerceros, string predioTercEmpresa, string ubicacion, string provincia, string respTecCliente,
        string respTecClienteTel, string respTecClienteEmail, string respSegCliente, string respSegClienteTel,
        string respSegClienteEmail, string respSegTerceros, string respSegTercerosTel, string respSegTercerosEmail,
        DateTime fechaInicio, string duracion, string descripcionTareas, DateTime fechaFinalizacion, List<int> personasMant,
        List<int> personasObra, List<int> vehiculos, string objetivoProyecto, int idGerente, IDbTransaction trans)
    {
        IDbCommand cmd;

        cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "INSERT INTO tbl_InformacionObrasHistoricos (idObra, Revision, idResponsableObra, TipoTrabajo, ";
        cmd.CommandText += "Imputacion, Cliente, OrdenCompra, FechaEntrega, Subcontratistas, SubcontratEmpresa, PredioTerceros, ";
        cmd.CommandText += "PredioTercEmpresa, Ubicacion, Provincia, RespTecCliente, RespTecClienteTel, RespTecClienteEmail, ";
        cmd.CommandText += "RespSegCliente, RespSegClienteTel, RespSegClienteEmail, ContAdminCliente, ContAdminClienteTel, ";
        cmd.CommandText += "ContAdminClienteEmail, FechaInicio, Duracion, DescripcionTareas, FechaFinalizacion, ObjetivoProyecto, ";
        cmd.CommandText += "idGerente, FechaModif) VALUES (@idObra, @Revision, ";
        cmd.CommandText += "@idResponsableObra, @TipoTrabajo, @Imputacion, @Cliente, @OrdenCompra, @FechaEntrega, ";
        cmd.CommandText += "@Subcontratistas, @SubcontratEmpresa, @PredioTerceros, ";
        cmd.CommandText += "@PredioTercEmpresa, @Ubicacion, @Provincia, @RespTecCliente, @RespTecClienteTel, ";
        cmd.CommandText += "@RespTecClienteEmail, @RespSegCliente, ";
        cmd.CommandText += "@RespSegClienteTel, @RespSegClienteEmail, @ContAdminCliente, @ContAdminClienteTel, ";
        cmd.CommandText += "@ContAdminClienteEmail, @FechaInicio, @Duracion, @DescripcionTareas, @FechaFinalizacion, ";
        cmd.CommandText += "@ObjetivoProyecto, @idGerente, @FechaModif);";
        cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_InformacionObrasHistoricos;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idObra", idObra));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Revision", GetUltimaRevisionObra(idObra, trans) + 1));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@IdResponsableObra", idResponsableObra));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoTrabajo", tipoTrabajo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Imputacion", imputacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Cliente", cliente));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@OrdenCompra", ordenCompra));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaEntrega", fechaEntrega));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Subcontratistas", subcontratistas));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@SubcontratEmpresa", subcontratEmpresa));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PredioTerceros", predioTerceros));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PredioTercEmpresa", predioTercEmpresa));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Ubicacion", ubicacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Provincia", provincia));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespTecCliente", respTecCliente));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespTecClienteTel", respTecClienteTel));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespTecClienteEmail", respTecClienteEmail));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespSegCliente", respSegCliente));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespSegClienteTel", respSegClienteTel));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@RespSegClienteEmail", respSegClienteEmail));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@ContAdminCliente", respSegTerceros));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@ContAdminClienteTel", respSegTercerosTel));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@ContAdminClienteEmail", respSegTercerosEmail));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaInicio", fechaInicio));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Duracion", duracion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@DescripcionTareas", descripcionTareas));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaFinalizacion", fechaFinalizacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@ObjetivoProyecto", objetivoProyecto));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idGerente", idGerente));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaModif", DateTime.Now));

        int idHistorico = Convert.ToInt32(cmd.ExecuteScalar());

        AgregarPersonasMant(idHistorico, personasMant, trans);
        AgregarPersonasObra(idHistorico, personasObra, trans);
        AgregarVehiculos(idHistorico, vehiculos, trans);
    }
    /// <summary>
    /// Obtiene el último número de revisión de la obra.
    /// </summary>
    private static int GetUltimaRevisionObra(int idObra, IDbTransaction trans)
    {
        int result;

        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "SELECT TOP 1 Revision FROM tbl_InformacionObrasHistoricos WHERE idObra = @idObra ";
        cmd.CommandText += "ORDER BY idObraHistorico DESC";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idObra", idObra));

        result = Convert.ToInt32(cmd.ExecuteScalar());

        return result;
    }
    /// <summary>
    /// Agrega un registro histórico a una obra.
    /// </summary>
    public static void AgregarHistoricoObra(int idObra, int idResponsableObra, TiposTrabajoObra tipoTrabajo, string imputacion,
        string cliente, string ordenCompra, DateTime fechaEntrega, bool subcontratistas, string subcontratEmpresa,
        bool predioTerceros, string predioTercEmpresa, string ubicacion, string provincia, string respTecCliente,
        string respTecClienteTel, string respTecClienteEmail, string respSegCliente, string respSegClienteTel,
        string respSegClienteEmail, string respSegTerceros, string respSegTercerosTel, string respSegTercerosEmail,
        DateTime fechaInicio, string duracion, string descripcionTareas, DateTime fechaFinalizacion, List<int> personasMant,
        List<int> personasObra, List<int> vehiculos, string objetivoProyecto, int idGerente)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            AgregarHistoricoObra(idObra, idResponsableObra, tipoTrabajo, imputacion, cliente, ordenCompra, fechaEntrega,
                subcontratistas, subcontratEmpresa, predioTerceros, predioTercEmpresa, ubicacion, provincia, respTecCliente,
                respTecClienteTel, respTecClienteEmail, respSegCliente, respSegClienteTel, respSegClienteEmail,
                respSegTerceros, respSegTercerosTel, respSegTercerosEmail, fechaInicio, duracion, descripcionTareas,
                fechaFinalizacion, personasMant, personasObra, vehiculos, objetivoProyecto, idGerente, trans);

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
            if (conn != null) { conn.Close(); }
        }

        EnviarInformacionObra(idObra, true);
    }
    /// <summary>
    /// Agregar las personas de una obra.
    /// </summary>
    private static void AgregarPersonasObra(int idObraHistorico, List<int> personas, IDbTransaction trans)
    {
        AgregarPersonas(idObraHistorico, personas, TablaPersObra, trans);
    }
    /// <summary>
    /// Agregar las personas de una obra.
    /// </summary>
    private static void AgregarPersonasMant(int idObraHistorico, List<int> personas, IDbTransaction trans)
    {
        AgregarPersonas(idObraHistorico, personas, TablaPersMant, trans);
    }
    /// <summary>
    /// Agrega las personas de una obra.
    /// </summary>
    private static void AgregarPersonas(int idObraHistorico, List<int> personas, string tabla, IDbTransaction trans)
    {
        IDbCommand cmd;

        // Inserto los ítems.
        foreach (int idPersona in personas)
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO " + tabla + " (idObraHistorico, idPersona) VALUES (@idObraHistorico, @idPersona)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObraHistorico", idObraHistorico));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", idPersona));
            cmd.ExecuteNonQuery();
        }
    }
    /// <summary>
    /// Agrega los vehiculos de una obra.
    /// </summary>
    private static void AgregarVehiculos(int idObraHistorico, List<int> vehiculos, IDbTransaction trans)
    {
        IDbCommand cmd;

        // Inserto los ítems.
        foreach (int idVehiculo in vehiculos)
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_InformacionObrasVehiculos (idObraHistorico, idVehiculo) VALUES (@idObraHistorico, ";
            cmd.CommandText += "@idVehiculo)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObraHistorico", idObraHistorico));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idVehiculo", idVehiculo));
            cmd.ExecuteNonQuery();
        }
    }
    /// <summary>
    /// Obtiene las informaciones de obras que coincidan con los filtros.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<object[]> GetInformacionesObras(int pagina, List<Filtro> filtros)
    {
        List<object[]> result;

        result = DataAccess.GetDataList<object[]>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetFilaInfomacionObra);

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
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosInformeObra.NumObra:
                    filtroWhere += "AND io.idObra = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosInformeObra.Cliente:
                    filtroWhere += "AND Cliente LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosInformeObra.Imputacion:
                    filtroWhere += "AND Imputacion LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosInformeObra.Responsable:
                    filtroWhere += "AND idResponsableObra = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosInformeObra.Informante:
                    filtroWhere += "AND idInforma = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosInformeObra.OrdenCompra:
                    filtroWhere += "AND OrdenCompra LIKE '%" + filtro.Valor + "%' ";
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
            consulta = "SELECT COUNT(io.idObra) as TotalRegistros ";
        }
        else
        {
            consulta = "SELECT io.idObra, Cliente, p1.Nombre AS ResponsableObra, p2.Nombre AS Informa, OrdenCompra, Imputacion ";
        }

        if (filtroWhere.Length > 0)
        {
            filtroWhere = "WHERE " + filtroWhere;
        }
        consulta += "FROM tbl_InformacionObras io ";
        consulta += "INNER JOIN tbl_InformacionObrasHistoricos hi ON io.idObra = hi.idObra ";
        consulta += "AND hi.Revision = (SELECT TOP 1 hi1.Revision FROM tbl_InformacionObrasHistoricos hi1 WHERE ";
        consulta += "hi1.idObra = io.idObra ORDER BY hi1.Revision DESC) ";
        consulta += "LEFT JOIN tbl_Personal p1 ON hi.idResponsableObra = p1.idPersonal LEFT JOIN " + 
            "tbl_Personal p2 ON io.idInforma = p2.idPersonal " + filtroWhere;

        if (!cantidad)
        {
            consulta += " ORDER BY io.idObra DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene los tipos de trabajo.
    /// </summary>
    public static Dictionary<string, int> GetTiposTrabajo()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add("Mantenimiento", (int)TiposTrabajoObra.Mantenimiento);
        result.Add("Obra", (int)TiposTrabajoObra.Obra);
        result.Add("Auditoría", (int)TiposTrabajoObra.Auditoria);
		result.Add("Servicio", (int)TiposTrabajoObra.Servicio);

        return result;
    }
    /// <summary>
    /// Envía una información de obra.
    /// </summary>
    private static void EnviarInformacionObra(int idIO, bool actualizacion)
    {
        // Obtengo la plantilla.
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_IIO_EMAIL);
        if (plantilla == null)
        {
            throw new EmailException();
        }

        // Cargo la información de obra.
        InformacionObra io = GetInformacionObra(idIO, Constantes.ValorInvalido);
        if (io == null)
        {
            throw new EmailException();
        }

        // Reemplazo las variables.
        plantilla = plantilla.Replace("@ENCABEZADO", "Se ha " + (actualizacion ? "actualizado una" : "generado una nueva") + 
            " Información Interna de Obra:");
        plantilla = plantilla.Replace("@TIPO_MENSAJE", "info");
        plantilla = plantilla.Replace("@NUMERO", idIO.ToString());
        plantilla = plantilla.Replace("@INFORMA", io.Informa.Nombre);
        plantilla = plantilla.Replace("@IMPUTACION", io.Datos.Imputacion);
        plantilla = plantilla.Replace("@CLIENTE", io.Datos.Cliente);
        plantilla = plantilla.Replace("@REVISION", io.Datos.Revision.ToString());
        plantilla = plantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/general/informacionObraAdmin.aspx", "id=" + idIO));

        string cc = io.Datos.Gerente.Email + "; " + io.Datos.ResponsableObra.Email;

        Email email = new Email(Constantes.EmailIntranet, Constantes.EmailResponsableIIO, cc, "Informe de Inicio de Obra " + 
            (actualizacion ? "[Revisión Nº " + io.Datos.Revision + "]" : ""), plantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene las revisiones de una obra.
    /// </summary>
    internal static List<RevisionObra> GetRevisionesObra(int idObra)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        List<RevisionObra> result = new List<RevisionObra>();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT FechaModif, Revision FROM tbl_InformacionObrasHistoricos WHERE idObra = @idObra ORDER BY ";
            cmd.CommandText += "FechaModif DESC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObra", idObra));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(new RevisionObra(Convert.ToDateTime(dr["FechaModif"]), Convert.ToInt32(dr["Revision"])));
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
    /// Obtiene los informes de obra que tienen faltantes de información.
    /// </summary>
    internal static List<AlertaInformacionObra> GetAlertasInformacionObra()
    {
        List<AlertaInformacionObra> result = new List<AlertaInformacionObra>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT io.idObra, hi.FechaInicio, p1.Email AS EmailResponsable, p2.Email AS EmailGerente, ";
            cmd.CommandText += "p3.Email AS EmailInforma ";
            cmd.CommandText += "FROM tbl_InformacionObras io ";
            cmd.CommandText += "INNER JOIN tbl_InformacionObrasHistoricos hi ON io.idObra = hi.idObra ";
            cmd.CommandText += "AND hi.Revision = (SELECT TOP 1 hi1.Revision FROM tbl_InformacionObrasHistoricos hi1 WHERE ";
            cmd.CommandText += "hi1.idObra = io.idObra ORDER BY hi1.Revision DESC) ";
            cmd.CommandText += "LEFT JOIN tbl_Personal p1 ON hi.idResponsableObra = p1.idPersonal LEFT JOIN ";
            cmd.CommandText += "tbl_Personal p2 ON hi.idGerente = p2.idPersonal LEFT JOIN tbl_Personal p3 ON ";
            cmd.CommandText += "io.idInforma = p3.idPersonal WHERE ";
            cmd.CommandText += "(SELECT COUNT(pm.idObraHistorico) FROM tbl_InformacionObrasPersMant pm WHERE pm.idObraHistorico = ";
            cmd.CommandText += "hi.idObraHistorico) = 0 AND ";
            cmd.CommandText += "(SELECT COUNT(po.idObraHistorico) FROM tbl_InformacionObrasPersObra po WHERE po.idObraHistorico = ";
            cmd.CommandText += "hi.idObraHistorico) = 0 ";
            cmd.CommandText += "AND DATEDIFF(dd, GetDate(), FechaInicio) IN (10, 15, 20)";

            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                AlertaInformacionObra alerta = new AlertaInformacionObra(
                    Convert.ToInt32(dr["idObra"]),
                    Convert.ToDateTime(dr["FechaInicio"]),
                    dr["EmailResponsable"].ToString() + ", " + dr["EmailGerente"].ToString() + ", " + dr["EmailInforma"].ToString());
                result.Add(alerta);
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
    /// Procesa las alertas de los informes de obra.
    /// </summary>
    public static void ProcesarAlertasInformacionObra()
    {
        List<AlertaInformacionObra> alertas = GetAlertasInformacionObra();

        alertas.ForEach(a => EnviarAlertaInformacionObra(a));
    }
    /// <summary>
    /// Envía una alerta de información de obra.
    /// </summary>
    private static void EnviarAlertaInformacionObra(AlertaInformacionObra alerta)
    {
        // Obtengo la plantilla.
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_ALERTA_IIO);
        if (plantilla == null)
        {
            throw new EmailException();
        }

        string tipo = "info";
        string observaciones = "";
        DateTime hoy = DateTime.Now;
        int dias = (int)Math.Round((alerta.FechaInicio - hoy).TotalDays, 0);
        if (dias == 20 || dias == 15)
        {
            tipo = dias == 20 ? "info" : "warning";
            observaciones = "restan " + dias + " días para el inicio de la obra. Recuerde que debe cargar el Personal / Vehículos ";
            observaciones += "necesarios con " + MinDiasAnticipo + " días de anticipo.";
        }
        else
        {
            tipo = "error";
            observaciones = "se ha superado la fecha límite para la carga de Personal / Vehículos. En caso de ser necesario, puede modificar la fecha estimada de inicio de obra en sitio o solicitar una excepción.";
        }

        // Reemplazo las variables.
        plantilla = plantilla.Replace("@TIPO_MENSAJE", tipo);
        plantilla = plantilla.Replace("@OBSERVACIONES", observaciones);

        plantilla = plantilla.Replace("@ID_OBRA", alerta.IdObra.ToString());
        plantilla = plantilla.Replace("@FECHA_INICIO", alerta.FechaInicio.ToShortDateString());
        plantilla = plantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/general/informacionObraAdmin.aspx", "id=" + 
            alerta.IdObra));

        Email email = new Email(Constantes.EmailIntranet, alerta.EmailPara, "", "Informe de Inicio de Obra " +
            "[Recordatorio]", plantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Solicita una autorización para la obra.
    /// </summary>
    public static int SolicitarAutorizacion(int idObra, string motivo)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;
        int idAutorizacion = Constantes.ValorInvalido;

        try
        {
            InformacionObra io = GetInformacionObra(idObra, Constantes.ValorInvalido);

            motivo = "[" + io.Datos.Imputacion + "] - Modificación de datos: " + motivo;

            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            idAutorizacion = Autorizaciones.NuevaAutorizacion(motivo, Constantes.IdPersonaGerencia, SeccionAutorizacion.InformacionInternaObra, trans);

            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_AutorizacionesIIO (idObra, idAutorizacion) VALUES  (@idObra, @idAutorizacion)";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idObra", idObra));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idAutorizacion", idAutorizacion));
            cmd.ExecuteNonQuery();

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

        Autorizaciones.EnviarAutorizacion(idAutorizacion);

        return idAutorizacion;
    }
}