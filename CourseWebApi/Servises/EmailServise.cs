using System.Net;
using System.Net.Mail;

namespace CourseWebApi.Servises
{
    public class EmailServise : IEmailServise
    {
        public async Task SendMessage(string message,string miniDescription ,string? userEmail)
        {
            if (userEmail is null)
                return;

            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string smtpUserName = "skillforge56@gmail.com";
            string smtpPassword = "gbud saan tqin pjui";

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 30000;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUserName);
                    mailMessage.To.Add(userEmail);
                    mailMessage.Subject = miniDescription;
                    mailMessage.Body = message;

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
