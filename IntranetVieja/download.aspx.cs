using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path;
        string nombre;

        string usr = WindowsIdentity.GetCurrent().Name;

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        usr = WindowsIdentity.GetCurrent().Name;

        try
        {
            path = parametros["f"];
            nombre = parametros["n"].Replace(' ', '_');
            if (parametros.ContainsKey("idPath"))
            {
                switch ((PathDescargas) Convert.ToInt32(parametros["idPath"]))
                {
                    case PathDescargas.ListadoInstrumentos:
                        //path = Instrumentos.GetPathCertifInstrumento(Convert.ToInt32(path));
                        break;
                    case PathDescargas.ITR:
                        RepositorioArchivos rep = GRepositorioArchivos.GetRepositorioArchivos(RepositoriosArchivos.ITR);
                        path = rep.CarpetaActual.Path + nombre;
                        break;
                    case PathDescargas.OC:
                        path = NotifVentas.GetFileOC(path);
                        nombre += ".pdf";
                        break;
                    default:
                        throw new Exception();
                }
            }
            else
            {
                path = path.Replace(@"\\SERVIDOR1", @"c:\Inetpub\wwwroot\intra.servaind.com");
                //Response.Write(path);
            }

            if (!ImpersionateHelper.Impersionate())
            {
                throw new Exception("impersionate!");
            }

            usr = WindowsIdentity.GetCurrent().Name;

            Response.AppendHeader("content-disposition", "attachment; filename=" + nombre);
            Response.WriteFile(path);
            Response.Flush();
        }
        catch(Exception ex)
        {
            usr = WindowsIdentity.GetCurrent().Name;

            throw ex;

            ImpersionateHelper.UndoImpersionate();
            Response.Redirect(Constantes.UrlIntranet);
            return;
        }

        try
        {
            int d;
            if (Int32.TryParse(parametros["d"], out d))
            {
                if (d != 0)
                {
                    // Borro el archivo.
                    System.IO.File.Delete(path);
                }
            }
        }
        catch
        {

        }

        ImpersionateHelper.UndoImpersionate();
    }
}