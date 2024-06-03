using ShipDiplom.Models.Entities;

namespace ShipDiplom.Interfaces;

public interface IShipService
{
    Task<string> CanDockShips(string shipId, string pierId);
    Task<string> CreateShip(Ship ship);
    Task<string> DeleteShip(string id);
    Task<List<Ship>> GetAllShip();
    Task<Ship> GetShip(string id);
    Task<string> UpdateShip(Ship ship);
    Task<string> LeaveDock(string pierId, string shipId);
    Task<List<List<Ship>>> CanDockShipsOptionalLocation(string pierId);
}
