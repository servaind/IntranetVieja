using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using ClosedXML.Excel;
using System.Configuration;

public class NotifVenta
{
    // Variables.
    private int notifVentaID;
    private TipoNotifVenta tipoVenta;
	private bool calibracionExterna;
    private string laboratorioExterno;	
    private int vendedorID;
    private string vendedor;
    private string vendedorEmail;
    private string cliente;
    private string oc;
    private string imputacion;
    private DateTime fechaOC;
    private Moneda moneda;
    private decimal montoOC;
	private string fechaEntrega;
	private string datosEnvio;
    private string observaciones;
    private int factura;
    private int remito;
    private string remitoDesc;
    private EstadoNotifVenta estado;
    private string fileITR;
    private decimal remitoMonto;
    private string remitoDestino;
    private string remitoEntrega;
    private string remitoContacto;
    private string remitoTransporte;

    // Propiedades.
    public int ID
    {
        get { return notifVentaID; }
    }
    public int VendedorID
    {
        get { return vendedorID; }
    }
    public string Vendedor
    {
        get { return vendedor; }
    }
    public string VendedorEmail
    {
        get { return vendedorEmail; }
    }
    public string Cliente
    {
        get { return cliente; }
    }
    public string OC
    {
        get { return oc; }
    }
    public string Imputacion
    {
        get { return imputacion; }
    }
    public DateTime FechaOC
    {
        get { return fechaOC; }
    }
    public Moneda Moneda
    {
        get { return moneda; }
    }
    public decimal MontoOC
    {
        get { return montoOC; }
    }
	
	public string FechaEntrega
    {
        get { return fechaEntrega; }
    }
	
	public string DatosEnvio
    {
        get { return datosEnvio; }
    }
	
    public string Observaciones
    {
        get { return observaciones; }
    }
    public int Factura
    {
        get { return factura; }
    }
    public int Remito
    {
        get { return remito; }
    }
    public string RemitoDesc
    {
        get { return remitoDesc; }
    }
    public EstadoNotifVenta Estado
    {
        get { return estado; }
    }
    public string FileITR
    {
        get { return fileITR; }
    }
    public bool LlevaITR
    {
        get { return TipoVenta == TipoNotifVenta.Servicio; }
    }
    public bool TieneItr
    {
        get { return !String.IsNullOrEmpty(fileITR); }
    }
    public string RemitoDestino
    {
        get { return remitoDestino; }
    }
    public string RemitoContacto
    {
        get { return remitoContacto; }
    }
    public string RemitoTransporte
    {
        get { return remitoTransporte; }
    }
    public string RemitoEntrega
    {
        get { return remitoEntrega; }
    }
    public TipoNotifVenta TipoVenta
    {
        get { return tipoVenta; }
    }
	public bool CalibracionExterna
	{
		get { return calibracionExterna; }
	}
	public string LaboratorioExterno
	{
		get { return laboratorioExterno; }
	}
    public bool LlevaFileOC
    {
        get { return TipoVenta != TipoNotifVenta.RMA; }
    }
    public bool TieneFileOC
    {
        get { return !String.IsNullOrWhiteSpace(NotifVentas.GetFileOC(Imputacion)); }
    }
    public decimal RemitoMonto
    {
        get { return remitoMonto; }
    }


    internal NotifVenta(int notifVentaID, TipoNotifVenta tipoVenta, int vendedorID, string vendedor, string vendedorEmail,
                        string cliente, string oc, string imputacion, DateTime fechaOC, Moneda moneda, decimal montoOC, 
                        string fechaEntrega, string datosEnvio,
						string observaciones, int factura, int remito, decimal remitoMonto,
                        string remitoDesc, EstadoNotifVenta estado, string fileITR, string remitoDestino,
                        string remitoEntrega, string remitoContacto, string remitoTransporte, bool calibracionExterna = false, string laboratorioExterno = "")
    {
        this.notifVentaID = notifVentaID;
        this.tipoVenta = tipoVenta;
		this.calibracionExterna = calibracionExterna;
		this.laboratorioExterno = laboratorioExterno;
        this.vendedorID = vendedorID;
        this.vendedor = vendedor;
        this.vendedorEmail = vendedorEmail;
        this.cliente = cliente;
        this.oc = oc;
        this.imputacion = imputacion;
        this.fechaOC = fechaOC;
        this.moneda = moneda;
        this.montoOC = montoOC;
		this.fechaEntrega = fechaEntrega;
		this.datosEnvio = datosEnvio;
        this.observaciones = observaciones;
        this.factura = factura;
        this.remito = remito;
        this.remitoMonto = remitoMonto;
        this.remitoDesc = remitoDesc;
        this.estado = estado;
        this.fileITR = fileITR;
        this.remitoDestino = remitoDestino;
        this.remitoEntrega = remitoEntrega;
        this.remitoContacto = remitoContacto;
        this.remitoTransporte = remitoTransporte;
    }

    public string TipoVentaToString()
    {
        return NotifVentas.TipoVentaToString(TipoVenta);
    }

    public string MonedaToString()
    {
        return NotifVentas.MonedaToString(moneda);
    }

    public string MontoOCToString()
    {
        return String.Format("{0} {1:0.00}", NotifVentas.MonedaToString(moneda), montoOC);
    }
    
    public string FacturaToString()
    {
        return factura == Constantes.ValorInvalido ? "-" : factura.ToString();
    }

    public string RemitoToString()
    {
        return remito == Constantes.ValorInvalido ? "-" : remito.ToString();
    }

    public string RemitoMontoOCToString()
    {
        return remitoMonto == Constantes.ValorInvalido
                   ? "-"
                   : String.Format("{0} {1:0.00}", NotifVentas.MonedaToString(moneda), remitoMonto);
    }

    public bool AbleToVendedor()
    {
        bool result = Estado == EstadoNotifVenta.CargandoDatos && GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_Vendedor);

        return result;
    }

    public bool AbleToCargaRemito()
    {
        bool result = Estado == EstadoNotifVenta.CargandoRemito && GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_AltaTransporte);

        return result;
    }

    public bool AbleToAprobar()
    {
        bool result = Estado == EstadoNotifVenta.EsperandoAprobacion && GPermisosPersonal.TieneAcceso(PermisosPersona.RolGerencia);

        return result;
    }

    public bool AbleToRem()
    {
        bool result = Estado == EstadoNotifVenta.ConfeccionRem && Remito == Constantes.ValorInvalido &&
                      GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_AltaFacRem);

        return result;
    }

    public bool AbleToFac()
    {
        bool result = Estado == EstadoNotifVenta.ConfeccionFac && Factura == Constantes.ValorInvalido &&
                      GPermisosPersonal.TieneAcceso(PermisosPersona.SNV_AltaFacRem);

        return result;
    }

    internal bool AbleToCerrar()
    {
        bool result = Estado == EstadoNotifVenta.EsperandoITR ||
                      (Estado == EstadoNotifVenta.ConfeccionFac && TipoVenta != TipoNotifVenta.Servicio);

        return result;
    }
}

