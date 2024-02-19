using System.Net;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Showcase.Domain.Common.Errors;

namespace Showcase.Infrastructure.Recaptcha.Queries;

public class ValidateRecaptchaQueryHandler : IRequestHandler<ValidateRecaptchaQuery, ErrorOr<ValidateRecaptchaResponse>>
{
    public async Task<ErrorOr<ValidateRecaptchaResponse>> Handle(ValidateRecaptchaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            HttpClient httpClient = new HttpClient();
            var res = httpClient
                .GetAsync(
                    //TODO ADD A FUCKING API KEY!!!!
                    $"https://www.google.com/recaptcha/api/siteverify?secret=6Lf8u3IpAAAAABloZKq98oJKdcmU4XKraZ-06m-o&response={request.RecaptchaToken}")
                .Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return new ValidateRecaptchaResponse(false);
            }
            
            string jsonRes = res.Content.ReadAsStringAsync().Result;
            using (JsonDocument document = JsonDocument.Parse(jsonRes))
            {
                JsonElement root = document.RootElement;
                bool success = root.GetProperty("success").GetBoolean();
                decimal? score = root.GetProperty("score").GetDecimal();

                if (success != true || score <= 0.5m)
                {
                    return new ValidateRecaptchaResponse(false);
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return Errors.Authorisation.ReCaptchaFailed;
        }
        return new ValidateRecaptchaResponse(true);
    }
}