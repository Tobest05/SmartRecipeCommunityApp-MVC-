using Application.Dto;
using Application.Interfaces.Services;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;

        public RecipeController( IRecipeService recipeService,ICategoryService categoryService,ICustomerService customerService)
        {
            _recipeService = recipeService;
            _categoryService = categoryService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _recipeService.GetAllRecipeAsync();

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(new List<CreateRecipeResponseModel>());
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Guid? userId = null;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            var response = await _recipeService.GetRecipeDetailsAsync(id, userId);

            if (!response.Status || response.Data == null)
            {
                TempData["Error"] = response.Message;
                return NotFound();
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var response = await _categoryService.GetAllCategoryAsync();

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(new CreateRecipeRequestModel());
            }

            ViewBag.Categories = response.Data;

            return View(new CreateRecipeRequestModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var response = await _recipeService
                .AddRecipeAsync(model, userId);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;

                await LoadCategories();

                return View(model);
            }

            TempData["Success"] = "Recipe created successfully!";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _recipeService.GetRecipeByIdAsync(id);

            if (!response.Status || response.Data == null)
                return NotFound();

            var recipe = response.Data;

            var model = new UpdateRecipeRequestModel
            {
                Id = recipe.Id,
                CategoryId = recipe.CategoryId,
                Name = recipe.Name,
                Description = recipe.Description,
                ExistingImageUrl = recipe.ImageUrl,
                PreparationTimeMinutes = recipe.PreparationTimeMinutes,
                CookingTimeMinutes = recipe.CookingTimeMinutes,
                Servings = recipe.Servings,
                Difficulty = recipe.Difficulty,
                Status = recipe.Status
            };

            await LoadCategories();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRecipeRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(model);
            }

            var response = await _recipeService.UpdateRecipeAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;

                await LoadCategories();

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
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
            var response = await _recipeService.DeleteRecipeAsync(id);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string name)
        {
            var response = await _recipeService.SearchRecipeAsync(name);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;

                return View("Index",new List<CreateRecipeResponseModel>());
            }

            return View("Index", response.Data);
        }

        public async Task<IActionResult> ByCategory(Guid categoryId)
        {
            var response =
                await _recipeService.GetRecipeByCategoryAsync(categoryId);

            return View("Index", response.Data);
        }
        [HttpGet]
        public async Task<IActionResult> MyRecipes()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim ==null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }
            var customerResponse = await _customerService.GetCustomerByUserIdAsync(userId);
            if(!customerResponse.Status || customerResponse.Data == null)
            {
                TempData["Error"] = "Customer not found";
                return RedirectToAction("Feed", "Home");
            }
            var customerId = customerResponse.Data.Id;
            var response  = await _recipeService.GetMyRecipesAsync(customerId);
            if(!response.Status)
            {
                TempData["Error"] = response.Message;
                return View(new List<MyRecipeResponseModel>());
            }
            return View(response.Data);
        }

        public async Task<IActionResult> ByDifficulty(
            Difficulty difficulty)
        {
            var response =
                await _recipeService.GetRecipeByDifficultyAsync(
                    difficulty);

            return View("Index", response.Data);
        }


        public async Task<IActionResult> Discover()
        {
            Guid? userId = null;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedUserId))
            {
                userId = parsedUserId;
            }
            var response = await _recipeService.GetPublishedRecipeAsync(userId);
            if(!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(new List<CreateRecipeResponseModel>());

            }

            return View(response.Data);
        }

        public async Task<IActionResult> ByCustomer(
            Guid customerId)
        {
            var response = await _recipeService.GetRecipeByCustomerAsync(customerId);

            return View("Index", response.Data);
        }


        private async Task LoadCategories()
        {
            var response = await _categoryService.GetAllCategoryAsync();

            ViewBag.Categories = response.Data;
        }
    }
}
