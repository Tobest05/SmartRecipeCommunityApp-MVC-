using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        
        public async Task<IActionResult> Index()
        {
            var response = await _ingredientService.GetAllIngredientAsync();
            return View(response.Data);
        }

        
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _ingredientService.GetIngredientByIdAsync(id);

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
        public async Task<IActionResult> Create(CreateIngredientRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _ingredientService.AddIngredientAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _ingredientService.GetIngredientByIdAsync(id);

            if (!response.Status)
                return NotFound();

            var ingredient = response.Data!;

            var model = new UpdateIngredientRequestModel
            {
                Id = id,
                RecipeId = ingredient.RecipeId,
                Name = ingredient.Name,
                Quantity = ingredient.Quantity,
                Unit = ingredient.Unit
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateIngredientRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _ingredientService.UpdateIngredientAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _ingredientService.GetIngredientByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _ingredientService.DeleteIngredientAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
