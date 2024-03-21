using Showcase.Domain.Entities;

namespace Showcase.Application.Common.Interfaces.Services;

public interface IJwtTokenService
{
    Task<string> GenerateUserTokenAsync(ApplicationUser user);
    Task<string> DecodeUserIdFromToken(string token);
}