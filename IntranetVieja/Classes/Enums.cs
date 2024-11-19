using System;

//TIPOS DE DATO--------------------------------------------------------------------------->
using System.ComponentModel;

public enum EstadosLicencia
{
    NoRecibida = 0, RechazadaResponsable = 1, AprobadaResponsable = 2, RechazadaRRHH = 3, Confirmada = 4
}
public enum TiposLicencia
{
    Vacaciones = 0, Franco = 1, Examen = 2, SinGoceHaberes = 3, Casamiento = 4, Nacimiento = 5, 
    ModificacionHorario = 6, LicenciaSinAviso = 7, Presente = 99
}
public enum EstadosParteDiario
{
    Presente = 0, Licencia = 1
}
public enum EstadosVDM
{
    Enviada = 0, RecibidaResponsable = 1, AprobadaResponsable = 2, RecibidaDeposito = 3, EntregadaDeposito = 4, 
    RechazadaResponsable = 5
}
public enum EstadosNC
{
    ProcesandoSGI = 0, ProcesandoImputado = 1, EsperandoCierre = 2,
    Cerrada = 3, NoCorresponde = 4
}
public enum ConclusionesNC
{
    Satisfactoria = 1,
    EnProceso = 3,
    NoCorresponde = 2
}
public enum CategoriasNC
{
    NotaNoConformidad = 0,
    Observacion = 1,
    OportunidadMejora = 2,
    Stock = 3,
    NoCorresponde = 4
}
public enum FiltrosNC
{
    Asunto = 0, Categoria = 1, Area = 2, Estado = 3, EmitidaPor = 4, Numero = 5
}
public enum FiltrosVDM
{
    Codigo = 0, Imputacion = 1, Solicito = 2, Estado = 3
}
public enum TIPO_FILTRO_SEM
{
    Todas = 0
}
public enum TIPO_FILTRO_SECCION
{
    Todas,
    Personal
}
public enum FiltrosHerramienta
{
    Marca = 0,
    Descripcion = 1,
    PersonaCargo = 2,
    Tipo = 3,
    NumeroHerramienta = 4,
    NumeroInstrumento = 5,
    Clasificacion = 6
}
public enum FrecCalHerramienta
{
    Mensual = 0,
    Bimestral = 1,
    UnAno = 2,
    DosAnos = 3
}
public enum TiposCalHerramienta
{
    Externa = 0,
    Interna = 1
}
public enum FiltrosSolViaje
{
    Solicito = 0,
    Destino = 1,
    Imputacion = 2,
    Vehiculo = 3,
    Estado = 4
}
public enum VehiculosSolViaje 
{ 
    Moto = 0, Taxi = 1, Flete = 2, Auto = 3
}
public enum ImporanciasSolViaje 
{ 
    Baja = 0, Normal = 1, Alta = 2 
}
public enum EstadosSolViaje
{ 
    Enviada = 0, Leida = 1, Aprobada = 2, Confirmada = 3, Cancelada = 4 
}
public enum EstadosCodArt
{
    Revision = 0,
    Rechazado = 1,
    Aprobado = 2,
    NoCorresponde = 3
}
public enum FiltrosCodArt
{
    Solicito = 0,
    Estado = 1
}
public enum FiltrosImputacion
{
    Numero = 0,
    Descripcion = 1,
    Estado = 2
}
public enum EstadosImputacion
{
    Activa = 1,
    Inactiva = 0
}
public enum EstadosPersona
{
    Activa = 1,
    Inactiva = 0
}
public enum FiltrosPersona
{
    Id = 0,
    Nombre = 1,
    Usuario = 2,
    Autoriza = 3,
    EnPanelControl = 4,
    Estado = 5
}
public enum PermisosPersona
{
    // Administración del sistema.
    Administrador = 0,
    AdminImputaciones = 1,
    AdminPanelesControlPD = 2,
    AdminPersonal = 3,
    AdminPartesDiarios = 4,

    Publico = 0x0A,

    // Vale de Materiales.
    ValeMaterialesRecibeResp = 10,
    ValeMaterialesApruebaResp = 11,
    ValeMaterialesRecibeDep = 12,
    ValeMaterialesEntrega = 13,
    ValeMaterialesVer = 14,
    ValeMaterialesInforme = 15,

