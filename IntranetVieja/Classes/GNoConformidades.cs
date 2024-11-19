/*
 * Historial:
 * ===================================================================================
 * [01/07/2011]
 * - Al cerrar una No Conformidad, le llega una copia de la misma al emisor.
 * [13/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// Clase para el manejo de No Conformidad.
/// </summary>
public partial class NoConformidad
{
    // Variables.
    private int idNotaNoConformidad;
    private int numero;
    private DateTime fechaEmision;
    private string asunto;
    private bool normaISO9001;
    private bool normaISO14001;
    private bool normaOHSAS18001;
    private bool normalIRAM301;
    private bool revMatrizRiesgo;
    private string apartado;
    private CategoriasNC categoria;
    private Persona emitidaPor;
    private Area area;
    private string equipo;
    private string hallazgo;
    private string accionInmediata;
    private string causasRaices;
    private string accionCorrectiva;
    private ConclusionesNC conclusion;
    private string comentarios;
    private Persona firmaCierre;
    private DateTime fechaCierre;
    private EstadosNC estado;

    // Propiedades.
    public int ID
    {
        get { return idNotaNoConformidad; }
    }
    public int Numero
    {
        get { return numero; }
    }
    public DateTime FechaEmision
    {
        get { return fechaEmision; }
    }
    public string Asunto
    {
        get { return asunto; }
    }
    public bool NormaISO9001
    {
        get { return normaISO9001; }
    }
    public bool NormaISO14001
    {
        get { return normaISO14001; }
    }
    public bool NormaOHSAS18001
    {
        get { return normaOHSAS18001; }
    }
    public bool NormaIRAM301
    {
        get { return normalIRAM301; }
    }
    public bool RevMatrizRiesgo
    {
        get { return revMatrizRiesgo; }
    }
    public string Apartado
    {
        get { return apartado; }
    }
    public CategoriasNC Categoria
    {
        get { return categoria; }
    }
    public Persona EmitidaPor
    {
        get { return emitidaPor; }
    }
    public Area Area
    {
        get { return area; }
    }
    public string Equipo
    {
        get { return equipo; }
    }
    public string Hallazgo
    {
        get { return hallazgo; }
    }
    public string AccionInmediata
    {
        get { return accionInmediata; }
    }
    public string CausasRaices
    {
        get { return causasRaices; }
    }
    public string AccionCorrectiva
    {
        get { return accionCorrectiva; }
    }
    public ConclusionesNC Conclusion
    {
        get { return conclusion; }
    }
    public string Comentarios
    {
        get { return comentarios; }
    }
    public Persona FirmaCierre
    {
        get { return firmaCierre; }
    }
    public DateTime FechaCierre
    {
        get { return fechaCierre; }
    }
    public EstadosNC Estado
    {
        get { return estado; }
    }


    /// <summary>
    /// Carga una No Conformidad a partir de un ID existente.
    /// </summary>
    internal NoConformidad(int idNotaNoConformidad, int numero, DateTime fechaEmision, string asunto, bool normaISO9001, 
        bool normaISO14001, bool normaOHSAS18001, bool normaIRAM301, bool revisionMatrizRiesgo, string apartado, CategoriasNC categoria, 
        Persona emitidaPor, Area area, string equipo, string hallazgo, string accionInmediata,
        string causasRaices, string accionCorrectiva, ConclusionesNC conclusion, string comentarios,
        Persona firmaCierre, DateTime fechaCierre, EstadosNC estado)
    {
        this.idNotaNoConformidad = idNotaNoConformidad;
        this.numero = numero;
        this.fechaEmision = fechaEmision;
        this.asunto = asunto;
        this.normaISO9001 = normaISO9001;
        this.normaISO14001 = normaISO14001;
        this.normaOHSAS18001 = normaOHSAS18001;
        this.normalIRAM301 = normaIRAM301;
        this.revMatrizRiesgo = revisionMatrizRiesgo;
        this.apartado = apartado;
        this.categoria = categoria;
        this.emitidaPor = emitidaPor;
        this.area = area;
        this.equipo = equipo;
        this.hallazgo = hallazgo;
        this.accionInmediata = accionInmediata;
        this.causasRaices = causasRaices;
        this.accionCorrectiva = accionCorrectiva;
        this.conclusion = conclusion;
        this.comentarios = comentarios;
        this.firmaCierre = firmaCierre;
        this.fechaCierre = fechaCierre;
        this.estado = estado;
    }
    /// <summary>
    /// Obtiene el número de la NC.
    /// </summary>
    public string GetNumero()
    {
        string result;

        result = String.Format("000{0}-{1:000000}", this.asunto.StartsWith("RECLAMO") ? 2 : 1, this.Numero);

        return result;
    }
    /// <summary>
    /// Devuelve el estado de una NC en formato cadena.
    /// </summary>
    public string GetEstado()
    {
        switch (this.estado)
        {
            case EstadosNC.ProcesandoImputado:
                return "Procesando Imp";
            case EstadosNC.ProcesandoSGI:
                return "Procesando SGI";
            case EstadosNC.EsperandoCierre:
                return "Esperando cierre";
            case EstadosNC.Cerrada:
                return this.conclusion == ConclusionesNC.NoCorresponde ? "No corresponde" : "Cerrada";
        }

        return "No valido";
    }
}

/// <summary>
/// Descripción breve de GNoConformidades
/// </summary>
public static class GNoConformidades
{
    // Constantes.
    private const int MaxRegistrosPagina = 20;
    private const string QuerySelect = "SELECT idNC, Numero, Fecha, NormaISO9001, NormaISO14001, NormaOHSAS18001, NormaIRAM301, RevMatrizRiesgo, "
                                     + "Apartado, Categoria, idEmitidaPor, idAreaResponsabilidad, Equipo, Hallazgo, AccionInmediata, "
                                     + "CausasRaices, AccionCorrectiva, Conclusion, Comentarios, idFirmaCierre, " 
                                     + "FechaFirma, Estado, Asunto FROM tbl_NoConformidades nc ";
    
    /// <summary>
    /// Guarda una No Conformidad.
    /// </summary>
    public static void NuevaNC(string asunto, string equipo, string hallazgo, string accionInmediata, string comentarios, 
        out int numero)
    {
        IDbCommand cmd;
        IDbConnection conn = null;

        if (asunto.Trim().Length == 0 || equipo.Trim().Length == 0 || hallazgo.Trim().Length == 0 || 
            accionInmediata.Trim().Length == 0)
        {
            throw new Exception("Todos los campos, excepto los comentarios, deben ser completados.");
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "INSERT INTO tbl_NoConformidades (Numero, Fecha, idEmitidaPor, Equipo, ";
            cmd.CommandText += "Hallazgo, AccionInmediata, Comentarios, Estado, Asunto) ";
            cmd.CommandText += "VALUES (@Numero, @Fecha, @EmitidaPor, @Equipo, @Hallazgo, ";
            cmd.CommandText += "@AccionInmediata, @Comentarios, @Estado, @Asunto);";
            cmd.CommandText += "SELECT Numero FROM tbl_NoConformidades WHERE idNC = SCOPE_IDENTITY();";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", GetUltimoNumeroNC(conn) + 1));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", DateTime.Now));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@EmitidaPor", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Equipo", equipo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Hallazgo", hallazgo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AccionInmediata", accionInmediata));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Estado", (int)EstadosNC.ProcesandoSGI));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Comentarios", comentarios));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Asunto", asunto));

            numero = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            numero = Constantes.ValorInvalido;

            throw new ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        EnviarNC(asunto, equipo, hallazgo, accionInmediata, comentarios, numero, Constantes.Usuario.Nombre, Constantes.Usuario.Email,
            Constantes.EmailCalidad, "", "Solicitud", false);
    }
    /// <summary>
    /// Obtiene el último número de NC.
    /// </summary>
    private static int GetUltimoNumeroNC()
    {
        int ultimo;

        IDbConnection conn = null;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);

            ultimo = GetUltimoNumeroNC(conn);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        return ultimo;
    }
    /// <summary>
    /// Obtiene el último número de NC.
    /// </summary>
    private static int GetUltimoNumeroNC(IDbConnection conn)
    {
        int ultimo;
        IDbCommand cmd;

        try
        {
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT TOP 1 Numero FROM tbl_NoConformidades ORDER BY Numero DESC";

            ultimo = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (InvalidCastException)
        {
            // No hay NC ingresadas.
            ultimo = 0;
        }
        catch
        {
            throw new Exception("Error al obtener el Número.");
        }

        return ultimo;
    }
    /// <summary>
    /// Obtiene el ID de la NC.
    /// </summary>
    public static int GetIdNC(int nroNC)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        int id = -1;
        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idNC FROM tbl_NoConformidades WHERE Numero=@Numero";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", nroNC));

            id = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            id = Constantes.ValorInvalido;
        }
        finally
        {
            if(conn != null)
            {
            conn.Close();
            }
        }

        return id;
    }
    /// <summary>
    /// Obtiene una No Conformidad.
    /// </summary>
    public static NoConformidad GetNoConformidad(int idNC)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        NoConformidad result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = QuerySelect + "WHERE idNC = @idNC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idNC", idNC));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                dr.Close();
                throw new Exception("La NC no existe.");
            }

            result = new NoConformidad(
                idNC,
                Convert.ToInt32(dr["Numero"]),
                DateTime.Parse(dr["Fecha"].ToString()),
                dr["Asunto"].ToString().Trim(),
                Convert.ToBoolean(dr["NormaISO9001"]),
                Convert.ToBoolean(dr["NormaISO14001"]),
                Convert.ToBoolean(dr["NormaOHSAS18001"]),
                Convert.ToBoolean(dr["NormaIRAM301"]),
                Convert.ToBoolean(dr["RevMatrizRiesgo"]),
                dr["Apartado"].ToString().Trim(),
                (CategoriasNC)Convert.ToInt32(dr["Categoria"]),
                GPersonal.GetPersona(Convert.ToInt32(dr["idEmitidaPor"])),
                GAreas.GetArea(Convert.ToInt32(dr["idAreaResponsabilidad"])),
                dr["Equipo"].ToString().Trim(),
                dr["Hallazgo"].ToString().Trim(),
                dr["AccionInmediata"].ToString().Trim(),
                dr["CausasRaices"].ToString().Trim(),
                dr["AccionCorrectiva"].ToString().Trim(),
                (ConclusionesNC)Convert.ToByte(dr["Conclusion"]),
                dr["Comentarios"].ToString().Trim(),
                GPersonal.GetPersona(Convert.ToInt32(dr["idFirmaCierre"].ToString())),
                DateTime.Parse(dr["FechaFirma"].ToString()),
                (EstadosNC)Convert.ToInt16(dr["Estado"])
            );

            dr.Close();
        }
        catch
        {
            result = null;
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
    /// Envia la No Conformidad.
    /// </summary>
    private static void EnviarNC(string asunto, string equipo, string hallazgo, string accionInmediata, string comentarios,
        int numero, string emitidaPor, string de, string para, string cc, string estado, bool cerrada)
    {
        string strPlantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_NC);

        if (strPlantilla == null)
        {
            throw new PlantillaInexistenteException();
        }

        strPlantilla = strPlantilla.Replace("@ENCABEZADO", "");
        strPlantilla = strPlantilla.Replace("@TIPO_MENSAJE", cerrada ? "success" : "info");
        strPlantilla = strPlantilla.Replace("@EMITIDA_POR", emitidaPor);
        strPlantilla = strPlantilla.Replace("@ASUNTO", asunto);
        strPlantilla = strPlantilla.Replace("@EQUIPO", equipo);
        strPlantilla = strPlantilla.Replace("@HALLAZGO", hallazgo);
        strPlantilla = strPlantilla.Replace("@ACCION_INMEDIATA", accionInmediata);
        strPlantilla = strPlantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/calidad/ncAdmin.aspx", "id=" + GetIdNC(numero)));

        Email email = new Email(de, para, cc, "NC Nº" + numero + " [" + estado + "]", strPlantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Actualiza una No Conformidad.
    /// </summary>
    public static void ActualizarNC(int idNC, bool normaISO9001, bool normaISO14001, bool normaOHSAS18001, bool normaIRAM301,
        bool revisionMatrizRiesgo, string apartado, CategoriasNC categoria, int idArea, string equipo, string hallazgo, 
        string accionInmediata, string causasRaices, 
        string accionCorrectiva, ConclusionesNC conclusion, string comentarios, string asunto, bool cerrar)
    {
        NoConformidad nc = GNoConformidades.GetNoConformidad(idNC);
        EstadosNC estadoNuevo;

        if (nc == null || nc.Estado == EstadosNC.Cerrada || 
            (!normaISO9001 && !normaISO14001 && !normaOHSAS18001 && !normaIRAM301) ||
            (!GPermisosPersonal.TieneAcceso(PermisosPersona.NNCAdministrador, Constantes.Usuario) &&
            (cerrar || nc.Estado != EstadosNC.ProcesandoImputado || (nc.Estado == EstadosNC.ProcesandoImputado &&
            (!nc.Area.EsResponsable() || causasRaices.Trim().Length == 0 ||
            accionCorrectiva.Trim().Length == 0)))))
        {
            string msg = "Se produjo un error al intentar guardar la solicitud de no conformidad. Verifique que los datos ingresados "
                       + "sean válidos y que posee los permisos necesarios e intente nuevamente.<br /><br />Si el problema "
                       + "persiste, contáctese con el Área de Sistemas.";

            throw new Exception(msg);
        }

        estadoNuevo = nc.Estado;
        string para = "";
        string cc = "";
        string asunto_mail = "";

        if (!cerrar)
        {
            switch (nc.Estado)
            {
                case EstadosNC.ProcesandoSGI:   // E-mail para el imputado.
                    estadoNuevo = EstadosNC.ProcesandoImputado;
                    asunto_mail = "Completar por imputado";
                    Area area = GAreas.GetArea(idArea);
                    para = area.GetEmailsResponsables();
                    break;
                case EstadosNC.ProcesandoImputado:  // E-mail para el responsable de SGC.
                    estadoNuevo = EstadosNC.EsperandoCierre;
                    asunto_mail = "Esperando cierre";
                    para = Constantes.EmailCalidad;
                    break;
                case EstadosNC.EsperandoCierre:
                    if (conclusion == ConclusionesNC.NoCorresponde)
                    {
                        // E-mail para el imputado.
                        estadoNuevo = EstadosNC.ProcesandoImputado;
                        asunto_mail = "Completar por imputado";
                        para = nc.Area.GetEmailsResponsables();
                    }
                    break;
                default:
                    throw new ErrorOperacionException();
            }
        }
        else
        {
            estadoNuevo = conclusion == ConclusionesNC.NoCorresponde ? EstadosNC.NoCorresponde : EstadosNC.Cerrada;
            asunto_mail = "Cerrada";
            para = nc.Area.GetEmailsResponsables();
            cc = nc.EmitidaPor.Email;
        }

        ActualizarNC(idNC, normaISO9001, normaISO14001, normaOHSAS18001, normaIRAM301, revisionMatrizRiesgo, apartado, categoria, idArea, equipo, 
            hallazgo, accionInmediata, causasRaices, accionCorrectiva, conclusion, comentarios, asunto, estadoNuevo, cerrar);

        if (para.Length > 0)
        {
            EnviarNC(asunto, equipo, hallazgo, accionInmediata, comentarios, nc.Numero, nc.EmitidaPor.Nombre, 
                Constantes.Usuario.Email,
                para, cc, asunto_mail, false);
        }
    }
    /// <summary>
    /// Actualiza una No Conformidad. No se hace comprobación de datos.
    /// </summary>
    private static void ActualizarNC(int idNC, bool normaISO9001, bool normaISO14001, bool normaOHSAS18001, bool normaIRAM301,
        bool revisionMatrizRiesgo, string apartado, CategoriasNC categoria,
        int idArea, string equipo, string hallazgo, string accionInmediata, string causasRaices,
        string accionCorrectiva, ConclusionesNC conclusion, string comentarios, string asunto, EstadosNC estado, bool cerrar)
    {
        IDbConnection conn = null;
        IDbCommand cmd;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "UPDATE tbl_NoConformidades SET ";
            cmd.CommandText += "NormaISO9001=@NormaISO9001, ";
            cmd.CommandText += "NormaISO14001=@NormaISO14001, ";
            cmd.CommandText += "NormaOHSAS18001=@NormaOHSAS18001, ";
            cmd.CommandText += "NormaIRAM301=@NormaIRAM301, ";
            cmd.CommandText += "RevMatrizRiesgo=@RevMatrizRiesgo, ";
            cmd.CommandText += "Apartado=@Apartado, ";
            cmd.CommandText += "Categoria=@Categoria, ";
            cmd.CommandText += "idAreaResponsabilidad=@AreaResponsabilidad, ";
            cmd.CommandText += "Equipo=@Equipo, ";
            cmd.CommandText += "Hallazgo=@Hallazgo, ";
            cmd.CommandText += "AccionInmediata=@AccionInmediata, ";
            cmd.CommandText += "CausasRaices=@CausasRaices, ";
            cmd.CommandText += "AccionCorrectiva=@AccionCorrectiva, ";
            cmd.CommandText += "Conclusion=@Conclusion, ";
            cmd.CommandText += "Comentarios=@Comentarios, ";
            cmd.CommandText += "idFirmaCierre=@FirmaCierre, ";
            cmd.CommandText += "FechaFirma=@FechaFirma, ";
            cmd.CommandText += "Estado=@Estado, ";
            cmd.CommandText += "Asunto=@Asunto ";
            cmd.CommandText += "WHERE idNC=@idNC";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idNC", idNC));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NormaISO9001", normaISO9001));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NormaISO14001", normaISO14001));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NormaOHSAS18001", normaOHSAS18001));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@NormaIRAM301", normaIRAM301));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@RevMatrizRiesgo", revisionMatrizRiesgo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Apartado", apartado));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Categoria", categoria));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AreaResponsabilidad", idArea));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Equipo", equipo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Hallazgo", hallazgo));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AccionInmediata", accionInmediata));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@CausasRaices", causasRaices));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@AccionCorrectiva", accionCorrectiva));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Conclusion", (int)conclusion));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Comentarios", comentarios));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FirmaCierre", Constantes.Usuario.ID));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@FechaFirma", DateTime.Now));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Estado", (int)estado));
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Asunto", asunto));

            cmd.ExecuteNonQuery();
        }
        catch
        {
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
    /// <summary>
    /// Actualiza una No Conformidad.
    /// </summary>
    public static void GuardarNCImputado(int idNC, string accionInmediata, string causasRaices, string accionCorrectiva)
    {
        NoConformidad nc = GNoConformidades.GetNoConformidad(idNC);

        if (nc == null || nc.Estado != EstadosNC.ProcesandoImputado || !nc.Area.EsResponsable() ||
            accionInmediata.Trim().Length == 0 || causasRaices.Trim().Length == 0 || accionCorrectiva.Trim().Length == 0)
        {
            string msg = "Se produjo un error al intentar guardar la solicitud de no conformidad. Verifique que los datos ingresados "
                       + "sean válidos y que posee los permisos necesarios e intente nuevamente.<br /><br />Si el problema "
                       + "persiste, contáctese con el Área de Sistemas.";

            throw new Exception(msg);
        }

        ActualizarNC(idNC, nc.NormaISO9001, nc.NormaISO14001, nc.NormaOHSAS18001, nc.NormaIRAM301, nc.RevMatrizRiesgo, nc.Apartado, nc.Categoria, 
            nc.Area.ID, nc.Equipo, nc.Hallazgo, accionInmediata, 
            causasRaices, accionCorrectiva, nc.Conclusion, nc.Comentarios, nc.Asunto, EstadosNC.EsperandoCierre, false);

        EnviarNC(nc.Asunto, nc.Equipo, nc.Hallazgo, accionInmediata, nc.Comentarios, nc.Numero, nc.EmitidaPor.Nombre,
            Constantes.Usuario.Email, Constantes.EmailCalidad, "", "Esperando cierre", false);
    }
    /// <summary>
    /// Obtiene las NC.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<NoConformidad> GetNoConformidades(int pagina, List<Filtro> filtros)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDbDataAdapter adap;
        DataSet ds = new DataSet();
        List<NoConformidad> result = new List<NoConformidad>();

        pagina = pagina - 1;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = GetConsultaFiltro(filtros, false);
            adap = DataAccess.GetDataAdapter(cmd);
            ((System.Data.Common.DbDataAdapter)adap).Fill(ds, pagina * MaxRegistrosPagina, MaxRegistrosPagina, "NoConformidades");

            if (ds.Tables["NoConformidades"].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables["NoConformidades"].Rows)
                {
                    NoConformidad NC = new NoConformidad(
                        Convert.ToInt32(dr["idNC"]),
                        Convert.ToInt32(dr["Numero"]),
                        DateTime.Parse(dr["Fecha"].ToString()),
                        dr["Asunto"].ToString(), false, false, false, false, false,
                        "", (CategoriasNC)Convert.ToInt32(dr["Categoria"]),
                        GPersonal.GetPersona(Convert.ToInt32(dr["idEmitidaPor"])),
                        GAreas.GetArea(Convert.ToInt32(dr["idAreaResponsabilidad"])), 
                        "", "", "", "", "", (ConclusionesNC)Convert.ToByte(dr["Conclusion"]), "", 
                        GPersonal.GetPersona(Constantes.IdPersonaInvalido),
                        DateTime.Parse(dr["FechaFirma"].ToString()),
                        (EstadosNC)Convert.ToInt32(dr["Estado"])
                    );

                    result.Add(NC);
                }
            }
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
    /// Obtiene la cantidad de páginas de NC.
    /// </summary>
    public static int GetCantidadPaginasNCs(List<Filtro> filtros)
    {
        return Funciones.CantidadPaginas(GetConsultaFiltro(filtros, true), MaxRegistrosPagina);
    }
    /// <summary>
    /// Obtiene la cadena de consulta.
    /// </summary>
    private static string GetConsultaFiltro(List<Filtro> filtros, bool cantidad)
    {
        string filtroWhere = "";
        string filtroJoin = "";
        string consulta;

        foreach (Filtro filtro in filtros)
        {
            switch (filtro.Tipo)
            {
                case (int)FiltrosNC.Asunto:
                    filtroWhere += "AND Asunto like '%" + filtro.Valor + "%' ";
                    break;
                case (int)FiltrosNC.Categoria:
                    filtroWhere += "AND Categoria = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosNC.Area:
                    filtroWhere += "AND idAreaResponsabilidad = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosNC.EmitidaPor:
                    filtroWhere += "AND idEmitidaPor = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosNC.Estado:
                    filtroWhere += "AND Estado = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosNC.Numero:
                    filtroWhere += "AND Numero = " + filtro.Valor + " ";
                    break;
                default:
                    filtroWhere += "";
                    break;
            }
        }
		filtroWhere += "AND Year(Fecha) < 2015 ";
		
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT Count(idNC) as TotalRegistros FROM tbl_NoConformidades nc " + filtroJoin + " " 
                     + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "");
        }
        else
        {
            consulta = QuerySelect + filtroJoin + " " + (filtroWhere.Length > 0 ? "WHERE " + filtroWhere : "") + " ORDER BY Numero DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene una descripción corta para la categoría.
    /// </summary>
    public static string GetDescripcionCategoriaC(CategoriasNC categoria)
    {
        string result;

        switch (categoria)
        {
            case CategoriasNC.NotaNoConformidad:
                result = "NC";
                break;
            case CategoriasNC.Observacion:
                result = "OBS";
                break;
            case CategoriasNC.OportunidadMejora:
                result = "OM";
                break;
            case CategoriasNC.Stock:
                result = "Stock";
                break;
            case CategoriasNC.NoCorresponde:
                result = "N/A";
                break;
            default:
                result = "-";
                break;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una descripción larga para la categoría.
    /// </summary>
    public static string GetDescripcionCategoriaL(CategoriasNC categoria)
    {
        string result;

        switch (categoria)
        {
            case CategoriasNC.NotaNoConformidad:
                result = "No Conformidad";
                break;
            case CategoriasNC.Observacion:
                result = "Observación";
                break;
            case CategoriasNC.OportunidadMejora:
                result = "Oportunidad de mejora";
                break;
            case CategoriasNC.Stock:
                result = "Stock";
                break;
            case CategoriasNC.NoCorresponde:
                result = "No corresponde";
                break;
            default:
                result = "-";
                break;
        }

        return result;
    }
    /// <summary>
    /// Obtiene una categoría de NC en base a la descripción corta.
    /// </summary>
    public static CategoriasNC GetCategoriaNC(string descripcionC)
    {
        descripcionC = descripcionC.Trim().ToUpper();

        if (descripcionC.Equals("NC")) return CategoriasNC.NotaNoConformidad;
        else if (descripcionC.Equals("OBS")) return CategoriasNC.Observacion;
        else if (descripcionC.Equals("OM")) return CategoriasNC.OportunidadMejora;
        else if (descripcionC.Equals("STOCK")) return CategoriasNC.Stock;
        else if (descripcionC.Equals("N/A")) return CategoriasNC.NoCorresponde;
        else return CategoriasNC.NotaNoConformidad;
    }
    /// <summary>
    /// Obtiene las categorías de NC.
    /// </summary>
    public static Dictionary<string, int> GetCategoriasNC()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add(GetDescripcionCategoriaL(CategoriasNC.NotaNoConformidad), (int)CategoriasNC.NotaNoConformidad);
        result.Add(GetDescripcionCategoriaL(CategoriasNC.Observacion), (int)CategoriasNC.Observacion);
        result.Add(GetDescripcionCategoriaL(CategoriasNC.OportunidadMejora), (int)CategoriasNC.OportunidadMejora);
        result.Add(GetDescripcionCategoriaL(CategoriasNC.Stock), (int)CategoriasNC.Stock);
        result.Add(GetDescripcionCategoriaL(CategoriasNC.NoCorresponde), (int)CategoriasNC.NoCorresponde);

        return result;
    }
    /// <summary>
    /// Obtiene las categorías de NC.
    /// </summary>
    public static Dictionary<string, int> GetCategoriasNC_C()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add(GetDescripcionCategoriaC(CategoriasNC.NotaNoConformidad), (int)CategoriasNC.NotaNoConformidad);
        result.Add(GetDescripcionCategoriaC(CategoriasNC.Observacion), (int)CategoriasNC.Observacion);
        result.Add(GetDescripcionCategoriaC(CategoriasNC.OportunidadMejora), (int)CategoriasNC.OportunidadMejora);
        result.Add(GetDescripcionCategoriaC(CategoriasNC.Stock), (int)CategoriasNC.Stock);
        result.Add(GetDescripcionCategoriaC(CategoriasNC.NoCorresponde), (int)CategoriasNC.NoCorresponde);

        return result;
    }
    /// <summary>
    /// Obtiene los tipos de cierre de NC.
    /// </summary>
    public static Dictionary<string, int> GetCierresNC()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add("En proceso", (int)ConclusionesNC.EnProceso);
        result.Add("No corresponde", (int)ConclusionesNC.NoCorresponde);
        result.Add("Cierre eficaz", (int)ConclusionesNC.Satisfactoria);

        return result;
    }
    /// <summary>
    /// Obtiene si el ID de la categoría es válido.
    /// </summary>
    public static bool CategoriaValida(int idCategoria)
    {
        return idCategoria >= 0 && idCategoria <= (int)CategoriasNC.OportunidadMejora;
    }
    /// <summary>
    /// Obtiene si el ID de la conclusión es válido.
    /// </summary>
    public static bool ConclusionValida(int idConclusion)
    {
        return idConclusion >= 0 && idConclusion <= (int)ConclusionesNC.EnProceso;
    }
    /// <summary>
    /// Obtiene las categorías de NC.
    /// </summary>
    public static Dictionary<string, int> GetEstadosNC()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add("Procesando SGI", (int)EstadosNC.ProcesandoSGI);
        result.Add("Procesando imputado", (int)EstadosNC.ProcesandoImputado);
        result.Add("Esperando cierre", (int)EstadosNC.EsperandoCierre);
        result.Add("Cerrada", (int)EstadosNC.Cerrada);
        result.Add("No corresponde", (int)EstadosNC.NoCorresponde);

        return result;
    }

    public static void ResetNumeracion()
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idNC FROM tbl_NoConformidades ORDER BY Fecha";
            dr = cmd.ExecuteReader();

            List<int> lista = new List<int>();
            while (dr.Read()) lista.Add(Convert.ToInt32(dr["idNC"]));
            dr.Close();

            int c = lista.Count;
            for (int i = 0; i < c; i++)
            {
                cmd = DataAccess.GetCommand(conn);
                cmd.CommandText = "UPDATE tbl_NoConformidades SET Numero = @Numero WHERE idNC = @idNC";
                cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", i + 1));
                cmd.Parameters.Add(DataAccess.GetDataParameter("@idNC", lista[i]));
                cmd.ExecuteNonQuery();
            }
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
    }

    public static List<string> ListaOrigen()
    {
        List<string> result = new List<string>
        {
            "Reclamo / Queja de Cliente Externo",
            "Reclamo / Queja de Cliente Interno",
			"Auditoria Externa",
            "Auditoria Interna",
            "Plan de Gestión y Planes Derivados",
            "Reporte de Accidentes / Incidentes",
            "Reportes Ambientales",
            "Requisitos Legales",
            "Monitoreo de Procesos",
            "Producto No Conforme",
            "Oportunidad de Mejora",
            "Rechazo a Proveedores"
        };
        
        return result;
    }
}
