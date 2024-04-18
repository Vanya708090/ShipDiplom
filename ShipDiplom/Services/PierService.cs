using ShipDiplom.Database;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Services;

public class PierService : IPierService
{
    private readonly AppDbContext _context;

    public PierService(AppDbContext context)
    {
        _context = context;
    }
    public async Task CreatePier(Pier pier)
    {
        await _context.Set<Pier>().AddAsync(pier);
        await _context.SaveChangesAsync();
    }
    public async Task UpdatePier(Pier pier)
    {
        if (!_context.Piers.Any(s => s.Id == pier.Id))
        {
            throw new Exception("Pier с таким идентификатором не существует.");
        }

        var newPier = _context.Set<Pier>().Update(pier);
        await _context.SaveChangesAsync();
    }
    public async Task DeletePier(string id)
    {
        if (!_context.Piers.Any(s => s.Id == id))
        {
            throw new Exception("Pier с таким идентификатором не существует.");
        }

        var pierByDelete = _context.Set<Pier>().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Pier с таким идентификатором не существует.");
        _context.Set<Pier>().Remove(pierByDelete);
        await _context.SaveChangesAsync();
    }
    public async Task<Pier> GetPier(string id)
    {
        var pier = _context.Set<Pier>().Find(id);
        if (pier == null)
        {
            throw new Exception("Pier с таким идентификатором не существует.");
        }
        return pier;
    }

    public async Task<List<Pier>> GetAllPiers()
    {
        var allPiers = _context.Set<Pier>().ToList();
        return allPiers;
    }
}
