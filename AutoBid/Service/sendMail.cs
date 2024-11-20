using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Authentication.Service
{
    public class MailService : Interface.IEmail
    {
        public async Task SendTestEmailAsync(string reciverName, string reciverEmail, string reciverMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Sandith", "sandithkariyawasam2001@gmail.com"));
                message.To.Add(new MailboxAddress(reciverName, reciverEmail));
                message.Subject = "Test Email using MailKit";

                // Email body
                message.Body = new TextPart("plain")
                {
                    Text = reciverMessage
                };

                // Initialize SmtpClient and connect to Gmail SMTP
                using (var client = new SmtpClient())
                {
                    // Connect to the Gmail SMTP server asynchronously
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate asynchronously with Gmail
                    await client.AuthenticateAsync("sandithkariyawasam2001@gmail.com", "orip gwit jaer jarf"); // Replace with your actual app password

                    // Send the message asynchronously
                    await client.SendAsync(message);

                    // Disconnect asynchronously
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
