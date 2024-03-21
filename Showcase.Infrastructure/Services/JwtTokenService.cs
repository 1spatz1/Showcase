using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Showcase.Application.Common.Interfaces.Services;
using Showcase.Domain.Entities;
using Showcase.Utilities;

namespace Showcase.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILogger<JwtTokenService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenService(UserManager<ApplicationUser> userManager, ILogger<JwtTokenService> logger)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<string> GenerateUserTokenAsync(ApplicationUser user)
    {
        // Add identity claims to token
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        // Add role claims to token
        IList<string> roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(EnvironmentReader.Jwt.SigningKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        DateTime expires = DateTime.Now.AddMinutes(Convert.ToDouble(EnvironmentReader.Jwt.ExpiryMinutes));
        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken? securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = EnvironmentReader.Jwt.Issuer,
            Audience = EnvironmentReader.Jwt.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = credentials
        });

        string generateUserTokenAsync = tokenHandler.WriteToken(securityToken);
        _logger.LogInformation("Created new token for user {user}", user.UserName);
        return generateUserTokenAsync;
    }

    public async Task<string> DecodeUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid").Value;
    
        return id;
    }
    // public async Task<Guid> DecodeUserIdFromToken(string token)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     if (!tokenHandler.CanReadToken(token))
    //     {
    //         Console.WriteLine("Invalid JWT token");
    //         return Guid.Empty;
    //     }
    //
    //     var jwtToken = tokenHandler.ReadJwtToken(token);
    //     var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid");
    //     if (claim == null)
    //     {
    //         Console.WriteLine("JWT token does not contain a NameIdentifier claim");
    //         return Guid.Empty;
    //     }
    //
    //     if (!Guid.TryParse(claim.Value, out var id))
    //     {
    //         Console.WriteLine("NameIdentifier claim is not a valid Guid");
    //         Console.WriteLine(claim.Value);
    //         return Guid.Empty;
    //     }
    //
    //     return id;
    // }
}