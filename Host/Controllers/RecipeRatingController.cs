using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{
    [Authorize]
    public class RecipeRatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly ICustomerRepository _customerRepository;

        public RecipeRatingController(
            IRatingService ratingService,
            ICustomerRepository customerRepository)
        {
            _ratingService = ratingService;
            _customerRepository = customerRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CreateRecipeRatingRequestModel model)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return Unauthorized();
            }

            var response = await _ratingService.AddRatingAsync(model,customer.Id);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = response.Message;
            }

            return RedirectToAction("Details","Recipe",new { id = model.RecipeId });
        }

        [HttpGet]
        public async Task<IActionResult> MyRatings()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var customer = await _customerRepository.GetByUserIdAsync(userId);

            if (customer == null)
            {
                return Unauthorized();
            }

            var response = await _ratingService.GetMyRatingsAsync(customer.Id);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
                return View(new List<MyRatingResponseModel>());
            }

            return View(response.Data);
        }
    }
}
