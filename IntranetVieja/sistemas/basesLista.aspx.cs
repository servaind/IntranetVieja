using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class sistemas_basesLista : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod()]
    public static object GetBases()
    {
        List<object> result = new List<object>();
        List<Base> bases = BaseFac.GetBases();

        bases.ForEach(b => result.Add(new { ID = b.ID, Nombre = b.Nombre, b.Responsable, b.Alternate }) );

        return result.ToArray();
    }

    [WebMethod()]
    public static object GetResponsables()
    {
        List<object> result = new List<object>();
        List<Persona> personas = GPersonal.GetPersonasActivas();

        personas.ForEach(p => result.Add(new { ID = p.ID, Nombre = p.Nombre }));

        return result.ToArray();
    }

    [WebMethod()]
    public static void AddBase(string nombre, int responsableID, int alternateID)
    {
        try
        {
            BaseFac.AddBase(nombre, responsableID, alternateID);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }

    [WebMethod()]
    public static void UpdateBase(int baseID, string nombre, int responsableID, int alternateID, int activa)
    {
        try
        {
            BaseFac.UpdateBase(baseID, nombre, responsableID, alternateID, activa == 1);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }

    [WebMethod()]
    public static void DeleteBase(int baseID)
    {
        try
        {
            BaseFac.DeleteBase(baseID);
        }
        catch (Exception ex)
        {
            throw new Exception("Se produjo un error al intentar completar la operación.<br>Detalle: " + ex.Message);
        }
    }

    [WebMethod()]
    public static object GetBase(int baseID)
    {
        object result = null;

        Base b = BaseFac.GetBase(baseID);
        if (b != null) result = new { ID = b.ID, Nombre = b.Nombre, b.ResponsableID, b.AlternateID, b.Activa };

        return result;
    }
}