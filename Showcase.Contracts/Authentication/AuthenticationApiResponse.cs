namespace Showcase.Contracts.Authentication;

public record AuthenticationApiResponse(Guid UserId, string Username, string Token)
{
}