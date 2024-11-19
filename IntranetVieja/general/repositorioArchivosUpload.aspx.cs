using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_repositorioArchivosUpload : System.Web.UI.Page
{
    // Constantes.
    private const string PrefSession = "repositorioArchivos.aspx.";

    // Propiedades.
    public static RepositorioArchivos Repositorio
    {
        get
        {
            return (RepositorioArchivos)GSessions.GetSession(PrefSession + "Repositorio");
        }
        set
        {
            GSessions.CambiarValorSession(PrefSession + "Repositorio", value);
        }
    }


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
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; this.parent.ErrorMsg('No se ha seleccionado ningún archivo.');</script>");
            return;
        }

        // Controlar la extensión del archivo que se está subiendo.
        string[] extensiones = Constantes.TiposArchivosRepositorio.Split(' ');
        int posPunto = txtArchivo.FileName.LastIndexOf(".");
        if(posPunto <= 0)
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; this.parent.ErrorMsg('El archivo seleccionado no es válido.');</script>");
            return;
        }
        string extension = txtArchivo.FileName.Substring(posPunto, txtArchivo.FileName.Length - posPunto).ToLower();
        if (!extensiones.Contains(extension))
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; this.parent.ErrorMsg('El archivo seleccionado no es válido.');</script>");
            return;
        }

        if (Repositorio != null)
        {
            if (!Repositorio.CarpetaActual.TienePermiso(PermisosRDA.LecturaEscritura))
            {
                Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; this.parent.ErrorMsg('Se "
                 + "produjo un error al intentar cargar "
                 + "el archivo. Verifique que posee los permisos necesarios y vuelva a intentarlo.<br />Si el problema persiste, "
                 + "contáctese con el Área de Sistemas.');</script>");
            }

            string fullPath = Repositorio.CarpetaActual.Path;

			ImpersionateHelper.Impersionate();
			
            try
            {
                // La propiedad txtArchivo.PostedFile.FileName en Firefox devuelve solo el nombre del archivo, pero en
                // IE trae todo el path completo del archivo.
                string nombreArchivo = txtArchivo.PostedFile.FileName;
                if (nombreArchivo.Contains("\\"))
                {
                    int posBarra = nombreArchivo.LastIndexOf("\\");
                    nombreArchivo = nombreArchivo.Substring(posBarra + 1, nombreArchivo.Length - posBarra - 1);
                }

                txtArchivo.SaveAs(fullPath + nombreArchivo);
                Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.GetDirectorio('" + Encriptacion.Encriptar("0")
                                             + "');</script>");
            }
            catch
            {
                Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; this.parent.ErrorMsg('Se "
                 + "produjo un error al intentar cargar "
                 + "el archivo. Verifique que posee los permisos necesarios y vuelva a intentarlo.<br />Si el problema persiste, "
                 + "contáctese con el Área de Sistemas.');</script>");
            }
			
			ImpersionateHelper.UndoImpersionate();
        }
        else
        {
            Page.RegisterClientScriptBlock("onInicio", "<script>this.parent.ult_ventana = 'divSubirArchivo'; " 
                                         + "this.parent.ErrorMsg('Se produjo un error al intentar completar la operación.');</script>");
            return;
        }
    }
}