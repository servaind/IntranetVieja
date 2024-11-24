/*
 * Historial:
 * ===================================================================================
 * [28/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;

public class Constantes
{
    #region Constantes
    
    public const bool TestMode = false;
    public const string DominioNombre = "DOMINIO";
    public const string DominioDC = "10.0.0.2";
    public const string TestUsers = "martin.duran|german.iglesias|spam";
    public const string PublicUsers = "petrobras;planillas*2011";
    public const int ValorInvalido = -1;
    public const int IdPersonaAdmin = 89;
    public const int IdPersonaGerencia = 36;
    //public const int IdPersonaGerencia = 89;
    public const int IdPersonaInvalido = 1;
	public const int IdImputacionInvalida = 269;
    public const int MaxHorasParteDiario = 12;
    public const string TiposArchivosRepositorio = ".exe .txt .jpg .png .gif .zip .rar .iso .xls .doc .xlsx .docx .pdf .ppt .pptx .pps .ppsx";
    public const char SepDescArtTango = '•';
    private const string PrefSession = "Constantes.cs.";

    #region Emails para circuitos

    public const string EmailIntranet = "intranet@servaind.com";
    public const string EmailRRHH = "rrhh@servaind.com";
    public const string EmailCalidad = "deposito@servaind.com, fernanda.calvi@servaind.com";
    public const string EmailDeposito = "deposito@servaind.com";
    public const string EmailResponsablesVDM = "deposito@servaind.com";
    public const string EmailResponsablesSV = "deposito@servaind.com, adrian.nielsen@servaind.com";
    public const string EmailDirector = "carlos.calvi@servaind.com";
    public const string MAIL_SEM_SOLICITO = "areacomercial@servaind.com";
    public const string EmailGerencia = "eric.nielsen@servaind.com";
    public const string EmailResponsableCodifArt = "deposito@servaind.com";
    public const string EmailResponsableHerramientas = "deposito@servaind.com";
    public const string EmailResponsableIIO = "paulo.velardes@servaind.com, carlos.calvi@servaind.com, "
        + "eric.nielsen@servaind.com, administracion@servaind.com, proveedores@servaind.com, "
		+ "admin-recursos@servaind.com, areacomercial@servaind.com, cristian.oviedo@servaind.com ";
    public const string EmailResponsablesVehiculos = "eric.nielsen@servaind.com, admin-recursos@servaind.com";
	public const string EmailAlertaStock = "paulo.velardes@servaind.com";
	public const string EmailBedelia = "recepcion@servaind.com";
	public const string EmailProveedores = "proveedores@servaind.com";
	
	
    #region Testing

	/*
    // IMPORTANTE: Comentar las líneas de las constantes de emails para circuitos y descomentar las de testing.
    public const string EmailRRHH = "paulo.velardes@servaind.com";
    public const string EmailCalidad = "paulo.velardes@servaind.com";
    public const string EmailDeposito = "paulo.velardes@servaind.com";
    public const string EmailResponsablesVDM = "paulo.velardes@servaind.com";
    public const string EmailResponsablesSV = "paulo.velardes@servaind.com";
    public const string EmailDirector = "paulo.velardes@servaind.com";
    public const string MAIL_SEM_SOLICITO = "paulo.velardes@servaind.com";
    public const string EmailGerencia = "paulo.velardes@servaind.com";
    public const string EmailSistemas = "paulo.velardes@servaind.com";
    public const string EmailResponsableCodifArt = "paulo.velardes@servaind.com";
    public const string EmailResponsableHerramientas = "paulo.velardes@servaind.com";
    public const string EmailResponsableIIO = "paulo.velardes@servaind.com";
    public const string EmailResponsablesVehiculos = "paulo.velardes@servaind.com";
    public const string EmailAlertaStock = "paulo.velardes@servaind.com";
    public const string EmailBedelia = "paulo.velardes@servaind.com";
    public const string EmailAsistComercial = "paulo.velardes@servaind.com";
    public const string EmailAdministracion = "paulo.velardes@servaind.com";
	*/

    #endregion
    
    #endregion

    public const string EmailServer = "10.0.0.10";
	public const string EmailUser = "dominio\\intranet";
	public const string EmailPwd = "gador.1";

    #region Paths

    public const string PATH_SISSER =
    //    @"D:\Documentos\Desarrollos\Servaind S.A\Web\intra.servaind.com\";
        @"C:\Inetpub\wwwroot\intra.servaind.com\";
    public const string PATH_TEMP = PATH_SISSER +
        @"temp\";
	public const string PATH_PLANTILLA_NC = PATH_SISSER + 
		@"plantillas\NoConformidad.htm";
    public const string PATH_PLANTILLA_LICENCIA = PATH_SISSER +
        @"plantillas\Licencia.htm";
	public const string PATH_PLANTILLA_SV = PATH_SISSER + 
		@"plantillas\SV_Plantilla.htm";
	public const string PATH_PLANTILLA_SV_EMAIL = PATH_SISSER +
		@"plantillas\SolicitudViaje.htm";
	public const string PATH_PLANTILLA_VDM_EMAIL = PATH_SISSER +
		@"plantillas\ValeDeMateriales.htm";
    public const string PATH_RDA = PATH_SISSER +
        @"repositorio\";
	public const string PATH_PLANTILLA_PD_RECORDATORIO = PATH_SISSER +
        @"plantillas\RecordatorioParteDiario.htm";
    public const string PATH_PLANTILLA_SEM_EMAIL = PATH_SISSER +
        @"plantillas\SEM_Email.htm";
    public const string PATH_PLANTILLA_SEM_EMAIL_ITEM = PATH_SISSER +
        @"plantillas\SEM_Email_item.htm";
    public const string PATH_PLANTILLA_SEM_REM_NO_OF_XLS = PATH_SISSER +
        @"plantillas\SEM_RemitoNoOficial.xls";
    public const string PATH_PLANTILLA_SEM_REM_OF_XLS = PATH_SISSER +
        @"plantillas\SEM_RemitoOficial.xls";
    public const string PATH_PLANTILLA_COTIZACION = PATH_SISSER +
        @"plantillas\Cotizacion.xls";
	public const string PATH_PLANTILLA_LISTADOVENTAS = PATH_SISSER +
        @"plantillas\ListadoVentas.xls";
    public const string PATH_PLANTILLA_RECORDATORIO_EVENTO = PATH_SISSER +
        @"plantillas\RecordatorioEvento.htm";
    public const string PATH_PLANTILLA_CODIF_ART = PATH_SISSER +
        @"plantillas\CodificacionArticulo.htm";
    public const string PATH_PLANTILLA_IIO = PATH_SISSER +
        @"plantillas\InformacionObra.htm";
    public const string PATH_PLANTILLA_IIO_EMAIL = PATH_SISSER +
        @"plantillas\InformacionObraEmail.htm";
    public const string PATH_PLANTILLA_SSM_RECORDATORIO = PATH_SISSER +
        @"plantillas\RecordatorioSitioSSM.htm";
    public const string PATH_PLANTILLA_ALERTA_VENCIMIENTO = PATH_SISSER +
        @"plantillas\AlertaVencimiento.htm";
    public const string PATH_PLANTILLA_ALERTA_IIO = PATH_SISSER +
        @"plantillas\AlertaInformacionObra.htm";
    public const string PATH_PLANTILLA_AUTORIZACION = PATH_SISSER +
        @"plantillas\Autorizacion.htm";
    public const string PATH_PLANTILLA_ALERTA_STOCK = PATH_SISSER +
        @"plantillas\AlertaStock.htm";
    public const string PATH_PLANTILLA_ALERTA_INSTRUMENTOS = PATH_SISSER +
        @"plantillas\AlertaInstrumento.htm";
    public const string PATH_PLANTILLA_NOTIF_VENTA = PATH_SISSER +
        @"plantillas\NotifVenta.htm";
    public const string PATH_PLANTILLA_NOTIF_VENTA_IMP = PATH_SISSER +
        @"plantillas\NotifVentaImputacion.htm";
    public const string PATH_PLANTILLA_NOTIF_VENTA_CLI = PATH_SISSER +
        @"plantillas\NotifVentaCliente.htm";
    public const string PATH_PLANTILLA_NOTIF_VENTA_REC = PATH_SISSER +
        @"plantillas\NotifVentaRecordatorio.htm";
    public const string PATH_PLANTILLA_PROP_CAMBIO = PATH_SISSER +
        @"plantillas\PropCambio.htm";
		
    // URLs
    public const string UrlIntraDefault = "/";
    public const string UrlServaind = "http://www.servaind.com/";
    public const string UrlIntranet = "http://intra.servaind.com/";
    //public const string UrlIntranet = "http://localhost:49884/";

    #endregion

    #endregion

    #region Propiedades

    /// <summary>
    /// Obtiene el usuario que se encuentra logueado en el sistema.
    /// </summary>
    public static Persona Usuario
    {
        get
        {
            return (Persona)GSessions.GetSession(PrefSession + "Usuario");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "Usuario", value);
        }
    }
    /// <summary>
    /// Obtiene o establece si el usuario actual es público.
    /// </summary>
    public static bool EsUsuarioPublico
    {
        get
        {
            return (bool)GSessions.GetSession(PrefSession + "EsUsuarioPublico");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "EsUsuarioPublico", value);
        }
    }
    /// <summary>
    /// Obtiene una fecha inválida.
    /// </summary>
    public static DateTime FechaInvalida
    {
        get
        {
            return new DateTime(2000, 01, 01, 0, 0, 0);
        }
    }
    /// <summary>
    /// Obtiene los repositorios de cada usuario público.
    /// </summary>
    public static Dictionary<int, RepositoriosArchivos[]> RepositoriosPublicos
    {
        get
        {
            Dictionary<int, RepositoriosArchivos[]> result = new Dictionary<int, RepositoriosArchivos[]>();

            // Petrobras.
            result.Add(202, new RepositoriosArchivos[] { RepositoriosArchivos.Petrobras });

            return result;
        }
    }

    #endregion

    #region Metodos

    /// <summary>
    /// Inicializa el usuario del sistema.
    /// </summary>
    public static bool InicializarUsuario(System.Web.HttpRequest request)
    {
        bool result;

        result = GSessions.CrearSession(PrefSession + "Usuario", ValidacionUsuario.IniciarUsuario(request));

        return result;
    }

    #endregion
}
