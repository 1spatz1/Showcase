namespace Showcase.Api.Routes;

public static class V1Routes
{
    public static class Authentication
    {
        public const string Controller = "/auth";

        public const string Login = "login";
        public const string Register = "register";
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
}