using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ExtensionMethods
/// </summary>
public static class ExtensionMethods
{
    public static string Concatenar<T>(this List<T> origen, char c)
    {
        string result = "";

        if (origen.Count > 0)
        {
            int cant = origen.Count;
            for (int i = 0; i < cant; i++)
            {
                result += origen[i].ToString() + c;
            }
            result = result.TrimEnd(c);
        }

        return result;
    }
}