using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class calidad_sgim : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
		return;
    }
}