using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showcase.Domain.Entities;

public class Game
{
    [Key]
    public Guid Id { get; set;  }
    [ForeignKey("PlayerOne")]
    public Guid PlayerOneId { get; set; }
    public ApplicationUser PlayerOne { get; set; }
    [ForeignKey("PlayerTwo")]
    public Guid? PlayerTwoId { get; set; }
    public ApplicationUser? PlayerTwo { get; set; }
    public Guid? PlayerTurn { get; set; }
    public GameState State  { get; set; } = GameState.WaitingForPlayerTwo;
    public Guid? WinnerId { get; set; }
    public int Turns { get; set; }
    public BoardPosition[] Board { get; set; } = new BoardPosition[9];
    [Column(TypeName = "DateTime")]
    public DateTime CreatedAt { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime UpdatedAt { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime? FinishedAt { get; set; }
    
    public Game()
    {
        for (int i = 0; i < 9; i++)
        {
            Board[i] = new BoardPosition { Position = i };
        }
    }
}