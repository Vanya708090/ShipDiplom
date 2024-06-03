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
    //public async Task<string> CanDockShips(string shipId, string pierId)
    //{
    //    var pier = await _context.Piers.FirstOrDefaultAsync(x => x.Id == pierId);
    //    if (pier == null)
    //    {
    //        return "Такого пирса не существует.";
    //    }

    //    var newShip = await _context.Ships.FirstOrDefaultAsync(x => x.Id == shipId);
    //    var allShips = await _context.Ships.Where(x => x.Pier.Id == pierId).ToListAsync();

    //    if (newShip != null)
    //    {
    //        allShips.Add(newShip);
    //    }

    //    var sortedShips = allShips.OrderByDescending(ship => ship.Length)
    //                              .ThenByDescending(ship => ship.Width)
    //                              .ToList();

    //    int levelNum = 1;
    //    double?[] heights = new double?[sortedShips.Count + 1];
    //    double?[] widths = new double?[sortedShips.Count + 1];
    //    double? countWidths = 0;

    //    foreach (var ship in sortedShips)
    //    {
    //        if (ship.Width > pier.Width || ship.Length > pier.Length)
    //        {
    //            return "Невозможно причалить, корабль слишком большой.";
    //        }

    //        int bestLevel = -1;
    //        double? minResidualSpace = double.MaxValue;

    //        for (int i = 0; i < levelNum; i++)
    //        {
    //            var residualSpaceLength = pier.Length - heights[i];
    //            if (ship.Length <= residualSpaceLength && (ship.Width <= widths[i] || widths[i] == null))
    //            {
    //                if (residualSpaceLength >= minResidualSpace.GetValueOrDefault())
    //                {
    //                    continue;
    //                }
    //                bestLevel = i;
    //                minResidualSpace = residualSpaceLength;
    //            }
    //        }

    //        if (bestLevel != -1)
    //        {
    //            heights[bestLevel] += ship.Length;
    //            widths[bestLevel] = ship.Width;
    //            countWidths += ship.Width;
    //        }
    //        else
    //        {
    //            if (countWidths + ship.Width > pier.Width)
    //            {
    //                return "Корабль не может пришвартоваться из-за превышения размеров причала";
    //            }
    //            heights[levelNum] = ship.Length;
    //            widths[levelNum] = ship.Width;
    //            countWidths += ship.Width;
    //            levelNum++;
    //        }
    //    }
    //    pier.Ships.Add(newShip);
    //    await _context.SaveChangesAsync();
    //    return "Корабль причалил.";
    //}

    //public async Task<string> CanDockShips(string shipId, string pierId)
    //{
    //    var pier = await _context.Piers.FirstOrDefaultAsync(x => x.Id == pierId);
    //    if (pier == null)
    //    {
    //        return "Такого пирса не существует.";
    //    }

    //    var newShip = await _context.Ships.FirstOrDefaultAsync(x => x.Id == shipId);
    //    var allShips = await _context.Ships.Where(x => x.Pier.Id == pierId).ToListAsync();

    //    if (newShip != null)
    //    {
    //        allShips.Add(newShip);
    //    }

    //    var sortedShips = allShips.OrderByDescending(ship => ship.Length)
    //                              .ThenByDescending(ship => ship.Width)
    //                              .ToList();

    //    double?[] heights = new double?[sortedShips.Count + 1];
    //    double?[] widths = new double?[sortedShips.Count + 1];

    //    bool canDock = TryDockShips(sortedShips, pier.Width, pier.Length, heights, widths, 0);

    //    if (canDock)
    //    {
    //        pier.Ships.Add(newShip);
    //        await _context.SaveChangesAsync();
    //        return "Корабль причалил.";
    //    }
    //    else
    //    {
    //        return "Корабль не может пришвартоваться из-за превышения размеров причала";
    //    }
    //}

    //private bool TryDockShips(List<Ship> ships, double? pierWidth, double? pierLength, double?[] heights, double?[] widths, int index)
    //{
    //    if (index == ships.Count)
    //    {
    //        return true;
    //    }

    //    var ship = ships[index];

    //    for (int i = 0; i <= index; i++)
    //    {
    //        if (ship.Width > pierWidth || ship.Length > pierLength)
    //        {
    //            return false;
    //        }

    //        var residualSpaceLength = pierLength - heights[i];
    //        if (ship.Length <= residualSpaceLength && (ship.Width <= widths[i] || widths[i] == null))
    //        {
    //            double? originalHeight = heights[i];
    //            double? originalWidth = widths[i];
    //            heights[i] += ship.Length;
    //            widths[i] = ship.Width;

    //            if (TryDockShips(ships, pierWidth, pierLength, heights, widths, index + 1))
    //            {
    //                return true;
    //            }

    //            heights[i] = originalHeight;
    //            widths[i] = originalWidth;
    //        }
    //    }

    //    return false;
    //}


    public async Task<string> LeaveDock(string pierId, string shipId)
    {
        var pier = await _context.Piers.FirstOrDefaultAsync(x => x.Id == pierId);

        if(pier == null)
        {
            return "Причал не найден";
        }

        var ship = await _context.Ships.FirstOrDefaultAsync(x => x.Id == shipId);

        if (ship == null)
        {
            return "Корабль не найден";
        }

        pier.Ships.Remove(ship);
        await _context.SaveChangesAsync();

        return $"Корабль {ship.Name} отчалил от причала.";
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
        var ship = await _context.Ships.FindAsync(id);
        return ship == null ? throw new Exception("Корабль с таким идентификатором не существует.") : ship;
    }

    public async Task<List<Ship>> GetAllShip()
    {
        var allShips = _context.Ships.ToList();
        await _context.SaveChangesAsync();
        return allShips;
    }

    private (bool success, double?[] heights, double?[] widths, List<List<Ship>> shipsLevel, double? countWidths) BranchAndBound(
    List<Ship> ships,
    Pier pier,
    double?[] heights,
    double?[] widths,
    int levelNum,
    double? countWidths,
    List<List<Ship>> shipsLevel)
    {
        if (levelNum >= ships.Count)
        {
            return (countWidths <= pier.Width, heights, widths, shipsLevel, countWidths);
        }

        var ship = ships[levelNum];
        if (ship.Width > pier.Width || ship.Length > pier.Length)
        {
            return (false, heights, widths, shipsLevel, countWidths); // Не удовлетворяет размерам пирса
        }

        double? minResidualSpace = double.MaxValue;
        int bestLevel = -1;

        for (int i = 0; i < shipsLevel.Count; i++)
        {
            var residualSpaceLength = pier.Length - heights[i];
            if (ship.Length <= residualSpaceLength && ship.Width <= pier.Width)
            {
                if (residualSpaceLength < minResidualSpace)
                {
                    bestLevel = i;
                    minResidualSpace = residualSpaceLength;
                }
            }
        }

        if (bestLevel != -1)
        {
            shipsLevel[bestLevel].Add(ship);

            heights[bestLevel] += ship.Length;
            widths[bestLevel] = shipsLevel[bestLevel].Max(x => x.Width);
            countWidths = widths.Sum(w => w.GetValueOrDefault());

            var result = BranchAndBound(ships, pier, heights, widths, levelNum + 1, countWidths, shipsLevel);

            if (result.success)
            {
                return result;
            }

            shipsLevel[bestLevel].Remove(ship);
            heights[bestLevel] -= ship.Length;
            widths[bestLevel] = shipsLevel[bestLevel].Any() ? shipsLevel[bestLevel].Max(x => x.Width) : 0;
            countWidths = widths.Sum(w => w.GetValueOrDefault());
        }

        if (countWidths + ship.Width <= pier.Width && (heights[shipsLevel.Count] ?? 0) + ship.Length <= pier.Length)
        {
            if (shipsLevel.Count <= levelNum)
            {
                shipsLevel.Add(new List<Ship>());
            }

            shipsLevel[levelNum].Add(ship);
            heights[levelNum] += ship.Length;
            widths[levelNum] = ship.Width;
            countWidths = widths.Sum(w => w.GetValueOrDefault());

            var resultWithShip = BranchAndBound(ships, pier, heights, widths, levelNum + 1, countWidths, shipsLevel);
            if (resultWithShip.success)
            {
                return resultWithShip;
            }

            shipsLevel[levelNum].Remove(ship);
            heights[levelNum] -= ship.Length;
            widths[levelNum] = shipsLevel[levelNum].Any() ? shipsLevel[levelNum].Max(x => x.Width) : 0;
            countWidths = widths.Sum(w => w.GetValueOrDefault());
        }

        return BranchAndBound(ships, pier, heights, widths, levelNum + 1, countWidths, shipsLevel);
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

        if (newShip != null && !allShips.Any(x => x.Id == newShip.Id))
        {
            allShips.Add(newShip);
        }
        else
        {
            return "Выбранный корабль не найден или уже причалил к причалу.";
        }

        var sortedShips = allShips.OrderByDescending(ship => ship.Length)
                                  .ThenByDescending(ship => ship.Width)
                                  .ToList();

        double?[] heights = new double?[sortedShips.Count + 1];
        double?[] widths = new double?[sortedShips.Count + 1];

        for (int i = 0; i < heights.Length; i++)
        {
            heights[i] = 0;
            widths[i] = 0;
        }

        List<List<Ship>> shipsLevel = new List<List<Ship>>();
        for (int i = 0; i < sortedShips.Count + 1; i++)
        {
            shipsLevel.Add(new List<Ship>());
        }

        var result = BranchAndBound(sortedShips, pier, heights, widths, 0, 0, shipsLevel);

        if (result.success && result.shipsLevel.Sum(x => x.Count) == sortedShips.Count)
        {
            pier.Ships.Add(newShip);
            await _context.SaveChangesAsync();
            return "Корабль причалил.";
        }
        else
        {
            return "Корабль не может пришвартоваться из-за превышения размеров причала.";
        }
    }

    public async Task<List<List<Ship>>> CanDockShipsOptionalLocation(string pierId)
    {
        // Найти причал по id
        var pier = await _context.Piers.FirstOrDefaultAsync(x => x.Id == pierId);
        if (pier == null)
        {
            throw new ArgumentException("Такого пирса не существует.");
        }

        // Найти все корабли на данном причале
        var allShips = await _context.Ships.Where(x => x.Pier.Id == pierId).ToListAsync();

        // Сортировка кораблей по длине и ширине
        var sortedShips = allShips.OrderByDescending(ship => ship.Length)
                                  .ThenByDescending(ship => ship.Width)
                                  .ToList();

        // Инициализация массивов для высоты и ширины
        double?[] heights = new double?[sortedShips.Count + 1];
        double?[] widths = new double?[sortedShips.Count + 1];

        for (int i = 0; i < heights.Length; i++)
        {
            heights[i] = 0;
            widths[i] = 0;
        }

        // Инициализация списка уровней кораблей
        List<List<Ship>> shipsLevel = new List<List<Ship>>();
        for (int i = 0; i < sortedShips.Count + 1; i++)
        {
            shipsLevel.Add(new List<Ship>());
        }

        // Вызов метода ветвей и границ для оптимального размещения кораблей
        var result = BranchAndBound(sortedShips, pier, heights, widths, 0, 0, shipsLevel);

        // Проверка успешности размещения кораблей
        if (result.success && result.shipsLevel.Sum(x => x.Count) == sortedShips.Count)
        {
            return result.shipsLevel;
        }
        else
        {
            throw new InvalidOperationException("Не удалось распределить корабли.");
        }
    }
}
