/*
 * Historial:
 * ===================================================================================
 * [16/06/2011]
 * - Agregados permisos.
 * [10/06/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


public class PermisoPersonal
{
    // Variables.
    private Persona personal;
    private PermisosPersona permiso;

    // Propiedades.
    public Persona Personal
    {
        get { return personal; }
    }
    public PermisosPersona Permiso
    {
        get { return permiso; }
    }


    internal PermisoPersonal(Persona personal, PermisosPersona permiso)
    {
        this.personal = personal;
        this.permiso = permiso;
    }
}

public class GrupoPermisos : IComparable<GrupoPermisos>
{
    // Variables.
    private string nombre;
    private SortedList<string, PermisosPersona> permisos;

    // Propiedades.
    public string Nombre
    {
        get { return nombre; }
    }
    public SortedList<string, PermisosPersona> Permisos
    {
        get { return permisos; }
    }


    internal GrupoPermisos(string nombre, SortedList<string, PermisosPersona> permisos)
    {
        this.nombre = nombre;
        this.permisos = permisos;
    }

    public int CompareTo(GrupoPermisos other)
    {
        return Nombre.CompareTo(other.Nombre);
    }
}

/// <summary>
/// Descripción breve de GPermisos
/// </summary>
public class GPermisosPersonal
{
    /// <summary>
    /// Obtiene los permisos para la persona.
    /// </summary>
    public static List<PermisoPersonal> GetPermisosPersonal()
    {
        return GetPermisosPersonal(Constantes.Usuario.ID);
    }
    /// <summary>
    /// Obtiene los permisos para la persona.
    /// </summary>
    public static List<PermisoPersonal> GetPermisosPersonal(int idPersonal)
    {
        Persona personal = GPersonal.GetPersona(idPersonal);

        return GetPermisosPersonal(personal);
    }
    /// <summary>
    /// Obtiene los permisos para la persona.
    /// </summary>
    public static List<PermisoPersonal> GetPermisosPersonal(Persona personal)
    {
        List<PermisoPersonal> lstPermisos = new List<PermisoPersonal>();

        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idPermiso FROM tbl_PermisosPersonal WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", personal.ID));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                try
                {
                    PermisoPersonal permiso = new PermisoPersonal(personal, 
                        (PermisosPersona)Convert.ToInt32(dr["idPermiso"]));
                    lstPermisos.Add(permiso);
                }
                catch
                {

                }
            }

            dr.Close();
        }
        catch
        {

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return lstPermisos;
    }
    /// <summary>
    /// Verifica si el usuario actual tiene acceso.
    /// </summary>
    public static bool TieneAcceso(PermisosPersona permiso)
    {
        return TieneAcceso(permiso, Constantes.Usuario.Permisos);
    }
    /// <summary>
    /// Verifica si la persona tiene acceso.
    /// </summary>
    public static bool TieneAcceso(PermisosPersona permiso, Persona personal)
    {
        List<PermisoPersonal> lstPermisos = GetPermisosPersonal(personal);

        return TieneAcceso(permiso, lstPermisos);
    }
    /// <summary>
    /// Verifica si la persona tiene acceso.
    /// </summary>
    public static bool TieneAcceso(List<PermisosPersona> permisos)
    {
        bool result = Constantes.Usuario.Permisos.Any(p => permisos.Contains(p.Permiso));

        if (!result) result = permisos.Contains(PermisosPersona.Publico);

        return result;
    }
    /// <summary>
    /// Verifica si la persona tiene acceso.
    /// </summary>
    public static bool TieneAcceso(PermisosPersona permiso, 
        List<PermisoPersonal> lstPermisos)
    {
        bool result = false;

        foreach (PermisoPersonal p in lstPermisos)
        {
            if (p.Permiso == PermisosPersona.Administrador ||
                p.Permiso == permiso || permiso == PermisosPersona.Publico)
            {
                result = true;
                break;
            }
        }

        return result;
    }
    /// <summary>
    /// Obtiene una lista con los permisos disponibles.
    /// </summary>
    public static List<GrupoPermisos> GetGruposPermisos()
    {
        List<GrupoPermisos> result = new List<GrupoPermisos>();


        // Administración del sistema.
        SortedList<string, PermisosPersona> lstAdminSistema = new SortedList<string, PermisosPersona>();
        lstAdminSistema.Add("Administrador", PermisosPersona.Administrador);
        lstAdminSistema.Add("Administración de imputaciones", PermisosPersona.AdminImputaciones);
        lstAdminSistema.Add("Administración de paneles de control de partes diarios", PermisosPersona.AdminPanelesControlPD);
        lstAdminSistema.Add("Administración de personal", PermisosPersona.AdminPersonal);
        lstAdminSistema.Add("Administración de partes diarios", PermisosPersona.AdminPartesDiarios);
        GrupoPermisos grpAdminSistema = new GrupoPermisos("Administración del sistema", lstAdminSistema);

        // Vale de Materiales.
        SortedList<string, PermisosPersona> lstValeMateriales = new SortedList<string, PermisosPersona>();
        lstValeMateriales.Add("Recibir responsable", PermisosPersona.ValeMaterialesRecibeResp);
        lstValeMateriales.Add("Aprobar responsable", PermisosPersona.ValeMaterialesApruebaResp);
        lstValeMateriales.Add("Recibir depósito", PermisosPersona.ValeMaterialesRecibeDep);
        lstValeMateriales.Add("Entregar", PermisosPersona.ValeMaterialesEntrega);
        lstValeMateriales.Add("Ver", PermisosPersona.ValeMaterialesVer);
        lstValeMateriales.Add("Informe", PermisosPersona.ValeMaterialesInforme);
        GrupoPermisos grpValeMateriales = new GrupoPermisos("Vale de materiales", lstValeMateriales);

        // Solicitud de Envío de Materiales.
        SortedList<string, PermisosPersona> lstSolEnvMat = new SortedList<string, PermisosPersona>();
        lstSolEnvMat.Add("Ver", PermisosPersona.SolEnvMatVer);
        lstSolEnvMat.Add("Guardar", PermisosPersona.SolEnvMatGuardar);
        lstSolEnvMat.Add("Confirmar", PermisosPersona.SolEnvMatConfirmar);
        lstSolEnvMat.Add("Cerrar", PermisosPersona.SolEnvMatCerrar);
        GrupoPermisos grpSolEnvMat = new GrupoPermisos("Solicitud de envío de materiales", lstSolEnvMat);

        // Nota de No Conformidad.
        SortedList<string, PermisosPersona> lstNNC = new SortedList<string, PermisosPersona>();
        lstNNC.Add("Administrador", PermisosPersona.NNCAdministrador);
        lstNNC.Add("Ver", PermisosPersona.NNCVer);
        lstNNC.Add("Editar", PermisosPersona.NNCEditar);
        GrupoPermisos grpNNC = new GrupoPermisos("Nota de no conformidad", lstNNC);

        // Solicitud de Viajes.
        SortedList<string, PermisosPersona> lstSolViajes = new SortedList<string, PermisosPersona>();
        lstSolViajes.Add("Ver", PermisosPersona.SolViajeVer);
        lstSolViajes.Add("Editar", PermisosPersona.SolViajeEditar);
        GrupoPermisos grpSolViajes = new GrupoPermisos("Solicitud de viajes", lstSolViajes);

        // Repositorio de archivos.
        SortedList<string, PermisosPersona> lstRDA = new SortedList<string, PermisosPersona>();
        lstRDA.Add("Ver", PermisosPersona.RDAVer);
        lstRDA.Add("Crear", PermisosPersona.RDACrear);
        GrupoPermisos grpRDA = new GrupoPermisos("Repositorio de archivos", lstRDA);

        // Stock.
        SortedList<string, PermisosPersona> lstStock = new SortedList<string, PermisosPersona>();
        lstStock.Add("Cotizador on-line", PermisosPersona.CotizadorOnLine);
        lstStock.Add("Cotizador on-line de equipos", PermisosPersona.CotizadorOnLineEquipos);
        lstStock.Add("Control de herramientas", PermisosPersona.ControlHerramientas);
        lstStock.Add("Responsable de herramientas", PermisosPersona.HerramientaAdministrador);
        lstStock.Add("Responsable de instrumentos", PermisosPersona.InstrumentoSeguimiento);
        lstStock.Add("Visualización de stock", PermisosPersona.StockVer);
        lstStock.Add("Ingreso de stock", PermisosPersona.StockIngreso);
        lstStock.Add("Egreso de stock", PermisosPersona.StockEgreso);
        lstStock.Add("Responsable de alta de artículos", PermisosPersona.SAAResponsable);
        GrupoPermisos grpStock = new GrupoPermisos("Stock", lstStock);

        // Ausencias - Licencias.
        SortedList<string, PermisosPersona> lstAL = new SortedList<string, PermisosPersona>();
        lstAL.Add("Recursos Humanos", PermisosPersona.LicRRHH);
        lstAL.Add("Carga de asistencia [entrada]", PermisosPersona.ADP_CargaEntrada);
        lstAL.Add("Carga de asistencia [salida]", PermisosPersona.ADP_CargaSalida);
        lstAL.Add("Panel de control de asistencia", PermisosPersona.ADP_PanelControlAsistencia);
        lstAL.Add("Panel de control de asistencia (Responsable)", PermisosPersona.ADP_PcResponsable);
        GrupoPermisos grpAL = new GrupoPermisos("Administración de personal", lstAL);

        // Paneles de control.
        SortedList<string, PermisosPersona> lstPC = new SortedList<string, PermisosPersona>();
        lstPC.Add("Ver panel de control general", PermisosPersona.PCVerGeneral);
        GrupoPermisos grpPC = new GrupoPermisos("Paneles de control", lstPC);

        // Información interna de obra.
        SortedList<string, PermisosPersona> lstIIO = new SortedList<string, PermisosPersona>();
        lstIIO.Add("Generar", PermisosPersona.IIOGenerar);
        lstIIO.Add("Ver", PermisosPersona.IIOVer);
        GrupoPermisos grpIIO = new GrupoPermisos("Información interna de obra", lstIIO);

        // Roles.
        SortedList<string, PermisosPersona> lstRoles = new SortedList<string, PermisosPersona>();
        lstRoles.Add("Dirección", PermisosPersona.RolDireccion);
        lstRoles.Add("Gerencia", PermisosPersona.RolGerencia);
        GrupoPermisos grpRoles = new GrupoPermisos("Roles", lstRoles);

        // SSM.
        SortedList<string, PermisosPersona> lstSSM = new SortedList<string, PermisosPersona>();
        lstSSM.Add("Administrador", PermisosPersona.SSM_Admin);
        GrupoPermisos grpSSM = new GrupoPermisos("Sistema de Seguimiento Multisitio", lstSSM);

        // Vehículos.
        SortedList<string, PermisosPersona> lstVehiculos = new SortedList<string, PermisosPersona>();
        lstVehiculos.Add("Administrador", PermisosPersona.VehicAdmin);
        lstVehiculos.Add("Ver", PermisosPersona.VehicVer);
        GrupoPermisos grpVehiculos = new GrupoPermisos("Administración de vehículos", lstVehiculos);

        // Autorizaciones.
        SortedList<string, PermisosPersona> lstAutorizaciones = new SortedList<string, PermisosPersona>();
        lstAutorizaciones.Add("Administrar", PermisosPersona.AutorizAdministrar);
        GrupoPermisos grpAutorizaciones = new GrupoPermisos("Autorizaciones", lstAutorizaciones);

        // Sistema de Notificación de Ventas.
        SortedList<string, PermisosPersona> lstSNV = new SortedList<string, PermisosPersona>();
        lstSNV.Add("Visualización", PermisosPersona.SNV_Visualizacion);
        lstSNV.Add("Vendedor", PermisosPersona.SNV_Vendedor);
        lstSNV.Add("Alta de factura / remito", PermisosPersona.SNV_AltaFacRem);
        lstSNV.Add("Alta de clientes", PermisosPersona.SNV_AltaCliente);
        lstSNV.Add("Alta de imputaciones", PermisosPersona.SNV_AltaImputacion);
        lstSNV.Add("Alta de transporte", PermisosPersona.SNV_AltaTransporte);
        lstSNV.Add("Notificación de alta de OC", PermisosPersona.SNV_NotifOC);
        lstSNV.Add("Notificación de cierre", PermisosPersona.SNV_NotifCierre);
        lstSNV.Add("Notificación de recordatorio", PermisosPersona.SNV_NotifRecordatorio);
        lstSNV.Add("Notificación de RMA", PermisosPersona.SNV_NotifRMA);
        lstSNV.Add("Notificación de Producto", PermisosPersona.SNV_NotifProducto);
        GrupoPermisos grpSNV = new GrupoPermisos("Sistema de notificación de ventas", lstSNV);

        // General.
        SortedList<string, PermisosPersona> lstGeneral = new SortedList<string, PermisosPersona>();
        lstGeneral.Add("Carga parte diario", PermisosPersona.GEN_CargaParteDiario);
        GrupoPermisos grpGeneral = new GrupoPermisos("General", lstGeneral);

        result.Add(grpAdminSistema);
        result.Add(grpNNC);
        result.Add(grpSolEnvMat);        
        result.Add(grpSolViajes);
        result.Add(grpRDA);
        result.Add(grpStock);
        result.Add(grpAL);
        result.Add(grpPC);
        result.Add(grpIIO);
        result.Add(grpValeMateriales);
        result.Add(grpRoles);
        result.Add(grpSSM);
        result.Add(grpVehiculos);
        result.Add(grpAutorizaciones);
        result.Add(grpSNV);
        result.Add(grpGeneral);

        result.Sort();

        return result;
    }
    /// <summary>
    /// Actualiza los permisos de la persona.
    /// </summary>
    public static void ActualizarPermisosPersonal(int idPersonal, List<PermisosPersona> permisos)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = conn.BeginTransaction();

            // Borro los permisos existentes.
            IDbCommand cmd = DataAccess.GetCommand(conn, trans);
            cmd.CommandText = "DELETE FROM tbl_PermisosPersonal WHERE idPersonal = @idPersonal";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));

            cmd.ExecuteNonQuery();

            // Inserto los permisos.
            foreach (PermisosPersona permiso in permisos)
            {
                cmd = DataAccess.GetCommand(conn, trans);
                cmd.CommandText = "INSERT INTO tbl_PermisosPersonal (idPersonal, idPermiso) VALUES ";
                cmd.CommandText += "(@idPersonal, @idPermiso)";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPersonal", idPersonal));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idPermiso", (int)permiso));

                cmd.ExecuteNonQuery();
            }

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
    }
}
