using ErrorOr;
using MediatR;
using System.Net;
using System.Net.Mail;
using Serilog;
using Showcase.Utilities;


namespace Showcase.Infrastructure.Email.Commands;

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, ErrorOr<SendEmailResponse>>
{
    public async Task<ErrorOr<SendEmailResponse>> Handle(SendEmailCommand command, CancellationToken cancellationToken)
    {
        try
        {
            using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525);
            client.Credentials = new NetworkCredential($"{EnvironmentReader.Email.EmailUsername}", $"{EnvironmentReader.Email.EmailPassword}");
            client.EnableSsl = true;
            
            string body = $"from {command.FirstName} {command.LastName} \n\n " +
                          $"email: {command.Email} \n\n" +
                          $"phonenumber: {command.PhoneNumber} \n\n" +
                          $"{command.Message}";
            
            client.Send("from@example.com", "to@example.com", command.Subject, body);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Something went wrong in SendEmail");
            return Error.Failure();
        }
        return new SendEmailResponse(true);
    }
}