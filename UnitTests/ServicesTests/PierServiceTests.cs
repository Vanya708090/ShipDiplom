using Microsoft.EntityFrameworkCore;
using ShipDiplom.Database;
using ShipDiplom.Models.Entities;
using ShipDiplom.Services;

namespace UnitTests.ServicesTests;

public class PierServiceTests
{
    private readonly ShipService _shipService;
    private readonly PierService _pierService;
    private readonly AppDbContext _appDbContext;
    public PierServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options);

        _shipService = new ShipService(_appDbContext);
        _pierService = new PierService(_appDbContext);
    }

    [Fact]
    public async Task CreatePierTest()
    {
        var pier = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };

        var result = await _pierService.CreatePier(pier);

        Assert.NotNull(result);
        Assert.Equal("Причал добавлен", result);
    }

    [Fact]
    public async Task UpdatePierTest()
    {
        var pier = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };

        await _appDbContext.Piers.AddAsync(pier);
        await _appDbContext.SaveChangesAsync();

        var result = await _pierService.UpdatePier(pier);

        Assert.NotNull(result);
        Assert.Equal("Данные причала обновлены", result);
    }

    [Fact]
    public async Task GetPierTest()
    {
        var pier = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };

        await _appDbContext.Piers.AddAsync(pier);
        await _appDbContext.SaveChangesAsync();

        var result = await _pierService.GetPier(pier.Id);

        Assert.NotNull(result);
        Assert.True(result is Pier);
        Assert.True(pier.Length == result.Length);
        Assert.True(pier.Width == result.Width);
        Assert.True(pier.Location == result.Location);
        Assert.True(pier.Name == result.Name);
        Assert.True(pier.TrackingNumber == result.TrackingNumber);
    }

    [Fact]
    public async Task DeletePierTest()
    {
        var pier = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };

        await _appDbContext.Piers.AddAsync(pier);
        await _appDbContext.SaveChangesAsync();

        var result = await _pierService.DeletePier(pier.Id);

        Assert.NotNull(result);
        Assert.Equal("Причал удален", result);
    }

    [Fact]
    public async Task GetAllPiersTest()
    {
        var pier1 = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };
        var pier2 = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };

        await _appDbContext.Piers.AddAsync(pier1);
        await _appDbContext.Piers.AddAsync(pier2);
        await _appDbContext.SaveChangesAsync();

        var result = await _pierService.GetAllPiers();

        Assert.NotNull(result);
        Assert.True(result.Count == 2);
    }

    [Fact]
    public async Task AddPierShipsTest()
    {
        var pier = new Pier() { Id = Guid.NewGuid().ToString(), Length = 1, Width = 1, Location = "test", Name = "1", TrackingNumber = "1" };
        var ship = new Ship() { Name = "1", Id = "1", Length = 50, Width = 30, OwnerId = "1", SystemId = "1" };

        await _shipService.CreateShip(ship);
        await _appDbContext.Piers.AddAsync(pier);
        await _appDbContext.SaveChangesAsync();

        var result = await _pierService.AddPierShips(pier.Id, ship.Id);

        Assert.True(result);
    }
}
