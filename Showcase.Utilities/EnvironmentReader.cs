namespace Showcase.Utilities;

public class EnvironmentReader
{
    public static class Authentication
    {
        public static int? MaxFailedAttempts =>
            Convert.ToInt32(Environment.GetEnvironmentVariable("AUTHENTICATION_MAXFAILEDATTEMPTS") ?? string.Empty);
        
        public static int? LockoutMinutes =>
            Convert.ToInt32(Environment.GetEnvironmentVariable("AUTHENTICATION_LOCKOUTMINUTES") ?? string.Empty);
    }
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
    
    public static class Jwt
    {
        public static string SigningKey => Environment.GetEnvironmentVariable("JWT_SIGNINGKEY") ??
                                          throw new InvalidOperationException();

        public static int? ExpiryDays =>
            Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRYDAYS") ?? string.Empty);
        
        public static int? ExpiryHours =>
            Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRYHOURS") ?? string.Empty);
        
        public static int? ExpiryMinutes =>
            Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRYMINUTES") ?? string.Empty);

        public static string Issuer => Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                                       throw new InvalidOperationException();

        public static string Audience => Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                                         throw new InvalidOperationException();
    }

    public static class Database
    {
        public static string ConnectionString => Environment.GetEnvironmentVariable("DATABASE_CONNECTIONSTRING") ??
                                                 throw new InvalidOperationException();
    }
    
    public static class TwoFactorAuthentication
    {
        public static string Issuer => Environment.GetEnvironmentVariable("TOTP_ISSUER") ??
                                             throw new InvalidOperationException();
    }
}