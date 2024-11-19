using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

    public partial class login : System.Web.UI.Page
    {
        // Constantes.
        private const string COOKIE_USR = "INTRA_USUARIO";
        private const string COOKIE_PWD = "INTRA_PWD";
        private const string PrefSession = "login.aspx.";
        private const int MaxReintentos = 5;

        // Variables.
        public string password = "";

        // Propiedades.
        public static int Contador
        {
            get
            {
                return (int)GSessions.GetSession(PrefSession + "Contador");
            }
            set
            {
                if (GSessions.GetSession(PrefSession + "Contador") == null)
                {
                    GSessions.CrearSession(PrefSession + "Contador", value);
                }
                else
                {
                    GSessions.CambiarValorSession(PrefSession + "Contador", value);
                }
            }
        }
        private static string UrlInicio
        {
            get
            {
                return (string)GSessions.GetSession(PrefSession + "UrlInicio");
            }
            set
            {
                if (GSessions.GetSession(PrefSession + "UrlInicio") == null)
                {
                    GSessions.CrearSession(PrefSession + "UrlInicio", value);
                }
                else
                {
                    GSessions.CambiarValorSession(PrefSession + "UrlInicio", value);
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Constantes.UrlIntranet.Equals("http://" + Request.Url.Host + "/") && !Request.Url.ToString().Contains("localhost"))
            {
                Response.Redirect(Constantes.UrlIntranet);
                return;
            }

            if (!Page.IsPostBack)
            {
                if (GSessions.GetSession(PrefSession + "Contador") == null)
                {
                    Contador = MaxReintentos;
                }

                string url = Request.QueryString["ReturnUrl"];
                UrlInicio = url == null ? "" : url;

                LeerCookies();
            }
        }
        /// <summary>
        /// Loguea al usuario.
        /// </summary>
        [WebMethod()]
        public static string Login(string usr, string pwd, int chk)
        {
            if (Contador <= 0)
            {
                throw new Exception("Se ha bloqueado la cuenta debido al número de intentos fallidos de iniciar sesión. Contáctese con el área de sistemas.");
            }

            usr = System.Web.HttpUtility.UrlDecode(usr);
            pwd = System.Web.HttpUtility.UrlDecode(pwd);

            if (ValidacionUsuario.EsUsuarioValido("10.0.0.2", "DOMINIO", usr, pwd))
            {
                // Create the authetication ticket
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(usr, true, 500);

                // Now encrypt the ticket.
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                // Create a cookie and add the encrypted ticket to the
                // cookie as data.
                HttpCookie authCookie =
                new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                // Add the cookie to the outgoing cookies collection.
                HttpContext.Current.Response.Cookies.Add(authCookie);
                // Redirect the user to the originally requested page

                Constantes.Usuario = GPersonal.GetPersona(usr);

                // Log de la operación.
                //Funciones.Log("[Inicio de sesión] Usuario: (" + usr + ") [OK]");

                if (chk != 0)
                {
                    GrabarCookies(usr, pwd);
                }
                else
                {
                    EliminarCookies();
                }

                //var notifVenta = NotifVentas.GetNotifVenta(1);
                //NotifVentas.SendNotifVenta(6, "prueba");
                //SendNotifVenta


                if (UrlInicio.Length > 0)
                {
                    return UrlInicio;
                }
                else
                {
                    return Constantes.UrlIntranet;
                }
            }
            else
            {
                --Contador;
                //Funciones.Log("[Inicio de sesión] Usuario: (" + usr + ") - Password: (" + pwd + ") [ERROR]");
                if (Contador > 0)
                {
                    throw new Exception("El usuario o contraseña no son válidos. Le queda " + Contador + " reintento" +
                        (Contador == 1 ? "" : "s") + ".");
                }
                else
                {
                    throw new Exception("Se ha bloqueado la cuenta debido al número de intentos fallidos de iniciar sesión. Contáctese con el área de sistemas.");
                }
            }
        }
        /// <summary>
        /// Lee las cookies.
        /// </summary>
        protected void LeerCookies()
        {
            if (Request.Cookies[COOKIE_USR] != null && Request.Cookies[COOKIE_PWD] != null)
            {
                try
                {
                    txtUsuario.Value = Request.Cookies[COOKIE_USR].Value.ToString();
                    //txtPassword.Attributes.Add("value", Encriptacion.Desencriptar(Request.Cookies[COOKIE_PWD].Value));

                    this.password = Encriptacion.Desencriptar(Request.Cookies[COOKIE_PWD].Value);

                    chkRecordar.Checked = true;
                }
                catch
                {

                }
            }
        }
        /// <summary>
        /// Graba las cookies.
        /// </summary>
        protected static void GrabarCookies(string usr, string pwd)
        {
            HttpContext.Current.Response.Cookies[COOKIE_USR].Value = usr;
            HttpContext.Current.Response.Cookies[COOKIE_USR].Expires = DateTime.Now.AddDays(30);
            HttpContext.Current.Response.Cookies[COOKIE_PWD].Value = Encriptacion.Encriptar(pwd);
            HttpContext.Current.Response.Cookies[COOKIE_PWD].Expires = DateTime.Now.AddDays(30);
        }
        /// <summary>
        /// Elimina las cookies.
        /// </summary>
        protected static void EliminarCookies()
        {
            HttpContext.Current.Response.Cookies[COOKIE_USR].Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies[COOKIE_PWD].Expires = DateTime.Now.AddDays(-1);
        }
    }
