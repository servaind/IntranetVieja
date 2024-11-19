using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class general_repositorioArchivos : System.Web.UI.Page
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
        if (!GPermisosPersonal.TieneAcceso(PermisosPersona.RDAVer))
        {
            Response.Redirect(Constantes.UrlIntraDefault, true);
            return;
        }

		Response.Redirect("http://intranet.servaind.com/Usuario/LoginDirect/" + Constantes.Usuario.ID);
		return;
		
        RepositoriosArchivos repositorio = RepositoriosArchivos.Comun;
        Dictionary<int, string> repositorios = GRepositorioArchivos.GetRepositorios();
        if (Request.QueryString["p"] != null)
        {
            Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(Request.QueryString["p"]);

            int idRepositorio;
            if (!parametros.ContainsKey("idRepositorio") || !Int32.TryParse(parametros["idRepositorio"], out idRepositorio) || 
                !Enum.IsDefined(typeof(RepositoriosArchivos), idRepositorio))
            {
                Response.Redirect("repositorioArchivos.aspx?p=" + Encriptacion.GetParametroEncriptado("idRepositorio=" +
                    (int)RepositoriosArchivos.Comun), true);
                return;
            }

            repositorio = (RepositoriosArchivos)idRepositorio;

            Dictionary<string, string> repositoriosE = new Dictionary<string, string>();
            string selectedValue = "";
            foreach (int id in repositorios.Keys)
            {
                string repE = Encriptacion.GetParametroEncriptado("idRepositorio=" + id);
                repositoriosE.Add(repE, repositorios[id]);

                if (id == idRepositorio)
                {
                    selectedValue = repE;
                }
            }

            cbRepositorio.DataSource = repositoriosE;
            cbRepositorio.DataTextField = "Value";
            cbRepositorio.DataValueField = "Key";
            cbRepositorio.DataBind();

            cbRepositorio.Value = selectedValue;
        }
        else
        {
            foreach (int r in repositorios.Keys)
            {
                repositorio = (RepositoriosArchivos)r;
                break;
            }

            Repositorio = GRepositorioArchivos.GetRepositorioArchivos(repositorio);
            
            Response.Redirect("repositorioArchivos.aspx?p=" + Encriptacion.GetParametroEncriptado("idRepositorio=" +
                (int)repositorio), true);
            return;
        }

        Repositorio = GRepositorioArchivos.GetRepositorioArchivos(repositorio);
    }
    /// <summary>
    /// Obtiene los detalles del directorio actual.
    /// </summary>
    [WebMethod()]
    public static object[][] GetDirectorio(string nombre)
    {
        List<object[]> result = new List<object[]>();

        if (Repositorio != null)
        {
            try
            {
                nombre = Encriptacion.Desencriptar(nombre);
                if (!nombre.Equals("0"))
                {
                    Repositorio.AbrirCarpeta(nombre);
                }
            }
            catch
            {
                Repositorio.IrARoot();
            }

            // Agrego primero las carpetas.
            foreach (CarpetaRepositorio carpeta in Repositorio.CarpetaActual.Subcarpetas)
            {
                result.Add(new object[] { 0, Encriptacion.Encriptar(carpeta.Nombre), carpeta.Nombre, "Carpeta", "" });
            }

            // Agrego los archivos.
            FileInfo[] archivos = Repositorio.CarpetaActual.GetArchivos();
            foreach (FileInfo archivo in archivos)
            {
                result.Add(new object[] { 1, Encriptacion.GetURLEncriptada("download.aspx", "f=" + archivo.FullName + "&n=" + 
                archivo.Name), archivo.Name, GRepositorioArchivos.GetDescripcionTipoArchivo(archivo.Extension.Replace(".", "")), 
                Funciones.GetFileSize(archivo.Length), archivo.Extension.Replace(".", ""), Encriptacion.GetParametroEncriptado("f=" + archivo.Name) });
            }
        }

        return result.ToArray();
    }
    /// <summary>
    /// Obtiene la información de los niveles.
    /// </summary>
    [WebMethod()]
    public static string[] GetInfo()
    {
        string[] result;

        if (Repositorio != null)
        {
            result = new string[] { !Repositorio.EstaEnRoot ? 
            Encriptacion.Encriptar(Repositorio.CarpetaActual.CarpetaPadre.Nombre) : "" };
        }
        else
        {
            result = new string[0];
        }

        return result;
    }
    /// <summary>
    /// Obtiene las propiedades de la carpeta.
    /// </summary>
    [WebMethod()]
    public static object[][] GetPropiedades()
    {
        List<object[]> result = new List<object[]>();

        if (Repositorio != null)
        {
            if (!Repositorio.CarpetaActual.PuedeEditar())
            {
                throw new Exception("No posee los permisos para editar esta carpeta.");
            }

            result.Add(new object[] { Repositorio.CarpetaActual.Nombre });
        }

        return result.ToArray();
    }
    /// <summary>
    /// Crea una carpeta en el directorio actual.
    /// </summary>
    [WebMethod()]
    public static string CrearCarpeta(string nombre)
    {
        try
        {
            GRepositorioArchivos.CrearCarpeta(Repositorio.CarpetaActual, nombre);
        }
        catch
        {
            throw new Exception("Se produjo un error al intentar crear la carpeta. Verifique que el nombre no contenga " + 
                                "caracteres inválidos y que no exista en el directorio actual.");
        }

        return Encriptacion.Encriptar("0");
    }
    /// <summary>
    /// Actualiza el directorio actual.
    /// </summary>
    [WebMethod()]
    public static string ActualizarCarpeta(string nombre)
    {
        try
        {
            GRepositorioArchivos.ActualizarCarpeta(Repositorio.CarpetaActual, nombre);
        }
        catch
        {
            throw new Exception("Se produjo un error al intentar crear la carpeta. Verifique que el nombre no contenga " +
                                "caracteres inválidos y que no exista en el directorio actual.");
        }

        return Encriptacion.Encriptar("0");
    }
    /// <summary>
    /// Obtiene si el usuario tiene permisos sobre la carpeta actual.
    /// </summary>
    [WebMethod()]
    public static void TienePermisos()
    {
        if (!Repositorio.CarpetaActual.TienePermiso(PermisosRDA.LecturaEscritura))
        {
            throw new Exception();
        }
    }
    /// <summary>
    /// Obtiene la url a un documento.
    /// </summary>
    [WebMethod()]
    public static string GetDocumento(string documento)
    {
        string result;

        try
        {
            string pathDocumento = Repositorio.CarpetaActual.Path + documento;
            //result = Encriptacion.GetURLEncriptada("verDocumento.aspx", "f=" + pathDocumento);
            File.Copy(pathDocumento, HttpContext.Current.Server.MapPath("/temp") + "\\" + documento, true);
            result = "/temp/" + documento;
        }
        catch(Exception ex)
        {
			throw ex;
            throw new Exception();
        }

        return result;
    }
    /// <summary>
    /// Elimina un archivo.
    /// </summary>
    [WebMethod()]
    public static string EliminarArchivo(string archivo)
    {
        Dictionary<string, string> parametros = Encriptacion.GetParametrosURL(archivo);

        if (parametros == null || parametros.Count == 0)
        {
            throw new Exception("Parámetros incorrectos.");
        }

        try
        {
            GRepositorioArchivos.EliminarArchivo(Repositorio.CarpetaActual, parametros["f"]);
        }
        catch
        {
            throw new Exception("Se produjo un error al intentar eliminar el archivo. Verifique que el archivo exista " +
                                "y que posee los privilegios necesarios.");
        }

        return Encriptacion.Encriptar("0");
    }
}