/*
 * Historial:
 * ===================================================================================
 * [24/06/2011]
 * - Se puede agregar más de 1 responsable para cada área.
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;


public class ResponsableArea
{
    // Variables.
    private int idResponsable;
    private Persona responsable;

    // Propiedades.
    public int IdResponsable
    {
        get { return this.idResponsable; }
    }
    public Persona Responsable
    {
        get
        {
            if (this.responsable == null)
            {
                this.responsable = GPersonal.GetPersona(this.idResponsable);
            }

            return this.responsable;
        }
    }


    internal ResponsableArea(int idResponsable)
    {
        this.idResponsable = idResponsable;
    }
}
/// <summary>
/// Descripción breve de Area.
/// </summary>
public class Area
{
    // Variables.
    private int idArea;
    private string descripcion;
    private List<ResponsableArea> responsables;

    // Propiedades.
    /// <summary>
    /// Obtiene el ID.
    /// </summary>
    public int ID
    {
        get { return idArea; }
    }
    /// <summary>
    /// Obtiene la Descripción.
    /// </summary>
    public string Descripcion
    {
        get { return descripcion; }
    }
    /// <summary>
    /// Obtiene los Responsables del Área.
    /// </summary>
    public List<ResponsableArea> Responsables
    {
        get { return this.responsables; }
    }
    /// <summary>
    /// Obtiene si el Área existe.
    /// </summary>
    public bool Existe
    {
        get { return this.idArea == Constantes.ValorInvalido; }
    }


    internal Area(int idArea, string descripcion)
    {
        this.idArea = idArea;
        this.descripcion = descripcion;
        this.responsables = new List<ResponsableArea>();
    }
    /// <summary>
    /// Obtiene si la persona actual es responsable de área.
    /// </summary>
    public bool EsResponsable()
    {
        bool result = false;
        
        foreach (ResponsableArea responsable in this.responsables)
        {
            if (responsable.IdResponsable == Constantes.Usuario.ID)
            {
                result = true;
                break;
            }
        }

        if (!result)
        {
            result = GPermisosPersonal.TieneAcceso(PermisosPersona.NNCAdministrador);
        }

        return result;
    }
    /// <summary>
    /// Carga los responsables de área.
    /// </summary>
    public void CargarResponsables()
    {
        this.responsables = GAreas.GetResponsablesArea(this.idArea);
    }
    /// <summary>
    /// Obtiene la lista de emails de los responsables de área separados por ";".
    /// </summary>
    public string GetEmailsResponsables()
    {
        string result = "";

        foreach (ResponsableArea responsable in this.responsables)
        {
            result += responsable.Responsable.Email + ",";
        }
        result = result.TrimEnd(',');

        return result;
    }
}

/// <summary>
/// Descripción breve de GAreas.
/// </summary>
public class GAreas
{
    /// <summary>
    /// Obtiene un área.
    /// </summary>
    private static Area GetArea(IDataReader dr, bool cargarResponsables)
    {
        Area result;

        try
        {
            result = new Area(Convert.ToInt32(dr["idArea"]),
                dr["Descripcion"].ToString());

            if (cargarResponsables)
            {
                result.CargarResponsables();
            }
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Obtiene los responsables de área.
    /// </summary>
    internal static List<ResponsableArea> GetResponsablesArea(int idArea)
    {
        List<ResponsableArea> result = new List<ResponsableArea>();
        IDbConnection conn = null;
        IDataReader dr;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idResponsable FROM tbl_AreasResponsables WHERE idArea = @idArea";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idArea", idArea));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                result.Add(new ResponsableArea(Convert.ToInt32(dr["idResponsable"])));
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


        return result;
    }
    /// <summary>
    /// Obtiene un área.
    /// </summary>
    public static Area GetArea(int idArea)
    {
        IDataReader dr;
        IDbConnection conn = null;
        IDbCommand cmd;
        Area result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Areas WHERE idArea = @idArea";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idArea", idArea));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new Exception("No se ha encontrado el Area de Responsabilidad");
            }

            result = GetArea(dr, true);

            dr.Close();
        }
        catch
        {
            result = new Area(Constantes.ValorInvalido, "No encontrada");
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
    /// Obtiene las Áreas disponibles.
    /// </summary>
    public static List<Area> GetAreas()
    {
        List<Area> result = new List<Area>();
        IDataReader dr;
        IDbConnection conn = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Areas ORDER BY Descripcion";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Area area = GetArea(dr, false);

                if (area != null)
                {
                    result.Add(area);
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

        return result;
    }
}
