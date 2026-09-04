using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class InstructionController : Controller
    {
        private readonly IInstructionService _instructionService;

        public InstructionController(IInstructionService instructionService)
        {
            _instructionService = instructionService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _instructionService.GetAllInstructionAsync();

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(new List<CreateInstructionResponseModel>());
            }

            return View(response.Data);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _instructionService.GetInstructionByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        [HttpGet]
        public IActionResult Create(Guid recipeId)
        {
            if (recipeId == Guid.Empty)
                return BadRequest();

            var model = new CreateInstructionRequestModel
            {
                RecipeId = recipeId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateInstructionRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _instructionService.AddInstructionAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            
            return RedirectToAction("Details","Recipe",new { id = model.RecipeId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _instructionService.GetInstructionByIdAsync(id);

            if (!response.Status || response.Data == null)
                return NotFound();

            var instruction = response.Data;

            var model = new UpdateInstructionRequestModel
            {
                Id = instruction.Id,
                RecipeId = instruction.RecipeId,
                StepNumber = instruction.StepNumber,
                Description = instruction.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateInstructionRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _instructionService.UpdateInstructionAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction("Details","Recipe",new { id = model.RecipeId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response =
                await _instructionService.GetInstructionByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id,Guid recipeId)
        {
            await _instructionService.DeleteInstructionAsync(id);

            return RedirectToAction("Details","Recipe",new { id = recipeId });
        }
    }
}
