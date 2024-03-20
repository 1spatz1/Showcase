using Showcase.Contracts.Admin.DTOs;
using Showcase.Domain.Entities;

namespace Showcase.Application.Admin.Queries.GetUnlockedUsers;

public class GetAllUsersResponse
{
    public List<UserDto> UnlockedUsers;
    public List<UserDto> LockedUsers;
    
    public GetAllUsersResponse(List<UserDto> unlockedUsers, List<UserDto> lockedUsers)
    {
        UnlockedUsers = unlockedUsers;
        LockedUsers = lockedUsers;
    }
}