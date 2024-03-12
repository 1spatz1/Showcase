namespace Showcase.Domain.Entities;

public class Game
{
    public Guid Id { get; set;  }
    public Guid PlayerOneId { get; set; }
    public ApplicationUser PlayerOne { get; set; }
    public Guid PlayerTwoId { get; set; }
    public ApplicationUser PlayerTwo { get; set; }
    public Guid? PlayerTurn { get; set; }
    public GameState State  { get; set; } = GameState.WaitingForPlayerTwo;
    public int Turns { get; set; }
    public BoardPosition[] Board { get; set; } = new BoardPosition[9];
    public DateTime CreatedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    
    public Game()
    {
        for (int i = 0; i < 9; i++)
        {
            Board[i] = new BoardPosition { Id = i };
        }
    }
}