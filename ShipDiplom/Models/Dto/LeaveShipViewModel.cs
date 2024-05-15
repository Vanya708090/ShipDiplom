using ShipDiplom.Models.Entities;

namespace ShipDiplom.Models.Dto;

public class LeaveShipViewModel
{
    public string PierId {  get; set; }
    public List<Ship> Ships { get; set; }
}