public class NotifVentaResumen
{
    // Variables.
    private int notifVentaID;
    private string vendedor;
    private string cliente;
    private string oc;
    private string imputacion;
    private string fechaOC;
    private string factura;
    private string remito;
    private string estado;
	private bool calibracionExterna;
	private string laboratorioExterno;

    // Propiedades.
    public string Vendedor
    {
        get { return vendedor; }
    }
    public string Cliente
    {
        get { return cliente; }
    }
    public string OC
    {
        get { return oc; }
    }
    public string Imputacion
    {
        get { return imputacion; }
    }
    public string FechaOC
    {
        get { return fechaOC; }
    }
    public string Factura
    {
        get { return factura; }
    }
    public string Remito
    {
        get { return remito; }
    }
    public string Estado
    {
        get { return estado; }
    }
    public string ID
    {
        get { return Encriptacion.GetParametroEncriptado("id=" + notifVentaID); }
    }
    public int Numero
    {
        get { return notifVentaID; }
    }

	public bool CalibracionExterna
    {
        get { return calibracionExterna; }
    }

    public string LaboratorioExterno
    {
        get { return laboratorioExterno; }
    }



    internal NotifVentaResumen(int notifVentaID, string vendedor, string cliente, string oc, string imputacion,
                        DateTime fechaOC, string factura, string remito, EstadoNotifVenta estado)
    {
        this.notifVentaID = notifVentaID;
        this.vendedor = vendedor;
        this.cliente = cliente;
        this.oc = oc;
        this.imputacion = imputacion;
        this.fechaOC = fechaOC.ToShortDateString();
        this.factura = factura;
        this.remito = remito;
        this.estado = NotifVentas.EstadoToString(estado);
    }
}

public class NotifVentaResumenExcel
{
    // Variables.
    private int notifVentaID;
    private string vendedor;
    private string cliente;
    private string oc;
    private string imputacion;
    private string fechaOC;
	private string moneda;
	private string monto;
    private string estado;
	private bool calibracionExterna;
    private string laboratorioExterno;

    // Propiedades.
    public string Vendedor
    {
        get { return vendedor; }
    }
    public string Cliente
    {
        get { return cliente; }
    }
    public string OC
    {
        get { return oc; }
    }
    public string Imputacion
    {
        get { return imputacion; }
    }
    public string FechaOC
    {
        get { return fechaOC; }
    }
    public string Moneda
    {
        get { return moneda; }
    }
    public string Monto
    {
        get { return monto; }
    }
    public string Estado
    {
        get { return estado; }
    }
    public int Numero
    {
        get { return notifVentaID; }
    }


    internal NotifVentaResumenExcel(int notifVentaID, string vendedor, string cliente, string oc, string imputacion,
                        DateTime fechaOC, Moneda moneda, string monto, EstadoNotifVenta estado, bool calibExterna = false, string laboratorio = "")
    {
        this.notifVentaID = notifVentaID;
        this.vendedor = vendedor;
        this.cliente = cliente;
        this.oc = oc;
        this.imputacion = imputacion;
        this.fechaOC = fechaOC.ToString("dd/MM/yyyy");//ShortDateString();
        this.moneda = NotifVentas.MonedaToString(moneda);
        this.monto = monto;
        this.estado = NotifVentas.EstadoToString(estado, true);
		this.calibracionExterna = calibExterna;
		this.laboratorioExterno = laboratorio;
    }
}

/// <summary>
/// Descripción breve de NotifVentas
/// </summary>
public static class NotifVentas
{
    // Constantes.
    private const int MaxRegistrosPagina = 20;


