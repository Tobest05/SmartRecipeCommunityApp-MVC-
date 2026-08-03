using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeRatingRequestModel model, Guid customerId)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _ratingService.AddRatingAsync(model, customerId);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction("Details", "Recipe", new { id = model.RecipeId });
        }

        
        public IActionResult Edit(Guid id)
        {
            ViewBag.RatingId = id;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRecipeRatingRequestModel model, Guid customerId)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _ratingService.UpdateRatingAsync(customerId, model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction("Details", "Recipe", new { id = model.RecipeId });
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid recipeId, Guid customerId)
        {
            var response = await _ratingService.DeleteRatingAsync(recipeId, customerId);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
            }

            return RedirectToAction("Details", "Recipe", new { id = recipeId });
        }

      
        public async Task<IActionResult> AverageRating(Guid recipeId)
        {
            var response = await _ratingService.GetAverageRatingAsync(recipeId);

            return View(response.Data);
        }
    }
}
