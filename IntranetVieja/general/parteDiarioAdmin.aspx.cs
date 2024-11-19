using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_parteDiarioAdmin : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "parteDiarioAdmin.aspx.";

    // Variables.
    private bool autoAceptarFecha;

    // Propiedades.
    public static DateTime FechaParte
    {
        get
        {
            return (DateTime)GSessions.GetSession(PrefSession + "FechaParte");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "FechaParte", value);
        }
    }
    private static int IDParteDiario
    {
        get
        {
            return (int)GSessions.GetSession(PrefSession + "IDParteDiario");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "IDParteDiario", value);
        }
    }
    private static List<ImputacionParteDiario> Imputaciones
    {
        get
        {
            return (List<ImputacionParteDiario>)GSessions.GetSession(PrefSession + "Imputaciones");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "Imputaciones", value);
        }
    }
    public bool AutoAceptarFecha
    {
        get { return this.autoAceptarFecha; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        IDParteDiario = Constantes.ValorInvalido;
        Imputaciones = new List<ImputacionParteDiario>();

        // Imputaciones.
        cbImputacion.DataSource = GImputaciones.GetImputacionesActivas();
        cbImputacion.DataTextField = "DescripcionCompleta";
        cbImputacion.DataValueField = "ID";
        cbImputacion.DataBind();

        string[] horas = new string[GPartesDiarios.MaxHorasParteDiario];
        for (int i = 1; i <= GPartesDiarios.MaxHorasParteDiario; i++)
        {
            horas[i - 1] = i.ToString();
        }
        cbImputacionHoras.DataSource = horas;
        cbImputacionHoras.DataBind();

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        DateTime fechaParte = DateTime.Now;
        if (!(this.autoAceptarFecha = (parametros.ContainsKey("f") && DateTime.TryParse(parametros["f"], out fechaParte))))
        {
            fechaParte = DateTime.Now;
        }
        FechaParte = fechaParte;

    }
    /// <summary>
    /// Obtiene un parte diario.
    /// </summary>
    [WebMethod()]
    public static void CargaParteDiario(string fecha)
    {
        DateTime fechaParte;
        if (!DateTime.TryParse(fecha, out fechaParte))
        {
            throw new Exception("La fecha ingresada no es válida.");
        }

        if (fechaParte > Funciones.GetDateEnd(fechaParte))
        {
            throw new Exception("La fecha no puede ser superior a la fecha actual.");
        }

        DateTime now = DateTime.Now;
        DateTime today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        DateTime todaylimit = new DateTime(today.Year, today.Month, today.Day, 17, 30, 0);
        DateTime yesterday = today.AddDays(-1);

        bool expired = (fechaParte < yesterday) || ((fechaParte.Day == yesterday.Day) && (now > todaylimit));
		
		if (expired)
		{
			throw new Exception("La fecha ingresada supera el tiempo límite establecido para la carga de un parte diario.");	
		}
		
        try
        {
            // Creo el parte diario.
            ParteDiario pd = GPartesDiarios.CrearParteDiario(fechaParte);

            IDParteDiario = pd.ID;

            pd.CargarImputaciones();

            Imputaciones = pd.Imputaciones;
        }
        catch (DatosInvalidosException)
        {
            throw new Exception("La fecha ingresada no es válida. Verifique que no exista un parte diario ya cargado.");
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Obtiene las imputaciones del parte diario.
    /// </summary>
    [WebMethod()]
    public static object[][] GetImputaciones()
    {
        List<object[]> result = new List<object[]>();

        foreach (ImputacionParteDiario imputacion in Imputaciones)
        {
            result.Add(new object[] { imputacion.Imputacion.ID, imputacion.Imputacion.DescripcionCompleta, imputacion.Horas });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Elimina la imputación.
    /// </summary>
    [WebMethod()]
    public static void EliminarImputacion(int idImputacion)
    {
        ImputacionParteDiario imp = Imputaciones.Find(i => i.Imputacion.ID == idImputacion);
        
        if (imp.HayItr)
        {
            ITR.EliminarITRTemp(FechaParte, imp.Imputacion.Numero, Constantes.Usuario.Usuario);
        }

        Imputaciones.Remove(imp);
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
    /// Agrega una nueva imputación.
    /// </summary>
    [WebMethod()]
    public static void AgregarImputacion(int idImputacion, short horas, string tareasR, string tareasP, string novedadesV, 
        string novedadesH, bool hayITR, object[][] personasIntervienen)
    {
        foreach (ImputacionParteDiario i in Imputaciones)
        {
            if (i.Imputacion.ID == idImputacion)
            {
                throw new Exception("La imputación seleccionada ya se encuentra en la lista.");
            }
        }

        List<PersonaInterviene> intervinieron = new List<PersonaInterviene>();
        if (personasIntervienen != null && personasIntervienen.Length > 0)
        {
            foreach (object[] persona in personasIntervienen)
            {
                PersonaInterviene p = new PersonaInterviene(Convert.ToInt32(persona[0]), Convert.ToInt16(persona[1]));
                intervinieron.Add(p);
            }
        }

        Imputacion imputacion = GImputaciones.GetImputacion(idImputacion);

        Imputaciones.Add(new ImputacionParteDiario(horas, imputacion, tareasR, tareasP, novedadesH, novedadesV,
                                                   hayITR, intervinieron));
    }
    /// <summary>
    /// Agrega una nueva imputación.
    /// </summary>
    [WebMethod()]
    public static void ActualizarImputacion(int item, int idImputacion, short horas, string tareasR, string tareasP, 
        string novedadesV, string novedadesH, bool hayITR, object[][] personasIntervienen)
    {
        int idx = Constantes.ValorInvalido;
        int cant = Imputaciones.Count;
        for (int i = 0; i < cant; i++)
        {
            if (Imputaciones[i].Imputacion.ID == item)
            {
                idx = i;
                break;
            }
        }

        if (idx == Constantes.ValorInvalido)
        {
            throw new Exception("La imputación no se encuentra en la lista.");
        }

        List<PersonaInterviene> intervinieron = new List<PersonaInterviene>();
        if (personasIntervienen != null && personasIntervienen.Length > 0)
        {
            foreach (object[] persona in personasIntervienen)
            {
                PersonaInterviene p = new PersonaInterviene(Convert.ToInt32(persona[0]), Convert.ToInt16(persona[1]));
                intervinieron.Add(p);
            }
        }

        Imputacion imputacion = GImputaciones.GetImputacion(idImputacion);

        if ((Imputaciones[idx].Imputacion.ID != imputacion.ID && hayITR) || !hayITR)
        {
            ITR.EliminarITRTemp(FechaParte, Imputaciones[idx].Imputacion.Numero, Constantes.Usuario.Usuario);
        }

        Imputaciones[idx] = new ImputacionParteDiario(horas, imputacion, tareasR, tareasP, novedadesH, novedadesV,
                                                      hayITR, intervinieron);
    }
    /// <summary>
    /// Obtiene una imputación.
    /// </summary>
    [WebMethod()]
    public static object[] GetImputacion(int item)
    {
        object[] result;

        ImputacionParteDiario imputacion = Imputaciones.Find(i => i.Imputacion.ID == item);
        if (imputacion == null)
        {
            throw new Exception("La imputación seleccionada no es válida.");
        }

        int cant = imputacion.PersonalIntervinieron.Count;
        object[][] intervinieron = new object[cant][];
        for (int i = 0; i < cant; i++)
        {
            intervinieron[i] = new object[] { imputacion.PersonalIntervinieron[i].IdPersona,
                imputacion.PersonalIntervinieron[i].Horas};
        }

        result = new object[] { imputacion.Horas, imputacion.TareasRealizadas, imputacion.TareasProgramadas, 
            imputacion.NovedadesVehiculo, imputacion.NovedadesHerramienta, imputacion.HayItr, intervinieron};

        return result;
    }
    /// <summary>
    /// Guarda el parte diario.
    /// </summary>
    [WebMethod()]
    public static void GuardarParteDiario()
    {
        try
        {
            GPartesDiarios.GuardarParteDiario(IDParteDiario, Imputaciones, false);
        }
        catch(Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación. <br> Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Guarda el parte diario.
    /// </summary>
    [WebMethod()]
    public static void FinalizarParteDiario()
    {
        try
        {
            GPartesDiarios.GuardarParteDiario(IDParteDiario, Imputaciones, true);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
}