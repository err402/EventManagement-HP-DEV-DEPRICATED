using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Core.Models;

[Table("events", Schema = "event_management")]
public class Event
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("is_draft")]
    public bool IsDraft { get; set; }
}