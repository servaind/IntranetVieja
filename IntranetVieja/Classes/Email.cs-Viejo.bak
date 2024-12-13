using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

public class AdjuntoEmail
{
    // Variables.
    private string path;
    private string nombre;

    // Propiedades.
    public string Path
    {
        get { return this.path; }
    }
    public string Nombre
    {
        get { return this.nombre; }
    }


    public AdjuntoEmail(string path, string nombre)
    {
        this.path = path;
        this.nombre = nombre;
    }
}

/// <summary>
/// Descripción breve de Email
/// </summary>
public class Email
{
	//Variables privadas.
	private string de;
	private string para;
	private string cc;
	private string asunto;
	private string cuerpo;
	private bool formatoHTML;
    private List<AdjuntoEmail> adjuntos;
	private System.Net.NetworkCredential credenciales;
	private Error _error = null;

	//Variables públicas.
	/// <summary>
	/// Obtiene o establece el remitente.
	/// </summary>
	public string De
	{
		get { return de; }
		set { de = value; }
	}
	/// <summary>
	/// Obtiene o establece el destinatario.
	/// </summary>
	public string Para
	{
		get { return para; }
		set { para = value; }
	}
	/// <summary>
	/// Obtiene o establece los destinatarios copia.
	/// </summary>
	public string CC
	{
		get { return cc; }
		set { cc = value; }
	}
	/// <summary>
	/// Obtiene o establece el asunto del mensaje.
	/// </summary>
	public string Asunto
	{
		get { return asunto; }
		set { asunto = value; }
	}
	/// <summary>
	/// Obtiene o establece el cuerpo del mensaje.
	/// </summary>
	public string Cuerpo
	{
		get { return cuerpo; }
		set { cuerpo = value; }
	}
	/// <summary>
	/// Obtiene o establece si el mensaje se enviará con formato HTML.
	/// </summary>
	public bool FormatoHTML
	{
		get { return formatoHTML; }
		set { formatoHTML = value; }
	}
	/// <summary>
	/// Obtiene o establece las credenciales para el envío del mensaje.
	/// </summary>
	public System.Net.NetworkCredential Credenciales
	{
		get { return credenciales; }
		set { credenciales = value; }
	}
	/// <summary>
	/// Obtiene los detalles del error.
	/// </summary>
	public Error Error
	{
		get
		{
			return _error;
		}
	}
    public List<AdjuntoEmail> Adjuntos
    {
        get { return this.adjuntos; }
        set { this.adjuntos = value; }
    }


	/// <summary>
	/// Almacena un E-mail.
	/// </summary>
	public Email(string de, string para, string cc, string asunto, string cuerpo)
        : this(de, para, cc, asunto, cuerpo, new List<AdjuntoEmail>())
	{

	}
    /// <summary>
    /// Almacena un E-mail.
    /// </summary>
    public Email(string de, string para, string cc, string asunto, string cuerpo, List<AdjuntoEmail> adjuntos)
    {
        this.de = de;
        this.para = para;
        this.cc = cc;
        this.asunto = asunto;
        this.cuerpo = cuerpo;
        this.formatoHTML = true;
        this.adjuntos = adjuntos;
        this.credenciales = new System.Net.NetworkCredential(Constantes.EmailUser,
                Constantes.EmailPwd);
    }
	/// <summary>
	/// Envía el e-mail.
	/// </summary>
	/// <returns></returns>
	public bool Enviar()
	{	
		MailMessage msg = new MailMessage();
		SmtpClient smtp = new SmtpClient();
		bool resultado;
        
		Funciones.Log("E-mail: [To]: |" + Para + "|");
		
		try
		{
            // [28/10/2014] - Parche para los emails de YPF.
            if (De.Contains("@set.ypf.com")) De = De.Replace("@set.ypf.com", "@servaind.com");
		
			msg.From = new MailAddress(this.De);			
			Funciones.Log("E-mail: [From]: |" + De + "|");			
			msg.From = new MailAddress("intranet@servaind.com");
			
            string[] to = this.Para.Split(';');
            if (to.Length > 0)
            {
                foreach (string recipient in to)
                {
					if(recipient.Trim().Length == 0) continue;
                    msg.To.Add(recipient);
                }
            }
            string[] cc = this.CC.Split(';');
			if (this.cc.Trim().Length > 0)
			{
                foreach (string recipient in cc)
                {
					if(recipient.Trim().Length == 0) continue;
                    msg.CC.Add(recipient);
                }
			}
			msg.Subject = this.asunto;
			msg.IsBodyHtml = formatoHTML;
			msg.Body = this.cuerpo;

            if (this.Adjuntos != null && this.Adjuntos.Count > 0)
            {
                foreach (AdjuntoEmail adjunto in this.Adjuntos)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(adjunto.Path);
                        msg.Attachments.Add(new Attachment(sr.BaseStream, adjunto.Nombre));
                    }
                    catch
                    {

                    }
                }
            }

			smtp.Host = Constantes.EmailServer;
			smtp.EnableSsl = true;
			smtp.Credentials = this.credenciales;
			ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;
			smtp.Send(msg);

			resultado = true;
		}
		catch (Exception ex)
		{
			Funciones.Log("E-mail: " + ex.Message + " - " + ex.StackTrace + "//" + para + "//" + cc);
			resultado = false;
		}

		return resultado;
	}
}
