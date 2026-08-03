using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

       
        public async Task<IActionResult> Index()
        {
            var response = await _commentService.GetAllCommentAsync();
            return View(response.Data);
        }

       
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _commentService.GetCommentByIdAsync(id);

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
        public async Task<IActionResult> Create(CreateRecipeCommentRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _commentService.AddCommentAsync(model, model.CustomerId);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _commentService.GetCommentByIdAsync(id);

            if (!response.Status)
                return NotFound();

            var comment = response.Data!;

            var model = new UpdateRecipeCommentRequestModel
            {
                Id = comment.Id,
                Comment = comment.Comment
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateRecipeCommentRequestModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _commentService.UpdateCommentAsync(model.Id, model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _commentService.GetCommentByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _commentService.DeleteCommentAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
