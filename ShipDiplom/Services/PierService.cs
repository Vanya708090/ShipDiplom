using Microsoft.EntityFrameworkCore;
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
    public async Task<string> CreatePier(Pier pier)
    {
        await _context.Set<Pier>().AddAsync(pier);
        await _context.SaveChangesAsync();

        return "Причал добавлен";
    }
    public async Task<string> UpdatePier(Pier pier)
    {
        if (!_context.Piers.Any(s => s.Id == pier.Id))
        {
            throw new Exception("Pier с таким идентификатором не существует.");
        }

        var newPier = _context.Set<Pier>().Update(pier);
        await _context.SaveChangesAsync();

        return "Данные причала обновлены";
    }

    public async Task<string> DeletePier(string id)
    {
        if (!_context.Piers.Any(s => s.Id == id))
        {
            throw new Exception("Причал с таким идентификатором не существует.");
        }

        var pierByDelete = _context.Set<Pier>().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Причал с таким идентификатором не существует.");
        _context.Set<Pier>().Remove(pierByDelete);
        await _context.SaveChangesAsync();

        return "Причал удален";
    }
    public async Task<Pier> GetPier(string id)
    {
        var pier = _context.Set<Pier>().FindAsync(id).Result;
        if (pier == null)
        {
            throw new Exception("Причал с таким идентификатором не существует.");
        }
        return pier;
    }

    public async Task<List<Ship>> GetPierShips(string id)
    {
        var pierShips = await _context.Piers.Include(x => x.Ships).FirstOrDefaultAsync(x => x.Id == id);

        return pierShips == null ? throw new Exception("Причал с таким идентификатором не существует.") : pierShips.Ships;
    }

    public async Task<bool> AddPierShips(string pierId, string shipId)
    {
        var pierShips = await _context.Piers.Include(x => x.Ships).FirstOrDefaultAsync(x => x.Id == pierId);
        var ship = await _context.Ships.FindAsync(shipId) ?? throw new Exception("Корабль с таким идентификатором не существует.");
        if (pierShips != null)
        {
            pierShips.Ships.Add(ship);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Причал с таким идентификатором не существует.");
        }
        return true;
    }


    public async Task<List<Pier>> GetAllPiers()
    {
        var allPiers = await _context.Piers.ToListAsync();
        return allPiers;
    }
}
