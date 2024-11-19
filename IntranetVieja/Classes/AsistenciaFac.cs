using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

public class DetalleAsistencia
{
    // Variables.
    private int asistenciaID;
    private Persona persona;
    private DateTime fecha;
    private EstadoAsistencia estado;
    private string observacion;
    private DateTime horaEntrada;
    private DateTime horaSalida;
	private string modoEntrada;
	private string modoSalida;
    private bool llegoTarde;

	
    // Propiedades.
    public int ID
    {
        get { return asistenciaID; }
    }
    public Persona Persona
    {
        get { return persona; }
    }
    public DateTime Fecha
    {
        get { return fecha; }
    }
    public EstadoAsistencia Estado
    {
        get { return estado; }
    }
    public string Observacion
    {
        get { return observacion; }
    }
    public DateTime HoraEntrada
    {
        get { return horaEntrada; }
    }
    public DateTime HoraSalida
    {
        get { return horaSalida; }
    }
	public string ModoEntrada
    {
        get { return modoEntrada; }
    }
	public string ModoSalida
    {
        get { return modoSalida; }
    }
	
    public bool LlegoTarde
    {
        get { return llegoTarde; }
    }


    internal DetalleAsistencia(int asistenciaID, Persona persona, DateTime fecha, EstadoAsistencia estado, string observacion,
        DateTime horaEntrada, DateTime horaSalida, string modoEntrada = "", string modoSalida = "")
    {
        this.asistenciaID = asistenciaID;
        this.persona = persona;
        this.fecha = fecha;
        this.estado = estado;
        this.observacion = observacion;
        this.horaEntrada = horaEntrada;
        this.horaSalida = horaSalida;
		this.modoEntrada = modoEntrada;
		this.modoSalida = modoSalida;
		
		
        if (persona != null && estado == EstadoAsistencia.Presente && horaEntrada != Constantes.FechaInvalida)
        {
            DateTime horaEntradaNormal = new DateTime(horaEntrada.Year, horaEntrada.Month, horaEntrada.Day, persona.HoraEntrada.Hour,
                                                      persona.HoraEntrada.Minute, persona.HoraEntrada.Second);
            llegoTarde = (horaEntrada - horaEntradaNormal).Minutes > AsistenciaFac.ToleranciaTarde;

            if (llegoTarde)
            {
                string s = "";
            }
        }
        else llegoTarde = false;
    }
}

public class CargaAsistenciaEntrada
{
    // Variables.
    private int personalID;
    private EstadoAsistencia estado;
    private string observacion;
    private DateTime horaEntrada;

    // Propiedades.
    internal int PersonalID
    {
        get { return personalID; }
    }
    public EstadoAsistencia Estado
    {
        get { return estado; }
    }
    public string Observacion
    {
        get { return observacion; }
    }
    public DateTime HoraEntrada
    {
        get { return horaEntrada; }
    }


    public CargaAsistenciaEntrada(int personalID, EstadoAsistencia estado, string observacion, DateTime horaEntrada)
    {
        this.personalID = personalID;
        this.estado = estado;
        this.observacion = observacion;
        this.horaEntrada = horaEntrada;
    }
}

public class CargaAsistenciaSalida
{
    // Variables.
    private int personalID;
    private DateTime horaSalida;

    // Propiedades.
    internal int PersonalID
    {
        get { return personalID; }
    }
    public DateTime HoraSalida
    {
        get { return horaSalida; }
    }


    public CargaAsistenciaSalida(int personalID, DateTime horaSalida)
    {
        this.personalID = personalID;
        this.horaSalida = horaSalida;
    }
}

public class RenglonAsistenciaEntrada
{
    // Variables.
    private int personalID;
    private string personal;
    private EstadoAsistencia estado;
    private bool puedeCargarFecha;
    private string observacion;
	private int tipoCA;
	private string horaEntrada;

    // Propiedades.
    public int PersonalID
    {
        get { return personalID; }
    }
    public string Personal
    {
        get { return personal; }
    }
    public bool PuedeCargarFecha
    {
        get { return puedeCargarFecha; }
    }
    public EstadoAsistencia Estado
    {
        get { return estado; }
    }
    public string Observacion
    {
        get { return observacion; }
    }
	
	public int TipoCA
    {
        get { return tipoCA; }
    }
	
