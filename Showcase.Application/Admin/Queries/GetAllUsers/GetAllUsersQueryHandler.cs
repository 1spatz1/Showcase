using System.Xml.XPath;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Showcase.Application.Common.Interfaces.Persistence;
using Showcase.Contracts.Admin.DTOs;
using Showcase.Domain.Common.Errors;
using Showcase.Domain.Entities;

namespace Showcase.Application.Admin.Queries.GetUnlockedUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ErrorOr<GetAllUsersResponse>>
{
    private readonly ILogger<GetAllUsersQueryHandler> _logger;
    private readonly IApplicationDbContext _context;
    
    public GetAllUsersQueryHandler(ILogger<GetAllUsersQueryHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<ErrorOr<GetAllUsersResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<UserDto> unlockedUsers;
        List<UserDto> lockedUsers;

        try
        {
            unlockedUsers = await (from user in _context.Users
               where !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow
               select new UserDto
               {
                   Id = user.Id,
                   UserName = user.UserName!,
                   Email = user.Email!,
                   LockoutEnd = user.LockoutEnd,
                   Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                   TotalGamesPlayed = user.PlayerOneGames.Count + user.PlayerTwoGames.Count,
                   GamesWon = user.PlayerOneGames.Count(g => g.WinnerId == user.Id) + user.PlayerTwoGames.Count(g => g.WinnerId == user.Id)
               })
               .ToListAsync(cancellationToken);

            lockedUsers = await (from user in _context.Users
                 where user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow
                 select new UserDto
                 {
                     Id = user.Id,
                     UserName = user.UserName!,
                     Email = user.Email!,
                     LockoutEnd = user.LockoutEnd,
                     Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                     TotalGamesPlayed = user.PlayerOneGames.Count + user.PlayerTwoGames.Count,
                     GamesWon = user.PlayerOneGames.Count(g => g.WinnerId == user.Id) + user.PlayerTwoGames.Count(g => g.WinnerId == user.Id)
                 })
                 .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users: {Message}", ex.Message);
            return Errors.Admin.GetAllusers.Error;
        }

        return new GetAllUsersResponse(unlockedUsers, lockedUsers);
    }
}