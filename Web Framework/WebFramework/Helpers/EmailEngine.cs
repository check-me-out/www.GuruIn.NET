namespace WebFramework.Helpers
{
    using log4net;
    using System;
    using System.Net;
    using System.Net.Mail;

    public static class EmailEngine
    {
        private const string EmailTemplate = @"<!DOCTYPE html>
<body style=""width: 100%"">
<div style=""background-color: #4291DB;color: #fff;"">
<div style=""min-width: 60%;display: inline-block;padding: 10px 50px;vertical-align: top;"">
<a href=""http://www.GuruIn.NET"" target=""_blank"" style=""color: #fff;text-decoration: none;cursor: pointer;font-size: 1.8em;"">GuruIn.NET</a></div>
<img src=""http://guruin.net/images/envelope.png"" alt="""" />
</div>
<div style=""background-color: #4291DB;color: #fff;"">
<div style=""display: table-cell;margin: 0 auto;padding: 10px 50px;text-transform: capitalize;font-size: 2em;"">{0}</div>
</div>
<div style=""padding-bottom: 15px"">
<img style=""min-width: 100%;max-width: 100%;max-height: 63px"" src=""http://guruin.net/images/banner.png"" alt="""" />
</div>
<div style=""display: block;background-color: #fff;color: #333;padding: 30px 20px;"">
{1}</div>
<div style=""display: block;background-color: #ccc;color: #000;padding: 25px;"">
© 2016 - GuruIn.NET
</div>
</body>";

        private static ILog _log;

        private static string _smtpPassword = "****";

        public static void Logger(ILog log)
        {
            _log = log;
        }

        public static string GetTemplate(string templateName)
        {
            var template = EmbeddedResource.GetContents("WebFramework.Content.EmailTemplate." + templateName + ".txt");
            return template;
        }

        public static bool SendEmail(string from, string fromName, string to, string subject, string title, string body, string cc = null, string bcc = null, string replyTo = null, Attachment attachment = null)
        {
            try
            {
                var host = ConfigManager.Get<string>("SmtpHost");
                var port = ConfigManager.Get<int>("SmtpPort");
                var userName = ConfigManager.Get<string>("SmtpUserName");
                var ssl = ConfigManager.Get<bool>("SmtpEnableSsl");

                var smtpClient = new SmtpClient(host, port);
                using (var mailMessage = new MailMessage(new MailAddress(from, fromName), new MailAddress(to)))
                {
                    mailMessage.Subject = subject;
                    //var emailBody = EmailTemplate.Replace("{0}", title).Replace("{1}", body);
                    AlternateView item = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    mailMessage.AlternateViews.Add(item);
                    mailMessage.IsBodyHtml = true;

                    if (!string.IsNullOrEmpty(cc)) mailMessage.CC.Add(new MailAddress(cc));
                    if (!string.IsNullOrEmpty(bcc)) mailMessage.Bcc.Add(new MailAddress(bcc));
                    if (!string.IsNullOrEmpty(replyTo)) mailMessage.ReplyToList.Add(replyTo);
                    if (attachment != null) mailMessage.Attachments.Add(attachment);

                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(userName, _smtpPassword);
                    smtpClient.EnableSsl = ssl;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Timeout = 300000;

                    smtpClient.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                _log.Error("EmailEngine", ex);
            }

            return false;
        }
    }
}