    public static int AddNotifVenta(int vendedorID, TipoNotifVenta tipoVenta, string cliente, string oc,
                                    string imputacion, Moneda moneda, decimal montoOC, string fechaEntrega, string datosEnvio, string observaciones, 
                                    decimal remitoMonto, string remitoDesc, bool calibExterna = false, string laboratorio = "")
    {
        int result;
        IDbConnection conn = null;

        if (String.IsNullOrEmpty(cliente) || String.IsNullOrEmpty(oc) || String.IsNullOrEmpty(imputacion) ||
		    String.IsNullOrEmpty(fechaEntrega) || String.IsNullOrEmpty(datosEnvio) ||
            (montoOC <= 0 && tipoVenta != TipoNotifVenta.RemitoOficial && tipoVenta != TipoNotifVenta.RemitoInterno) ||
            (remitoMonto <= 0 && tipoVenta != TipoNotifVenta.RemitoOficial && tipoVenta != TipoNotifVenta.RemitoInterno))
        {
            throw new DatosInvalidosException();
        }

        string[] auxImp = imputacion.Split('-');
        int c = auxImp.Length;
        try
        {
            int numero = Convert.ToInt32(auxImp[0]);
            string desc = "";
            for (int i = 1; i < c; i++) desc += (i == 1 ? "- " : "-") + auxImp[i].Trim();

            imputacion = String.Format("{0} {1}", numero, desc.Trim());
        }
        catch
        {
            throw new Exception("La imputación no tiene un formato válido.");
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_NotifVentas (VendedorID, TipoVentaID, Cliente, OC, Imputacion, FechaOC, ";
            cmd.CommandText += "MonedaID, MontoOC, FechaEntrega, DatosEnvio, Observaciones, Factura, Remito, RemitoMonto, RemitoDesc, EstadoID, CalibExterna, LabExterno) VALUES ";
            cmd.CommandText += "(@VendedorID, @TipoVentaID,@Cliente, @OC, @Imputacion, @FechaOC, @MonedaID, @MontoOC, ";
            cmd.CommandText += "@FechaEntrega, @DatosEnvio, @Observaciones, @Factura, @Remito, @RemitoMonto, @RemitoDesc, @EstadoID, @CalibExterna, @LabExterno); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_NotifVentas;";
            
			cmd.Parameters.Add(DataAccess.GetDataParameter("@VendedorID", vendedorID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoVentaID", (int) tipoVenta));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Cliente", cliente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@OC", oc));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Imputacion", imputacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaOC", Funciones.GetDate(DateTime.Now)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@MonedaID", (int) moneda));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@MontoOC", montoOC));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaEntrega", fechaEntrega));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@DatosEnvio", Funciones.ReemplazarEnters(datosEnvio)));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", Funciones.ReemplazarEnters(observaciones)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Factura", Constantes.ValorInvalido));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Remito", Constantes.ValorInvalido));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoMonto", remitoMonto));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoDesc", Funciones.ReemplazarEnters(remitoDesc)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int) EstadoNotifVenta.CargandoDatos));
			int calExt = (calibExterna) ? 1 : 0;
			cmd.Parameters.Add(DataAccess.GetDataParameter("@CalibExterna", calExt ));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@LabExterno", laboratorio));
			
            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch(Exception e)
        {
            throw new Exception(e.Message + e.StackTrace); //ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(result);

        return result;
    }

    public static void UpdateNotifVentaRemitoDesc(int notifVentaID, string observaciones, string remitoDesc, bool confirmar)
    {
        IDbConnection conn = null;

        if (confirmar && String.IsNullOrEmpty(remitoDesc))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET Observaciones = @Observaciones, RemitoDesc = @RemitoDesc ";
            if (confirmar)
            {
                cmd.CommandText += ", EstadoID = @EstadoID ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)EstadoNotifVenta.CargandoRemito));
            }
            cmd.CommandText += "WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", Funciones.ReemplazarEnters(observaciones)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoDesc", Funciones.ReemplazarEnters(remitoDesc)));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        if (confirmar) SendNotifVenta(notifVentaID);
    }

    public static void UpdateNotifVenta(int notifVentaID, string observaciones, string remitoDestino, string remitoContacto, 
                                        string remitoEntrega, string remitoDesc, bool confirmar)
    {
        IDbConnection conn = null;

        if (confirmar &&
            (String.IsNullOrEmpty(remitoDestino) || String.IsNullOrEmpty(remitoContacto) ||
             String.IsNullOrEmpty(remitoEntrega) || String.IsNullOrEmpty(remitoDesc)))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET Observaciones = @Observaciones, RemitoDestino = @RemitoDestino, ";
            cmd.CommandText += "RemitoContacto = @RemitoContacto, RemitoEntrega = @RemitoEntrega, ";
            if (confirmar)
            {
                cmd.CommandText += "EstadoID = @EstadoID, ";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)EstadoNotifVenta.CargandoRemito));
            }
            cmd.CommandText += "RemitoDesc = @RemitoDesc WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observaciones", Funciones.ReemplazarEnters(observaciones)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoDestino", remitoDestino));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoContacto", remitoContacto));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoEntrega", remitoEntrega));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoDesc", Funciones.ReemplazarEnters(remitoDesc)));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        if(confirmar) SendNotifVenta(notifVentaID);
    }

    public static void UpdateNotifVenta(int notifVentaID, string remitoTransporte)
    {
        IDbConnection conn = null;

        if (String.IsNullOrEmpty(remitoTransporte))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET RemitoTransporte = @RemitoTransporte, EstadoID = @EstadoID ";
            cmd.CommandText += "WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RemitoTransporte", remitoTransporte));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)EstadoNotifVenta.EsperandoAprobacion));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(notifVentaID);
    }

    public static void UpdateNotifVenta(int notifVentaID, bool aprobar, string motivo)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET EstadoID = @EstadoID ";
            cmd.CommandText += "WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID",
                                                           aprobar
                                                               ? (int) EstadoNotifVenta.ConfeccionRem
                                                               : (int) EstadoNotifVenta.Rechazada));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(notifVentaID, motivo);
    }

    private static void UpdateNotifVenta(int notifVentaID, EstadoNotifVenta estado)
    {
        IDbConnection conn = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET EstadoID = @EstadoID ";
            cmd.CommandText += "WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int) estado));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    internal static void UpdateNotifVenta(string imputacion, string fileITR)
    {
        IDbConnection conn = null;
        int notifVentaID = Constantes.ValorInvalido;

        if (String.IsNullOrEmpty(fileITR))
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET FileITR = @FileITR ";
            cmd.CommandText += "WHERE Imputacion = @Imputacion; ";
            cmd.CommandText += "SELECT NotifVentaID FROM tbl_NotifVentas WHERE Imputacion = @Imputacion;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FileITR", fileITR));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Imputacion", imputacion));

            notifVentaID = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        if (notifVentaID != Constantes.ValorInvalido)
        {
            try
            {
                NotifVenta nv = GetNotifVenta(notifVentaID);
                if (nv != null && nv.AbleToCerrar()) CerrarNotifVenta(notifVentaID); 
            }
            catch
            {

            }
        }
    }

    public static void UpdateNotifVentaRem(int notifVentaID, int remito)
    {
        IDbConnection conn = null;

        if (remito <= 0 && remito != Constantes.ValorInvalido)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET Remito = @Remito, ";
            cmd.CommandText += "EstadoID = @EstadoID WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Remito", remito));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)EstadoNotifVenta.ConfeccionFac));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(notifVentaID);
    }

    public static void UpdateNotifVentaFac(int notifVentaID, int factura)
    {
        IDbConnection conn = null;

        if (factura <= 0 && factura != Constantes.ValorInvalido)
        {
            throw new DatosInvalidosException();
        }

        NotifVenta notifVenta = GetNotifVenta(notifVentaID);
        if (notifVenta == null) throw new ElementoInexistenteException();
        bool cerrar = notifVenta.AbleToCerrar();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET Factura = @Factura, ";
            cmd.CommandText += "EstadoID = @EstadoID WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Factura", factura));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", cerrar
                                                               ? (int)EstadoNotifVenta.Cerrada
                                                               : (int)EstadoNotifVenta.EsperandoITR));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(notifVentaID);
    }

    public static void CerrarNotifVenta(int notifVentaID)
    {
        IDbConnection conn = null;

        NotifVenta nv = GetNotifVenta(notifVentaID);
        if(nv == null || !nv.AbleToCerrar()) throw new DatosInvalidosException();

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NotifVentas SET EstadoID = @EstadoID ";
            cmd.CommandText += "WHERE NotifVentaID = @NotifVentaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)EstadoNotifVenta.Cerrada));

            cmd.ExecuteNonQuery();
        }
        catch
        {
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        SendNotifVenta(notifVentaID);
    }

    private static NotifVenta GetNotifVenta(IDataReader dr)
    {
        NotifVenta result;

        try
        {
			bool calibExterna = dr["CalibExterna"].ToString() == "1";
			
            result = new NotifVenta(Convert.ToInt32(dr["NotifVentaID"]), (TipoNotifVenta)Convert.ToInt32(dr["TipoVentaID"]),
                                    Convert.ToInt32(dr["VendedorID"]),
                                    dr["Vendedor"].ToString(), dr["VendedorEmail"].ToString(), dr["Cliente"].ToString(),
                                    dr["OC"].ToString(), dr["Imputacion"].ToString(), Convert.ToDateTime(dr["FechaOC"]),
                                    (Moneda) Convert.ToInt32(dr["MonedaID"]), Convert.ToDecimal(dr["MontoOC"]),
                                    					
									dr["FechaEntrega"].ToString(),
									dr["DatosEnvio"].ToString(),
									
									dr["Observaciones"].ToString(),
                                    Convert.ToInt32(dr["Factura"]), Convert.ToInt32(dr["Remito"]),
                                    Convert.ToDecimal(dr["RemitoMonto"]),
                                    dr["RemitoDesc"].ToString(), (EstadoNotifVenta) Convert.ToInt32(dr["EstadoID"]),
                                    dr["FileITR"].ToString(), dr["RemitoDestino"].ToString(),
                                    dr["RemitoEntrega"].ToString(),
                                    dr["RemitoContacto"].ToString(), dr["RemitoTransporte"].ToString(), calibExterna, dr["LabExterno"].ToString());
        }
        catch
        {
            result = null;
        }

        return result;
    }

    public static NotifVenta GetNotifVenta(int notifVentaID)
    {
        NotifVenta result;
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT nv.*, p.idPersonal AS VendedorID, p.Nombre AS Vendedor, p.Email AS VendedorEmail ";
            cmd.CommandText += "FROM tbl_NotifVentas nv ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = nv.VendedorID ";
            cmd.CommandText += "WHERE nv.NotifVentaID = @NotifVentaID;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            dr = cmd.ExecuteReader();

            dr.Read();
            result = GetNotifVenta(dr);
            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }

    public static NotifVenta GetNotifVenta(string oc)
    {
        NotifVenta result;
        IDbConnection conn = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 nv.*, p.idPersonal AS VendedorID, p.Nombre AS Vendedor, p.Email AS VendedorEmail ";
            cmd.CommandText += "FROM tbl_NotifVentas nv ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = nv.VendedorID ";
            cmd.CommandText += "WHERE nv.OC = @OC;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@OC", oc));
            dr = cmd.ExecuteReader();

            dr.Read();
            result = GetNotifVenta(dr);
            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }

    private static NotifVentaResumen GetNotifVentaResumen(DataRow dr)
    {
        NotifVentaResumen result;

        try
        {
            int factura = Convert.ToInt32(dr["Factura"]);
            int remito = Convert.ToInt32(dr["Remito"]);

            result = new NotifVentaResumen(Convert.ToInt32(dr["NotifVentaID"]), dr["Vendedor"].ToString(),
                                           dr["Cliente"].ToString(), dr["OC"].ToString(), dr["Imputacion"].ToString(),
                                           Convert.ToDateTime(dr["FechaOC"]), 
                                           factura == Constantes.ValorInvalido ? "-" : factura.ToString(),
                                           remito == Constantes.ValorInvalido ? "-" : remito.ToString(),
                                           (EstadoNotifVenta) Convert.ToInt32(dr["EstadoID"]));
        }
        catch
        {
            result = null;
        }

        return result;
    }

    private static NotifVentaResumenExcel GetNotifVentaResumenExcel(DataRow dr)
    {
        NotifVentaResumenExcel result;

        try
        {
           
            result = new NotifVentaResumenExcel(Convert.ToInt32(dr["NotifVentaID"]), dr["Vendedor"].ToString(),
                                                dr["Cliente"].ToString(), dr["OC"].ToString(), dr["Imputacion"].ToString(),
                                                Convert.ToDateTime(dr["FechaOC"]), 
                                                (Moneda) Convert.ToInt32(dr["MonedaID"]),
                                                 dr["MontoOC"].ToString(),
                                                (EstadoNotifVenta) Convert.ToInt32(dr["EstadoID"]));
												
        }
        catch
        {
            result = null;
        }

        return result;
    }

    public static string MonedaToString(Moneda moneda)
    {
        string result;

        switch (moneda)
        {
            case Moneda.Peso:
                result = "$";
                break;
            case Moneda.Dolar:
                result = "U$S";
                break;
            case Moneda.Euro:
                result = "€";
                break;
            case Moneda.Real:
                result = "R$";
                break;
            default:
                result = "N/D";
                break;
        }

        return result;
    }

    public static string EstadoToString(EstadoNotifVenta estado, bool cap = false)
    {
        string result = String.Empty;

        switch (estado)
        {
            case EstadoNotifVenta.Cerrada:
                result = cap ? "Cerrada" : "cerrada";
                break;
            default:
                result = cap ? "Abierta" : "abierta";
                break;
        }

        return result;
    }

    public static string TipoVentaToString(TipoNotifVenta tipoVenta)
    {
        string result;

        switch (tipoVenta)
        {
            case TipoNotifVenta.Producto:
                result = "Producto";
                break;
            case TipoNotifVenta.Servicio:
                result = "Servicio";
                break;
            case TipoNotifVenta.ServicioCA:
                result = "Servicio CAISER";
                break;
            case TipoNotifVenta.ServicioWD:
                result = "Servicio Water Draw";
                break;
            case TipoNotifVenta.RMA:
                result = "RMA";
                break;
            case TipoNotifVenta.RemitoOficial:
                result = "Remito oficial";
                break;
            case TipoNotifVenta.RemitoInterno:
                result = "Remito interno";
                break;
            case TipoNotifVenta.Obra:
                result = "Obra";
                break;
            default:
                result = "N/D";
                break;
        }

        return result;
    }

    public static List<DataSourceItem> GetMonedas()
    {
        List<DataSourceItem> result = new List<DataSourceItem>();

        result.Add(new DataSourceItem(MonedaToString(Moneda.Dolar), (int)Moneda.Dolar));
        result.Add(new DataSourceItem(MonedaToString(Moneda.Peso), (int)Moneda.Peso));
        result.Add(new DataSourceItem(MonedaToString(Moneda.Euro), (int)Moneda.Euro));
        result.Add(new DataSourceItem(MonedaToString(Moneda.Real), (int)Moneda.Real));

        return result;
    }

    public static List<DataSourceItem> GetEstados()
    {
        List<DataSourceItem> result = new List<DataSourceItem>();

        result.Add(new DataSourceItem("Abierta", (int)EstadoNotifVenta.CargandoDatos));
        result.Add(new DataSourceItem("Cerrada", (int)EstadoNotifVenta.Cerrada));

        return result;
    }

    public static List<DataSourceItem> GetTiposVenta()
    {
        List<DataSourceItem> result = new List<DataSourceItem>();
        
		result.Add(new DataSourceItem("Producto Proser", (int)TipoNotifVenta.ProductoProser));
        result.Add(new DataSourceItem("Producto", (int)TipoNotifVenta.Producto));
        result.Add(new DataSourceItem("Servicio", (int)TipoNotifVenta.Servicio));
        result.Add(new DataSourceItem("Servicio CAISER", (int)TipoNotifVenta.ServicioCA));
        result.Add(new DataSourceItem("Servicio Water Draw", (int)TipoNotifVenta.ServicioWD));
        result.Add(new DataSourceItem("Obra", (int)TipoNotifVenta.Obra));
        result.Add(new DataSourceItem("RMA", (int)TipoNotifVenta.RMA));
        result.Add(new DataSourceItem("Remito oficial", (int)TipoNotifVenta.RemitoOficial));
        result.Add(new DataSourceItem("Remito interno", (int)TipoNotifVenta.RemitoInterno));

        return result;
    }

    public static void SendNotifVenta(int notifVentaID, string mensaje = "")
    {
        Email email;
        NotifVenta notifVenta = GetNotifVenta(notifVentaID);

        ///////////////////////////////
        string plantCli = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaCliente);
        if (plantCli == null) throw new EmailException();

        string link = Encriptacion.GetURLEncriptada("/comercial/notifVentaAdmin.aspx", "id=" + notifVentaID);
        plantCli = plantCli.Replace("@OC", notifVenta.OC);
        plantCli = plantCli.Replace("@CLIENTE", notifVenta.Cliente);
        plantCli = plantCli.Replace("@LINK", link);

        //string DEFAULT_SENDER = ConfigurationManager.AppSettings["DEFAULT_SENDER"].ToString();
        //email = new Email(DEFAULT_SENDER, "mogel10@gmail.com", "",
        //                  "Alta de cliente para OC " + notifVenta.OC, plantCli);
        ////email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaCliente), "",
        ////                  "Alta de cliente para OC " + notifVenta.OC, plantCli);
        //if (!email.Enviar()) throw new EmailException();

        //EmailHelper.Send(DEFAULT_SENDER, "mogel10@gmail.com", "", "Alta de cliente para OC " + notifVenta.OC, plantCli);

        //////////////////////////////


        if (notifVenta == null) throw new EmailException();

        //var link = Encriptacion.GetURLEncriptada("/comercial/notifVentaAdmin.aspx", "id=" + notifVentaID);

        string montoOC = notifVenta.MontoOC != Constantes.ValorInvalido ? notifVenta.MontoOCToString() : "-";

        switch (notifVenta.Estado)
        {
            case EstadoNotifVenta.CargandoDatos:
                // Si no existe el cliente, hay que darlo de alta.
                if (notifVenta.TipoVenta != TipoNotifVenta.RemitoOficial &&
                    notifVenta.TipoVenta != TipoNotifVenta.RemitoInterno && !Tango.ExisteCliente(notifVenta.Cliente))
                {
                    plantCli = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaCliente);
                    if (plantCli == null) throw new EmailException();

                    plantCli = plantCli.Replace("@OC", notifVenta.OC);
                    plantCli = plantCli.Replace("@CLIENTE", notifVenta.Cliente);
                    plantCli = plantCli.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaCliente), "",
                                      "Alta de cliente para OC " + notifVenta.OC, plantCli);
                    if (!email.Enviar()) throw new EmailException();
                }

                if (notifVenta.TipoVenta != TipoNotifVenta.RemitoOficial && notifVenta.TipoVenta != TipoNotifVenta.RemitoInterno)
                {
                    // Alta de imputación.
                    string plantImp = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaImputacion);
                    if (plantImp == null) throw new EmailException();

                    plantImp = plantImp.Replace("@OC", notifVenta.OC);
                    plantImp = plantImp.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantImp = plantImp.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaImputacion), "",
                                      "Alta de imputación para OC " + notifVenta.OC, plantImp);
                    if (!email.Enviar()) throw new EmailException();

                    // Aviso general.
                    string plantOC = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                    if (plantOC == null) throw new EmailException();

                    plantOC = plantOC.Replace("@ENCABEZADO", "Ha ingresado una nueva orden de compra:");
                    plantOC = plantOC.Replace("@TIPO_MENSAJE", "info");
                    plantOC = plantOC.Replace("@OC", notifVenta.OC);
                    plantOC = plantOC.Replace("@VENDEDOR", notifVenta.Vendedor);
                    plantOC = plantOC.Replace("@CLIENTE", notifVenta.Cliente);
                    plantOC = plantOC.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantOC = plantOC.Replace("@MONTO", montoOC);
                    plantOC = plantOC.Replace("@LINK", link);

