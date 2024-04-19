using Microsoft.EntityFrameworkCore;
using ShipDiplom.Database;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Services;

public class ShipService : IShipService
{
    private readonly AppDbContext _context;
    public ShipService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<string> CanDockShips(string shipId, string pierId)
    {
        var pier = await _context.Piers.FirstOrDefaultAsync(x => x.Id == pierId);
        if (pier == null)
        {
            return "Такого пирса не существует.";
        }

        var newShip = await _context.Ships.FirstOrDefaultAsync(x => x.Id == shipId);
        var allShips = await _context.Ships.Where(x => x.Pier.Id == pierId).ToListAsync();

        allShips.Add(newShip);

        var sortedShips = allShips.OrderByDescending(ship => ship.Length)
                                  .ThenByDescending(ship => ship.Width)
                                  .ToList();

        int levelNum = 1;
        double?[] heights = new double?[sortedShips.Count + 1];
        double?[] widths = new double?[sortedShips.Count + 1];
        double? countWidths = 0;

        foreach (var ship in sortedShips)
        {
            if (ship.Width > pier.Width || ship.Length > pier.Length)
            {
                return "Невозможно причалить, корабль слишком большой.";
            }

            int bestLevel = -1;
            double? minResidualSpace = double.MaxValue;

            for (int i = 0; i < levelNum; i++)
            {
                var residualSpaceLength = pier.Length - heights[i];
                if (ship.Length <= residualSpaceLength && (ship.Width <= widths[i] || widths[i] == null))
                {
                    if (residualSpaceLength >= minResidualSpace.GetValueOrDefault())
                    {
                        continue;
                    }
                    bestLevel = i;
                    minResidualSpace = residualSpaceLength;
                }
            }

            if (bestLevel != -1)
            {
                heights[bestLevel] += ship.Length;
                widths[bestLevel] = ship.Width;
                countWidths += ship.Width;
            }
            else
            {
                if (countWidths + ship.Width > pier.Width)
                {
                    return "Корабль не может пришвартоваться из-за превышения размеров причала";
                }
                heights[levelNum] = ship.Length;
                widths[levelNum] = ship.Width;
                countWidths += ship.Width;
                levelNum++;
            }
        }

        return "Корабль причалил.";
    }
    public async Task<string> CreateShip(Ship ship)
    {
        await _context.Ships.AddAsync(ship);
        await _context.SaveChangesAsync();

        return "Корабль добавлен";
    }
    public async Task<string> UpdateShip(Ship ship)
    {
        if (!_context.Ships.Any(s => s.Id == ship.Id))
        {
            throw new Exception("Корабль с таким идентификатором не существует.");
        }

        var newShip = _context.Ships.Update(ship);
        await _context.SaveChangesAsync();

        return "Данные корабля обновлены";
    }
    public async Task<string> DeleteShip(string id)
    {
        if (!_context.Ships.Any(s => s.Id == id))
        {
            throw new Exception("Корабль с таким идентификатором не существует.");
        }

        var shipByDelete = _context.Set<Ship>().FirstOrDefault(x => x.Id == id) ?? throw new Exception("Корабль с таким идентификатором не существует.");
        _context.Ships.Remove(shipByDelete);
        await _context.SaveChangesAsync();

        return "Корабль удален";
    }
    public async Task<Ship> GetShip(string id)
    {        
        var ship = _context.Ships.FindAsync(id).Result;
        return ship == null ? throw new Exception("Корабль с таким идентификатором не существует.") : ship;
    }

    public async Task<List<Ship>> GetAllShip()
    {
        var allShips = _context.Ships.ToList();
        await _context.SaveChangesAsync();
        return allShips;
    }
}
