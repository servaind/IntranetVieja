/*
 * Historial:
 * ===================================================================================
 * [01/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Descripción breve de SolicitudViaje
/// </summary>
public partial class SolicitudViaje
{
    // Variables.
    private int idViaje;
    private VehiculosSolViaje vehiculo;
    private ImporanciasSolViaje importancia;
    private DateTime fechaSolicitud;
    private string motivo;
    private string descripcion;
    private string origen;
    private string ruta;
    private string finRecorrido;
    private DateTime fechaCumplimiento;
    private string horaCumplimiento;
    private DateTime fechaLimite;
    private string horaLimite;
    private string destinatario;
    private string direccion;
    private string localidad;
    private string contacto;
    private string telefono;
    private string horarioAtencion;
    private string documentoReferencia;
    private bool retornaFactura;
    private bool retornaRemito;
    private bool retornaRecibo;
    private string retornaOtro;
    private string condicionesComerciales;
    private int idImputacion;
    private Imputacion imputacion;
    private float importe;
    private bool efectivo;
    private bool cheque;
    private string aLaOrden;
    private int idSolicito;
    private Persona solicito;
    private int idLectura;
    private Persona lectura;
    private DateTime fechaLectura;
    private int idAprobo;
    private Persona aprobo;
    private DateTime fechaAprobo;
    private int idConfirmo;
    private Persona confirmo;
    private DateTime fechaConfirmo;
    private int idCancelo;
    private Persona cancelo;
    private DateTime fechaCancelo;
    private EstadosSolViaje estado;
    private string observaciones;

    // Propiedades.
    public int IDViaje
    {
        get { return idViaje; }
    }
    public VehiculosSolViaje Vehiculo
    {
        get { return vehiculo; }
    }
    public ImporanciasSolViaje Importancia
    {
        get { return importancia; }
    }
    public DateTime FechaSolicitud
    {
        get { return fechaSolicitud; }
    }
    public string Motivo
    {
        get { return motivo; }
    }
    public string Descripcion
    {
        get { return descripcion; }
    }
    public string Origen
    {
        get { return origen; }
    }
    public string Ruta
    {
        get { return ruta; }
    }
    public string FinRecorrido
    {
        get { return finRecorrido; }
    }
    public DateTime FechaCumplimiento
    {
        get { return fechaCumplimiento; }
    }
    public string HoraCumplimiento
    {
        get { return horaCumplimiento; }
    }
    public DateTime FechaLimite
    {
        get { return fechaLimite; }
    }
    public string HoraLimite
    {
        get { return horaLimite; }
    }
    public string Destinatario
    {
        get { return destinatario; }
    }
    public string Direccion
    {
        get { return direccion; }
    }
    public string Localidad
    {
        get { return localidad; }
    }
    public string Contacto
    {
        get { return contacto; }
    }
    public string Telefono
    {
        get { return telefono; }
    }
    public string HorarioAtencion
    {
        get { return horarioAtencion; }
    }
    public string DocumentoReferencia
    {
        get { return documentoReferencia; }
    }
    public bool RetornaFactura
    {
        get { return retornaFactura; }
    }
    public bool RetornaRemito
    {
        get { return retornaRemito; }
    }
    public bool RetornaRecibo
    {
        get { return retornaRecibo; }
    }
    public string RetornaOtro
    {
        get { return retornaOtro; }
    }
    public string CondicionesComerciales
    {
        get { return condicionesComerciales; }
    }
    public Imputacion Imputacion
    {
        get 
        {
            if (this.imputacion == null)
            {
                this.imputacion = GImputaciones.GetImputacion(this.idImputacion);
            }

            return this.imputacion;
        }
    }
    public float Importe
    {
        get { return importe; }
    }
    public bool Efectivo
    {
        get { return efectivo; }
    }
    public bool Cheque
    {
        get { return cheque; }
    }
    public string AlaOrden
    {
        get { return aLaOrden; }
    }
    public Persona Solicito
    {
        get 
        {
            if (this.solicito == null)
            {
                this.solicito = GPersonal.GetPersona(this.idSolicito);
            }

            return this.solicito; 
        }
    }
    public Persona Lectura
    {
        get
        {
            if (this.lectura == null)
            {
                this.lectura = GPersonal.GetPersona(this.idLectura);
            }

            return this.lectura;
        }
    }
    public Persona Aprobo
    {
        get
        {
            if (this.aprobo == null)
            {
                this.aprobo = GPersonal.GetPersona(this.idAprobo);
            }

            return this.aprobo;
        }
    }
    public Persona Confirmo
    {
        get
        {
            if (this.confirmo == null)
            {
                this.confirmo = GPersonal.GetPersona(this.idConfirmo);
            }

            return this.confirmo;
        }
    }
    public Persona Cancelo
    {
        get
        {
            if (this.cancelo == null)
            {
                this.cancelo = GPersonal.GetPersona(this.idCancelo);
            }

            return this.cancelo;
        }
    }
    public DateTime FechaLectura
    {
        get { return fechaLectura; }
    }
    public DateTime FechaAprobo
    {
        get { return fechaAprobo; }
    }
    public DateTime FechaConfirmo
    {
        get { return fechaConfirmo; }
    }
    public DateTime FechaCancelo
    {
        get { return fechaCancelo; }
    }
    public EstadosSolViaje Estado
    {
        get { return estado; }
    }
    public string Observaciones
    {
        get { return observaciones; }
    }


    internal SolicitudViaje(int idViaje, VehiculosSolViaje vehiculo, ImporanciasSolViaje importancia, 
        DateTime fechaSolicitud, string motivo, string descripcion, string origen, string ruta, string finRecorrido, 
        DateTime fechaCumplimiento, string horaCumplimiento, DateTime fechaLimite, string horaLimite, string destinatario,
        string direccion, string localidad, string contacto, string telefono, string horarioAtencion, string documentoReferencia, 
        bool retornaFactura, bool retornaRemito, bool retornaRecibo, string retornaOtro, string condicionesComerciales,
        int idImputacion, float importe, bool efectivo, bool cheque, string aLaOrden, int idSolicito, int idLectura,
        DateTime fechaLectura, int idAprobo, DateTime fechaAprobo, int idConfirmo, DateTime fechaConfirmo, int idCancelo, 
        DateTime fechaCancelo, EstadosSolViaje estado, string observaciones)
    {
        this.idViaje = idViaje;
        this.vehiculo = vehiculo;
        this.importancia = importancia;
        this.fechaSolicitud = fechaSolicitud;
        this.motivo = motivo;
        this.descripcion = descripcion;
        this.origen = origen;
        this.ruta = ruta;
        this.finRecorrido = finRecorrido;
        this.fechaCumplimiento = fechaCumplimiento;
        this.horaCumplimiento = horaCumplimiento;
        this.fechaLimite = fechaLimite;
        this.horaLimite = horaLimite;
        this.destinatario = destinatario;
        this.direccion = direccion;
        this.localidad = localidad;
        this.contacto = contacto;
        this.telefono = telefono;
        this.horarioAtencion = horarioAtencion;
        this.documentoReferencia = documentoReferencia;
        this.retornaFactura = retornaFactura;
        this.retornaRemito = retornaRemito;
        this.retornaRecibo = retornaRecibo;
        this.retornaOtro = retornaOtro;
        this.condicionesComerciales = condicionesComerciales;
        this.idImputacion = idImputacion;
        this.importe = importe;
        this.efectivo = efectivo;
        this.cheque = cheque;
        this.aLaOrden = aLaOrden;
        this.idSolicito = idSolicito;
        this.idLectura = idLectura;
        this.fechaLectura = fechaLectura;
        this.idAprobo = idAprobo;
        this.fechaAprobo = fechaAprobo;
        this.idConfirmo = idConfirmo;
        this.fechaConfirmo = fechaConfirmo;
        this.idCancelo = idCancelo;
        this.fechaCancelo = fechaCancelo;
        this.estado = estado;
        this.observaciones = observaciones;
    }
}

/// <summary>
/// Descripción breve de GSolicitudViaje
/// </summary>
public static class GSolicitudesViaje
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;
    public const int MinHsAntipo = 12;
    public const int MaxHsSolicitud = 16;   // Hasta que hs del día se pueden cargar las solicitudes.


    /// <summary>
    /// Obtiene una solicitud de viaje.
    /// </summary>
    private static SolicitudViaje GetSolicitudViaje(IDataReader dr)
    {
        SolicitudViaje result;

        try
        {
            result = new SolicitudViaje(
                Convert.ToInt32(dr["idViaje"]),
                (VehiculosSolViaje)Convert.ToInt32(dr["idVehiculo"]),
                (ImporanciasSolViaje)Convert.ToInt32(dr["idImportancia"]),
                DateTime.Parse(dr["FechaSol"].ToString()),
                dr["Motivo"].ToString(),
                dr["Descripcion"].ToString(),
                dr["Origen"].ToString(),
                dr["Ruta"].ToString(),
                dr["FinRecorrido"].ToString(),
                DateTime.Parse(dr["FechaCump"].ToString()),
                dr["HoraCump"].ToString(),
                DateTime.Parse(dr["FechaLim"].ToString()),
                dr["HoraLim"].ToString(),
                dr["Destinatario"].ToString(),
                dr["Direccion"].ToString(),
                dr["Localidad"].ToString(),
                dr["Contacto"].ToString(),
                dr["Telefono"].ToString(),
                dr["HorarioAt"].ToString(),
                dr["DocRef"].ToString(),
                Convert.ToBoolean(dr["RetFac"]),
                Convert.ToBoolean(dr["RetRem"]),
                Convert.ToBoolean(dr["RetRec"]),
                dr["RetOtro"].ToString(),
                dr["CondComer"].ToString(),
                Convert.ToInt32(dr["idImputacion"]),
                Convert.ToSingle(dr["Importe"]),
                Convert.ToBoolean(dr["Efectivo"]),
                Convert.ToBoolean(dr["Cheque"]),
                dr["AlaOrden"].ToString(),
                Convert.ToInt32(dr["idSolicito"]),
                Convert.ToInt32(dr["idLectura"]),
                String.IsNullOrEmpty(dr["FechaLectura"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaLectura"].ToString()),
                Convert.ToInt32(dr["idAprobo"]),
                String.IsNullOrEmpty(dr["FechaAprobo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaAprobo"].ToString()),
                Convert.ToInt32(dr["idConfirmo"]),
                String.IsNullOrEmpty(dr["FechaConfirmo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaConfirmo"].ToString()),
                Convert.ToInt32(dr["idCancelo"]),
                String.IsNullOrEmpty(dr["FechaCancelo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaCancelo"].ToString()),
                (EstadosSolViaje)Convert.ToInt32(dr["Estado"]),
                dr["Observaciones"].ToString()
            );
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una solicitud de viaje.
    /// </summary>
    private static SolicitudViaje GetSolicitudViaje(DataRow dr)
    {
        SolicitudViaje result;

        try
        {
            result = new SolicitudViaje(
                Convert.ToInt32(dr["idViaje"]),
                (VehiculosSolViaje)Convert.ToInt32(dr["idVehiculo"]),
                (ImporanciasSolViaje)Convert.ToInt32(dr["idImportancia"]),
                DateTime.Parse(dr["FechaSol"].ToString()),
                dr["Motivo"].ToString(),
                dr["Descripcion"].ToString(),
                dr["Origen"].ToString(),
                dr["Ruta"].ToString(),
                dr["FinRecorrido"].ToString(),
                DateTime.Parse(dr["FechaCump"].ToString()),
                dr["HoraCump"].ToString(),
                DateTime.Parse(dr["FechaLim"].ToString()),
                dr["HoraLim"].ToString(),
                dr["Destinatario"].ToString(),
                dr["Direccion"].ToString(),
                dr["Localidad"].ToString(),
                dr["Contacto"].ToString(),
                dr["Telefono"].ToString(),
                dr["HorarioAt"].ToString(),
                dr["DocRef"].ToString(),
                Convert.ToBoolean(dr["RetFac"]),
                Convert.ToBoolean(dr["RetRem"]),
                Convert.ToBoolean(dr["RetRec"]),
                dr["RetOtro"].ToString(),
                dr["CondComer"].ToString(),
                Convert.ToInt32(dr["idImputacion"]),
                Convert.ToSingle(dr["Importe"]),
                Convert.ToBoolean(dr["Efectivo"]),
                Convert.ToBoolean(dr["Cheque"]),
                dr["AlaOrden"].ToString(),
                Convert.ToInt32(dr["idSolicito"]),
                Convert.ToInt32(dr["idLectura"]),
                String.IsNullOrEmpty(dr["FechaLectura"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaLectura"].ToString()),
                Convert.ToInt32(dr["idAprobo"]),
                String.IsNullOrEmpty(dr["FechaAprobo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaAprobo"].ToString()),
                Convert.ToInt32(dr["idConfirmo"]),
                String.IsNullOrEmpty(dr["FechaConfirmo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaConfirmo"].ToString()),
                Convert.ToInt32(dr["idCancelo"]),
                String.IsNullOrEmpty(dr["FechaCancelo"].ToString()) ? Constantes.FechaInvalida : DateTime.Parse(dr["FechaCancelo"].ToString()),
                (EstadosSolViaje)Convert.ToInt32(dr["Estado"]),
                dr["Observaciones"].ToString()
            );
        }
        catch
        {
            result = null;
        }

        return result;
    }
	/// <summary>
	/// Obtiene una Solicitud de Viaje.
	/// </summary>
	public static SolicitudViaje GetSolicitudViaje(int idSolicitud)
	{
		IDbConnection conn = null;
		IDbCommand cmd;
		IDataReader dr;
        SolicitudViaje result;

		try
		{
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
			cmd.CommandText = "SELECT * FROM tbl_SolicitudesViaje WHERE idViaje = @idViaje";
			cmd.Parameters.Add(DataAccess.GetDataParameter("@idViaje", idSolicitud));
			dr = cmd.ExecuteReader();

            if (!dr.Read())
			{
                throw new ElementoInexistenteException();
			}

            result = GetSolicitudViaje(dr);

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
    /// Guarda una Solicitud de Viaje.
    /// </summary>
    /// <param name="cc">Enviar con copia a Gerencia.</param>
    public static void NuevaSolicitudViaje(VehiculosSolViaje vehiculo, ImporanciasSolViaje importancia, string motivo, 
        string descripcion, string origen, string ruta, string finRecorrido, DateTime fechaCumplimiento, string horaCumplimiento, 
        DateTime fechaLimite, string horaLimite, string destinatario, string direccion, string localidad, 
        string contacto, string telefono, string horarioAtencion, string docReferencia, bool retFactura, bool retRemito, 
        bool retRecibo, string retOtro, string condComerciales, int idImputacion, float importe, bool efectivo, 
        bool cheque, string aLaOrden, string observaciones, bool cc, out int idSolicitud)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        if (motivo.Trim().Length == 0 || descripcion.Trim().Length == 0 || origen.Trim().Length == 0 ||
            finRecorrido.Trim().Length == 0 || destinatario.Trim().Length == 0 || direccion.Trim().Length == 0 ||
            localidad.Trim().Length == 0 || contacto.Trim().Length == 0 || telefono.Trim().Length == 0 ||
            horarioAtencion.Trim().Length == 0 || idImputacion == Constantes.IdImputacionInvalida)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_SolicitudesViaje(idVehiculo,idImportancia,";
            cmd.CommandText += "Motivo,Descripcion,Origen,Ruta,FinRecorrido,FechaCump,HoraCump,FechaLim,";
            cmd.CommandText += "HoraLim,Destinatario,Direccion,Localidad,Contacto,Telefono,HorarioAt,";
            cmd.CommandText += "DocRef,RetFac,RetRem,RetRec,RetOtro,CondComer,idImputacion,Importe,";
            cmd.CommandText += "Efectivo,Cheque,AlaOrden,idSolicito,Observaciones)";
            cmd.CommandText += "VALUES(@idVehiculo,@idImportancia,@Motivo,@Descripcion,";
            cmd.CommandText += "@Origen,@Ruta,@FinRecorrido,@FechaCump,@HoraCump,@FechaLim,@HoraLim,";
            cmd.CommandText += "@Destinatario,@Direccion,@Localidad,@Contacto,@Telefono,@HorarioAt,";
            cmd.CommandText += "@DocRef,@RetFac,@RetRem,@RetRec,@RetOtro,@CondComer,@idImputacion,@Importe,";
            cmd.CommandText += "@Efectivo,@Cheque,@AlaOrden,@idSolicito,@Observaciones);";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_SolicitudesViaje;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idVehiculo", (int)vehiculo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImportancia", (int)importancia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Motivo", motivo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Descripcion", descripcion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Origen", origen));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Ruta", ruta));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FinRecorrido", finRecorrido));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaCump", fechaCumplimiento));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraCump", horaCumplimiento));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaLim", fechaLimite));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraLim", horaLimite));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Destinatario", destinatario));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Direccion", direccion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Localidad", localidad));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Contacto", contacto));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Telefono", telefono));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HorarioAt", horarioAtencion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@DocRef", docReferencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RetFac", retFactura));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RetRem", retRemito));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RetRec", retRecibo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RetOtro", retOtro));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CondComer", condComerciales));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacion", idImputacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Importe", importe));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Efectivo", efectivo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Cheque", cheque));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AlaOrden", aLaOrden));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicito", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", observaciones));

            idSolicitud = Convert.ToInt32(cmd.ExecuteScalar());
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

        EnviarSolViaje(idSolicitud, cc ? Constantes.EmailGerencia : "");
    }
    /// <summary>
    /// Envía la Solicitud.
    /// </summary>
    private static void EnviarSolViaje(int idSolicitud)
    {
        EnviarSolViaje(idSolicitud, "");
    }
    /// <summary>
    /// Envía la Solicitud.
    /// </summary>
    private static void EnviarSolViaje(int idSolicitud, string cc)
    {
        //Cargo la Plantilla.
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_SV_EMAIL);
        if (strPlantilla == null)
        {
            throw new EmailException();
        }

        SolicitudViaje sv = GetSolicitudViaje(idSolicitud);
        if (sv == null)
        {
            throw new EmailException();
        }

        string para = "";
        string asunto = "";

        switch (sv.Estado)
        {
            case EstadosSolViaje.Enviada:
                para = Constantes.EmailResponsablesSV;
                asunto = "Solicitud de viaje Nº" + idSolicitud;
                break;
            case EstadosSolViaje.Aprobada:
                para = sv.Solicito.Email;
                asunto = "Solicitud de viaje Nº" + idSolicitud + " [APROBADA]";
                break;
            case EstadosSolViaje.Cancelada:
                para = sv.Solicito.Email;
                asunto = "Solicitud de viaje Nº" + idSolicitud + " [CANCELADA]";
                break;
        }

        if (para.Length > 0)
        {
            // Reemplazo las variables.
            strPlantilla = strPlantilla.Replace("@ENCABEZADO", "");
            strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", sv.Estado != EstadosSolViaje.Cancelada ? "info" : "error");
            strPlantilla = strPlantilla.Replace("@EMITIDA_POR", sv.Solicito.Nombre);
            strPlantilla = strPlantilla.Replace("@VEHICULO", sv.Vehiculo.ToString());
            strPlantilla = strPlantilla.Replace("@IMPORTANCIA", sv.Importancia.ToString());
            strPlantilla = strPlantilla.Replace("@DESTINO", sv.Destinatario);
            strPlantilla = strPlantilla.Replace("@IMPUTACION", sv.Imputacion.DescripcionCompleta);
            strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/general/viajeAdmin.aspx",
                "id=" + idSolicitud));

            // Envío el E-mail.
            Email email = new Email(Constantes.Usuario.Email, para, cc, asunto, strPlantilla);
            if (!email.Enviar())
            {
                throw new EmailException();
            }
        }
    }
    /// <summary>
    /// Obtiene el estado de una solicitud de viaje.
    /// </summary>
    private static EstadosSolViaje GetEstadoSolViaje(int idSolicitud)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        EstadosSolViaje result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Estado FROM tbl_SolicitudesViaje WHERE idViaje = @idSolicitud";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicitud", idSolicitud));

            result = (EstadosSolViaje)Convert.ToInt32(cmd.ExecuteScalar());
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

        return result;
    }
    /// <summary>
    /// Aprueba el estado actual de la solicitud de viaje.
    /// </summary>
    public static void AprobarEstadoSolViaje(int idSolicitud)
    {
        ActualizarSolViaje(idSolicitud, true);
    }
    /// <summary>
    /// Rechaza el estado actual de la solicitud de viaje.
    /// </summary>
    public static void RechazarEstadoSolViaje(int idSolicitud)
    {
        ActualizarSolViaje(idSolicitud, false);
    }
    /// <summary>
    /// Actualiza el estado de la Solicitud de Viaje.
    /// </summary>
    private static void ActualizarSolViaje(int idSolicitud, bool aprobarEstado)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.SolViajeEditar))
        {
            throw new PrivilegiosException();
        }

        EstadosSolViaje estadoActual = GetEstadoSolViaje(idSolicitud);
        EstadosSolViaje estadoNuevo;
        string query = "UPDATE tbl_SolicitudesViaje SET ";

        switch (estadoActual)
        {
            case EstadosSolViaje.Enviada:
                if (!aprobarEstado)
                {
                    throw new ErrorOperacionException();
                }
                query += "idLectura = @idPersona, FechaLectura = @Fecha";
                estadoNuevo = EstadosSolViaje.Leida;
                break;
            case EstadosSolViaje.Leida:
                if (aprobarEstado)
                {
                    query += "idAprobo = @idPersona, FechaAprobo = @Fecha";
                    estadoNuevo = EstadosSolViaje.Aprobada;
                }
                else
                {
                    query += "idCancelo = @idPersona, FechaCancelo = @Fecha";
                    estadoNuevo = EstadosSolViaje.Cancelada;
                }
                break;
            case EstadosSolViaje.Aprobada:
                if (!aprobarEstado)
                {
                    throw new ErrorOperacionException();
                }
                query += "idConfirmo = @idPersona, FechaConfirmo = @Fecha";
                estadoNuevo = EstadosSolViaje.Confirmada;
                break;
            default:
                throw new ErrorOperacionException();
        }

        query += ", Estado = @Estado WHERE idViaje = @idSolicitud";

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = query;
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicitud", idSolicitud));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersona", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", DateTime.Now));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Estado", (int)estadoNuevo));

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

        if (estadoNuevo == EstadosSolViaje.Aprobada || estadoNuevo == EstadosSolViaje.Cancelada)
        {
            EnviarSolViaje(idSolicitud);
        }
    }
    /// <summary>
    /// Obtiene las solicitudes de viaje que coincidan con el filtro.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<SolicitudViaje> GetSolicitudesViaje(int pagina, List<Filtro> filtros)
    {
        List<SolicitudViaje> result;

        result = DataAccess.GetDataList<SolicitudViaje>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetSolicitudViaje);

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
        string filtroJoin = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosSolViaje.Solicito:
                    filtroWhere += "AND idSolicito = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosSolViaje.Destino:
                    filtroWhere += "AND Destinatario LIKE '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosSolViaje.Imputacion:
                    filtroWhere += "AND idImputacion = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosSolViaje.Vehiculo:
                    filtroWhere += "AND idVehiculo = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosSolViaje.Estado:
                    filtroWhere += "AND Estado = " + filtro.Valor + " ";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT COUNT(idViaje) as TotalRegistros";
        }
        else
        {
            consulta = "SELECT *";
        }

        if (filtroWhere.Length > 0)
        {
            filtroWhere = "WHERE " + filtroWhere;
        }
        consulta += " FROM tbl_SolicitudesViaje " + filtroJoin + " " + filtroWhere;

        if (!cantidad)
        {
            consulta += " ORDER BY FechaSol DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene los tipos de estado para las solicitudes de viaje.
    /// </summary>
    public static Dictionary<int, string> GetEstadosSolViaje()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosSolViaje.Enviada, "Enviada");
        result.Add((int)EstadosSolViaje.Leida, "Leída");
        result.Add((int)EstadosSolViaje.Aprobada, "Aprobada");
        result.Add((int)EstadosSolViaje.Confirmada, "Confirmada");
        result.Add((int)EstadosSolViaje.Cancelada, "Cancelada");

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de vehículos para las solicitudes de viaje.
    /// </summary>
    public static Dictionary<int, string> GetVehiculosSolViaje()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)VehiculosSolViaje.Flete, "Flete");
        result.Add((int)VehiculosSolViaje.Moto, "Moto");
        result.Add((int)VehiculosSolViaje.Taxi, "Taxi");
        result.Add((int)VehiculosSolViaje.Auto, "Auto");

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de importancia para las solicitudes de viaje.
    /// </summary>
    public static Dictionary<int, string> GetImportanciasSolViaje()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)ImporanciasSolViaje.Alta, "Alta");
        result.Add((int)ImporanciasSolViaje.Normal, "Normal");
        result.Add((int)ImporanciasSolViaje.Baja, "Baja");

        return result;
    }
}