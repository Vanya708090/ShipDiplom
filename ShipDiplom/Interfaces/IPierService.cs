using ShipDiplom.Models.Entities;

namespace ShipDiplom.Interfaces;

public interface IPierService
{
    Task<string> CreatePier(Pier pier);
    Task<string> DeletePier(string id);
    Task<List<Pier>> GetAllPiers();
    Task<Pier> GetPier(string id);
    Task<string> UpdatePier(Pier pier);
}
