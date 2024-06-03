using Microsoft.AspNetCore.Mvc;
using ShipDiplom.Controllers;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Entities;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UnitTests.ControllerTests;

public class PierControllerTests
{
    private readonly Mock<IPierService> _pierServiceMock;
    private readonly Mock<IShipService> _shipServiceMock;
    private readonly PierController _pierController;
    private readonly Mock<ITempDataDictionary> _tempDataMock;

    public PierControllerTests()
    {
        _pierServiceMock = new Mock<IPierService>();
        _shipServiceMock = new Mock<IShipService>();
        _tempDataMock = new Mock<ITempDataDictionary>();
        _pierController = new PierController(_pierServiceMock.Object, _shipServiceMock.Object)
        {
            TempData = _tempDataMock.Object
        };
    }

    [Fact]
    public void CreatePier_Get_ReturnsViewResult()
    {
        // Act
        var result = _pierController.CreatePier();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreatePier_Post_ValidModel_ReturnsViewResult()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.CreatePier(It.IsAny<Pier>()))
            .ReturnsAsync("Новый причал создан");

        // Act
        var result = await _pierController.CreatePier(pier);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        _tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "Новый причал создан");
    }

    [Fact]
    public async Task CreatePier_Post_InvalidModel_ReturnsViewResultWithError()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.CreatePier(It.IsAny<Pier>()))
            .ReturnsAsync("Ошибка создания причала");

        // Act
        var result = await _pierController.CreatePier(pier);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        _tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Ошибка создания причала");
    }

    [Fact]
    public async Task UpdatePier_Get_ReturnsNotFoundIfPierIsNull()
    {
        // Arrange
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync((Pier)null);

        // Act
        var result = await _pierController.UpdatePier("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdatePier_Get_ReturnsViewResultWithPier()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync(pier);

        // Act
        var result = await _pierController.UpdatePier("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(pier, viewResult.Model);
    }

    [Fact]
    public async Task UpdatePier_Post_ValidModel_ReturnsRedirectToAction()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.UpdatePier(It.IsAny<Pier>()))
            .ReturnsAsync("Данные причала обновлены");

        // Act
        var result = await _pierController.UpdatePier(pier);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("GetAllPiers", redirectToActionResult.ActionName);
        Assert.Equal("Pier", redirectToActionResult.ControllerName);
    }

    [Fact]
    public async Task UpdatePier_Post_InvalidModel_ReturnsViewResult()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierController.ModelState.AddModelError("Error", "Invalid model state");

        // Act
        var result = await _pierController.UpdatePier(pier);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(pier, viewResult.Model);
    }

    [Fact]
    public async Task DeletePier_Get_ReturnsNotFoundIfPierIsNull()
    {
        // Arrange
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync((Pier)null);

        // Act
        var result = await _pierController.DeletePier("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePier_Get_ReturnsViewResultWithPier()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync(pier);

        // Act
        var result = await _pierController.DeletePier("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(pier, viewResult.Model);
    }

    [Fact]
    public async Task DeletePierConfirmed_ReturnsOkResult()
    {
        // Arrange
        _pierServiceMock.Setup(service => service.DeletePier(It.IsAny<string>()))
            .ReturnsAsync("Причал удален");

        // Act
        var result = await _pierController.DeletePierConfirmed("1");

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeletePierConfirmed_ReturnsBadRequestOnException()
    {
        // Arrange
        _pierServiceMock.Setup(service => service.DeletePier(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Ошибка удаления"));

        // Act
        var result = await _pierController.DeletePierConfirmed("1");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Ошибка удаления", badRequestResult.Value);
    }

    [Fact]
    public async Task GetPier_ReturnsNotFoundIfPierIsNull()
    {
        // Arrange
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync((Pier)null);

        // Act
        var result = await _pierController.GetPier("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetPier_ReturnsViewResultWithPier()
    {
        // Arrange
        var pier = new Pier { Id = "1", Name = "Pier 1" };
        _pierServiceMock.Setup(service => service.GetPier(It.IsAny<string>()))
            .ReturnsAsync(pier);

        // Act
        var result = await _pierController.GetPier("1");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(pier, viewResult.Model);
    }

    [Fact]
    public async Task GetAllPiers_ReturnsViewResultWithPiers()
    {
        // Arrange
        var piers = new List<Pier>
        {
            new Pier { Id = "1", Name = "Pier 1" },
            new Pier { Id = "2", Name = "Pier 2" }
        };
        _pierServiceMock.Setup(service => service.GetAllPiers())
            .ReturnsAsync(piers);

        // Act
        var result = await _pierController.GetAllPiers();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(piers, viewResult.Model);
    }
}
