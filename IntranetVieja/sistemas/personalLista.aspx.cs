﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sistemas_personalLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.Administrador))
        {
            Response.Redirect(Constantes.UrlIntraDefault);
            return;
        }

        // Personas.
        List<Persona> personas = GPersonal.GetPersonas(false);

        cbFiltroAutoriza.DataSource = personas;
        cbFiltroAutoriza.DataTextField = "Nombre";
        cbFiltroAutoriza.DataValueField = "ID";
        cbFiltroAutoriza.DataBind();

        cbPersonaAutoriza.DataSource = personas;
        cbPersonaAutoriza.DataTextField = "Nombre";
        cbPersonaAutoriza.DataValueField = "ID";
        cbPersonaAutoriza.DataBind();

        // Estado.
        Dictionary<int, string> estados = GPersonal.GetEstadosPersona();

        cbFiltroEstado.DataSource = estados;
        cbFiltroEstado.DataTextField = "Value";
        cbFiltroEstado.DataValueField = "Key";
        cbFiltroEstado.DataBind();

        cbPersonaEstado.DataSource = estados;
        cbPersonaEstado.DataTextField = "Value";
        cbPersonaEstado.DataValueField = "Key";
        cbPersonaEstado.DataBind();
    }

    /// <summary>
    /// Obtiene una lista con las personas.
    /// </summary>
    [WebMethod()]
    public static List<object[]> GetPersonas(int pagina, int idPersona, string nombre, string usuario, int idAutoriza, 
        int enPanelControl, int estado)
    {
        List<object[]> result = new List<object[]>();
        List<Filtro> filtros = new List<Filtro>();

        if (idPersona != Constantes.ValorInvalido && idPersona != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Id, idPersona));
        }
        if (nombre.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Nombre, nombre));
        }
        if (usuario.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Usuario, usuario));
        }
        if (idAutoriza != Constantes.ValorInvalido && idAutoriza != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Autoriza, idAutoriza));
        }
        if (enPanelControl >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.EnPanelControl, enPanelControl));
        }
        if (Enum.IsDefined(typeof(EstadosPersona), estado))
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Estado, estado));
        }

        List<Persona> personas = GPersonal.GetPersonas(pagina, filtros);
		
		foreach (Persona persona in personas)
		{
			if(persona != null) 
			{		
				object[] fila = new object[] { 
					persona.ID,
					persona.Nombre,
					persona.Usuario,
					persona.Autoriza.Nombre,
					persona.EnPanelControl ? 1 : 0,
					persona.Activo ? 1 : 0
				};
				
				result.Add(fila);
			}			
		}				
		
        return result;
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    [WebMethod()]
    public static int GetCantidadPaginas(int idPersona, string nombre, string usuario, int idAutoriza, int enPanelControl, 
        int estado)
    {
        int result;
        List<Filtro> filtros = new List<Filtro>();

        if (idPersona != Constantes.ValorInvalido && idPersona != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Id, idPersona));
        }
        if (nombre.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Nombre, nombre));
        }
        if (usuario.Trim().Length > 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Usuario, usuario));
        }
        if (idAutoriza != Constantes.ValorInvalido && idAutoriza != Constantes.IdPersonaInvalido)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Autoriza, idAutoriza));
        }
        if (enPanelControl >= 0)
        {
            filtros.Add(new Filtro((int)FiltrosPersona.EnPanelControl, enPanelControl));
        }
        if (Enum.IsDefined(typeof(EstadosPersona), estado))
        {
            filtros.Add(new Filtro((int)FiltrosPersona.Estado, estado));
        }

        result = GPersonal.GetCantidadPaginas(filtros);

        return result;
    }
    /// <summary>
    /// Da de alta una nueva imputación.
    /// </summary>
    [WebMethod()]
    public static void AltaPersona(string nombre, string email, string usuario, int idAutoriza, int enPanelControl, int estado,
        string legajo, string cuil, int horaEntrada, int horaSalida, int baseID)
    {
        try
        {
            GPersonal.AltaPersonal(nombre, email, usuario, idAutoriza, enPanelControl == 1, estado == 1, legajo, cuil,
                                   horaEntrada, horaSalida, baseID);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Actualiza una persona.
    /// </summary>
    [WebMethod()]
    public static void EditarPersona(int idPersona, string nombre, string email, string usuario, int idAutoriza,
        int enPanelControl, int estado, int horaEntrada, int horaSalida, int baseID)
    {
        try
        {
            GPersonal.ActualizarPersonal(idPersona, nombre, email, usuario, idAutoriza, enPanelControl == 1, estado == 1,
                                         horaEntrada, horaSalida, baseID);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }
    /// <summary>
    /// Obtiene una imputación.
    /// </summary>
    [WebMethod()]
    public static object[] GetPersona(int idPersona)
    {
        List<object> result = new List<object>();

        Persona persona = GPersonal.GetPersona(idPersona);
        if (persona != null)
        {
            result.Add(persona.Nombre);
            result.Add(persona.Email);
            result.Add(persona.Usuario);
            result.Add(persona.IdAutoriza);
            result.Add(persona.EnPanelControl ? 1 : 0);
            result.Add(persona.Activo ? 1 : 0);
            result.Add(persona.Cuil);
            result.Add(persona.HoraEntrada.ToString("HH:mm"));
            result.Add(persona.HoraSalida.ToString("HH:mm"));
            result.Add(persona.BaseID);
        }

        return result.ToArray();
    }

    [WebMethod()]
    public static object GetBases()
    {
        List<object> result = new List<object>();

        Dictionary<int, string> bases = BaseFac.GetBasesLista();
        foreach (int baseID in bases.Keys)
        {
            result.Add(new { BaseID = baseID, Nombre = bases[baseID] });
        }

        return result.ToArray();
    }
}