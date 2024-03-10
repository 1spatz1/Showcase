using System.Net;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Serilog;
using Showcase.Domain.Common.Errors;
using Showcase.Utilities;

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
                    $"https://www.google.com/recaptcha/api/siteverify?secret={EnvironmentReader.Recaptcha.GoogleRecaptchaClientSecret}&response={request.RecaptchaToken}")
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
        catch(Exception ex)
        {
            Log.Error(ex, "Something went wrong in Recaptcha");
            return Errors.Authorisation.ReCaptchaFailed;
        }
        return new ValidateRecaptchaResponse(true);
    }
}