//                    email = new Email(Constantes.EmailIntranet, notifVenta.VendedorEmail, 
//                                      "antonio.fedele@servaind.com" + ";" + "fernanda89949145+fbhg9co6dypdtkai8lps@boards.trello.com" 
//									  + ";" + "raul.gomez@servaind.com"
//									  + ";" + GPersonal.GetEmails(PermisosPersona.SNV_NotifOC),
//                                      "Ingreso de OC " + notifVenta.OC + " - " +  notifVenta.Cliente + " - " + notifVenta.Imputacion, plantOC);

                    email = new Email(Constantes.EmailIntranet, notifVenta.VendedorEmail, GPersonal.GetEmails(PermisosPersona.SNV_NotifOC),
                                      "Ingreso de OC " + notifVenta.OC + " - " +  notifVenta.Cliente + " - " + notifVenta.Imputacion, plantOC);
									  
                    if (!email.Enviar()) throw new EmailException();
                }

                if (notifVenta.TipoVenta == TipoNotifVenta.Producto ||  notifVenta.TipoVenta == TipoNotifVenta.RMA)
                {
                    // Aviso de producto.
                    string plantProd = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                    if (plantProd == null) throw new EmailException();

                    plantProd = plantProd.Replace("@OC", notifVenta.OC);
                    plantProd = plantProd.Replace("@VENDEDOR", notifVenta.Vendedor);
                    plantProd = plantProd.Replace("@CLIENTE", notifVenta.Cliente);
                    plantProd = plantProd.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantProd = plantProd.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet,
                                      notifVenta.TipoVenta == TipoNotifVenta.Producto
                                          ?  GPersonal.GetEmails(PermisosPersona.SNV_NotifProducto)
                                          :  GPersonal.GetEmails(PermisosPersona.SNV_NotifRMA), "",
                                      "Ingreso de " + (notifVenta.TipoVenta == TipoNotifVenta.Producto ? "OC" : "RMA") +
                                      notifVenta.OC, plantProd);
                    if (!email.Enviar()) throw new EmailException();                    
                }
				
				if (notifVenta.TipoVenta == TipoNotifVenta.ProductoProser)
                {
                    // Aviso de producto proser.
                    string plantProd = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                    if (plantProd == null) throw new EmailException();

                    plantProd = plantProd.Replace("@OC", notifVenta.OC);
                    plantProd = plantProd.Replace("@VENDEDOR", notifVenta.Vendedor);
                    plantProd = plantProd.Replace("@CLIENTE", notifVenta.Cliente);
                    plantProd = plantProd.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantProd = plantProd.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet
					                  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifProducto)
									  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifProductoProser)
									  ,"Ingreso de OC" + notifVenta.OC, plantProd);
                    if (!email.Enviar()) throw new EmailException();                    
                }
				
                
				if (notifVenta.TipoVenta == TipoNotifVenta.ServicioCA)
                {
                    // Aviso de producto proser.
                    string plantProd = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                    if (plantProd == null) throw new EmailException();

                    plantProd = plantProd.Replace("@OC", notifVenta.OC);
                    plantProd = plantProd.Replace("@VENDEDOR", notifVenta.Vendedor);
                    plantProd = plantProd.Replace("@CLIENTE", notifVenta.Cliente);
                    plantProd = plantProd.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantProd = plantProd.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet
					                  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifProducto)
									  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifServicioCA)
									  ,"Ingreso de OC" + notifVenta.OC, plantProd);
                    if (!email.Enviar()) throw new EmailException();                    
                }
				
                
				
				if (notifVenta.TipoVenta == TipoNotifVenta.ServicioWD)
                {
                    // Aviso de producto proser.
                    string plantProd = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                    if (plantProd == null) throw new EmailException();

                    plantProd = plantProd.Replace("@OC", notifVenta.OC);
                    plantProd = plantProd.Replace("@VENDEDOR", notifVenta.Vendedor);
                    plantProd = plantProd.Replace("@CLIENTE", notifVenta.Cliente);
                    plantProd = plantProd.Replace("@IMPUTACION", notifVenta.Imputacion);
                    plantProd = plantProd.Replace("@LINK", link);

                    email = new Email(Constantes.EmailIntranet
					                  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifProducto)
									  ,  GPersonal.GetEmails(PermisosPersona.SNV_NotifServicioWD)
									  ,"Ingreso de OC" + notifVenta.OC, plantProd);
                    if (!email.Enviar()) throw new EmailException();                    
                }
				
                break;
            case EstadoNotifVenta.CargandoRemito:
                string plantRem = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                if (plantRem == null) throw new EmailException();

                plantRem = plantRem.Replace("@OC", notifVenta.OC);
                plantRem = plantRem.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantRem = plantRem.Replace("@CLIENTE", notifVenta.Cliente);
                plantRem = plantRem.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantRem = plantRem.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaTransporte), "",
                                  "Completar datos de remito para OC " + notifVenta.OC, plantRem);
                if (!email.Enviar()) throw new EmailException();
                break;
            case EstadoNotifVenta.EsperandoAprobacion:
                string plantAprob = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                if (plantAprob == null) throw new EmailException();

                plantAprob = plantAprob.Replace("@ENCABEZADO", "Ha ingresado una nueva orden de compra:");
                plantAprob = plantAprob.Replace("@TIPO_MENSAJE", "info");
                plantAprob = plantAprob.Replace("@OC", notifVenta.OC);
                plantAprob = plantAprob.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantAprob = plantAprob.Replace("@CLIENTE", notifVenta.Cliente);
                plantAprob = plantAprob.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantAprob = plantAprob.Replace("@MONTO", montoOC);
                plantAprob = plantAprob.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.RolGerencia), "",
                                  "Solicitud de aprobación de OC " + notifVenta.OC, plantAprob);
                if (!email.Enviar()) throw new EmailException();
                break;
            case EstadoNotifVenta.Rechazada:
                // Actualizo el estado.
                UpdateNotifVenta(notifVentaID, EstadoNotifVenta.CargandoDatos);

                string plantRechazo = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                if (plantRechazo == null) throw new EmailException();

                plantRechazo = plantRechazo.Replace("@ENCABEZADO", "La solicitud de aprobación ha sido rechazada por " +
                                                                   "el siguiente motivo: " + mensaje);
                plantRechazo = plantRechazo.Replace("@TIPO_MENSAJE", "error");
                plantRechazo = plantRechazo.Replace("@OC", notifVenta.OC);
                plantRechazo = plantRechazo.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantRechazo = plantRechazo.Replace("@CLIENTE", notifVenta.Cliente);
                plantRechazo = plantRechazo.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantRechazo = plantRechazo.Replace("@MONTO", montoOC);
                plantRechazo = plantRechazo.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.RolGerencia), "",
                                  "Solicitud de aprobación de OC " + notifVenta.OC + " rechazada", plantRechazo);
                if (!email.Enviar()) throw new EmailException();
                break;
            case EstadoNotifVenta.ConfeccionRem:
                // Confección de factura/remito.
                string plantConfRem = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                if (plantConfRem == null) throw new EmailException();

                plantConfRem = plantConfRem.Replace("@ENCABEZADO", "Se necesita la confección del remito para la siguiente OC:");
                plantConfRem = plantConfRem.Replace("@TIPO_MENSAJE", "info");
                plantConfRem = plantConfRem.Replace("@OC", notifVenta.OC);
                plantConfRem = plantConfRem.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantConfRem = plantConfRem.Replace("@CLIENTE", notifVenta.Cliente);
                plantConfRem = plantConfRem.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantConfRem = plantConfRem.Replace("@MONTO", montoOC);
                plantConfRem = plantConfRem.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaFacRem), "",
                                  "Confección de remito para OC " + notifVenta.OC, plantConfRem);
                if (!email.Enviar()) throw new EmailException();
                break;
            case EstadoNotifVenta.ConfeccionFac:
                // Confección de factura/remito.
                string plantFac = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                if (plantFac == null) throw new EmailException();

                plantFac = plantFac.Replace("@ENCABEZADO", "Se necesita la confección de la factura para la siguiente OC:");
                plantFac = plantFac.Replace("@TIPO_MENSAJE", "info");
                plantFac = plantFac.Replace("@OC", notifVenta.OC);
                plantFac = plantFac.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantFac = plantFac.Replace("@CLIENTE", notifVenta.Cliente);
                plantFac = plantFac.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantFac = plantFac.Replace("@MONTO", montoOC);
                plantFac = plantFac.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_AltaFacRem), "",
                                  "Confección de factura para OC " + notifVenta.OC, plantFac);
                if (!email.Enviar()) throw new EmailException();
                break;
            case EstadoNotifVenta.EsperandoITR:
                string plantAct = Funciones.ObtenerPlantilla(EmailPlantilla.NVRecordatorio);
                if (plantAct == null) throw new EmailException();

                plantAct = plantAct.Replace("@ENCABEZADO", notifVenta.OC);
                plantAct = plantAct.Replace("@OC", notifVenta.OC);
                plantAct = plantAct.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantAct = plantAct.Replace("@CLIENTE", notifVenta.Cliente);
                plantAct = plantAct.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantAct = plantAct.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, GPersonal.GetEmails(PermisosPersona.SNV_Vendedor), "",
                                  "Actualización de datos de OC " + notifVenta.OC, plantAct);
                if (!email.Enviar()) throw new EmailException();
                break;
           case EstadoNotifVenta.Cerrada:
                string plantCierre = Funciones.ObtenerPlantilla(EmailPlantilla.NVAltaGeneralOC);
                if (plantCierre == null) throw new EmailException();

                plantCierre = plantCierre.Replace("@ENCABEZADO", "La siguiente orden de compra ha sido cerrada:");
                plantCierre = plantCierre.Replace("@TIPO_MENSAJE", "success");
                plantCierre = plantCierre.Replace("@OC", notifVenta.OC);
                plantCierre = plantCierre.Replace("@VENDEDOR", notifVenta.Vendedor);
                plantCierre = plantCierre.Replace("@CLIENTE", notifVenta.Cliente);
                plantCierre = plantCierre.Replace("@IMPUTACION", notifVenta.Imputacion);
                plantCierre = plantCierre.Replace("@MONTO", montoOC);
                plantCierre = plantCierre.Replace("@LINK", link);

                email = new Email(Constantes.EmailIntranet, notifVenta.VendedorEmail,
                                  GPersonal.GetEmails(PermisosPersona.SNV_NotifCierre), "Cierre de OC " + notifVenta.OC,
                                  plantCierre);
                if (!email.Enviar()) throw new EmailException();
                break;
        }
    }

  
  private static string GetConsultaFiltroToExcel(List<Filtro> filtros)
    {
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            if (Enum.IsDefined(typeof(FiltroNotifVenta), filtro.Tipo))
            {
                FiltroNotifVenta f = (FiltroNotifVenta)filtro.Tipo;
                switch (f)
                {
                    case FiltroNotifVenta.ID:
                        filtroWhere += "AND ve.NotifVentaID = " + filtro.Valor + " ";
                        break;
                    case FiltroNotifVenta.Vendedor:
                        filtroWhere += "AND p.Nombre LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Cliente:
                        filtroWhere += "AND ve.Cliente LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.OC:
                        filtroWhere += "AND ve.OC LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Imputacion:
                        filtroWhere += "AND ve.Imputacion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Estado:
                        EstadoNotifVenta estado = (EstadoNotifVenta) Convert.ToInt32(filtro.Valor);

                        filtroWhere += String.Format("AND ve.EstadoID {0} {1}", estado == EstadoNotifVenta.Cerrada ? "=" : "<>",
                            (int)EstadoNotifVenta.Cerrada);
                        break;
                }
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        consulta = "SELECT ve.NotifVentaID, p.Nombre AS Vendedor, ve.Cliente, ve.OC, ";
        consulta += "ve.MonedaID, ve.MontoOC , ve.Imputacion, ve.FechaOC, ve.EstadoID ";
        consulta += "FROM tbl_NotifVentas ve ";
        consulta += "INNER JOIN tbl_Personal p ON p.idPersonal = ve.VendedorID ";
		
		
        consulta += filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "";

        consulta += " ORDER BY ve.NotifVentaID DESC";

        return consulta;
    }

  private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            if (Enum.IsDefined(typeof(FiltroNotifVenta), filtro.Tipo))
            {
                FiltroNotifVenta f = (FiltroNotifVenta)filtro.Tipo;
                switch (f)
                {
                    case FiltroNotifVenta.ID:
                        filtroWhere += "AND ve.NotifVentaID = " + filtro.Valor + " ";
                        break;
                    case FiltroNotifVenta.Vendedor:
                        filtroWhere += "AND p.Nombre LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Cliente:
                        filtroWhere += "AND ve.Cliente LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.OC:
                        filtroWhere += "AND ve.OC LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Imputacion:
                        filtroWhere += "AND ve.Imputacion LIKE '%" + filtro.Valor + "%' ";
                        break;
                    case FiltroNotifVenta.Fecha:
                        filtroWhere += "AND ve.FechaOC = '" + Convert.ToDateTime(filtro.Valor).ToString("dd/MM/yyyy") + "' ";
                        break;
                    case FiltroNotifVenta.Factura:
                        filtroWhere += "AND ve.Factura = " + filtro.Valor + " ";
                        break;
                    case FiltroNotifVenta.Remito:
                        filtroWhere += "AND ve.Remito = " + filtro.Valor + " ";
                        break;
                    case FiltroNotifVenta.Estado:
                        EstadoNotifVenta estado = (EstadoNotifVenta) Convert.ToInt32(filtro.Valor);

                        filtroWhere += String.Format("AND ve.EstadoID {0} {1}", estado == EstadoNotifVenta.Cerrada ? "=" : "<>",
                            (int)EstadoNotifVenta.Cerrada);
                        break;
                }
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(NotifVentaID) as TotalRegistros ";
        }
        else
        {
            consulta = "SELECT ve.NotifVentaID, p.Nombre AS Vendedor, ve.Cliente, ve.OC, ve.Imputacion, ve.FechaOC, ";
            consulta += "ve.Factura, ve.Remito, ve.EstadoID ";
        }
        consulta += "FROM tbl_NotifVentas ve ";
        consulta += "INNER JOIN tbl_Personal p ON p.idPersonal = ve.VendedorID ";
        consulta += filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "";

        if (!cantidad) consulta += "ORDER BY ve.NotifVentaID DESC";

        return consulta;
    }

    public static int GetNotifVentaResumenPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
    }

    public static List<NotifVentaResumen> GetNotifVentaResumen(int pagina, List<Filtro> filtros)
    {
        List<NotifVentaResumen> result = DataAccess.GetDataList(BDConexiones.Intranet, pagina,
                                                                filtros, MaxRegistrosPagina,
                                                                GetConsultaFiltro,
                                                                GetNotifVentaResumen);

        return result;
    }

    public static List<NotifVentaResumenExcel> GetNotifVentaResumenExcel(List<Filtro> filtros)
    {
        List<NotifVentaResumenExcel> result = DataAccess.GetDataList(BDConexiones.Intranet,
                                                                    filtros, 
                                                                    GetConsultaFiltroToExcel,
                                                                    GetNotifVentaResumenExcel);

        return result;
    }

    private static string GetFileITR(int notifVentaID)
    {
        string result = String.Empty;
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "SELECT Imputacion FROM tbl_NotifVentas WHERE NotifVentaID = @NotifVentaID;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NotifVentaID", notifVentaID));
            int imputacion = Convert.ToInt32(cmd.ExecuteScalar().ToString().Split('-')[0].Trim());

            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "SELECT TOP 1 pd.FechaParte, p.Usuario ";
            cmd.CommandText += "FROM tbl_PartesDiarios pd ";
            cmd.CommandText += "INNER JOIN tbl_PDImputaciones pdi ON pdi.idParteDiario = pd.idParteDiario ";
            cmd.CommandText += "INNER JOIN tbl_Imputaciones i ON i.idImputacion = pdi.idImputacion ";
            cmd.CommandText += "INNER JOIN tbl_Personal p ON p.idPersonal = pd.idPersona ";
            cmd.CommandText += "WHERE i.Numero = @Imputacion AND pdi.HayITR = @HayITR;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Imputacion", imputacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HayITR", true));
            dr = cmd.ExecuteReader();

            if(dr.Read()) result = ITR.GetNombreITR(Convert.ToDateTime(dr["FechaParte"]), imputacion, dr["Usuario"].ToString());
            dr.Close();
        }
        catch
        {
            if (trans != null) trans.Rollback();
            result = String.Empty;
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) { conn.Close(); }
        }

        return result;
    }

    public static string GetFileOC(string imputacion)
    {
        string result = String.Empty;

        ImpersionateHelper.Impersionate();

        imputacion = imputacion.Replace(" - ", " ").ToUpper();
        result = @"\\Server-Storage1\Cotizaciones\";

        try
        {
            List<string> dirs = Directory.GetDirectories(result).ToList();
            string dirImputacion = dirs.FirstOrDefault(d => d.ToUpper().Contains(imputacion));
            if (String.IsNullOrWhiteSpace(dirImputacion)) throw new Exception();

            result = dirImputacion + "\\";
            string dirOC = Directory.GetDirectories(result).ToList().FirstOrDefault(d => d.ToUpper().EndsWith("OC"));
            if (String.IsNullOrWhiteSpace(dirOC)) throw new Exception();

            result = dirOC + "\\";
            string file = Directory.GetFiles(result).ToList().FirstOrDefault(f => f.ToLower().EndsWith(".pdf"));
            if (String.IsNullOrWhiteSpace(file)) throw new Exception();

            result = file;
        }
        catch
        {
            result = String.Empty;
        }

        ImpersionateHelper.UndoImpersionate();

        return result;
    }

    public static List<string> GetOCs(string filtro)
    {
        List<string> result = new List<string>();
        IDbConnection conn = null;
        IDataReader dr = null;

        if (filtro == null || filtro.Length < 3) return result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 10 OC FROM tbl_NotifVentas WHERE OC LIKE @Filtro ORDER BY OC;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Filtro", '%' + filtro.ToUpper() + '%'));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(dr["OC"].ToString());
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

    public static string GetDescripcionImputacion(string numero)
    {
        string result = String.Empty;

        string path = @"\\Server-Storage1\Cotizaciones\";

        ImpersionateHelper.Impersionate();

        try
        {
            List<string> dirs = Directory.GetDirectories(path).ToList();
            string dirImputacion = dirs.FirstOrDefault(d => d.ToUpper().Contains(numero));
            if (String.IsNullOrWhiteSpace(dirImputacion)) throw new Exception();

            string[] aux = dirImputacion.TrimEnd('\\').Split('\\');
            result = aux[aux.Length - 1];

            Match m = Regex.Match(result, @"\ ([^!])*");
            result = m.Groups[0].Value.Trim();
        }
        catch
        {
            result = String.Empty;
        }

        ImpersionateHelper.UndoImpersionate();

        return result;
    }
	
	
	public static string ExportarAExcel(List<NotifVentaResumenExcel> lista)
	{
		
        string path = Constantes.PATH_TEMP + "ListadoVentas.xlsx";
       
		var workbook = new XLWorkbook();
		var sheet1 = workbook.Worksheets.Add("Listado de Ventas");
		
		sheet1.Cell("A2").Value = "Nro.";
        sheet1.Cell("B2").Value = "Vendedor";
        sheet1.Cell("C2").Value = "Cliente";
        sheet1.Cell("D2").Value = "Orden de Compra";
        sheet1.Cell("E2").Value = "Monto";
        sheet1.Cell("F2").Value = "Imputación";
        sheet1.Cell("G2").Value = "Fecha";
        sheet1.Cell("H2").Value = "Estado";
    				
		foreach (var ro in Enumerable.Range(2, 1))
		{
			foreach (var co in Enumerable.Range(1, 8))
			{
				var cell = sheet1.Cell(ro, co);
				cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#4682b4");
				cell.Style.Font.FontColor = XLColor.White;              
				cell.Style.Font.Bold = true;
			}
		}
		
	    int c = lista.Count;
		
        for (int i = 0; i < c; i++)
        {
            NotifVentaResumenExcel item = lista[i];
            sheet1.Cell(i + 3, 1).Value = item.Numero;
            sheet1.Cell(i + 3, 2).Value = item.Vendedor;
            sheet1.Cell(i + 3, 3).Value = item.Cliente;
            sheet1.Cell(i + 3, 4).Value = item.OC;
            sheet1.Cell(i + 3, 5).Value = string.Format("{0} {1}", item.Moneda, item.Monto);
            sheet1.Cell(i + 3, 6).Value = item.Imputacion;
            sheet1.Cell(i + 3, 7).Value = item.FechaOC;
			sheet1.Cell(i + 3, 7).Style.NumberFormat.Format = "dd/MM/yyyy";
            sheet1.Cell(i + 3, 8).Value = item.Estado;
        }
		
		sheet1.Column("A").AdjustToContents();	
		sheet1.Column("B").AdjustToContents();
		sheet1.Column("C").AdjustToContents();
		sheet1.Column("D").AdjustToContents();
		sheet1.Column("E").AdjustToContents();
		sheet1.Column("F").AdjustToContents();
		
		var col = sheet1.Column("G");
        col.Width = 10;
		
		col = sheet1.Column("H");
        col.Width = 12;
		col.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

		workbook.SaveAs(path);
		
        return path;
		
	}
	
	
}