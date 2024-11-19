/*
 * Historial:
 * ===================================================================================
 * [27/05/2011]
 * - Versión estable.
 */

using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// Descripción breve de Cookies
/// </summary>
public class GCookies
{
    /// <summary>
    /// Almacena una cookie. Si no existe, la crea.
    /// </summary>
    public static void GuardarCookie(Page pagina, string nombre, string valor)
    {
        HttpCookie cookie = BuscarCookie(pagina, nombre);
        if (cookie == null)
        {
            cookie = new HttpCookie(nombre);
            cookie.Value = valor;
            cookie.Value = "sarasa";
            pagina.Response.Cookies.Add(cookie);
        }

        GuardarCookie(pagina, cookie, valor);
    }
    /// <summary>
    /// Almacena una cookie.
    /// </summary>
    public static void GuardarCookie(Page pagina, HttpCookie cookie, string valor)
    {
        cookie.Value = valor;
        cookie.Expires = DateTime.Now.AddDays(10);

        pagina.Response.Cookies.Set(cookie);
    }
    /// <summary>
    /// Elimina una cookie.
    /// </summary>
    public static void EliminarCookie(Page pagina, string nombre)
    {
        pagina.Response.Cookies.Remove(nombre);
    }
    /// <summary>
    /// Elimina una cookie.
    /// </summary>
    public static void EliminarCookie(Page pagina, HttpCookie cookie)
    {
        pagina.Response.Cookies.Remove(cookie.Name);
    }
    /// <summary>
    /// Busca una cookie.
    /// </summary>
    private static HttpCookie BuscarCookie(Page pagina, string nombre)
    {
        HttpCookie cookie = pagina.Response.Cookies.Get(nombre);

        return cookie;
    }
    /// <summary>
    /// Lee el valor de una cookie.
    /// </summary>
    public static string LeerCookie(Page pagina, string nombre)
    {
        HttpCookie cookie = BuscarCookie(pagina, nombre);
        string valor = "";

        if (cookie != null)
        {
            valor = cookie.Value;
        }
        else
        {
            pagina.Response.Write("Null");
        }

        return valor;
    }
}
