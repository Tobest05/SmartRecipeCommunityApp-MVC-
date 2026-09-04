using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{
    [Authorize]
    public class FavouriteRecipeController : Controller
    {
        private readonly IFavouriteService _favouriteService;
        private readonly ICustomerService _customerService;

        public FavouriteRecipeController(
            IFavouriteService favouriteService,
            ICustomerService customerService)
        {
            _favouriteService = favouriteService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerId = await GetLoggedInCustomerId();

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var response =
                await _favouriteService
                    .GetFavouriteRecipesByCustomerAsync(
                        customerId.Value);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;

                return View(
                    new List<FavouriteRecipeViewModel>());
            }

            return View(response.Data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            CreateFavouriteRecipeRequestModel model)
        {
            var customerId = await GetLoggedInCustomerId();

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var response =
                await _favouriteService
                    .AddFavouriteRecipeAsync(
                        model,
                        customerId.Value);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = response.Message;
            }

            return RedirectToAction(
                "Details",
                "Recipe",
                new { id = model.RecipeId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var customerId = await GetLoggedInCustomerId();

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _favouriteService.RemoveFavouriteRecipeAsync(id,customerId.Value);

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


        private async Task<Guid?> GetLoggedInCustomerId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return null;
            }

            if (!Guid.TryParse(userIdClaim.Value,out Guid userId))
            {
                return null;
            }

            var customerResponse = await _customerService.GetCustomerByUserIdAsync(userId);

            if (!customerResponse.Status || customerResponse.Data == null)
            {
                return null;
            }

            return customerResponse.Data.Id;
        }
    }
}
