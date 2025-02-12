using System.ComponentModel.DataAnnotations;

namespace EventManagement.DTOs;

public class CreateEventDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}