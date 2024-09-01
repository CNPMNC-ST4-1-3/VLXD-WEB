using System;
using System.Net;
using System.Net.Mail;

public  class MailUtilsHelpers
{
    public MailUtilsHelpers() { }
    public  void SendEmail(string toAddress, string subject, string body)
    {
        try
        {
            var fromAddress = new MailAddress("your-email@example.com", "Your Name");
            var recipientAddress = new MailAddress(toAddress, "Recipient Name");
            const string fromPassword = "your-email-password";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, recipientAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        catch (SmtpException smtpEx)
        {
            // Handle SMTP-specific errors
            Console.WriteLine($"SMTP Error: {smtpEx.Message}");
        }
        catch (Exception ex)
        {
            // Handle general errors
            Console.WriteLine($"General Error: {ex.Message}");
        }
    }
}