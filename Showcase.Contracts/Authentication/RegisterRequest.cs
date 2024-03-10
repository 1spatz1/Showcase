namespace Showcase.Contracts.Authentication;

public record RegisterRequest(string Email, string Username, string Password);