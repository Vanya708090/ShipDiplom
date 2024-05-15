using System.ComponentModel.DataAnnotations;

namespace ShipDiplom.Models.Entities;

public class Pier: BaseEntity
{
    [Required(ErrorMessage = "Наименование причала обязательно для заполнения.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Длина обязательна для заполнения.")]
    [Range(0.1, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом.")]
    public float? Length { get; set; }

    [Required(ErrorMessage = "Длина обязательна для заполнения.")]
    [Range(0.1, float.MaxValue, ErrorMessage = "Длина должна быть положительным числом.")]
    public float? Width { get; set; }

    [Required(ErrorMessage = "Номер трекинга обязателен для заполнения.")]
    public string TrackingNumber { get; set; }

    [Required(ErrorMessage = "Геолокация обязательна для заполнения.")]
    public string Location { get; set; }
    public List<Ship> Ships { get; set; } = new();
}
