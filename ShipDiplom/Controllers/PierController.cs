using Microsoft.AspNetCore.Mvc;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Dto;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Controllers;

public class PierController : Controller
{
    private readonly IPierService _pierService;
    private readonly IShipService _shipService;

    public PierController(IPierService pierService, IShipService shipService)
    {
        _pierService = pierService;
        _shipService = shipService;
    }

    [HttpGet]
    public ActionResult CreatePier()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreatePier(Pier pier)
    {
        var message = await _pierService.CreatePier(pier);

        if (message.StartsWith("Новый причал создан"))
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return View(pier);
    }

    [HttpGet]
    public async Task<ActionResult> UpdatePier(string pierId)
    {
        var pier = await _pierService.GetPier(pierId);
        if (pier == null)
        {
            return NotFound();
        }

        return View(pier);
    }

    [HttpPost]
    public async Task<ActionResult> UpdatePier(Pier pier)
    {
        if (!ModelState.IsValid)
        {
            return View(pier);
        }

        var message = await _pierService.UpdatePier(pier);

        if (message.StartsWith("Данные причала обновлены"))
        {
            return RedirectToAction("GetAllPiers", "Pier");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<ActionResult> DeletePier(string pierId)
    {
        var pier = await _pierService.GetPier(pierId);
        if (pier == null)
        {
            return NotFound();
        }

        return View(pier);
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePierConfirmed(string pierId)
    {
        try
        {
            var message = await _pierService.DeletePier(pierId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetPier(string pierId)
    {
        var pier = await _pierService.GetPier(pierId);
        if (pier == null)
        {
            return NotFound();
        }

        return View(pier);
    }

    [HttpGet]
    public async Task<ActionResult> GetPierShips(string pierId)
    {
        var ships = await _pierService.GetPierShips(pierId);
        if (ships == null)
        {
            return NotFound();
        }

        return View(new LeaveShipViewModel { PierId = pierId, Ships = ships });
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPiers()
    {
        var piers = await _pierService.GetAllPiers();
        return View(piers);
    }
    [HttpGet]
    public async Task<IActionResult> GetShipLevels(string pierId)
    {
        try
        {
            var shipLevels = await _shipService.CanDockShipsOptionalLocation(pierId);
            return View(shipLevels);
        }
        catch (ArgumentException ex)
        {
            // Обработка ошибки, если причал не существует
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
        catch (InvalidOperationException ex)
        {
            // Обработка ошибки, если не удалось разместить корабли
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
    }
}