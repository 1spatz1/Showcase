namespace Showcase.Api.Routes;

public static class V1Routes
{
    public static class Authentication
    {
        public const string Controller = "/auth";

        public const string Login = "login";
        public const string Register = "register";
        public const string ConfigureTotp = "configuretotp";
        public const string DisableTotp = "disabletotp";
    }
    public static class Contact
    {
        public const string Controller = "/contact";

        public const string SendEmail = "";
    }
    
    public static class Game
    {
        public const string Controller = "/game";

        public const string Create = "create";
        public const string Join = "join";
        public const string Turn = "turn";
        public const string Get = "get";
    }
    
    public static class Admin
    {
        public const string Controller = "/admin";
        
        public const string LockUser = "lock";
        public const string UnlockUser = "unlock";
        public const string GetAllUsers = "users";
    }
}