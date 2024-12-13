using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Encriptacion.GetURLEncriptada("/rrhh/licenciaAdmin.aspx", "id=" + Request.QueryString["id"]));
        //GSolicitudesViaje.EnviarSolViaje(16);
        //Response.Write(Encriptacion.GetParametroEncriptado("t=" + (int)TipoListadoHerramientas.Instrumentos));
        //Vehiculos.GetAlertasVencimientos();
        //Vehiculos.ProcesarVencimientos();
        //IntranetServaind srv = new IntranetServaind();
        //srv.ControlAlertasVencimientosVehiculos("spam", "gador.1");

        //InformacionObras.ProcesarAlertasInformacionObra();

        //Dictionary<string, List<ItemVencimiento>> vencimientos = Vehiculos.GetVencimientosMes(08, 2012);
        //Vehiculos.InformarVencimientosMes();
        //Vehiculos.ExportarVehiculos();
        //Vehiculos.GetAlertasVencimientos();

        //DateTime dt = new DateTime(2012, 06, 25, 0, 0, 0);

        //Constantes.Usuario = GPersonal.GetPersona("sebastian.bondar");

        //for (int i = 0; i < 23; i++)
        //{
        //    dt = dt.AddDays(i);
        //    if (dt.Day != 30 && dt.Day != 1 && dt.Day != 7 && dt.Day != 8 && dt.Day != 14
        //        && dt.Day != 15)
        //    {
        //        ParteDiario pd = GPartesDiarios.CrearParteDiario(dt);

        //        List<ImputacionParteDiario> imputaciones = new List<ImputacionParteDiario>();

        //        imputaciones.Add(new ImputacionParteDiario(8, GImputaciones.GetImputacion(336), ".", "", "", "", new List<PersonaInterviene>()));

        //        GPartesDiarios.GuardarParteDiario(pd.ID, imputaciones, true);
        //    }
        //}

        //Response.Write("index.aspx?" + Request.QueryString.ToString());
        //DateTime p = DateTime.Now.AddDays(60);
        //List<GrupoInstrumentos> grupos = Instrumentos.GetGruposInstrumentos();
        //Instrumentos.EnviarAlertasVencimientos();
        //try
        //{
        //    Funciones.ActualizarPagina();
        //}
        //catch(Exception ex)
        //{
        //    throw new Exception(ex.Message);
        //}
        //GLicencias.EnviarLicencia(62971);

        //Response.Write(Encriptacion.GetURLEncriptada("/rrhh/licenciaAdmin.aspx", "id=" + 60303));

        //AsistenciaFac.GetCargaAsistencia(75, DateTime.Now);

        //BaseFac.AddBase("Base prueba", 89, new List<int>() {75});

        //AsistenciaPanelControlFac.GetPanelesControl(DateTime.Now);

        GLicencias.EnviarLicencia(81704);

        //Instrumentos.GetInstrumentosProxVencer();

        //Response.Write(NotifVentas.GetDescripcionImputacion(Request["imp"]));

        /*List<DataSourceItem> p = GPersonal.GetPersonas(PermisosPersona.SNV_NotifRMA);
        p.ForEach(pe => Response.Write(pe.TextField + "<br/>"));*/

        //NotifVentas.SendNotifVenta(83);

        //Response.Write((int)PermisosPersona.GEN_CargaParteDiario);
    }
}