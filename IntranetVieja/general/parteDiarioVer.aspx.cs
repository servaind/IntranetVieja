using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_parteDiarioVer : System.Web.UI.Page
{
    // Variables.
    private ParteDiario pd;

    // Propiedades.
    public ParteDiario PD
    {
        get { return this.pd; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        int idPD;
        if (parametros.ContainsKey("id") && Int32.TryParse(parametros["id"], out idPD))
        {
            this.pd = GPartesDiarios.GetParteDiario(idPD);

            if (this.pd != null)
            {
                if (!TieneAccesoEspecial(this.pd) && this.pd.Persona.ID != Constantes.Usuario.ID && 
                    this.pd.Persona.IdAutoriza != Constantes.Usuario.ID && 
                    !GPermisosPersonal.TieneAcceso(PermisosPersona.RolDireccion))
                {
                    this.pd = null;
                }
                else
                {
                    this.pd.CargarImputaciones();
                }
            }
        }
    }
    /// <summary>
    /// Obtiene la descripción para la licencia.
    /// </summary>
    public string GetDescripcionLicencia()
    {
        string result = "";

        result = GLicencias.GetDescripcionTipoLicencia(this.PD.Licencia.Tipo) + ": " + this.PD.Licencia.Observaciones;

        return result;
    }
    /// <summary>
    /// Obtiene si la persona tiene acceso especial.
    /// </summary>
    private bool TieneAccesoEspecial(ParteDiario pd)
    {
        bool result = false;

        /* Elio Zapata.
        if (Constantes.Usuario.ID == 125)
        {
            List<int> elioZapata = new List<int>() { 21, 97, 108, 113, 126, 127 };
            result = elioZapata.Contains(pd.Persona.ID);
        }
        // José Garcés.
        if (Constantes.Usuario.ID == 188)
        {
            List<int> joseGarces = new List<int>() { 29, 195, 196 };
            result = joseGarces.Contains(pd.Persona.ID);
        }*/

        // Paulo Velardes.
        if (Constantes.Usuario.ID == 56)
        {
            List<int> pauloVelardes = new List<int>
            {
                56,
				1368
            };
            result = pauloVelardes.Contains(pd.Persona.ID);
        }
        // Mariano Arévalo.
        /*if (Constantes.Usuario.ID == 276)
        {
            List<int> marianoArevalo = new List<int>
            {
                161,
                153,
                232,
                156,
                157,
                183
            };
            result = marianoArevalo.Contains(pd.Persona.ID);
        }*/

        return result;
    }
}