    // Solicitud de Envío de Materiales.
    SolEnvMatVer = 20,
    SolEnvMatGuardar = 21,
    SolEnvMatConfirmar = 22,
    SolEnvMatCerrar = 23,

    // Nota de No Conformidad.
    NNCAdministrador = 30,
    NNCVer = 31,
    NNCEditar = 32,

    // Solicitud de Viajes.
    SolViajeVer = 40,
    SolViajeEditar = 41,

    // Repositorio de archivos.
    RDACrear = 50,
    RDAVer = 51,

    // Stock.
    CotizadorOnLine = 60,
    CotizadorOnLineEquipos = 61,
    ControlHerramientas = 62,
    StockVer = 63,
    StockIngreso = 64,
    StockEgreso = 65,

    // Administración de personal.
    LicRRHH = 70,  // RR.HH.
    ADP_CargaEntrada,
    ADP_PanelControlAsistencia,
    ADP_CargaSalida,
    ADP_PcResponsable,

    // Solicitud de alta de artículos.
    SAAResponsable = 80,

    // Herramientas.
    HerramientaAdministrador = 91,
    InstrumentoSeguimiento = 92,

    // Paneles de control.
    PCVerGeneral = 100,

    // Información interna de obra.
    IIOGenerar = 110,
    IIOVer = 111,

    // Roles.
    RolDireccion = 120,
    RolGerencia = 121,

    // SSM.
    SSM_Admin = 130,

    // Vehículos.
    VehicAdmin = 140,
    VehicVer = 141,

    // Autorizaciones.
    AutorizAdministrar = 150,

    // Sistema de Notificación de Ventas.
    SNV_Visualizacion = 160,
    SNV_Vendedor = 161,
    SNV_AltaFacRem = 162,
    SNV_AltaCliente = 163,
    SNV_AltaImputacion = 164,
    SNV_NotifOC =165,
    SNV_NotifCierre = 166,
    SNV_NotifRecordatorio = 167,
    SNV_AltaTransporte = 168,
    SNV_NotifProducto = 169,
    SNV_NotifRMA = 170,
	SNV_NotifProductoProser = 171,
	SNV_NotifServicioCA = 172, //172
	SNV_NotifServicioWD = 173, //173
    // General.
    GEN_CargaParteDiario = 180
}
public enum ESTADO_SEM 
{ 
    EsperandoConfirmacion = 0, Confirmada = 1, Cerrada = 2, Rechazada = 3
}
public enum TIPO_TRANSP_SEM 
{ 
    Expreso = 0, Interno =1 
}
public enum TIPO_CAUSA_SEM
{
    Reemplazo = 0,
    Falla = 1,
    Otros = 2
}
public enum ORIGEN_ITEM_SEM
{
    SM = 0,
    VDM = 1
}
public enum BDConexiones
{
    Intranet,
    Tango,
    Proser
}
public enum PermisosRDA
{
    Lectura = 0,
    LecturaEscritura = 1
}
public enum TiposTrabajoObra
{
    Obra = 0,
    Mantenimiento = 1,
    Auditoria = 2,
	Servicio = 3
}
public enum FiltrosInformeObra
{
    NumObra = 0,
    Cliente = 1,
    Responsable = 2,
    Informante = 3,
    OrdenCompra = 4,
    Imputacion = 5
}
public enum TiposBinario
{
    No = 0,
    Si = 1
}
public enum EstadosSitio
{
    NoCumplido = 0,
    Cumplido = 1,
    NoAplica = 2
}
public enum FrecuenciasSitio
{
    Mensual = 0,
    Anual = 1
}
public enum RepositoriosArchivos
{
    Comun = 0,
    SGI_Multisitio,
    SGI_BuenosAires,
    SGI_Gas,
    SGI_Liquidos,
    SGI_Valvulas,
    SGI_Bolivia,
    Petrobras,
    Manual_SGI,
    Politica_SGI,
    Politica_Alcohol_Drogas,
    Certificaciones,
    SGI_BsAs_Compras,
    SGI_BsAs_Desarrollo,
    SGI_BsAs_Informatica,
    SGI_BsAs_Ingenieria,
    SGI_BsAs_Mantenimiento,
    SGI_BsAs_Obras,
    SGI_BsAs_RRHH,
    SGI_BsAs_Seguridad_Higiene,
    SGI_BsAs_Ventas,
    SGI_BsAs_Metrologia,
    SGI_BsAs_Organigramas,
    SGI_BsAs_Proyectos,
    SGI_BsAs_AdminFinanz,
    SGI_BsAs_Deposito,
    SGI_Politica_SGI,
    SGI_Manual_SGI,
    SGI_Politica_Alcohol_Drogas,
    SGI_Certificaciones,
    SGI_Normas,
    SGI_Procedimientos_SGI,
    MA_ControlResiduos,
    MA_Emergencias_Ambientales,
    MA_Actuacion_Derrames,
    SEG_Matriz,
    SEG_Investigacion_Incidentes,
    SEG_EPP,
    SEG_Seguridad_Salud_Operaciones,
    SEG_Plan_Emergencias,
    RRHH_Manual_Empleado,
    RRHH_Organigrama,
    RRHH_Registro_Capacitacion,
    DE_Lista_Doc_Ext,
    DE_Doc_Ext, 
    ITR,
    SGI,
    MedioAmbiente,
    Seguridad,
    RRHH,
    Mat_Leg_Int,
	DOC_SGI,
	DOC_MedioAmbiente,
	DOC_RRHH
}
public enum ClasifHerramienta
{
    Herramienta = 0,
    Instrumento = 1
}
public enum PathImage
{
    ListadoInstrumentos = 0
}
public enum PathDescargas
{
    ListadoInstrumentos = 0,
    ITR,
    OC
}
public enum TipoListadoHerramientas
{
    Herramientas = 0,
    Instrumentos = 1
}
public enum TipoAlertaVencimiento
{
    Precaucion,
    Vencido,
    Recordatorio
}
public enum EstadoAutorizacion
{
    Pendiente = 0,
    Aprobada,
    Rechazada
}
public enum SeccionAutorizacion
{
    InformacionInternaObra = 0
}
public enum FiltrosAutorizacion
{
    Solicito = 0,
    Estado,
    Referencia,
    Responsable
}
public enum StockOperaciones
{
    Ingreso = 0,
    Egreso
}
public enum FiltrosArticuloStock
{
    
}
public enum FrecuenciaCalibracion
{
    Mensual = 0,
    Anual,
    DosAnios
}

