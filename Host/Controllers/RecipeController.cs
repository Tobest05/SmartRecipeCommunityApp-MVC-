using Application.Dto;
using Application.Interfaces.Services;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

       
        public async Task<IActionResult> Index()
        {
            var response = await _recipeService.GetAllRecipeAsync();
            return View(response.Data);
        }

        
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _recipeService.GetRecipeByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _recipeService.AddRecipeAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _recipeService.GetRecipeByIdAsync(id);

            if (!response.Status)
                return NotFound();

            var recipe = response.Data!;

            var model = new UpdateRecipeRequestModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ImageUrl = recipe.ImageUrl,
                PreparationTimeMinutes = recipe.PreparationTimeMinutes,
                CookingTimeMinutes = recipe.CookingTimeMinutes,
                Servings = recipe.Servings,
                Difficulty = recipe.Difficulty,
                Status = recipe.Status
            };

            return View(model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRecipeRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _recipeService.UpdateRecipeAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _recipeService.GetRecipeByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _recipeService.DeleteRecipeAsync(id);

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Search(string name)
        {
            var response = await _recipeService.SearchRecipeAsync(name);
            return View("Index", response.Data);
        }

        
        public async Task<IActionResult> ByCategory(Guid categoryId)
        {
            var response = await _recipeService.GetRecipeByCategoryAsync(categoryId);
            return View("Index", response.Data);
        }

       
        public async Task<IActionResult> ByDifficulty(Difficulty difficulty)
        {
            var response = await _recipeService.GetRecipeByDifficultyAsync(difficulty);
            return View("Index", response.Data);
        }

        
        public async Task<IActionResult> Published()
        {
            var response = await _recipeService.GetPublishedRecipeAsync();
            return View("Index", response.Data);
        }

        
        public async Task<IActionResult> ByCustomer(Guid customerId)
        {
            var response = await _recipeService.GetRecipeByCustomerAsync(customerId);
            return View("Index", response.Data);
        }
    }
}
