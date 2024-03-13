using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showcase.Domain.Entities;

public class Game
{
    [Key]
    public Guid Id { get; set;  }
    public Guid PlayerOneId { get; set; }
    public Guid? PlayerTwoId { get; set; }
    public Guid? PlayerTurn { get; set; }
    public GameState State  { get; set; } = GameState.WaitingForPlayerTwo;
    public Guid? WinnerId { get; set; }
    public int BoardSize { get; set; } = 0;
    [Column(TypeName = "DateTime")]
    public DateTime CreatedAt { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime UpdatedAt { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime? FinishedAt { get; set; }
    public ICollection<BoardPosition>? Board { get; set; }
}