using Microsoft.EntityFrameworkCore;
using ShipDiplom.Database;
using ShipDiplom.Models.Entities;
using ShipDiplom.Services;

namespace UnitTests.ServicesTests;

public class ShipServiceTests
{
    private readonly ShipService _shipService;
    private readonly PierService _pierService;
    private readonly AppDbContext _appDbContext;

    public ShipServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options);

        _shipService = new ShipService(_appDbContext);
        _pierService = new PierService(_appDbContext);
    }

    [Fact]
    public async Task CanDockShipsTestNormalParams()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship = new Ship() { Name = "1", Id = "1", Length = 100, Width = 90, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship);

        var result = await _shipService.CanDockShips(ship.Id, pier.Id);

        Assert.True(result.Equals("Корабль причалил."));

    }

    [Fact]
    public async Task CanDockShipsTestNotNormalWidth()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship = new Ship() { Name = "1", Id = "1", Length = 100, Width = 91, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship);

        var result = await _shipService.CanDockShips(ship.Id, pier.Id);

        Assert.False(result.Equals("Корабль причалил."));

    }

    [Fact]
    public async Task CanDockShipsTestNotNormalLength()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship = new Ship() { Name = "1", Id = "1", Length = 101, Width = 90, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship);

        var result = await _shipService.CanDockShips(ship.Id, pier.Id);

        Assert.False(result.Equals("Корабль причалил."));

    }

    [Fact]
    public async Task CanDockShipsTestNoSpaceWithManyShips()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship1);

        var ship2 = new Ship() { Name = "2", Id = "2", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship2);

        var ship3 = new Ship() { Name = "3", Id = "3", Length = 60, Width = 40, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship3);

        var ship4 = new Ship() { Name = "4", Id = "4", Length = 40, Width = 50, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship4);

        var ship5 = new Ship() { Name = "5", Id = "5", Length = 60, Width = 50, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship5);

        pier.Ships.AddRange(new List<Ship>() { ship1, ship2, ship3, ship4 });

        _appDbContext.SaveChanges();

        var result = await _shipService.CanDockShips(ship5.Id, pier.Id);

        Assert.False(result.Equals("Корабль причалил."));

    }

    [Fact]
    public async Task CanDockShipsTestNotEnouhSpaceWithManyShips()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship1);

        var ship2 = new Ship() { Name = "2", Id = "2", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship2);

        var ship3 = new Ship() { Name = "3", Id = "3", Length = 60, Width = 40, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship3);

        var ship4 = new Ship() { Name = "4", Id = "4", Length = 70, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship4);

        pier.Ships.AddRange(new List<Ship>() { ship1, ship2, ship3 });

        _appDbContext.SaveChanges();

        var result = await _shipService.CanDockShips(ship4.Id, pier.Id);

        Assert.False(result.Equals("Корабль причалил."));

    }

    [Fact]
    public async Task CanDockShipsTestEnouhSpaceWithManyShips()
    {
        //act
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship1);

        var ship2 = new Ship() { Name = "2", Id = "2", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship2);

        var ship3 = new Ship() { Name = "3", Id = "3", Length = 60, Width = 40, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship3);

        var ship4 = new Ship() { Name = "4", Id = "4", Length = 40, Width = 30, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship4);

        pier.Ships.AddRange(new List<Ship>() { ship1, ship2, ship3 });

        _appDbContext.SaveChanges();

        var result = await _shipService.CanDockShips(ship4.Id, pier.Id);

        Assert.True(result.Equals("Корабль причалил."));

    }
    [Fact]
    public async Task CreateShipTest()
    {
        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };

        await _shipService.CreateShip(ship1);

        var result = await _shipService.GetShip(ship1.Id);

        Assert.True(ship1.Name == result.Name);
        Assert.True(ship1.Width == result.Width);
        Assert.True(ship1.Length == result.Length);
        Assert.True(ship1.OwnerId == result.OwnerId);
        Assert.True(ship1.SystemId == result.SystemId);
    }

    [Fact]
    public async Task DeleteShipTest()
    {
        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };

        await _shipService.CreateShip(ship1);

        var result = await _shipService.DeleteShip(ship1.Id);

        Assert.Equal("Корабль удален", result);
    }

    public async Task UpdateShipTest()
    {
        string id = "1";
        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = id };

        ship1.SystemId = "2";

        await _shipService.CreateShip(ship1);

        var result = await _shipService.UpdateShip(ship1);

        var updatedShip = await _shipService.GetShip(ship1.Id);

        Assert.Equal("Данные корабля обновлены", result);
        Assert.True(id == updatedShip.Id);
    }


    [Fact]
    public async Task GetShipTest()
    {
        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };

        await _shipService.CreateShip(ship1);

        var result = await _shipService.GetShip(ship1.Id);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllShipsTest()
    {
        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };
        var ship2 = new Ship() { Name = "1", Id = "2", Length = 50, Width = 30, OwnerId = "1", SystemId = "2" };

        await _shipService.CreateShip(ship1);
        await _shipService.CreateShip(ship2);

        var result = await _shipService.GetAllShip();

        Assert.NotNull(result);
        Assert.True(result.Count == 2);
    }

    [Fact]
    public async Task LeaveDockShipTest()
    {
        var pier = new Pier() { Name = "1", Id = "1", Length = 100, Width = 90, Location = "1", TrackingNumber = "1" };
        await _pierService.CreatePier(pier);

        var ship1 = new Ship() { Name = "1", Id = "1", Length = 50, Width = 90, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship1);

        var ship2 = new Ship() { Name = "1", Id = "2", Length = 50, Width = 90, OwnerId = "1", SystemId = "1" };
        await _shipService.CreateShip(ship2);

        pier.Ships.Add(ship1);
        pier.Ships.Add(ship2);

        await _appDbContext.SaveChangesAsync();

        var result = await _shipService.LeaveDock(pier.Id, ship2.Id);

        Assert.NotNull(result);
        Assert.Equal(result, $"Корабль {ship2.Name} отчалил от причала.");
    }
}
