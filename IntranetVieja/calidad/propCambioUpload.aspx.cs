using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_propCambioUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }

    protected void SubirArchivo(object sender, EventArgs e)
    {
        string adjuntoFilename = String.Empty;
        string adjuntoName = String.Empty;

        if (txtArchivo.HasFile)
        {
            try
            {
                adjuntoFilename = PropCambioFac.AdjuntoTempFile();
                txtArchivo.SaveAs(adjuntoFilename);
                adjuntoName = txtArchivo.FileName;
            }
            catch
            {
                Page.RegisterClientScriptBlock("onInicio",
                    "<script>this.parent.SendError('Se "
                    + "produjo un error al intentar cargar el archivo. Verifique que posee los permisos necesarios y vuelva a intentarlo.<br />Si el problema persiste, "
                    + "contáctese con el Área de Sistemas.');</script>");
                return;
            }
        }

        try
        {
            int sectorId = Convert.ToInt32(txtSectorId.Text);
            int responsableId = Convert.ToInt32(txtResponsableId.Text);
            string cambioPropuesto = txtCambioPropuesto.Text;
            int urgenciaId = Convert.ToInt32(txtUrgenciaId.Text);

            PropCambioFac.Create(sectorId, responsableId, cambioPropuesto, urgenciaId, adjuntoFilename, adjuntoName);
        }
        catch (ArgumentException ex)
        {
            Page.RegisterClientScriptBlock("onInicio",
                "<script>this.parent.SendError('" + ex.Message + "');</script>");
            return;            
        }
        catch(Exception ex)
        {
            Page.RegisterClientScriptBlock("onInicio",
                "<script>this.parent.SendError('Se produjo un error al intentar completar la operación. Contáctese con el Área de Sistemas. " + ex.Message + "');</script>");
            return;
        }

        Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.SendSuccess();</script>");
    }
}