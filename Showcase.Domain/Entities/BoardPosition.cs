namespace Showcase.Domain.Entities;

public class BoardPosition
{
    public int Id { get; set; }
    private Guid? PlayerGuid { get; set; }
    private DateTime? ChangedAt { get; set; }
}