using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class getImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string path;
        PathImage idPath;

        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

        try
        {
            path = parametros["path"];
            idPath = (PathImage)Convert.ToInt32(parametros["idPath"]);

            switch (idPath)
            {
                case PathImage.ListadoInstrumentos:
                    break;
                default:
                    throw new Exception();
            }

            Response.WriteFile(path);
            Response.Flush();
        }
        catch
        {
            return;
        }
    }
}