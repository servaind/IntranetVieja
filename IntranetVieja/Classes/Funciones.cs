﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;


/// <summary>
/// Clase con funciones comunes.
/// </summary>
public static class Funciones
{
    public const bool DEBUG = true;


    /// <summary>
    /// Arma un listado con las Fechas intermedias.
    /// </summary>
    public static List<DateTime> ListarIntervaloFechas(DateTime comienzo, DateTime finalizacion)
    {
        List<DateTime> lista = new List<DateTime>();
        DateTime i, f;

        i = comienzo;
        f = finalizacion;

        while (DateTime.Compare(i, f) <= 0)
        {
            lista.Add(i);
            i = i.AddDays(1);
        }

        return lista;
    }
	/// <summary>
	/// Devuelve una cadena en "formato log".
	/// </summary>
    public static string MSG_DEBUG(Exception ex, string f, int errno)
    {
        if (DEBUG)
        {
            return (f + ": '" + errno + "' " + ex.Message + "<br>");
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Separa un texto en renglones de largoRenglon longitud.
    /// </summary>
    /// <param name="__texto">Texto a separar en renglones.</param>
    /// <param name="largoRenglon">Cantidad maxima de caracteres por renglon.</param>
    /// <returns>Devuelve un listado de renglones.</returns>
    public static ArrayList SepararTexto(string __texto, int largoRenglon)
    {
        ArrayList txt = new ArrayList();
        //Me fijo si es menor al largo del renglon
        if (__texto.Length <= largoRenglon)
        {
            txt.Add(__texto);
            return txt;
        }

        string[] palabras = __texto.Split(' ');

        //Me fijo si es una unica palabra y si es mayor al largo permitido
        if (palabras.Length == 0 && __texto.Length > largoRenglon)
        {
            return null;
        }

        string aux;
        int l = palabras.Length; //Cantidad de palabras

        //Recorro Todas las palabras y las separo en renglones
        for (int x = 0; x < l; )
        {
            aux = "";

            for (; x < l; x++)
            {
                if ((aux.Length + palabras[x].Length + 1/*espacio*/) > largoRenglon)
                {
                    break;
                }
                aux += palabras[x] + " ";
            }

            txt.Add(aux);
        }
        return txt;
    }
    /// <summary>
    /// Devuelve un Dia pasado en ingles a español. Patch Provisorio.
    /// </summary>
    public static string DiaAEsp(string f)
    {
        if (String.Compare(f, "Monday") == 0) return "Lunes";
        if (String.Compare(f, "Tuesday") == 0) return "Martes";
        if (String.Compare(f, "Wednesday") == 0) return "Miércoles";
        if (String.Compare(f, "Thursday") == 0) return "Jueves";
        if (String.Compare(f, "Friday") == 0) return "Viernes";
        if (String.Compare(f, "Saturday") == 0) return "Sábado";
        if (String.Compare(f, "Sunday") == 0) return "Domingo";
        return null;
    }
    /// <summary>
    /// Reemplaza los Enter de una cadena por <br>
    /// </summary>
    public static string ReemplazarEnters(string cadena)
    {
        char[] c = cadena.ToCharArray();
        string texto = "";
        foreach (char t in c)
        {
            if (t == 13 || t == '\n')
            {
                texto += "<br>";
            }
            else
            {
                texto += t;
            }
        }

        return texto;
    }
    /// <summary>
    /// Determina si el texto finaliza con un caracter dado
    /// </summary>
    public static bool TerminaCon(string texto, char caracter)
    {
        char[] t = texto.ToCharArray();

        return t[t.Length - 1] == caracter;
    }
    /// <summary>
    /// Arma una lista en base a dos listas sin repetir los datos.
    /// </summary>
    public static string ListaComunes(string l1, string[] l2)
    {
        string comunes = l1;
        for (int x = 0; x < l2.Length; x++)
        {
            if (String.Compare(l2[x], "all") != 0)
            {
                if (!l1.Contains(l2[x]))
                {
                    comunes += "," + l2[x];
                }
            }
        }

        return comunes;
    }
    /// <summary>
    /// Completa una cadena con 0
    /// </summary>
    public static string CompletarCeros(string t, int c)
    {
        for (int x = 0; x < c; x++)
        {
            t = "0" + t;
        }

        return t;
    }
    /// <summary>
    /// Obtiene el texto perteneciente a la plantilla.
    /// </summary>
    public static string ObtenerPlantilla(string path)
    {
        string plantilla;

        try
        {
            System.IO.StreamReader arch = new System.IO.StreamReader(path);
            plantilla = arch.ReadToEnd();
            arch.Close();
            arch = null;
        }
        catch
        {
            plantilla = null;
        }

        return plantilla;
    }
	
	/// <summary>
    /// Obtiene el texto perteneciente a la plantilla.
    /// </summary>
	public static string ObtenerPlantilla(EmailPlantilla tipo)
	{
		string plantilla = "";
		
		switch(tipo)
		{
			case EmailPlantilla.NVAltaCliente:
				//plantilla = ObtenerPlantilla(@"C:\Inetpub\wwwroot\intra.servaind.com\comercial\plantillas\NotifVentaCliente.htm");
                plantilla = ObtenerPlantilla(@"C:\Desarrollo\Freelance\servaind\web\Intranet_Vieja\IntranetVieja\IntranetVieja\comercial\plantillas\NotifVentaCliente.htm");
                break;
			
			case EmailPlantilla.NVAltaImputacion:
				plantilla = ObtenerPlantilla(@"C:\Inetpub\wwwroot\intra.servaind.com\comercial\plantillas\NotifVentaImputacion.htm");
			break;
			
			case EmailPlantilla.NVAltaGeneralOC:
				plantilla = ObtenerPlantilla(@"C:\Inetpub\wwwroot\intra.servaind.com\comercial\plantillas\NotifVenta.htm");
			break;
			
			case EmailPlantilla.NVRecordatorio:
				plantilla = ObtenerPlantilla(@"C:\Inetpub\wwwroot\intra.servaind.com\comercial\plantillas\NotifVentaRecordatorio.htm");
			break;
		
		}
		
		if(plantilla == null)
		{
			Log("ES NULL " + tipo.ToString());
			
		}
		else
		{
			Log("NO ES NULL " + tipo.ToString());
		}
		
		return plantilla;	
	}
	
	
    /// <summary>
    /// Graba un mensaje en el archivo de log.
    /// </summary>
    public static void Log(string texto)
    {
        Log(texto, Constantes.PATH_SISSER + "intra.log");
    }
    /// <summary>
    /// Graba un mensaje en el archivo de log.
    /// </summary>
    public static void Log(string texto, string nombreArchivo)
    {
        try
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(nombreArchivo, true);
            string s = String.Format("{0} - {1}", DateTime.Now.ToString(), texto);
            sw.WriteLine(s);
            sw.Close();
            sw = null;
        }
        catch
        {
            //Nada...
        }
    }
    /// <summary>
    /// Obtiene la cantidad de páginas disponibles.
    /// </summary>
    public static int CantidadPaginas(string consulta, int tamanoPagina)
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        int cantidadPaginas;

        try
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["intranet"].ToString();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = consulta;

            int cantidadRegistros = Convert.ToInt32(cmd.ExecuteScalar());
            cantidadPaginas = cantidadRegistros / tamanoPagina;
            if (cantidadRegistros % tamanoPagina > 0)
            {
                cantidadPaginas++;
            }
        }
        catch
        {
            cantidadPaginas = 0;
        }
        finally
        {
            conn.Close();
        }

