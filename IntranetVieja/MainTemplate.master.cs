using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MainTemplate : System.Web.UI.MasterPage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Constantes.Usuario == null)
        {
            Response.Redirect("/login.aspx?ReturnUrl=" + Request.Url);
        }
    }
}