	public string HoraEntrada
    {
        get { return horaEntrada; }
    }

    internal RenglonAsistenciaEntrada(int personalID, string personal, EstadoAsistencia estado, bool puedeCargarFecha,
                                      string observacion, int tipoCA, string horaEntrada)
    {
        this.personalID = personalID;
        this.personal = personal;
        this.estado = estado;
        this.puedeCargarFecha = puedeCargarFecha;
        this.observacion = observacion;
		this.tipoCA = tipoCA;
		this.horaEntrada = horaEntrada;
    }
}

public class RenglonAsistenciaSalida
{
    // Variables.
    private int personalID;
    private string personal;
	private int tipoCA;
	private string horaSalida;

    // Propiedades.
    public int PersonalID
    {
        get { return personalID; }
    }
    public string Personal
    {
        get { return personal; }
    }
	
	public int TipoCA
    {
        get { return tipoCA; }
    }
	
	public string HoraSalida
    {
        get { return horaSalida; }
    }

    internal RenglonAsistenciaSalida(int personalID, string personal, int tipoCA, string horaSalida)
    {
        this.personalID = personalID;
        this.personal = personal;
		this.tipoCA = tipoCA;
		this.horaSalida = horaSalida;
    }
}

/// <summary>
/// Descripción breve de AsistenciaFac
/// </summary>
public static class AsistenciaFac
{
    // Constantes.
    internal const int ToleranciaTarde = 0;


    public static void AddAsistenciaEntrada(DateTime fecha, List<CargaAsistenciaEntrada> asistencias)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            asistencias.ForEach(detalle => AddAsistenciaEntrada(fecha, detalle.PersonalID, detalle.Estado, detalle.Observacion,
                                                         detalle.HoraEntrada, Constantes.FechaInvalida, trans));

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

    internal static int AddAsistenciaEntrada(DateTime fecha, int personalID, EstadoAsistencia estado, string observacion,
                                             DateTime horaEntrada, DateTime horaSalida, int idLicencia, IDbTransaction trans)
    {
        return AddAsistenciaEntrada(fecha, personalID, estado, observacion, horaEntrada, horaSalida, true, idLicencia, trans); 
		
    }
	
	internal static int AddAsistenciaEntrada(DateTime fecha, int personalID, EstadoAsistencia estado, string observacion,
                                             DateTime horaEntrada, DateTime horaSalida, IDbTransaction trans)
    {
        return AddAsistenciaEntrada(fecha, personalID, estado, observacion, horaEntrada, horaSalida, true, trans); 
		
    }

    internal static int AddLicencia(DateTime fecha, int personalID, EstadoAsistencia estado, string observacion, 
        IDbTransaction trans)
    {
        return AddAsistenciaEntrada(fecha, personalID, estado, observacion, Constantes.FechaInvalida,
                                    Constantes.FechaInvalida, false, trans);
    }
	
