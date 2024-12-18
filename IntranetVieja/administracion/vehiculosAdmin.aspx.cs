﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class administracion_vehiculosAdmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.VehicAdmin))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }
		
		if (String.IsNullOrWhiteSpace(Request["old"]))
		{
			Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
		}
    }
    /// <summary>
    /// Obtiene los vehículos.
    /// </summary>
    [WebMethod()]
    public static object[][] GetVehiculos()
    {
        List<object[]> result = new List<object[]>();

        List<Vehiculo> vehiculos = Vehiculos.GetVehiculos();
        foreach (Vehiculo v in vehiculos)
        {
            object[] fila = new object[] { 
                Encriptacion.GetParametroEncriptado("id=" + v.IdVehiculo.ToString()),
                v.Patente, v.Modelo, v.TipoVehiculo.IdTipoVehiculo, v.Anio, v.Ubicacion, v.IdResponsable,
                v.AfectadoGasmed ? 1 : 0, 
                v.VtoCedulaVerde != Constantes.FechaInvalida ? v.VtoCedulaVerde.ToShortDateString() : Vehiculos.NoAplica, 
                v.NroRUTA, 
                v.VtoRUTA != Constantes.FechaInvalida ? v.VtoRUTA.ToShortDateString() : Vehiculos.NoAplica, 
                v.VtoVTV != Constantes.FechaInvalida ? v.VtoVTV.ToShortDateString() : Vehiculos.NoAplica, 
                v.VtoPatente != Constantes.FechaInvalida ? v.VtoPatente.ToShortDateString() : Vehiculos.NoAplica, 
                v.CiaSeguro, 
                v.PolizaSeguro,
                v.VtoSeguro != Constantes.FechaInvalida ? v.VtoSeguro.ToShortDateString() : Vehiculos.NoAplica, 
                v.NroChasis, 
                v.NroMotor,
                v.VtoStaCruz != Constantes.FechaInvalida ? v.VtoStaCruz.ToShortDateString() : Vehiculos.NoAplica, 
            };

            result.Add(fila);
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene las personas disponibles.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPersonas()
    {
        List<object[]> result = new List<object[]>();

        List<Persona> personas = GPersonal.GetPersonasActivas();
        foreach (Persona persona in personas)
        {
            result.Add(new object[] { persona.ID, persona.Nombre });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene los tipos de vehículo.
    /// </summary>
    [WebMethod()]
    public static object[][] GetTipos()
    {
        List<object[]> result = new List<object[]>();

        List<TipoVehiculo> tipos = TiposVehiculo.GetTiposVehiculo();
        foreach (TipoVehiculo t in tipos)
        {
            result.Add(new object[] { t.IdTipoVehiculo, t.Descripcion });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Actualiza los vehículos.
    /// </summary>
    [WebMethod()]
    public static string ActualizarVehiculos(object[][] datos)
    {
        string result = "";

        try
        {
            int c = datos.Length;
            for (int i = 0; i < c; i++)
            {
                Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(datos[i][0].ToString().Replace("'", ""));
                datos[i][0] = parametros["id"];
            }

            Vehiculos.ActualizarVehiculos(datos);

            result = "Los cambios se han guardado de forma correcta.";
        }
        catch
        {
            throw new Exception("Se produjo un error al intentar completar la operación. Por favor, contáctese con el Área de Sistemas.");
        }

        return result;
    }
    /// <summary>
    /// Exporta los vehiculos.
    /// </summary>
    [WebMethod()]
    public static string ExportarVehiculos()
    {
        string result;

        try
        {
            string path = Vehiculos.ExportarVehiculos();
            result = Encriptacion.GetURLEncriptada("download.aspx", "f=" + path + "&n=Vehiculos.xls&d=1");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result;
    }
}