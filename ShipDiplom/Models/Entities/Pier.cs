namespace ShipDiplom.Models.Entities;

public class Pier: BaseEntity
{
    public string Name { get; set; }
    public float? Length { get; set; }
    public float? Width { get; set; }
    public string TrackingNumber { get; set; }
    public string Location { get; set; }
    public List<Ship> Ships { get; set; } = new();
}
