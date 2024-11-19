using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_repositorioArchivosUpload : System.Web.UI.Page
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
        if (!txtArchivo.HasFile)
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divImputacion'; this.parent.ErrorMsg('No se ha seleccionado ningún archivo.');</script>");
            return;
        }

        // Controlar la extensión del archivo que se está subiendo.
        string[] extensiones = new string[] { ".pdf" };
        int posPunto = txtArchivo.FileName.LastIndexOf(".");
        if(posPunto <= 0)
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divImputacion'; this.parent.ErrorMsg('El archivo seleccionado no es válido.');</script>");
            return;
        }
        string extension = txtArchivo.FileName.Substring(posPunto, txtArchivo.FileName.Length - posPunto).ToLower();
        if (!extensiones.Contains(extension))
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divImputacion'; this.parent.ErrorMsg('El archivo seleccionado no es válido.');</script>");
            return;
        }

        try
        {
            string nombreArchivo = ITR.GetNombreITR(DateTime.Parse(txtFecha.Text),
                                                                       Convert.ToInt32(txtImputacion.Text),
                                                                       Constantes.Usuario.Usuario);

            txtArchivo.SaveAs(Constantes.PATH_TEMP + nombreArchivo);
        }
        catch
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ErrorMsg('Se "
             + "produjo un error al intentar cargar "
             + "el archivo. Verifique que posee los permisos necesarios y vuelva a intentarlo.<br />Si el problema persiste, "
             + "contáctese con el Área de Sistemas.');</script>");
        }
    }
}