        return cantidadPaginas;
    }
    /// <summary>
    /// Carga un enum en un DropDownList.
    /// </summary>
    public static void CargarEnumEn(DropDownList ddl, Array enumValores)
    {
        foreach (object t in enumValores)
        {
            ListItem it = new ListItem(t.ToString(), ((int)t).ToString());
            ddl.Items.Add(it);
        }
    }
    /// <summary>
    /// Crea una cadena JSON en base a los parámetros.
    /// </summary>
    public static string CrearJSONSimple(params object[][] campos)
    {
        string result = "";

        // Debe ser una cantidad de datos par <Nombre, Valor>.
        if (campos.Length > 0)
        {
            result = "[";

            for (int i = 0; i < campos.Length; i++)
            {
                if (campos[i].Length % 2 == 0)
                {
                    result += "{";

                    for (int j = 0; j < campos[i].Length; j += 2)
                    {
                        result += "\"" + campos[i][j] + "\": ";
                        result += "\"" + campos[i][j + 1] + "\",";
                    }

                    // Borro la última ,.
                    result = result.TrimEnd(',');

                    result += "},";
                }
            }

            // Borro la última ,.
            result = result.TrimEnd(',');

            result += "]";
        }

        return result;
    }
    /// <summary>
    /// Encripta una url.
    /// </summary>
    public static string EncriptarURL(string pagina, params string[] parametros)
    {
        string result = "";

        result = pagina + (parametros != null && parametros.Length > 0 ? "?" : "");

        foreach (string parametro in parametros)
        {
            result += parametro + "&";
        }
        result = result.TrimEnd('&');

        // Encripto la cadena.
        try
        {
            result = Encriptacion.Encriptar(result);
        }
        catch
        {
            result = "";
        }

        return result;
    }
    /// <summary>
    /// Desencripta una URL.
    /// </summary>
    public static string DesencriptarURL(string url)
    {
        string result;

        try
        {
            result = Encriptacion.Desencriptar(url);
        }
        catch
        {
            result = "";
        }

        return result;
    }
    /// <summary>
    /// Returns a string number adapted to the current culture.
    /// </summary>
    public static string GetDecimalNumber(string number)
    {
        string result;

        if (String.IsNullOrEmpty(number))
        {
            number = "0";
        }

        if (number != null)
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;

            result = number;
            result = result.Replace(".", culture.NumberFormat.NumberDecimalSeparator);
            result = result.Replace(",", culture.NumberFormat.NumberDecimalSeparator);
        }
        else
        {
            result = "";
        }

        return result;
    }
    /// <summary>
    /// Get a DateTime like dd/MM/yyyy 00:00:00.
    /// </summary>
    public static DateTime GetDate(DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
    }
    /// <summary>
    /// Get a DateTime like dd/MM/yyyy 23:59:59.
    /// </summary>
    public static DateTime GetDateEnd(DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
    }
    /// <summary>
    /// Concatena los elementos de una lista.
    /// </summary>
    public static string UnirArray<T>(T[] lista, string separador)
    {
        string result = "";

        if (lista != null)
        { 
            int cant = lista.Length;
            for (int i = 0; i < cant; i++)
            {
                result += lista[i] + separador;
            }
            result = result.TrimEnd(separador.ToCharArray());
        }

        return result;
    }
    /// <summary>
    /// Obtiene el dia de la semana.
    /// </summary>
    public static string GetDiaSemana(DateTime dia)
    {
        System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CurrentCulture;
        string result = ci.DateTimeFormat.GetDayName(dia.DayOfWeek);
        result = result[0].ToString().ToUpper() + result.Substring(1, result.Length - 1);

        return result;
    }
    /// <summary>
    /// Obtiene la representación del tamaño del archivo.
    /// </summary>
    public static string GetFileSize(long bytes)
    {
        float v;
        string unidad;

        if (bytes < 1048576)
        {
            // KB.
            v = bytes / 1024f;
            unidad = "KB";
        }
        else if (bytes < 1073741824)
        {
            // MB.
            v = bytes / 1048576;
            unidad = "MB";
        }
        else if (bytes > 1073741824)
        {
            // GB.
            v = bytes / 1073741824f;
            unidad = "GB";
        }
        else
        {
            return "";
        }

        return String.Format("{0} {1}", v.ToString("0.#"), unidad);
    }
    /// <summary>
    /// Obtiene los tipos binarios.
    /// </summary>
    public static Dictionary<string, int> GetTiposBinarios()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        result.Add("No", (int)TiposBinario.No);
        result.Add("Si", (int)TiposBinario.Si);

        return result;
    }
    /// <summary>
    /// Obtiene el número de semana.
    /// </summary>
    public static int GetNumeroSemana(DateTime dtPassed)
    {
        System.Globalization.CultureInfo ciCurr = System.Globalization.CultureInfo.CurrentCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, System.Globalization.CalendarWeekRule.FirstFourDayWeek, 
            DayOfWeek.Monday);

        return weekNum;
    }
    /// <summary>
    /// Limita una cadena de texto.
    /// </summary>
    public static string LimitarTexto(string texto, int largo, string continua)
    {
        string result = "";
        int c = texto.Length;

        if (c <= largo)
        {
            result = texto;
        }
        else
        {
            for (int i = 0; i < largo; i++)
            {
                result += texto[i];
            }
            result += continua;
        }

        return result;
    }
    /// <summary>
    /// Obtiene si las listas son iguales.
    /// </summary>
    public static bool ListasIguales<T>(List<T> lista, List<T> lista2)
    {
        bool result = true;

        if (lista.Count != lista2.Count)
        {
            result = false;
        }
        else
        {
            foreach (T item in lista)
            {
                if (!lista2.Contains(item))
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }
    /// <summary>
    /// Actualiza la página actual.
    /// </summary>
    public static void ActualizarPagina()
    {
        System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString() +
            "?" + System.Web.HttpContext.Current.Request.QueryString.ToString());
    }

    public static string ToTitleCase(string s)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
    }

    public static string Concatenate<T>(List<T> list, char separator)
    {
        string result = "";

        if (list != null && list.Count > 0)
        {
            result = list.Aggregate(result, (current, e) => current + (e.ToString() + separator));

            result = result.TrimEnd(separator);
        }

        return result;
    }
}