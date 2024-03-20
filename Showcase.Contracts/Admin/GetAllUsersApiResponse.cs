using Showcase.Contracts.Admin.DTOs;
using Showcase.Domain.Entities;

namespace Showcase.Contracts.Admin;

public class GetAllUsersApiResponse
{
    public List<UserDto> UnlockedUsers { get; set; }
    public List<UserDto> LockedUsers { get; set; }
}