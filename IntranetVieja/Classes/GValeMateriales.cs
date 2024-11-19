/*
 * Historial:
 * ===================================================================================
 * [24/05/2011]
 * - Versión estable.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public class ItemVDM : IComparable
{
    // Variables.
    private ArticuloTango articulo;
    private short cantidad;
    private int idImputacion;
    private string obra;

    // Propiedades.
    public ArticuloTango Articulo
    {
        get { return articulo; }
        set { articulo = value; }
    }
    public short Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
    }
    public int IDImputacion
    {
        get { return idImputacion; }
        set { idImputacion = value; }
    }
    public string Obra
    {
        get { return obra; }
        set { obra = value; }
    }


    /// <summary>
    /// Almacena un Ítem.
    /// </summary>
    public ItemVDM(ArticuloTango articulo, short cantidad, int idImputacion, string obra)
    {
        this.articulo = articulo;
        this.cantidad = cantidad;
        this.idImputacion = idImputacion;
        this.obra = obra;
    }

    public int CompareTo(object obj)
    {
        ItemVDM item = (ItemVDM)obj;

        return this.Articulo.Descripcion.CompareTo(item.Articulo.Descripcion);
    }
}

/// <summary>
/// Descripción breve de ValeDeMateriales
/// </summary>
public class ValeDeMateriales
{
    // Variables.
    private int idVDM;
    private int numero;
    private DateTime fechaSolicitud;
    private string departamento;
    private int smtl;
    private Persona solicito;
    private string cargo;
    private string destino;
    private EstadosVDM estado;
    private DateTime fechaRecibioResponsable;
    private DateTime fechaAproboResponsable;
    private DateTime fechaRecibioDeposito;
    private DateTime fechaEntregoDeposito;
    private DateTime fechaRechazoResponsable;
    private Persona recibioResponsable;
    private Persona aproboResponsable;
    private Persona recibioDeposito;
    private Persona entregoDeposito;
    private Persona rechazoResponsable;
    private List<ItemVDM> lstItems;

    // Propiedades.
    public int ID
    {
        get { return idVDM; }
    }
    public int Numero
    {
        get { return numero; }
    }
    public DateTime FechaSolicitud
    {
        get { return fechaSolicitud; }
    }
    public string Departamento
    {
        get { return departamento; }
    }
    public int SMTL
    {
        get { return smtl; }
    }
    public Persona Solicito
    {
        get { return solicito; }
    }
    public string Cargo
    {
        get { return cargo; }
    }
    public string Destino
    {
        get { return destino; }
    }
    public EstadosVDM Estado
    {
        get { return estado; }
    }
    public DateTime FechaRecibioResponsable
    {
        get { return fechaRecibioResponsable; }
    }
    public DateTime FechaAproboResponsable
    {
        get { return fechaAproboResponsable; }
    }
    public DateTime FechaRecibioDeposito
    {
        get { return fechaRecibioDeposito; }
    }
    public DateTime FechaEntregoDeposito
    {
        get { return fechaEntregoDeposito; }
    }
    public DateTime FechaRechazoResponsable
    {
        get { return fechaRechazoResponsable; }
    }
    public Persona RecibioResponsable
    {
        get { return recibioResponsable; }
    }
    public Persona AproboResponsable
    {
        get { return aproboResponsable; }
    }
    public Persona RecibioDeposito
    {
        get { return recibioDeposito; }
    }
    public Persona EntregoDeposito
    {
        get { return entregoDeposito; }
    }
    public Persona RechazoResponsable
    {
        get { return rechazoResponsable; }
    }
    public List<ItemVDM> Items
    {
        get { return lstItems; }
        set { lstItems = value; }
    }
    public ItemVDM this[int index]
    {
        get
        {
            ItemVDM result = null;

            if (this.lstItems != null && this.lstItems.Count > index)
            {
                result = this.lstItems[index];
            }

            return result;
        }
    }


    internal ValeDeMateriales(int idVDM, int numero, DateTime fechaSolicitud, string departamento,
        int smtl, Persona solicito, string cargo, string destino, EstadosVDM estado,
        DateTime fechaRecibioResponsable, DateTime fechaAproboResponsable, DateTime fechaRecibioDeposito, 
        DateTime fechaEntregoDeposito, DateTime fechaRechazoResponsable, Persona recibioResponsable, Persona aproboResponsable, 
        Persona recibioDeposito, Persona entregoDeposito, Persona rechazoResponsable, List<ItemVDM> listaItems)
    {
        this.idVDM = idVDM;
        this.numero = numero;
        this.fechaSolicitud = fechaSolicitud;
        this.departamento = departamento;
        this.smtl = smtl;
        this.solicito = solicito;
        this.cargo = cargo;
        this.destino = destino;
        this.estado = estado;
        this.fechaRecibioResponsable = fechaRecibioResponsable;
        this.fechaAproboResponsable = fechaAproboResponsable;
        this.fechaRecibioDeposito = fechaRecibioDeposito;
        this.fechaEntregoDeposito = fechaEntregoDeposito;
        this.fechaRechazoResponsable = fechaRechazoResponsable;
        this.recibioResponsable = recibioResponsable;
        this.aproboResponsable = aproboResponsable;
        this.recibioDeposito = recibioDeposito;
        this.entregoDeposito = entregoDeposito;
        this.rechazoResponsable = rechazoResponsable;
        this.lstItems = listaItems;
    }
    /// <summary>
    /// Genera una cadena en base al número de vale de materiales.
    /// </summary>
    public string GetNumero()
    {
        string result = "-";

        result = String.Format("0001-{0:000000}", this.Numero);

        return result;
    }
    /// <summary>
    /// Carga los Ítems.
    /// </summary>
    public void CargarItems()
    {
        this.lstItems = GValeDeMateriales.GetItemsVDM(this.idVDM);
    }
}

/// <summary>
/// Descripción breve de GValeMateriales
/// </summary>
public static class GValeDeMateriales
{
    // Constantes.
    private const int MaxRegistrosPagina = 30;
    //private static string[] AutoAprobables = { "sebastian.guadagnini", "alan.ferreira" };
    private static string[] AutoAprobables = { "carlos.meccia" };


    /// <summary>
    /// Obtiene un vale de materiales.
    /// </summary>
    private static ValeDeMateriales GetValeDeMateriales(DataRow dr)
    {
        ValeDeMateriales result;

        result = new ValeDeMateriales(
            Convert.ToInt32(dr["idVDM"]),
            Convert.ToInt32(dr["Numero"]),
            DateTime.Parse(dr["Fecha"].ToString()),
            dr["Depto"].ToString(),
            Convert.ToInt32(dr["smtl"]),
            GPersonal.GetPersona(Convert.ToInt32(dr["idSolicito"])),
            dr["Cargo"].ToString(),
            dr["Destino"].ToString(),
            (EstadosVDM)Convert.ToInt32(dr["idEstado"]),
            DateTime.Parse(dr["FecRecibida"].ToString()),
            DateTime.Parse(dr["FecAprobada"].ToString()),
            DateTime.Parse(dr["FecDeposito"].ToString()),
            DateTime.Parse(dr["FecEntrega"].ToString()),
            DateTime.Parse(dr["FecRechazo"].ToString()),
            GPersonal.GetPersona(Convert.ToInt32(dr["idRecibio"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idAprobo"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idDeposito"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idEntrego"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idRechazo"])),
            null
        );

        return result;
    }
    /// <summary>
    /// Obtiene un vale de materiales.
    /// </summary>
    private static ValeDeMateriales GetValeDeMateriales(IDataReader dr)
    {
        ValeDeMateriales result;

        result = new ValeDeMateriales(
            Convert.ToInt32(dr["idVDM"]),
            Convert.ToInt32(dr["Numero"]),
            DateTime.Parse(dr["Fecha"].ToString()),
            dr["Depto"].ToString(),
            Convert.ToInt32(dr["smtl"]),
            GPersonal.GetPersona(Convert.ToInt32(dr["idSolicito"])),
            dr["Cargo"].ToString(),
            dr["Destino"].ToString(),
            (EstadosVDM)Convert.ToInt32(dr["idEstado"]),
            DateTime.Parse(dr["FecRecibida"].ToString()),
            DateTime.Parse(dr["FecAprobada"].ToString()),
            DateTime.Parse(dr["FecDeposito"].ToString()),
            DateTime.Parse(dr["FecEntrega"].ToString()),
            DateTime.Parse(dr["FecRechazo"].ToString()),
            GPersonal.GetPersona(Convert.ToInt32(dr["idRecibio"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idAprobo"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idDeposito"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idEntrego"])),
            GPersonal.GetPersona(Convert.ToInt32(dr["idRechazo"])),
            null
        );

        return result;
    }
    /// <summary>
    /// Obtiene la lista de Items asociados a una VDM.
    /// </summary>
    public static List<ItemVDM> GetItemsVDM(int idVDM)
    {
        List<ItemVDM> result = new List<ItemVDM>();
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT Codigo,Cantidad,idImputacion,Obra FROM tbl_VDMItems ";
            cmd.CommandText += "WHERE idVDM = @idVDM";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idVDM", idVDM));
            dr = cmd.ExecuteReader();

            // Leo los Ítems.
            while (dr.Read())
            {
                ItemVDM item = new ItemVDM(
                    GArticuloTango.GetArticuloTango(dr["Codigo"].ToString()),
                    Convert.ToInt16(dr["Cantidad"]),
                    Convert.ToInt32(dr["idImputacion"]),
                    dr["Obra"].ToString()
                    );

                result.Add(item);
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
    /// Borra los Items asociados a una VDM.
    /// </summary>
    private static void BorrarItemsVDM(IDbConnection conn, IDbTransaction trans, int idVDM)
    {
        IDbCommand cmd = DataAccess.GetCommand(conn, trans);
        cmd.CommandText = "DELETE FROM tbl_VDMItems WHERE idVDM = @idVDM";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idVDM", idVDM));
        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Guarda el Ítem.
    /// </summary>
    private static void InsertarItemVDM(IDbConnection conn, IDbTransaction trans, ItemVDM item, int idVDM)
    {
        IDbCommand cmd = DataAccess.GetCommand(conn, trans);
        cmd.CommandText = "INSERT INTO tbl_VDMItems(idVDM, Codigo, Cantidad, idImputacion, Obra) ";
        cmd.CommandText += "VALUES (@idVDM, @Codigo, @Cantidad, @idImputacion, @Obra)";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idVDM", idVDM));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Codigo", item.Articulo.Codigo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Cantidad", item.Cantidad));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idImputacion", item.IDImputacion));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Obra", item.Obra));
        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Carga los Datos principales de la VDM.
    /// </summary>
    public static ValeDeMateriales GetValeDeMateriales(int idVDM)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        IDataReader dr;
        ValeDeMateriales result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT * FROM tbl_VDM WHERE idVDM = @idVDM";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@idVDM", idVDM));
            dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                throw new ElementoInexistenteException();
            }

            result = GetValeDeMateriales(dr);

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
    /// Obtiene el ID de un Vale de Materiales.
    /// </summary>
    public static int GetIdVDM(int numero)
    {
        IDbConnection conn = null;
        IDbCommand cmd;
        int result;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            cmd = DataAccess.GetCommand(conn);
            cmd.CommandText = "SELECT idVDM FROM tbl_VDM WHERE Numero = @Numero";
            cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));

            result = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch
        {
            result = Constantes.ValorInvalido;
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
    /// Aprueba el estado actual del Vale de materiales.
    /// </summary>
    public static void AprobarEstadoActual(int idVDM)
    {
        ConfirmarEstado(idVDM, true, "");
    }
    /// <summary>
    /// Rechaza el estado actual del Vale de materiales.
    /// </summary>
    public static void RechazarEstadoActual(int idVDM, string motivo)
    {
        if (motivo == null || motivo.Trim().Length == 0)
        {
            throw new DatosInvalidosException();
        }

        ConfirmarEstado(idVDM, false, motivo);
    }
    /// <summary>
    /// Confirma o rechaza el estado actual.
    /// </summary>
    private static void ConfirmarEstado(int idVDM, bool aceptar, string motivoRechazo)
    {
        ValeDeMateriales vdm = GetValeDeMateriales(idVDM);

        if (vdm == null || vdm.Estado == EstadosVDM.EntregadaDeposito)
        {
            throw new ErrorOperacionException();
        }

        EstadosVDM nuevoEstado = vdm.Estado;
        string para = "";
        string cc = "";
        string asunto = "Solicitud de vale de materiales Nº " + vdm.Numero + " ";

        switch (vdm.Estado)
        {
            case EstadosVDM.Enviada:
                if (!aceptar) throw new ErrorOperacionException();

                if (vdm.Solicito.IdAutoriza != Constantes.Usuario.ID 
				&& !((vdm.Solicito.ID == Constantes.Usuario.ID 
				&& Constantes.Usuario.IdAutoriza == Constantes.IdPersonaGerencia) || AutoAprobables.Contains(Constantes.Usuario.Usuario)))
                {
                    throw new ErrorOperacionException();
                }

                nuevoEstado = EstadosVDM.RecibidaResponsable;
                break;
            case EstadosVDM.RecibidaResponsable:
                if (vdm.Solicito.IdAutoriza != Constantes.Usuario.ID 
				&& !((vdm.Solicito.ID == Constantes.Usuario.ID 
				&& Constantes.Usuario.IdAutoriza == Constantes.IdPersonaGerencia) || AutoAprobables.Contains(Constantes.Usuario.Usuario)))
                {
                    throw new ErrorOperacionException();
                }

                if (aceptar)
                {
                    nuevoEstado = EstadosVDM.AprobadaResponsable;
                    para = Constantes.EmailDeposito;
                    cc = vdm.Solicito.Email;
                    asunto += "[APROBADA POR RESPONSABLE DE AREA]";
                }
                else
                {
                    nuevoEstado = EstadosVDM.RechazadaResponsable;
                    para = vdm.Solicito.Email;
                    asunto += "[RECHAZADA POR RESPONSABLE DE AREA]";
                }
                break;
            case EstadosVDM.AprobadaResponsable:
                if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ValeMaterialesRecibeDep))
                {
                    throw new ErrorOperacionException();
                }

                nuevoEstado = EstadosVDM.RecibidaDeposito;
                break;
            case EstadosVDM.RecibidaDeposito:
                if (!GPermisosPersonal.TieneAcceso(PermisosPersona.ValeMaterialesEntrega))
                {
                    throw new ErrorOperacionException();
                }

                nuevoEstado = EstadosVDM.EntregadaDeposito;
                para = vdm.Solicito.Email;
                cc = vdm.Solicito.Autoriza.Email;
                asunto += "[ENTREGADO]";
                break;
            default:
                throw new ErrorOperacionException();
        }

        ActualizarEstadoVDM(idVDM, nuevoEstado);

        // Enviar e-mail a quien corresponda.
        if (para.Length > 0)
        {
            EnviarValeDeMateriales(idVDM, Constantes.Usuario.Email, para, cc, asunto, motivoRechazo);
        }
    }
    /// <summary>
    /// Actualiza el estado de la Solicitud de Viaje.
    /// </summary>
    private static void ActualizarEstadoVDM(int idVDM, EstadosVDM estadoNuevo)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            ActualizarEstadoVDM(idVDM, estadoNuevo, conn, trans);

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
    /// <summary>
    /// Actualiza el estado de la Solicitud.
    /// </summary>
    private static void ActualizarEstadoVDM(int idVDM, EstadosVDM estadoNuevo, IDbConnection conn, IDbTransaction trans)
    {
        IDbCommand cmd;

        cmd = DataAccess.GetCommand(conn, trans);
        cmd.CommandText = "UPDATE tbl_VDM SET idEstado = @idEstado ";
        switch (estadoNuevo)
        {
            case EstadosVDM.RecibidaResponsable:
                cmd.CommandText += ", FecRecibida = @fecha, idRecibio = @idUsr ";
				cmd.Parameters.Add(DataAccess.GetDataParameter("@idUsr", Constantes.Usuario.IdAutoriza));
                break;
            case EstadosVDM.AprobadaResponsable:
                cmd.CommandText += ", FecAprobada = @fecha, idAprobo = @idUsr ";
				cmd.Parameters.Add(DataAccess.GetDataParameter("@idUsr", Constantes.Usuario.IdAutoriza));
                break;
            case EstadosVDM.RecibidaDeposito:
                cmd.CommandText += ", FecDeposito = @fecha, idDeposito = @idUsr ";
				cmd.Parameters.Add(DataAccess.GetDataParameter("@idUsr", Constantes.Usuario.ID));
                break;
            case EstadosVDM.EntregadaDeposito:
                cmd.CommandText += ", FecEntrega = @fecha, idEntrego = @idUsr ";
				cmd.Parameters.Add(DataAccess.GetDataParameter("@idUsr", Constantes.Usuario.ID));
                break;
            case EstadosVDM.RechazadaResponsable:
                cmd.CommandText += ", FecRechazo = @fecha, idRechazo = @idUsr ";
				cmd.Parameters.Add(DataAccess.GetDataParameter("@idUsr", Constantes.Usuario.ID));
                break;
        }
        cmd.CommandText += "WHERE idVDM = @idVDM";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idVDM", idVDM));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)estadoNuevo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@fecha", DateTime.Now));
     
        cmd.ExecuteNonQuery();
    }
    /// <summary>
    /// Guarda el Vale de Materiales.
    /// </summary>
    public static void AltaVDM(string departamento, int smtl, string cargo, string destino, 
        List<ItemVDM> listaItems, out int numero)
    {
        IDbConnection conn = null;
        IDbTransaction trans = null;
        int idVDM;
        numero = Constantes.ValorInvalido;

        if (departamento.Trim().Length == 0 || smtl < 0 || cargo.Trim().Length == 0 || destino.Trim().Length == 0
            || listaItems == null || listaItems.Count == 0)
        {
            throw new DatosInvalidosException();
        }

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Inserto el Vale de Materiales.
            InsertarVDM(conn, trans, departamento, smtl, cargo, destino, listaItems, out idVDM, out numero);

            // Guardo los ítems.
            foreach (ItemVDM item in listaItems)
            {
                InsertarItemVDM(conn, trans, item, idVDM);
            }

            trans.Commit();
        }
        catch(Exception e)
        {
            if (trans != null)
            {
                trans.Rollback();
            }

            throw new Exception(e.Message); //ErrorOperacionException();
        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
        }

        // Enviar e-mail al responsable.
        string para = Constantes.Usuario.Autoriza.Email;
		
		string solicitante = Constantes.Usuario.Email;
		
        string asunto = "Solicitud de vale de materiales Nº " + numero;

        EnviarValeDeMateriales(idVDM, solicitante, para, solicitante + "; deposito@servaind.com", asunto);

        // Parche para SB. 13/03/2014.
        if (Constantes.Usuario.IdAutoriza == Constantes.IdPersonaGerencia || AutoAprobables.Contains(Constantes.Usuario.Usuario))
        {
            // Lo leo.
            AprobarEstadoActual(idVDM);

            // Lo apruebo.
            AprobarEstadoActual(idVDM);
        }
    }
    /// <summary>
    /// Inserta un Vale de materiales.
    /// </summary>
    private static void InsertarVDM(IDbConnection conn, IDbTransaction trans, string departamento, int smtl, string cargo,
        string destino, List<ItemVDM> listaItems, out int idVDM, out int numero)
    {
        IDbCommand cmd;

        // Obtengo el último número.
        cmd = DataAccess.GetCommand(conn, trans);
        cmd.CommandText += "SELECT CASE COUNT(*) WHEN 0 THEN 0 ELSE (SELECT TOP 1 Numero ";
        cmd.CommandText += "FROM tbl_VDM ORDER BY Numero DESC) END as Numero FROM tbl_VDM";
        numero = Convert.ToInt32(cmd.ExecuteScalar()) + 1;

        // Inserto el vale de materiales.
        cmd = DataAccess.GetCommand(conn, trans);
        cmd.CommandText = "INSERT INTO tbl_VDM (Numero, Fecha, Depto, SMTL, idSolicito, Cargo, ";
        cmd.CommandText += "Destino, idEstado) VALUES (@Numero, @Fecha, @Depto, @SMTL, @idSolicito, @Cargo, ";
        cmd.CommandText += "@Destino, @idEstado); ";
        cmd.CommandText += "SELECT SCOPE_IDENTITY() FROM tbl_VDM;";
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Numero", numero));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Fecha", DateTime.Now));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Depto", departamento));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@SMTL", smtl));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idSolicito", Constantes.Usuario.ID));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Cargo", cargo));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@Destino", destino));
        cmd.Parameters.Add(DataAccess.GetDataParameter("@idEstado", (int)EstadosVDM.Enviada));

        idVDM = Convert.ToInt32(cmd.ExecuteScalar());
    }
    /// <summary>
    /// Actualiza un Vale de Materiales.
    /// </summary>
    public static void ActualizarVDM(int idVDM, List<ItemVDM> listaItems)
    {
        if (listaItems == null || listaItems.Count == 0)
        {
            throw new DatosInvalidosException();
        }

        ValeDeMateriales vdm = GetValeDeMateriales(idVDM);
        if (vdm == null || vdm.Estado != EstadosVDM.RechazadaResponsable || vdm.Solicito.ID != Constantes.Usuario.ID)
        {
            throw new ErrorOperacionException();
        }

        IDbConnection conn = null;
        IDbTransaction trans = null;

        try
        {
            conn = DataAccess.GetConnection(BDConexiones.Intranet);
            trans = DataAccess.GetTransaction(conn);

            // Borro los Ítems existentes.
            BorrarItemsVDM(conn, trans, idVDM);

            // Guardo los ítems.
            foreach (ItemVDM item in listaItems)
            {
                InsertarItemVDM(conn, trans, item, idVDM);
            }

            // Actualizo el estado del vale de materiales.
            ActualizarEstadoVDM(idVDM, EstadosVDM.Enviada, conn, trans);

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

        // Enviar e-mail al responsable.
        string para = Constantes.Usuario.Autoriza.Email;
        string asunto = "Solicitud de vale de materiales Nº " + vdm.Numero + " [REVISION]";

        EnviarValeDeMateriales(idVDM, Constantes.Usuario.Email, para, "", asunto);
    }
    /// <summary>
    /// Devuelve el estado.
    /// </summary>
    public static string EstadoVDM(EstadosVDM e, bool largo)
    {
        switch (e)
        {
            case EstadosVDM.Enviada:
                if(largo)
                    return "La solicitud se encuentra enviada, pero no ha sido 'Recibida' por el "
                    + "encargado del Area.";
                else return "Enviada.";
            case EstadosVDM.RecibidaResponsable:
                if (largo)
                    return "La solicitud ha sido 'Recibida' por el Encargado de Area.";
                else
                    return "Recibida por responsable.";
            case EstadosVDM.AprobadaResponsable:
                if (largo)
                    return "La solicitud se encuentra aprobada, pero no ha sido 'Recibida' por Deposito.";
                else
                    return "Aprobada por responsable.";
            case EstadosVDM.RecibidaDeposito:
                if (largo)
                    return "La solicitud ha sido recibida por Deposito.";
                else
                    return "Recibida por depósito.";
            case EstadosVDM.EntregadaDeposito:
                if (largo)
                    return "La solicitud ha sido completada y entregada al solicitante.";
                else
                    return "Entregada por depósito.";
            case EstadosVDM.RechazadaResponsable:
                if (largo)
                    return "La solicitud se encuentra rechazada.";
                else
                    return "Rechazada por responsable.";
        }

        if (largo)
            return "La solicitud se encuentra enviada, pero no ha sido 'Recibida' por el Encargado de Area.";
        else
            return "Enviada...";
    }
    /// <summary>
    /// Envía una VDM.
    /// </summary>
    public static void EnviarValeDeMateriales(int idVDM, string de, string para, string cc, string asunto)
    {
        EnviarValeDeMateriales(idVDM, de, para, cc, asunto, "");
    }
    /// <summary>
    /// Envía una VDM.
    /// </summary>
    public static void EnviarValeDeMateriales(int idVDM, string de, string para, string cc, string asunto, string motivoRechazo)
    {
        // Obtengo la Plantilla.
        string plantilla = Funciones.ObtenerPlantilla(Constantes.PATH_PLANTILLA_VDM_EMAIL);
        if (plantilla == null)
        {
            throw new EmailException();
        }

        // Cargo el vale de materiales.
        ValeDeMateriales vdm = GetValeDeMateriales(idVDM);
        if (vdm == null)
        {
            throw new EmailException();
        }

        // Cargo los ítems.
        vdm.CargarItems();
        if (vdm.Items == null || vdm.Items.Count == 0)
        {
            throw new EmailException();
        }

        string tipo = "info";
        switch (vdm.Estado)
        {
            case EstadosVDM.RechazadaResponsable:
                tipo = "error";
                break;
            case EstadosVDM.EntregadaDeposito:
                tipo = "success";
                break;
        }

        // Reemplazo las variables.
        if (motivoRechazo.Length > 0)
        {
            plantilla = plantilla.Replace("@ENCABEZADO", "El vale de materiales ha sido rechazado por el siguiente motivo: " 
                + motivoRechazo);
        }
        else
        {
            plantilla = plantilla.Replace("@ENCABEZADO", "");
        }
        plantilla = plantilla.Replace("@TIPO_MENSAJE", tipo);
        plantilla = plantilla.Replace("@NUMERO", vdm.GetNumero());
        plantilla = plantilla.Replace("@SOLICITO", vdm.Solicito.Nombre);
        plantilla = plantilla.Replace("@FECHA_SOLICITUD", vdm.FechaSolicitud.ToShortDateString());
        plantilla = plantilla.Replace("@DEPARTAMENTO", vdm.Departamento);
        plantilla = plantilla.Replace("@SMTL", vdm.SMTL.ToString());
        plantilla = plantilla.Replace("@CARGO", vdm.Cargo);
        plantilla = plantilla.Replace("@DESTINO", vdm.Destino);
        plantilla = plantilla.Replace("@LINK", Encriptacion.GetURLEncriptada("/stock/vdmAdmin.aspx", "id=" + vdm.ID));

        Email email = new Email(de, para, cc, asunto, plantilla);

        if (!email.Enviar())
        {
            throw new EmailException();
        }
    }
    /// <summary>
    /// Obtiene los vales de materiales que coincidan con el filtro.
    /// </summary>
    /// <param name="pagina">Comienza en 1.</param>
    public static List<ValeDeMateriales> GetValesDeMateriales(int pagina, List<Filtro> filtros)
    {
        List<ValeDeMateriales> result;

        result = DataAccess.GetDataList<ValeDeMateriales>(BDConexiones.Intranet, pagina, filtros, MaxRegistrosPagina,
            GetConsultaFiltro, GetValeDeMateriales);

        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int GetCantidadPaginas(List<Filtro> filtros)
    {
        return DataAccess.GetCantidadPaginasData(filtros, MaxRegistrosPagina, GetConsultaFiltro);
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
                case (int)FiltrosVDM.Codigo:
                    filtroJoin += "INNER JOIN tbl_VDMItems vi ON vdm.idVDM = vi.idVDM ";
                    filtroJoin += "AND vi.Codigo = '" + filtro.Valor + "' ";
                    break;
                case (int)FiltrosVDM.Solicito:
                    filtroWhere += "AND idSolicito = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosVDM.Estado:
                    filtroWhere += "AND idEstado = " + filtro.Valor + " ";
                    break;
                case (int)FiltrosVDM.Imputacion:
                    filtroJoin += "INNER JOIN tbl_VDMItems vi ON vdm.idVDM = vi.idVDM AND vi.idImputacion = " + filtro.Valor + " ";
                    break;
                default:
                    filtroWhere += "";
                    break;
            }
        }
        if (filtroWhere.Length > 0)
        {
            filtroWhere = filtroWhere.TrimStart('A', 'N', 'D');
        }

        if (cantidad)
        {
            consulta = "SELECT COUNT(DISTINCT vdm.idVDM) as TotalRegistros";
        }
        else
        {
            consulta = "SELECT DISTINCT vdm.*";
        }

        if (filtroWhere.Length > 0)
        {
            filtroWhere = "WHERE " + filtroWhere;
        }
        consulta += " FROM tbl_VDM vdm " + filtroJoin + " " + filtroWhere;

        if (!cantidad)
        {
            consulta += " ORDER BY vdm.Numero DESC";
        }

        return consulta;
    }
    /// <summary>
    /// Obtiene los tipos de estado para las solicitudes.
    /// </summary>
    public static Dictionary<int, string> GetEstadosVDM()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();

        result.Add((int)EstadosVDM.Enviada, "Enviada");
        result.Add((int)EstadosVDM.RecibidaResponsable, "Recibida por responsable");
        result.Add((int)EstadosVDM.AprobadaResponsable, "Aprobada por responsable");
        result.Add((int)EstadosVDM.RechazadaResponsable, "Rechazada por responsable");
        result.Add((int)EstadosVDM.RecibidaDeposito, "Recibida por depósito");
        result.Add((int)EstadosVDM.EntregadaDeposito, "Entregada por depósito");

        return result;
    }
}
