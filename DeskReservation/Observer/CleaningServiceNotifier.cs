using System.Net.Mail;
using DeskReservation.Models;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace DeskReservation.Observer;

public class CleaningServiceNotifier : IObserver
{
    private readonly IConfiguration _configuration;

    public CleaningServiceNotifier(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Update(Desk desk)
    {
        if (desk.Status == DeskState.Cleaning)
        {
            SendEmail(desk);
        }
    }

    public async Task SendEmail(Desk desk)
    {
        var myEmail = _configuration["EmailSettings:Email"];
        var myPassword = _configuration["EmailSettings:Password"];
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("System Rezerwacji", myEmail));
        message.To.Add(new MailboxAddress("DeskReservation", myEmail));
        message.Subject = $"Cleaning: {desk.Name}";
        message.Body = new TextPart("plain") { Text = $"Desk: {desk.Name} is ready for cleaning" };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(myEmail, myPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Filed to seend email: {ex.Message}");
            }
        }
    }
}