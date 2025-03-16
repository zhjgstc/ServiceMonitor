using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace ServiceMonitor.Web.Logics
{
    public class EmailHelper
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailHelper(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort,SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // 处理异常
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        //var smtpServer = "smtp.example.com";
        //var smtpPort = 587;
        //var smtpUser = "your-email@example.com";
        //var smtpPass = "your-email-password";

        //var emailHelper = new EmailHelper(smtpServer, smtpPort, smtpUser, smtpPass);

        //var from = "your-email@example.com";
        //var to = "recipient@example.com";
        //var subject = "Test Email";
        //var body = "<h1>This is a test email</h1><p>Sent using MailKit and MimeKit in .NET Core.</p>";

        //await emailHelper.SendEmailAsync(from, to, subject, body);

        //Console.WriteLine("Email sent successfully.");
    }
}
