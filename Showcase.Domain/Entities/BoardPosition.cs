using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showcase.Domain.Entities;

public class BoardPosition
{
    [Key]
    public Guid Id { get; set;  }
    public Guid PlayerGuid { get; set; }
    public Guid GameId { get; set; }
    public int RowIndex { get; set; }
    public int ColIndex { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime CreatedAt { get; set; }
}