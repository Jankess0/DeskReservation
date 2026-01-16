using System.Net.Mail;
using DeskReservation.Models;
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

    public void SendEmail(Desk desk)
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
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(myEmail, myPassword);
                client.Send(message);
                client.Disconnect(true);

            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Filed to seend email: {ex.Message}");
            }
        }
    }
}