using Microsoft.AspNetCore.Mvc;
using ShipDiplom.Interfaces;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Controllers;

public class PierController : Controller
{
    private readonly IPierService _pierService;

    public PierController(IPierService pierService)
    {
        _pierService = pierService;
    }

    [HttpGet]
    public ActionResult CreatePier()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreatePier(Pier pier)
    {
        if (!ModelState.IsValid)
        {
            return View(pier);
        }

        var message = await _pierService.CreatePier(pier);

        if (message.StartsWith("Новый причал создан"))
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
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
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
        return View(pier);
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

    [HttpPost]
    public async Task<ActionResult> DeletePierConfirmed(string pierId)
    {
        var message = await _pierService.DeletePier(pierId);

        if (message.StartsWith("Причал удален"))
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = message;
        return View();
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
    public async Task<ActionResult> GetAllPiers()
    {
        var piers = await _pierService.GetAllPiers();
        return View(piers);
    }
}

