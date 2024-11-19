using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

public class Encriptacion
{
    //Variables privadas.
    private const string CLAVE = "$erVainD_SisSer";

    /// <summary>
    /// Encripta una cadena de caracteres por medio de encriptación doble.
    /// </summary>
    public static string Encriptar(string cadena)
    {
        return EncriptarMD5(cadena, true, CLAVE);
    }
    /// <summary>
    /// Desencripta una cadena de caracteres usando desencriptación doble.
    /// </summary>
    public static string Desencriptar(string cadenaCifrada)
    {
        return DesencriptarMD5(cadenaCifrada, true, CLAVE);
    }
    /// <summary>
    /// Encripta una cadena de caracteres por medio de encriptación doble.
    /// </summary>
    private static string EncriptarMD5(string cadena, bool usarHashing, string clave)
    {
        byte[] keyArray;
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(cadena);

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

        if (usarHashing)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            hashmd5.Clear();
        }
        else
            keyArray = UTF8Encoding.UTF8.GetBytes(clave);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        tdes.Clear();
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }
    /// <summary>
    /// Desencripta una cadena de caracteres usando desencriptación doble.
    /// </summary>
    private static string DesencriptarMD5(string cadenaCifrada, bool usarHashing, string clave)
    {
        byte[] keyArray;
        byte[] toEncryptArray = Convert.FromBase64String(cadenaCifrada);

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

        if (usarHashing)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            hashmd5.Clear();
        }
        else
            keyArray = UTF8Encoding.UTF8.GetBytes(clave);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        tdes.Clear();
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
    /// <summary>
    /// Encripta una URL.
    /// </summary>
    public static string GetURLEncriptada(string pagina, string parametros)
    {
        string result;

        result = String.Format("{0}{1}?p={2}", Constantes.UrlIntranet, pagina, Encriptar(parametros).Replace("/", "%2F").Replace("+","%2B"));

        return result;
    }
    /// <summary>
    /// Encripta un parametro.
    /// </summary>
    public static string GetParametroEncriptado(string parametro)
    {
        string result;

        result = String.Format("{0}", Encriptar(parametro).Replace("/", "%2F").Replace("+", "%2B"));

        return result;
    }
    /// <summary>
    /// Obtiene una URL encriptada.
    /// </summary>
    public static Dictionary<string, string> GetParametrosURL(string paramEncriptados)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        try
        {
            string[] parametros = Desencriptar(paramEncriptados.Replace("%2F", "/").Replace("%2B", "+").Replace("&", "%26")).Split('&');
            
            foreach (string parametro in parametros)
            {
                string[] p = parametro.Split('=');
                if (p.Length == 2)
                {
                    result.Add(p[0], p[1].Replace("%26", "&"));
                }
            }
        }
        catch
        {

        }

        return result;
    }

}