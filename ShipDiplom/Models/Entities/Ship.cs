namespace ShipDiplom.Models.Entities;

public class Ship : BaseEntity
{
    public string Name { get; set; }
    public string SystemId { get; set; }
    public float? Length { get; set; }
    public float? Width { get; set; }
    public string OwnerId { get; set; }
    public string PierId { get; set; }
    public Pier Pier { get; set; }
}
