/*
 * Historial:
 * ===================================================================================
 * [13/05/2011]
 * - Versión estable.
 */

using System;
using System.Web;

/// <summary>
/// Summary description for GSessions
/// </summary>
public class GSessions
{
    /// <summary>
    /// Crea una session.
    /// </summary>
    public static bool CrearSession(string nombre, object valor)
    {
        bool result = true;

        try
        {
            HttpContext.Current.Session.Add(nombre, valor);
        }
        catch
        {
            result = false;
        }

        return result;
    }
    /// <summary>
    /// Obtiene el contenido de una session.
    /// </summary>
    public static object GetSession(string nombre)
    {
        object result = null;

        try
        {
            result = HttpContext.Current.Session[nombre];
        }
        catch
        {
            result = null;
        }

        return result;
    }
    /// <summary>
    /// Cambia el valor de una session.
    /// </summary>
    public static bool CambiarValorSession(string nombre, object valor)
    {
        bool result = true;

        try
        {
            HttpContext.Current.Session[nombre] = valor;
        }
        catch
        {
            result = false;
        }

        return result;
    }
}