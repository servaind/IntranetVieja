using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for IntranetServaind
/// </summary>
[WebService(Namespace = "http://intra.servaind.com/public/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class IntranetServaind : System.Web.Services.WebService 
{
    public IntranetServaind()
    {

    }

    /// <summary>
    /// Ejecuta los controles sobre los datos de la intranet.
    /// </summary>
    [WebMethod()]
    public void EjecutarControles(string usr, string pwd)
    {
        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }

        try
        {
            GHerramientas.ControlarEventosHerramientas();
        }
        catch(Exception ex)
        {
            Funciones.Log("WebService error: " + ex.Message);
        }
    }
    /// <summary>
    /// Controla la carga de partes diarios.
    /// </summary>
    [WebMethod()]
    public void ControlCargaPD(string usr, string pwd, DateTime desde, DateTime hasta)
    {
        if (desde > hasta)
        {
            throw new DatosInvalidosException();
        }

        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }

        GPartesDiarios.ControlarCargaPartesDiarios(desde, hasta);
    }
    /// <summary>
    /// Controla las alertas para los vencimientos de los vehículos.
    /// </summary>
    [WebMethod()]
    public void ControlAlertasVencimientosVehiculos(string usr, string pwd)
    {
        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }

        //Vehiculos.ProcesarVencimientos();
    }
    /// <summary>
    /// Controla las alertas para las informaciónes de obra.
    /// </summary>
    [WebMethod()]
    public void ControlAlertasInformacionObra(string usr, string pwd)
    {
        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }

        InformacionObras.ProcesarAlertasInformacionObra();
    }
    /// <summary>
    /// Envía un detalle de los vencimientos de vehiculos.
    /// </summary>
    [WebMethod()]
    public void EnviarVencimientosVehiculos(string usr, string pwd)
    {
        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }
        
        Vehiculos.InformarVencimientosMes();
    }
    /// <summary>
    /// Controla las alertas para los vencimientos de los instrumentos.
    /// </summary>
    [WebMethod()]
    public void ControlInstrumentos(string usr, string pwd)
    {
        if (!ValidacionUsuario.EsUsuarioValido(usr, pwd))
        {
            throw new LoginException();
        }

        Instrumentos.EnviarAlertasVencimientos();
    }
}
