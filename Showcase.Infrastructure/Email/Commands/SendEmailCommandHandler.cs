using System.Data;
using ErrorOr;
using MediatR;
using System.Net;
using System.Net.Mail;


namespace Showcase.Infrastructure.Email.Commands;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, ErrorOr<SendEmailResponse>>
{
    public async Task<ErrorOr<SendEmailResponse>> Handle(SendEmailCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //TODO ADD LOGIN CREDENTIALS!!!!
            using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);
            client.Credentials = new NetworkCredential("USERNAME", "PASSWORD");
            client.EnableSsl = true;
            
            string body = $"from {command.FirstName} {command.LastName} \n\n " +
                          $"email: {command.Email} \n\n" +
                          $"phonenumber: {command.PhoneNumber} \n\n" +
                          $"{command.Message}";
            
            client.Send("from@example.com", "to@example.com", command.Subject, body);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Failure();
        }
        return new SendEmailResponse(true);
    }
}