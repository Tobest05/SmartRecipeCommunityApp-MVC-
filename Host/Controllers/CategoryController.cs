using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.GetAllCategoryAsync();
            return View(response.Data);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

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
        public async Task<IActionResult> Create(CreateCategoryRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _categoryService.AddCategoryAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

            if (!response.Status)
                return NotFound();

            var model = new UpdateCategoryRequestModel
            {
                Id = response.Data!.Id,
                Name = response.Data.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _categoryService.UpdateCategoryAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
