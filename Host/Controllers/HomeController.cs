using Application.Dto;
using Application.Interfaces.Services;
using Domain.Entities;
using Host.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

public class HomeController : Controller
{
    private readonly IRecipeService _recipeService;

    public HomeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Feed()
    {
        Guid? userId = null;
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
        {
            userId = parsedUserId;
        }
        var response = await _recipeService.GetPublishedRecipeAsync(userId);

        if (!response.Status)
        {
            ViewBag.Message = response.Message;

            return View(new List<CreateRecipeResponseModel>());
        }

        return View(response.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}