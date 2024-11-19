using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GRepositorioArchivos.CrearEstructuraRepositorio();

        if (Request.QueryString["p"] == null)
        {
            Response.Write(Encriptacion.GetURLEncriptada("general/viajeAdmin.aspx", "id=27331"));
        }
        else
        {
            Dictionary<string, string> param = Encriptacion.GetParametrosURL("FAf2of1eu3j93BbT TdEuThMILPHf2xj/KNl4yaqT/72ZWUrnGXpLJsbLsLkRjpTlReRYOh1PuZ7hJnO7uqzpCQMttawnRnV1 NxbDeH8vLWutxrmldsyZTF/ ttg4YsU6xIKCjiZFc=");

            foreach (string k in param.Keys)
            {
                Response.Write(String.Format("Param: {0} - Valor: {1}<br />", k, param[k]));
            }
        }
    }
}