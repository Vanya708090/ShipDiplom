using ShipDiplom.Models.Entities;

namespace ShipDiplom.Models;

public class DockShipViewModel
{
    public string ShipId {  get; set; }
    public List<Pier> Piers { get; set; }
}
