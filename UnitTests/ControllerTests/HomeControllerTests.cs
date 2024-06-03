using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ShipDiplom.Controllers;
using ShipDiplom.Database;

namespace UnitTests.ControllerTests;

public class HomeControllerTests
{
    private readonly Mock<ILogger<HomeController>> _loggerMock;
    private readonly AppDbContext _context;

    public HomeControllerTests()
    {
        _loggerMock = new Mock<ILogger<HomeController>>();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new AppDbContext(options);
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController(_loggerMock.Object, _context);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);  // проверяет, что представление по умолчанию
    }

    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        // Arrange
        var controller = new HomeController(_loggerMock.Object, _context);

        // Act
        var result = controller.Privacy();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.ViewName);  // проверяет, что представление по умолчанию
    }
}