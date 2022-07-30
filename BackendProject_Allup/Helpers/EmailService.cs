using SelectPdf;
using System.Net.Mail;

namespace BackendProject_Allup.Helpers
{
    public class EmailService
    {

        private readonly string _privateEmail;
        private readonly string _privatePassword;

        public EmailService(string privateEmail, string privatePassword)
        {
            _privateEmail = privateEmail;
            _privatePassword = privatePassword;
        }


        public bool SendEmail(string UserEmail,string subject,string body,string filename, byte[] bytes)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_privateEmail);
            mailMessage.To.Add(new MailAddress(UserEmail));



            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body =body;

            mailMessage.Attachments.Add(new Attachment(new MemoryStream(bytes), filename));

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_privateEmail, _privatePassword);
            client.Host = "smtp.mail.ru";
            client.EnableSsl = true;
            client.Port = 587;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (System.Exception)
            {


            }

            return false;
        }
    }
}
