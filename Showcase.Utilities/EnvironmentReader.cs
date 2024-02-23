namespace Showcase.Utilities;

public class EnvironmentReader
{
    public static class Email
    {
        public static string EmailUsername =>
            Environment.GetEnvironmentVariable("MAILTRAP_USERNAME") ?? throw new InvalidOperationException();
        
        public static string EmailPassword =>
            Environment.GetEnvironmentVariable("MAILTRAP_PASSWORD") ?? throw new InvalidOperationException();
    }
    public static class Recaptcha
    {
        public static string GoogleRecaptchaClientSecret =>
            Environment.GetEnvironmentVariable("GOOGLE_RECAPTCHA_CLIENT_SECRET") ?? throw new InvalidOperationException();
    }
}