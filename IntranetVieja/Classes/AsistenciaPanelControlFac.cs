using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

public class AsistenciaPanelControl
{
    // Variables.
    private Base _base;
    private List<RenglonPanelControlAsistencia> datos;

    // Propiedades.
    public Base Base
    {
        get { return _base; }
    }
    public List<RenglonPanelControlAsistencia> Datos
    {
        get { return datos; }
    }


    internal AsistenciaPanelControl(Base _base, List<RenglonPanelControlAsistencia> datos)
    {
        this._base = _base;
        this.datos = datos;
    }
}

public class RenglonPanelControlAsistencia
{
    // Variables.
    private Persona persona;
    private List<DetalleAsistencia> datos;

    // Propiedades.
    public Persona Persona
    {
        get { return persona; }
    }
    public List<DetalleAsistencia> Datos
    {
        get { return datos; }
    }


    internal RenglonPanelControlAsistencia(Persona persona, List<DetalleAsistencia> datos)
    {
        this.persona = persona;
        this.datos = datos;
    }
}

public static class AsistenciaPanelControlFac
{
    // Constantes.
    public const int DiasPanelControl = 7;

    public static List<AsistenciaPanelControl> GetPanelesControl(DateTime fecha)
    {
        List<AsistenciaPanelControl> result = new List<AsistenciaPanelControl>();

        List<Base> bases = BaseFac.GetBasesActivas();

        IDbConnection conn = null;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            bases.ForEach(_base =>
                {
                    List<RenglonPanelControlAsistencia> renglones = new List<RenglonPanelControlAsistencia>();
                    _base.Integrantes.ForEach(p =>
                        {
                            List<DetalleAsistencia> datos = AsistenciaFac.GetDetalleAsistencia(p, fecha,
                                                                                               DiasPanelControl, conn);
                            renglones.Add(new RenglonPanelControlAsistencia(p, datos));
                        });

                    result.Add(new AsistenciaPanelControl(_base, renglones));
                });
        }
        catch
        {

        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }

    public static void UpdatePcResponsable(int responsableId, List<int> personas)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Borro las personas asignadas.
            IDbCommand cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "DELETE FROM tbl_AsistPcPersonas WHERE ResponsableId = @ResponsableId";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
            cmd.ExecuteNonQuery();

            // Agrego las personas asignadas.
            personas.ForEach(p =>
                {
                    cmd = DataAccess.GetCommand(trans);
                    cmd.CommandText = "INSERT INTO tbl_AsistPcPersonas (ResponsableId, PersonaId) VALUES ";
                    cmd.CommandText += "(@ResponsableId, @PersonaId)";
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
                    cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonaId", p));
                    cmd.ExecuteNonQuery();
                });

            trans.Commit();
        }
        catch
        {
            if (trans != null) trans.Rollback();
            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null) conn.Close();
        }
    }

    public static List<Persona> GetPersonasPcResponsable(int responsableId)
    {
        List<Persona> result = new List<Persona>();

        IDbConnection conn = null;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            result = GetPersonasPcResponsable(responsableId, conn);
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }

    private static List<Persona> GetPersonasPcResponsable(int responsableId, IDbConnection conn)
    {
        List<Persona> result = new List<Persona>();
        IDataReader dr = null;      

        try
        {
            IDbCommand cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT p.*, 1 AS BaseID FROM tbl_Personal p ";
            cmd.CommandText += "INNER JOIN tbl_PersonalLegajos l ON l.PersonalID = p.idPersonal ";
            cmd.CommandText += "INNER JOIN tbl_AsistPcPersonas a ON a.PersonaID = p.idPersonal ";
            cmd.CommandText += "WHERE a.ResponsableId = @ResponsableId AND l.Activo = @Activo AND p.Activo = @Activo ";
            cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableId", responsableId));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Persona persona = GPersonal.GetPersona(dr);
                if (persona != null) result.Add(persona);
            }
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
        }

        return result;
    }

    public static List<AsistenciaPanelControl> GetPcResponsable(int responsableId, DateTime fecha)
    {
        List<AsistenciaPanelControl> result = new List<AsistenciaPanelControl>();

        IDbConnection conn = null;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            List<Persona> personas = GetPersonasPcResponsable(responsableId, conn);

            List<RenglonPanelControlAsistencia> renglones = new List<RenglonPanelControlAsistencia>();
            personas.ForEach(p =>
            {
                List<DetalleAsistencia> datos = AsistenciaFac.GetDetalleAsistencia(p, fecha, DiasPanelControl, conn);
                renglones.Add(new RenglonPanelControlAsistencia(p, datos));
            });

            result.Add(new AsistenciaPanelControl(null, renglones));
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if (conn != null) conn.Close();
        }

        return result;
    }
}