	internal static int AddAsistenciaEntrada(DateTime fecha, int personalID, EstadoAsistencia estado, string observacion, 
        DateTime horaEntrada, DateTime horaSalida, bool procesado, int idLicencia, IDbTransaction trans)
    {
        int result;

        // Verifico si existe.
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "SELECT AsistenciaID FROM tbl_Asistencia WHERE PersonalID = @PersonalID AND Fecha = @Fecha";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(fecha)));
        result = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (result == 0)
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Asistencia (PersonalID, Fecha, EstadoID, Observacion, HoraEntrada, ";
            cmd.CommandText += "HoraSalida, ProcesadoID, idLicencia) VALUES (@PersonalID, @Fecha, @EstadoID, @Observacion, @HoraEntrada, ";
            cmd.CommandText += "@HoraSalida, @ProcesadoID, @LicenciaID); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Asistencia;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(fecha)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)estado));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observacion", observacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraEntrada", Convert.ToInt16(horaEntrada.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraSalida", Convert.ToInt16(horaSalida.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ProcesadoID",
                                                           procesado
                                                               ? (int) ProcesoAsistencia.Procesada
                                                               : (int) ProcesoAsistencia.SinProcesar));
															   
		    cmd.Parameters.Add(DataAccess.GetDataParameter("@LicenciaID", idLicencia));
															   
															   
															   
            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        else
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE tbl_Asistencia SET HoraEntrada = @HoraEntrada, ProcesadoID = @ProcesadoID, idLicencia = @LicenciaID ";
            cmd.CommandText += "WHERE AsistenciaID = @AsistenciaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AsistenciaID", result));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraEntrada", Convert.ToInt16(horaEntrada.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ProcesadoID", (int)ProcesoAsistencia.Procesada));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@LicenciaID", idLicencia));
			
            cmd.ExecuteNonQuery();
        }

        return result;
    }

    internal static int AddAsistenciaEntrada(DateTime fecha, int personalID, EstadoAsistencia estado, string observacion, 
        DateTime horaEntrada, DateTime horaSalida, bool procesado, IDbTransaction trans)
    {
        int result;

        // Verifico si existe.
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "SELECT AsistenciaID FROM tbl_Asistencia WHERE PersonalID = @PersonalID AND Fecha = @Fecha";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(fecha)));
        result = Convert.ToInt32(cmd.ExecuteScalar());
        
        if (result == 0)
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "INSERT INTO tbl_Asistencia (PersonalID, Fecha, EstadoID, Observacion, HoraEntrada, ";
            cmd.CommandText += "HoraSalida, ProcesadoID) VALUES (@PersonalID, @Fecha, @EstadoID, @Observacion, @HoraEntrada, ";
            cmd.CommandText += "@HoraSalida, @ProcesadoID); ";
            cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_Asistencia;";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(fecha)));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)estado));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observacion", observacion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraEntrada", Convert.ToInt16(horaEntrada.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraSalida", Convert.ToInt16(horaSalida.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ProcesadoID",
                                                           procesado
                                                               ? (int) ProcesoAsistencia.Procesada
                                                               : (int) ProcesoAsistencia.SinProcesar));
            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        else
        {
            cmd = DataAccess.GetCommand(trans);
            cmd.CommandText = "UPDATE tbl_Asistencia SET HoraEntrada = @HoraEntrada, ProcesadoID = @ProcesadoID, Observacion = @Observacion ";
            cmd.CommandText += "WHERE AsistenciaID = @AsistenciaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AsistenciaID", result));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraEntrada", Convert.ToInt16(horaEntrada.ToString("HHmm"))));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Observacion", observacion));
			cmd.Parameters.Add(DataAccess.GetDataParameter("@ProcesadoID", (int)ProcesoAsistencia.Procesada));
			
            cmd.ExecuteNonQuery();
        }

        return result;
    }

    public static void AddAsistenciaSalida(DateTime fecha, List<CargaAsistenciaSalida> asistencias)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            asistencias.ForEach(detalle => AddAsistenciaSalida(fecha, detalle.PersonalID, detalle.HoraSalida, trans));

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

    internal static void AddAsistenciaSalida(DateTime fecha, int personalID, DateTime horaSalida, IDbTransaction trans)
    {
        IDbCommand cmd = DataAccess.GetCommand(trans);
        cmd.CommandText = "UPDATE tbl_Asistencia SET HoraSalida = @HoraSalida, ProcesadoID = @ProcesadoID ";
        cmd.CommandText += "WHERE PersonalID = @PersonalID AND Fecha = @Fecha";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", personalID));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", Funciones.GetDate(fecha)));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraSalida", Convert.ToInt16(horaSalida.ToString("HHmm"))));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@ProcesadoID", (int)ProcesoAsistencia.Procesada));
        cmd.ExecuteNonQuery();
    }

    public static void UpdateAsistencia(int asistenciaID, int personalID, DateTime fecha, EstadoAsistencia estado,
                                        string observacion, int hEntrada, int hSalida)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        IDbCommand cmd;

        if (estado != EstadoAsistencia.Presente && String.IsNullOrEmpty(observacion))
        {
            throw new InvalidDataException();
        }

        DateTime now = DateTime.Now;
        DateTime horaEntrada;
        DateTime horaSalida;

        try
        {
            horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada/100,
                                       hEntrada - ((int) hEntrada/100)*100, 0);
            horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida/100,
                                      hSalida - ((int) hSalida/100)*100, 0);
        }
        catch
        {
            throw new Exception("El horario de entrada o salida no es válido.");
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            if (asistenciaID == Constantes.ValorInvalido)
            {
                asistenciaID = AddAsistenciaEntrada(fecha, personalID, estado, observacion, horaEntrada, horaSalida, trans);
            }
            else
            {
                cmd = DataAccess.GetCommand(trans);
                cmd.CommandText = "UPDATE tbl_Asistencia SET EstadoID = @EstadoID, Observacion = @Observacion, ";
                cmd.CommandText += "HoraEntrada = @HoraEntrada, HoraSalida = @HoraSalida WHERE AsistenciaID = @AsistenciaID";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@AsistenciaID", asistenciaID));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@EstadoID", (int)estado));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Observacion", observacion));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraEntrada", Convert.ToInt16(horaEntrada.ToString("HHmm"))));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraSalida", Convert.ToInt16(horaSalida.ToString("HHmm"))));
                cmd.ExecuteNonQuery();
            }

            trans.Commit();
        }
        catch
        {
            if (trans != null) trans.Rollback();
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

    public static List<RenglonAsistenciaEntrada> GetCargaAsistenciaEntrada(int responsableID, DateTime fecha)
    {
        // Se tienen que mostrar solamente aquellos de los que no se cargó información para el día actual.
        List<RenglonAsistenciaEntrada> result = new List<RenglonAsistenciaEntrada>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;
		
        fecha = Funciones.GetDate(fecha);

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT ISNULL(a.HoraEntrada, 0) AS HoraEntrada, p.idPersonal, b.TipoCA, p.Nombre, ISNULL(a.EstadoID, 0) AS EstadoID, a.Observacion, ";
            cmd.CommandText += "(CASE WHEN a.EstadoID = @LicenciaID THEN ";
            cmd.CommandText += "(SELECT TOP 1 l.idTipo FROM tbl_PDLicencias l WHERE a.Fecha BETWEEN l.FechaInicio AND l.FechaFin ";
            cmd.CommandText += "AND l.idSolicito = p.idPersonal) ELSE @TipoLicencia END) AS TipoLicenciaID ";
            cmd.CommandText += "FROM tbl_Personal p ";
            cmd.CommandText += "LEFT JOIN tbl_Asistencia a ON a.PersonalID = p.idPersonal AND a.Fecha = @Fecha ";
            cmd.CommandText += "INNER JOIN tbl_Bases b ON b.ResponsableID = @ResponsableID OR b.AlternateID = @ResponsableID ";
            cmd.CommandText += "AND b.Activa = @Activa ";
            cmd.CommandText += "INNER JOIN tbl_BasesPersonal bp ON bp.BaseID = b.BaseID AND bp.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE (a.AsistenciaID IS NULL OR b.TipoCA = 1 OR (a.EstadoID <> @PresenteID AND a.HoraEntrada = @HoraInvalida AND ";
            cmd.CommandText += "a.ProcesadoID = @SinProcesar))AND p.Activo = @Activo ";
            cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PresenteID", (int)EstadoAsistencia.Presente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraInvalida", 0));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableID", responsableID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@LicenciaID", (int)EstadoAsistencia.Licencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoLicencia", (int)TiposLicencia.SinGoceHaberes));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@SinProcesar", (int)ProcesoAsistencia.SinProcesar));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int personalID = Convert.ToInt32(dr["idPersonal"]);
                string nombre = dr["Nombre"].ToString().ToUpper();
				int tipoCA = Convert.ToInt32(dr["TipoCA"]);
				string horaEntrada = dr["HoraEntrada"].ToString();
				
                try
                {
                    EstadoAsistencia estado = (EstadoAsistencia)Convert.ToInt32(dr["EstadoID"]);
                    TiposLicencia tipoLicencia = (TiposLicencia)Convert.ToInt32(dr["TipoLicenciaID"]);
                    bool puedeCargarFecha = estado == EstadoAsistencia.Ausente || tipoLicencia == TiposLicencia.ModificacionHorario;
                    string observacion = dr["Observacion"] == null ? "" : dr["Observacion"].ToString();

                    if (!puedeCargarFecha && tipoCA == 0)
                    {
                        continue;
                    }
					
					string fmtHoraEntrada = FormatInfoHour(horaEntrada);

                    result.Add(new RenglonAsistenciaEntrada(personalID, nombre, estado, puedeCargarFecha, observacion, tipoCA, fmtHoraEntrada ));
                }
                catch
                {
                    
                }
            }

            dr.Close();
        }
        catch
        {
            result.Clear();
        }
        finally
        {
            if(dr != null && !dr.IsClosed) dr.Close();
            if (conn != null) conn.Close();
        }

        return result;
    }


    public static string FormatInfoHour(string inHour)
	{
		string result = "";
		int hlen = inHour.Length;

		if (hlen == 3)
		{
			result = string.Format("{0}:{1}", inHour.Substring(0, 1), inHour.Substring(1, 2));
		}

		if (hlen == 4)
		{
			result = string.Format("{0}:{1}", inHour.Substring(0, 2), inHour.Substring(2, 2));
		}

		return result;
	}
	

    public static List<RenglonAsistenciaSalida> GetCargaAsistenciaSalida(int responsableID, DateTime fecha)
    {
        // Se tienen que mostrar solamente aquellos de los que se cargó información para el día actual.
        List<RenglonAsistenciaSalida> result = new List<RenglonAsistenciaSalida>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        fecha = Funciones.GetDate(fecha);
		
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT ISNULL(a.HoraSalida, 0) AS HoraSalida, p.idPersonal, p.Nombre, b.TipoCA ";
            cmd.CommandText += "FROM tbl_Personal p ";
            cmd.CommandText += "LEFT JOIN tbl_Asistencia a ON a.PersonalID = p.idPersonal AND a.Fecha = @Fecha ";
            cmd.CommandText += "INNER JOIN tbl_Bases b ON b.ResponsableID = @ResponsableID OR b.AlternateID = @ResponsableID ";
            cmd.CommandText += "AND b.Activa = @Activa ";
            cmd.CommandText += "INNER JOIN tbl_BasesPersonal bp ON bp.BaseID = b.BaseID AND bp.PersonalID = p.idPersonal ";
            cmd.CommandText += "WHERE (a.EstadoID = @PresenteID OR a.EstadoID = @LicenciaID) AND p.Activo = @Activo ";
            cmd.CommandText += "AND a.HoraEntrada <> @HoraInvalida ";
			cmd.CommandText += "ORDER BY p.Nombre";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", fecha));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activo", true));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PresenteID", (int)EstadoAsistencia.Presente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@LicenciaID", (int)EstadoAsistencia.Licencia));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@ResponsableID", responsableID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@HoraInvalida", 0));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AusenteID", (int)EstadoAsistencia.Ausente));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@TipoLicencia", (int)TiposLicencia.SinGoceHaberes));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Activa", true));
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int personalID = Convert.ToInt32(dr["idPersonal"]);
                string nombre = dr["Nombre"].ToString().ToUpper();
				int tipoCA = Convert.ToInt32(dr["TipoCA"]);
				string horaSalida = dr["HoraSalida"].ToString();
						
				// if(horaSalida != "0" && tipoCA == 0)
				// {
					// continue;				
				// }
						
                string fmtHoraSalida = FormatInfoHour(horaSalida);

                result.Add(new RenglonAsistenciaSalida(personalID, nombre, tipoCA, fmtHoraSalida));
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

    internal static List<DetalleAsistencia> GetDetalleAsistencia(Persona persona, DateTime fecha, int dias, IDbConnection conn)
    {
        List<DetalleAsistencia> result = new List<DetalleAsistencia>();
        IDbCommand cmd;
        IDataReader dr = null;

        fecha = new DateTime(fecha.Year, fecha.Month, fecha.Day);
        DateTime fBase = fecha.AddDays(-dias + 1);
        DateTime now = DateTime.Now;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT AsistenciaID, Fecha, EstadoID, Observacion, HoraEntrada, HoraSalida, ModoEntrada, ModoSalida FROM tbl_Asistencia ";
            cmd.CommandText += "WHERE PersonalID = @PersonalID AND Fecha BETWEEN @FechaInicio AND @FechaFin ";
            cmd.CommandText += "ORDER BY Fecha";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@PersonalID", persona.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaInicio", fBase));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaFin", fecha));
            dr = cmd.ExecuteReader();

            DateTime f = Constantes.FechaInvalida;
            for (int i = 0; i < dias; i++)
            {
                if (i == 0 && dr.Read()) f = Convert.ToDateTime(dr["Fecha"]);

                DateTime fechaActual = fBase.AddDays(i);
                if (f == fechaActual)
                {
                    int hEntrada = Convert.ToInt32(dr["HoraEntrada"]);
                    int hSalida = Convert.ToInt32(dr["HoraSalida"]);
					int mEntrada = Convert.ToInt32(dr["ModoEntrada"]);
					int mSalida = Convert.ToInt32(dr["ModoSalida"]);
				

                    DateTime horaEntrada = new DateTime(f.Year, f.Month, f.Day, hEntrada / 100,
                                                        hEntrada - ((int)hEntrada / 100) * 100, 0);
                    DateTime horaSalida = new DateTime(f.Year, f.Month, f.Day, hSalida / 100,
                                                        hSalida - ((int)hSalida / 100) * 100, 0);
					
					
				    string strModoEntrada = "";
					string strModoSalida = "";
					
					switch(mEntrada)
					{
						case 1:
						strModoEntrada = "Huella";
						break;
						case 4:
						strModoEntrada = "Clave Numérica";
						break;
						case 8:
						strModoEntrada = "Tarjeta";
						break;
						default:
						strModoEntrada = "";
						break;
					}
					
					switch(mSalida)
					{
						case 1:
						strModoSalida = "Huella";
						break;
						case 4:
						strModoSalida = "Clave Numérica";
						break;
						case 8:
						strModoSalida = "Tarjeta";
						break;
						default:
						strModoSalida = "";
						break;
					}

                    result.Add(new DetalleAsistencia(Convert.ToInt32(dr["AsistenciaID"]), persona, f,
                                                     (EstadoAsistencia)Convert.ToInt32(dr["EstadoID"]),
                                                     dr["Observacion"].ToString(), horaEntrada, horaSalida, strModoEntrada, strModoSalida));
                    if (dr.Read()) f = Convert.ToDateTime(dr["Fecha"]);
                }
                else
                {
                    result.Add(new DetalleAsistencia(Constantes.ValorInvalido, persona, fechaActual,
                                                     EstadoAsistencia.Ausente, "", Constantes.FechaInvalida,
                                                     Constantes.FechaInvalida, "", ""));
                }
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
        }

        return result;
    }

    public static DetalleAsistencia GetDetalleAsistencia(int asistenciaID)
    {
        DetalleAsistencia result = null;
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_Asistencia WHERE AsistenciaID = @AsistenciaID";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AsistenciaID", asistenciaID));
            dr = cmd.ExecuteReader();

            dr.Read();
            DateTime now = DateTime.Now;
            int hEntrada = Convert.ToInt32(dr["HoraEntrada"]);
            int hSalida = Convert.ToInt32(dr["HoraSalida"]);
			int mEntrada = Convert.ToInt32(dr["ModoEntrada"]);
		    int mSalida = Convert.ToInt32(dr["ModoSalida"]);

            DateTime horaEntrada = new DateTime(now.Year, now.Month, now.Day, hEntrada / 100,
                                                hEntrada - ((int)hEntrada / 100) * 100, 0);
            DateTime horaSalida = new DateTime(now.Year, now.Month, now.Day, hSalida / 100,
                                                hSalida - ((int)hSalida / 100) * 100, 0);
												
            string strModoEntrada = "";
					string strModoSalida = "";
					
					switch(mEntrada)
					{
						case 1:
						strModoEntrada = "Huellas";
						break;
						case 4:
						strModoEntrada = "Clave Numérica";
						break;
						default:
						strModoEntrada = "Desconocido";
						break;
					}
					
					switch(mSalida)
					{
						case 1:
						strModoSalida = "Huellas";
						break;
						case 4:
						strModoSalida = "Clave Numérica";
						break;
						default:
						strModoSalida = "Desconocido";
						break;
					}

            result = new DetalleAsistencia(Convert.ToInt32(dr["AsistenciaID"]), null, Convert.ToDateTime(dr["Fecha"]),
                                           (EstadoAsistencia) Convert.ToInt32(dr["EstadoID"]),
                                           dr["Observacion"].ToString(), horaEntrada, horaSalida, strModoEntrada, strModoSalida);
            dr.Close();
        }
        catch
        {
            result = null;
        }
        finally
        {
            if (dr != null && !dr.IsClosed) dr.Close();
            if(conn != null) conn.Close();
        }

        return result;
    }
}