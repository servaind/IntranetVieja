/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Configuration;
using System.DirectoryServices;
using System.Web;

/// <summary>
/// Descripción breve de ValidacionUsuario
/// </summary>
public class ValidacionUsuario : System.Web.UI.Page 
{
    /// <summary>
    /// Obtiene si el Usuario es válido.
    /// </summary>
    public static bool EsUsuarioValido(string usuario, string password)
    {
        return EsUsuarioValido(Constantes.DominioDC, Constantes.DominioNombre, usuario, password);
    }
	/// <summary>
	/// Obtiene si el Usuario es válido.
	/// </summary>
	public static bool EsUsuarioValido(string ipDC, string dominio, string usuario, string password)
	{
		string dominioUsuario = dominio + @"\" + usuario;
		DirectoryEntry entrada = new DirectoryEntry(
			"LDAP://" + ipDC, dominioUsuario, password);

        if (Constantes.TestMode)
        {
            string[] testUsers = Constantes.TestUsers.Split('|');

            if (testUsers.Length == 0) return false;

            bool valido = false;
            foreach (string testUser in testUsers)
            {
                if (testUser == usuario)
                {
                    valido = true;
                    break;
                }
            }

            if (!valido)
            {
                return false;
            }
        }

        string[] publicUsers = Constantes.PublicUsers.Split('|');
        if (publicUsers != null)
        {
            foreach (string publicUser in publicUsers)
            {
                string[] user = publicUser.Split(';');
                if (user[0].Equals(usuario) && user[1].Equals(password))
                {
                    Constantes.EsUsuarioPublico = true;
                    return true;
                }
            }
        }

        Constantes.EsUsuarioPublico = false;

		//try
		//{
		//	// Fuerzo la autentificación.
		//	object obj = entrada.NativeObject;

		//	DirectorySearcher search = new DirectorySearcher(entrada);

		//	search.Filter = "(SAMAccountName=" + usuario + ")";
		//	search.PropertiesToLoad.Add("cn");
		//	SearchResult result = search.FindOne();

		//	if (null == result)
		//	{
		//		return false;
		//	}
		//}
		//catch
		//{
		//	return false;
		//}

		return true;
	}
	/// <summary>
	/// Inicializa el usuario.
	/// </summary>
	public static Persona IniciarUsuario(HttpRequest request)
	{
		Persona __Usuario;

		//Obtengo la cadena de inicio de sesion: 'dominio\usuario'
		string[] usr = request.ServerVariables.Get("AUTH_USER").Split('\\');
		
		if (usr.Length == 0)
		{
			//Usuario invalido
			return null;
		}

        __Usuario = GPersonal.GetPersona(usr[usr.Length - 1]);

		if (__Usuario.ID == 0)
		{
			//La persona no existe
			return null;
		}

		return __Usuario;
	}
}
