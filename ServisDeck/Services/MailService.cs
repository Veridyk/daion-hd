using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ServisDeck.Services
{
    public class MailService : IMailService
    {
        public static string CREATE_TITLE = "Požadavek byl vytvořen uživatelem: ";
        public static string UPDATE_TITLE = "Požadavek byl změněn uživatelem: ";
        public void SendMail(List<string> receivers, string title, string subject, string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.Host = "wes1-smtp.wedos.net";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("service-desk@daion.cz", "#VeriJeDyk1");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage message = CreateEmail(receivers, title, subject, body);
                smtp.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public MailMessage CreateEmail(List<string> receivers, string title, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("service-desk@daion.cz");
            foreach(string mail in receivers)
                message.To.Add(new MailAddress(mail));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = RenderTemplate(receivers[0], title, subject, body);

            return message;
        }

        public string RenderTemplate(string sender, string title, string subject, string body)
        {
            return "<body>" +
                "<h2>" + title + sender + "</h2>" +
                "<h3>" + subject + "</h3>" +
                "<br />" +
                body +
                "</body>";
        }
    }
}
