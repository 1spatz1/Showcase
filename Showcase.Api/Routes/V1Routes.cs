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
}