//german 

public enum FrecuenciaMantenimiento
{
    Mensual = 0,
    Anual,
    DosAnios
}

public enum FrecuenciaComprobacion
{
    Mensual = 0,
    Anual,
    DosAnios
}

// fin german

public enum FiltroInstrumento
{
    Numero,
    Tipo,
    Descripcion,
    Grupo,
    Marca,
    Modelo,
    NumSerie,
    Todo
}
public enum EstadoAsistencia
{
    Ausente = 0,
    Presente,
    Licencia,
    AusenteArt,
    AusentePmc,
    AusenteFall,
    AusenteFeriado,
    FrancoPersonalObras
}
public enum ProcesoAsistencia
{
    SinProcesar = 0,
    Procesada
}
public enum Moneda
{
    Peso,
    Dolar,
    Euro,
    Real
}
public enum EstadoNotifVenta
{
    CargandoDatos = 0,
    CargandoRemito,
    EsperandoAprobacion,
    ConfeccionRem,
    ConfeccionFac,
    EsperandoITR,
    Cerrada,
    Rechazada
}
public enum FiltroNotifVenta
{
    ID,
    Vendedor,
    Cliente,
    OC,
    Imputacion,
    Fecha,
    Factura,
    Remito,
    Estado
}
public enum TipoNotifVenta
{
    Producto = 0,
    Servicio,
    RMA,
    RemitoOficial,
    RemitoInterno,
    Obra,
    ProductoProser,
    ServicioCA,
    ServicioWD
}
public enum PropCambioUrgencia
{
    [Description("Alta")]
    Alta = 0,
    [Description("Media")]
    Media,
    [Description("Baja")]
    Baja
}

// plantillas de email

public enum EmailPlantilla
{
	NVAltaCliente = 0,
	NVAltaImputacion,
	NVAltaGeneralOC,
	NVRecordatorio	
}



//FIN TIPOS DE DATO----------------------------------------------------------------------->

