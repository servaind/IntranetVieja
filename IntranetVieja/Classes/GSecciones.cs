/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


/// <summary>
/// Descripción breve de Seccion
/// </summary>
public class Seccion : IComparable<Seccion>
{
    // Variables.
    private string descripcion;
    private string url;
    private List<PermisosPersona> permisos;

    // Propiedades.
    public string Descripcion
    {
        get { return descripcion; }
    }
    public string URL
    {
        get { return url; }
    }
    public List<PermisosPersona> Permisos
    {
        get { return permisos; }
    }


    internal Seccion(string descripcion, string url, List<PermisosPersona> permisos)
    {
        this.descripcion = descripcion;
        this.url = url;
        this.permisos = permisos;
    }

    public bool TieneAcceso()
    {
        bool result = false;

        if (permisos != null) result = permisos.Any(GPermisosPersonal.TieneAcceso);

        return result;
    }

    public int CompareTo(Seccion other)
    {
        return Descripcion.CompareTo(other.Descripcion);
    }
}

/// <summary>
/// Descripción breve de GestorSecciones
/// </summary>
public static class GSecciones
{
    private static List<Seccion> GetSecciones()
    {
        List<Seccion> result = new List<Seccion>();

        result.Add(new Seccion("Administracion de personal", "sistemas/personalLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Administrador}));
        result.Add(new Seccion("Solicitud de licencia", "rrhh/licenciaAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Publico}));
        result.Add(new Seccion("Administración de imputaciones", "sistemas/imputacionesLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Administrador}));
        result.Add(new Seccion("Administración de permisos", "sistemas/personalPermisos.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Administrador}));
        result.Add(new Seccion("Partes diarios: cargar", "general/parteDiarioAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Publico}));
        result.Add(new Seccion("Repositorio de Archivos", "general/repositorioArchivos.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Publico}));
        result.Add(new Seccion("Vale de Materiales: solicitud", "stock/vdmAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.ValeMaterialesVer}));
        result.Add(new Seccion("Viajes: solicitud", "general/viajeAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.SolViajeVer}));
        result.Add(new Seccion("Vale de Materiales: listado", "stock/vdmLista.aspx",
                               new List<PermisosPersona>() { PermisosPersona.ValeMaterialesVer }));
        result.Add(new Seccion("Viajes: listado", "general/viajesLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.SolViajeVer}));
        result.Add(new Seccion("Sistema de NC", "calidad/ncAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.NNCVer}));
        result.Add(new Seccion("Sistema de NC (hasta 2014)", "calidad/ncLista.aspx",
                               new List<PermisosPersona>() { PermisosPersona.NNCVer }));
        result.Add(new Seccion("Detalle de asistencia", "rrhh/detalleAsistencia.aspx",
                               new List<PermisosPersona>() {PermisosPersona.LicRRHH}));
        result.Add(new Seccion("Cotizador", "stock/cotizador.aspx",
                               new List<PermisosPersona>() {PermisosPersona.CotizadorOnLine}));
        result.Add(new Seccion("Herramientas: listado", "stock/herramientasLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.HerramientaAdministrador}));
        result.Add(new Seccion("Envio de materiales: solicitud", "general/semFormulario.aspx",
                               new List<PermisosPersona>() {}));
        result.Add(new Seccion("Envio de Materiales: listado", "general/semDetalles.aspx",
                               new List<PermisosPersona>() {}));
        result.Add(new Seccion("Herramientas: alta", "stock/herramientaAdmin.aspx",
                               new List<PermisosPersona>() { PermisosPersona.HerramientaAdministrador }));
        result.Add(new Seccion("Alta de artículos: solicitud", "stock/altaArticuloAdmin.aspx",
                               new List<PermisosPersona>() { PermisosPersona.Publico }));
        result.Add(new Seccion("Alta de artículos: listado", "stock/altaArticulosLista.aspx",
                               new List<PermisosPersona>() { PermisosPersona.Publico }));
        result.Add(new Seccion("Partes diarios: panel de control", "general/partesDiariosPC.aspx",
                               new List<PermisosPersona>()
                                   {
                                       PermisosPersona.GEN_CargaParteDiario,
                                       PermisosPersona.AdminPanelesControlPD
                                   }));
        /*result.Add(new Seccion("Instrumentos: listado", "stock/instrumentosLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.InstrumentoSeguimiento}));*/
        result.Add(new Seccion("Información de obra: generar", "general/informacionObraAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.IIOVer}));
        result.Add(new Seccion("Información de obra: listado", "general/informacionObraLista.aspx",
                               new List<PermisosPersona>() { PermisosPersona.IIOVer }));
        result.Add(new Seccion("SGI: SSM", "calidad/ssm.aspx",
                               new List<PermisosPersona>() {PermisosPersona.SSM_Admin}));
        result.Add(new Seccion("SGI: Multisitio", "calidad/sgim.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Publico}));
        result.Add(new Seccion("Vehiculos: administración", "administracion/vehiculosAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.VehicAdmin}));
        result.Add(new Seccion("Autorizaciones", "general/autorizLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.AutorizAdministrar}));
        result.Add(new Seccion("Vehiculos: vencimientos", "administracion/vehiculosVencimientos.aspx",
                               new List<PermisosPersona>() {PermisosPersona.VehicVer}));
        result.Add(new Seccion("Stock: listado", "stock/listaArticulos.aspx",
                               new List<PermisosPersona>() {PermisosPersona.StockVer}));
        result.Add(new Seccion("Stock: disponibilidad equipos", "stock/equiposDisponib.aspx",
                               new List<PermisosPersona>() {PermisosPersona.StockVer}));
        result.Add(new Seccion("Stock: equipos en producción", "stock/equiposProduccion.aspx",
                               new List<PermisosPersona>() { PermisosPersona.StockVer }));
        result.Add(new Seccion("Instrumentos: alta", "stock/altaInstrumento.aspx",
                               new List<PermisosPersona>() {PermisosPersona.InstrumentoSeguimiento}));
        result.Add(new Seccion("Instrumentos: vencimientos", "stock/instrumentosVencimientos.aspx",
                               new List<PermisosPersona>() { PermisosPersona.InstrumentoSeguimiento }));
        result.Add(new Seccion("Bases: listado", "sistemas/basesLista.aspx",
                               new List<PermisosPersona>() {PermisosPersona.Administrador}));
        result.Add(new Seccion("Asistencia: carga entrada", "rrhh/cargaAsistenciaEntrada.aspx",
                               new List<PermisosPersona>() {PermisosPersona.ADP_CargaEntrada}));
        result.Add(new Seccion("Asistencia: panel de control", "rrhh/panelControlAsistencia.aspx",
                               new List<PermisosPersona>() {PermisosPersona.ADP_PanelControlAsistencia}));
        result.Add(new Seccion("Asistencia: carga salida", "rrhh/cargaAsistenciaSalida.aspx",
                               new List<PermisosPersona>() {PermisosPersona.ADP_CargaSalida}));
        result.Add(new Seccion("Asistencia: editar responsables", "rrhh/asistPcResponsablesCarga.aspx",
                               new List<PermisosPersona>() { PermisosPersona.ADP_PanelControlAsistencia }));
        result.Add(new Seccion("SNV: Alta de venta", "comercial/notifVentaAdmin.aspx",
                               new List<PermisosPersona>() {PermisosPersona.SNV_Vendedor, PermisosPersona.SNV_Visualizacion}));
        result.Add(new Seccion("SNV: Listado de ventas", "comercial/notifVentaLista.aspx",
                               new List<PermisosPersona>()
                                   {
                                       PermisosPersona.SNV_Visualizacion,
                                       PermisosPersona.SNV_Vendedor,
                                       PermisosPersona.SNV_AltaImputacion,
                                       PermisosPersona.SNV_AltaCliente,
                                       PermisosPersona.SNV_AltaFacRem,
                                       PermisosPersona.SNV_NotifCierre,
                                       PermisosPersona.SNV_NotifOC,
                                       PermisosPersona.SNV_NotifRecordatorio,
                                       PermisosPersona.SNV_AltaTransporte,
                                       PermisosPersona.SNV_NotifProducto,
                                       PermisosPersona.SNV_NotifRMA
                                   }));


        return result;
    }

    public static List<Seccion> GetSeccionesPersona()
    {
        List<Seccion> result;

        List<Seccion> secciones = GetSecciones();
        result = secciones.FindAll(s => s.TieneAcceso());

        result.Sort();

        return result;
    }
}
