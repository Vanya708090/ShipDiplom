using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShipDiplom.Interfaces;
using ShipDiplom.Models;
using ShipDiplom.Models.Dto.Request;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Controllers;

public class ShipController : Controller
{
    private readonly IShipService _shipService;
    private readonly IPierService _pierService;
    private readonly IMapper _mapper;

    public ShipController(IShipService shipService, IPierService pierService, IMapper mapper)
    {
        _shipService = shipService;
        _pierService = pierService;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult CreateShip()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateShip(Ship ship)
    {
        var message = await _shipService.CreateShip(ship);

        if (message.StartsWith("Корабль добавлен"))
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return View();
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

    [HttpGet]
    public async Task<ActionResult> UpdateShip(string shipId)
    {
        var ship = await _shipService.GetShip(shipId);
        if (ship == null)
        {
            return NotFound();
        }

        var shipDto = _mapper.Map<ShipUpdateDto>(ship);

        return View(shipDto);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateShipConfirmed(ShipUpdateDto shipDto)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("UpdateShipConfirmed", "Ship");
        }

        var ship = _mapper.Map<Ship>(shipDto);

        var message = await _shipService.UpdateShip(ship);

        if (message.StartsWith("Данные корабля обновлены"))
        {
            return RedirectToAction("GetAllShips", "Ship");
        }
        else
        {
            return BadRequest(message);
        }
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

    [HttpDelete]
    public async Task<ActionResult> DeleteShipConfirmed(string shipId)
    {
        try
        {
            var message = await _shipService.DeleteShip(shipId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    public async Task<ActionResult> GetAllShips()
    {
        var ships = await _shipService.GetAllShip();
        return View(ships);
    }

    [HttpGet]
    public async Task<ActionResult> LeaveDockShips(string shipId, string pierId)
    {
        var message = await _shipService.LeaveDock(pierId, shipId);
        return RedirectToAction("GetAllPiers", "Pier");
    }

    [HttpGet]
    public async Task<ActionResult> CanDockShips(string shipId, string pierId)
    {
        var message = await _shipService.CanDockShips(shipId, pierId);
        return Json(new { message });
    }

    [HttpGet]
    public async Task<ActionResult> CanDockShipsView(string shipId)
    {
        var model = new DockShipViewModel
        {
            ShipId = shipId,
            Piers = await _pierService.GetAllPiers()
        };

        return View("SelectDockForShip", model);
    }

}