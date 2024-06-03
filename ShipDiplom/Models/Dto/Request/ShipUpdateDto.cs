using System.ComponentModel.DataAnnotations;

namespace ShipDiplom.Models.Dto.Request;

public class ShipUpdateDto
{
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int SystemId { get; set; }

    [Range(0.1, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом.")]
    public float? Length { get; set; }

    [Range(0.1, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом.")]
    public float? Width { get; set; }

    [Required]
    public int OwnerId { get; set; }
    public string? PierId { get; set; }
}

