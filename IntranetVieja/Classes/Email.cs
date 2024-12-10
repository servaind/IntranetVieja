using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Proser.Communications.Network.Mailing;

public class Email
{
    //18/11/2024-AGM: Tomo los valores de configuracion para envio de mail del App.config
    private static string DEFAULT_SERVER = ConfigurationManager.AppSettings["DEFAULT_SERVER"];
    private static int DEFAULT_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["DEFAULT_PORT"].ToString());
    private static string DEFAULT_USER = ConfigurationManager.AppSettings["DEFAULT_USER"];
    private static string DEFAULT_PWD = ConfigurationManager.AppSettings["DEFAULT_PWD"];
    private static string DEFAULT_SENDER = ConfigurationManager.AppSettings["DEFAULT_SENDER"];

    private string de;
    private string para;
    private string cc;
    private string asunto;
    private string cuerpo;
    private List<AdjuntoEmail> adjuntos;


    public Email(string de, string para, string cc, string asunto, string cuerpo): this(de, para, cc, asunto, cuerpo, null)
    {
    }

    public Email(string de, string para, string cc, string asunto, string cuerpo, List<AdjuntoEmail> adjuntos)
    {
        this.de = de;
        this.para = para;
        this.cc = cc;
        this.asunto = asunto;
        this.cuerpo = cuerpo;
        this.adjuntos = adjuntos;
    }

    public bool Enviar()
    {
        bool resultado;

        try
        {
            List<Attachment> attachments = null;
            if (adjuntos != null && adjuntos.Count > 0)
            {
                attachments = new List<Attachment>();

                foreach (AdjuntoEmail adjunto in adjuntos)
                {
                    try
                    {
                        var attachment = new Attachment(File.ReadAllBytes(adjunto.Path), adjunto.Nombre);
                        attachments.Add(attachment);
                    }
                    catch
                    {

                    }
                }
            }

            Send(DEFAULT_SENDER, para, cc, asunto, cuerpo, attachments);
            resultado = true;
        }
        catch (Exception ex)
        {
            Funciones.Log("E-mail: " + ex.Message + " - " + ex.StackTrace + "//" + para + "//" + cc);
            resultado = false;
        }

        return resultado;
    }

    // 18/11/2024-AGM: Reemplazo de uso proser.Communications por librería mailkit para envío de mails
    public void Send(string from, string to, string cc, string subject, string contenido, 
        List<Attachment> attachments = null)
    {
        try
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress(from, from));

            var destinatarios = to.Split(';');
            foreach (var destinatario in destinatarios)
            {
                mensaje.To.Add(new MailboxAddress(destinatario, destinatario));
            }

            var copias = cc.Split(';');
            foreach (var copia in copias)
            {
                mensaje.Cc.Add(new MailboxAddress(copia, copia));
            }

            mensaje.Subject = subject;
            var body = new TextPart(TextFormat.Html)
            {
                Text = contenido
            };

            var multipart = new Multipart("mixed");
            multipart.Add(body);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var mimePart = new MimePart("application", "octet-stream")
                    {
                        Content = new MimeContent(new MemoryStream(attachment.File), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = attachment.Name
                    };
                    multipart.Add(mimePart);
                }
            }

            mensaje.Body = multipart;

            var client = new SmtpClient();
            client.Connect(DEFAULT_SERVER, DEFAULT_PORT, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(DEFAULT_USER, DEFAULT_PWD);
            client.Send(mensaje);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {
            throw new Exception("No se pudo enviar el email. " + ex.Message);
        }
    }
}
