namespace ShipDiplom.Interfaces;

public interface IShipService
{
    Task<string> CanDockShips(string shipId, string pierId);

}
