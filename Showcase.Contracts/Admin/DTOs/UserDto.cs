namespace Showcase.Contracts.Admin.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public IList<string> Roles { get; set; }
    public int TotalGamesPlayed { get; set; }
    public int GamesWon { get; set; }
}