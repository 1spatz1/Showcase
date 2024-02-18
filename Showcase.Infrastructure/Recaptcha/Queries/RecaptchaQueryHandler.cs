using System.Net;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Showcase.Domain.Common.Errors;

namespace Showcase.Infrastructure.Recaptcha.Queries;

public class RecaptchaQueryHandler : IRequestHandler<RecaptchaQuery, ErrorOr<RecaptchaResponse>>
{
    public async Task<ErrorOr<RecaptchaResponse>> Handle(RecaptchaQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Console.Write(request.RecaptchaToken);
            HttpClient httpClient = new HttpClient();
            var res = httpClient
                .GetAsync(
                    //TODO ADD A FUCKING API KEY!!!!
                    $"https://www.google.com/recaptcha/api/siteverify?secret=API_KEY&response={request.RecaptchaToken}")
                .Result;
            if (res.StatusCode != HttpStatusCode.OK)
            {
                return new RecaptchaResponse(false);
            }
            
            string jsonRes = res.Content.ReadAsStringAsync().Result;
            using (JsonDocument document = JsonDocument.Parse(jsonRes))
            {
                JsonElement root = document.RootElement;
                bool success = root.GetProperty("success").GetBoolean();
                decimal? score = root.GetProperty("score").GetDecimal();

                if (success != true || score <= 0.5m)
                {
                    return new RecaptchaResponse(false);
                }
            }
        }
        catch(Exception ex)
        {
            return Errors.Authorisation.ReCaptchaFailed;
        }
        return new RecaptchaResponse(true);
    }
}