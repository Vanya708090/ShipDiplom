using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShipDiplom.Controllers;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Dto.Request;
using ShipDiplom.Models.Entities;
using ShipDiplom.Models;

namespace UnitTests.ControllerTests;

public class ShipControllerTests
{
    private readonly Mock<IShipService> _shipServiceMock;
    private readonly Mock<IPierService> _pierServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ShipController _shipController;
    private readonly Mock<ITempDataDictionary> _tempDataMock;

    public ShipControllerTests()
    {
        _shipServiceMock = new Mock<IShipService>();
        _pierServiceMock = new Mock<IPierService>();
        _mapperMock = new Mock<IMapper>();
        _tempDataMock = new Mock<ITempDataDictionary>();

        _shipController = new ShipController(_shipServiceMock.Object, _pierServiceMock.Object, _mapperMock.Object)
        {
            TempData = _tempDataMock.Object
        };
    }

    [Fact]
    public void CreateShip_Get_ReturnsViewResult()
    {
        // Act
        var result = _shipController.CreateShip();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateShip_Post_ValidModel_ReturnsViewResult()
    {
        // Arrange
        var ship = new Ship { Id = "1", Name = "Ship 1" };
        _shipServiceMock.Setup(service => service.CreateShip(It.IsAny<Ship>()))
            .ReturnsAsync("Корабль добавлен");

        // Act
        var result = await _shipController.CreateShip(ship);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        _tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "Корабль добавлен");
    }

    [Fact]
    public async Task CreateShip_Post_InvalidModel_ReturnsViewResultWithError()
    {
        // Arrange
        var ship = new Ship { Id = "1", Name = "Ship 1" };
        _shipServiceMock.Setup(service => service.CreateShip(It.IsAny<Ship>()))
            .ReturnsAsync("Ошибка добавления корабля");

        // Act
        var result = await _shipController.CreateShip(ship);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        _tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Ошибка добавления корабля");
    }

    [Fact]
    public async Task GetShip_ReturnsNotFoundIfShipIsNull()
    {
        // Arrange
        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync((Ship)null);

        // Act
        var result = await _shipController.GetShip("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetShip_ReturnsViewResultWithShip()
    {
        // Arrange
        var ship = new Ship { Id = "1", Name = "Ship 1" };
        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync(ship);

        // Act
        var result = await _shipController.GetShip("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(ship, viewResult.Model);
    }

    [Fact]
    public async Task UpdateShip_Get_ReturnsNotFoundIfShipIsNull()
    {
        // Arrange
        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync((Ship)null);

        // Act
        var result = await _shipController.UpdateShip("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateShip_Get_ReturnsViewResultWithShipDto()
    {
        // Arrange
        var ship = new Ship { Id = "1", Name = "Ship 1" };
        var shipDto = new ShipUpdateDto { Id = "1", Name = "Ship 1 Updated" };

        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync(ship);
        _mapperMock.Setup(mapper => mapper.Map<ShipUpdateDto>(ship))
            .Returns(shipDto);

        // Act
        var result = await _shipController.UpdateShip("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(shipDto, viewResult.Model);
    }

    [Fact]
    public async Task UpdateShipConfirmed_Post_ValidModel_ReturnsRedirectToAction()
    {
        // Arrange
        var shipDto = new ShipUpdateDto { Id = "1", Name = "Ship 1 Updated" };
        var ship = new Ship { Id = "1", Name = "Ship 1 Updated" };

        _mapperMock.Setup(mapper => mapper.Map<Ship>(shipDto))
            .Returns(ship);
        _shipServiceMock.Setup(service => service.UpdateShip(ship))
            .ReturnsAsync("Данные корабля обновлены");

        // Act
        var result = await _shipController.UpdateShipConfirmed(shipDto);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("GetAllShips", redirectToActionResult.ActionName);
        Assert.Equal("Ship", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task UpdateShipConfirmed_Post_InvalidModel_ReturnsRedirectToAction()
    {
        // Arrange
        var shipDto = new ShipUpdateDto { Id = "1", Name = "Ship 1 Updated" };
        _shipController.ModelState.AddModelError("Error", "Invalid model state");

        // Act
        var result = await _shipController.UpdateShipConfirmed(shipDto);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("UpdateShipConfirmed", redirectToActionResult.ActionName);
        Assert.Equal("Ship", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task DeleteShip_Get_ReturnsNotFoundIfShipIsNull()
    {
        // Arrange
        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync((Ship)null);

        // Act
        var result = await _shipController.DeleteShip("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteShip_Get_ReturnsViewResultWithShip()
    {
        // Arrange
        var ship = new Ship { Id = "1", Name = "Ship 1" };
        _shipServiceMock.Setup(service => service.GetShip(It.IsAny<string>()))
            .ReturnsAsync(ship);

        // Act
        var result = await _shipController.DeleteShip("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(ship, viewResult.Model);
    }

    [Fact]
    public async Task DeleteShipConfirmed_ReturnsOkResult()
    {
        // Arrange
        _shipServiceMock.Setup(service => service.DeleteShip(It.IsAny<string>()))
            .ReturnsAsync("Корабль удален");

        // Act
        var result = await _shipController.DeleteShipConfirmed("1");

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteShipConfirmed_ReturnsBadRequestOnException()
    {
        // Arrange
        _shipServiceMock.Setup(service => service.DeleteShip(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Ошибка удаления"));

        // Act
        var result = await _shipController.DeleteShipConfirmed("1");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Ошибка удаления", badRequestResult.Value);
    }

    [Fact]
    public async Task GetAllShips_ReturnsViewResultWithShips()
    {
        // Arrange
        var ships = new List<Ship>
            {
                new Ship { Id = "1", Name = "Ship 1" },
                new Ship { Id = "2", Name = "Ship 2" }
            };
        _shipServiceMock.Setup(service => service.GetAllShip())
            .ReturnsAsync(ships);

        // Act
        var result = await _shipController.GetAllShips();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(ships, viewResult.Model);
    }

    [Fact]
    public async Task LeaveDockShips_RedirectsToGetAllPiers()
    {
        // Arrange
        var shipId = "1";
        var pierId = "1";
        _shipServiceMock.Setup(service => service.LeaveDock(pierId, shipId))
            .ReturnsAsync("Корабль покинул причал");

        // Act
        var result = await _shipController.LeaveDockShips(shipId, pierId);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("GetAllPiers", redirectToActionResult.ActionName);
        Assert.Equal("Pier", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task CanDockShips_ReturnsJsonResult()
    {
        // Arrange
        var shipId = "1";
        var pierId = "1";
        var message = "Корабль может пришвартоваться";
        _shipServiceMock.Setup(service => service.CanDockShips(shipId, pierId))
            .ReturnsAsync(message);

        // Act
        var result = await _shipController.CanDockShips(shipId, pierId);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var actualMessage = jsonResult.Value.GetType().GetProperty("message").GetValue(jsonResult.Value, null);
        Assert.Equal(message, actualMessage);
    }

    [Fact]
    public async Task CanDockShipsView_ReturnsViewResultWithDockShipViewModel()
    {
        // Arrange
        var shipId = "1";
        var piers = new List<Pier>
            {
                new Pier { Id = "1", Name = "Pier 1" },
                new Pier { Id = "2", Name = "Pier 2" }
            };
        _pierServiceMock.Setup(service => service.GetAllPiers())
            .ReturnsAsync(piers);

        // Act
        var result = await _shipController.CanDockShipsView(shipId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<DockShipViewModel>(viewResult.Model);
        Assert.Equal(shipId, model.ShipId);
        Assert.Equal(piers, model.Piers);
    }
}
