using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class FavouriteRecipeController : Controller
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteRecipeController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        
        public async Task<IActionResult> Index()
        {
            var response = await _favouriteService.GetAllFavouriteRecipeAsync();
            return View(response.Data);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateFavouriteRecipeRequestModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Recipe");

            var response = await _favouriteService.AddFavouriteRecipeAsync(model, model.CustomerId);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = response.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _favouriteService.RemoveFavouriteRecipeAsync(id);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = "Favourite recipe removed successfully.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
