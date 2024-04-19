using Microsoft.AspNetCore.Mvc;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Controllers;

public class ShipController : Controller
{
    private readonly IShipService _shipService;

    public ShipController(IShipService shipService)
    {
        _shipService = shipService;
    }

    [HttpGet]
    public ActionResult CreateShip()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateShip(Ship ship)
    {
        if (!ModelState.IsValid)
        {
            return View(ship);
        }

        var message = await _shipService.CreateShip(ship);

        if (message.StartsWith("Корабль добавлен"))
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
        return View(ship);
    }

    [HttpGet]
    public async Task<ActionResult> GetShip(string shipId)
    {
        var ship = await _shipService.GetShip(shipId);
        if (ship == null)
        {
            return NotFound();
        }

        return View(ship);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateShip(Ship ship)
    {
        if (!ModelState.IsValid)
        {
            return View(ship);
        }

        var message = await _shipService.UpdateShip(ship);

        if (message.StartsWith("Данные корабля обновлены"))
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
        return View(ship);
    }

    [HttpGet]
    public async Task<ActionResult> DeleteShip(string shipId)
    {
        var ship = await _shipService.GetShip(shipId);
        if (ship == null)
        {
            return NotFound();
        }

        return View(ship);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteShipConfirmed(string shipId)
    {
        var message = await _shipService.DeleteShip(shipId);

        if (message.StartsWith("Корабль удален"))
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> GetAllShips()
    {
        var ships = await _shipService.GetAllShip();
        return View(ships);
    }

    [HttpGet]
    public async Task<ActionResult> CanDockShips(string shipId, string pierId)
    {
        var message = await _shipService.CanDockShips(shipId, pierId);
        return View(new { Message = message });
    }
}