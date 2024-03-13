using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showcase.Domain.Entities;

public class BoardPosition
{
    [Key]
    public Guid Id { get; set;  }
    public int Position { get; set; }
    public Guid? PlayerGuid { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime? ChangedAt { get; set; }
}