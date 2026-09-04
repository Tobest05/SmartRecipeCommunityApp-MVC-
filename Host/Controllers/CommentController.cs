using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICustomerService _customerService;


        public CommentController(
            ICommentService commentService,
            ICustomerService customerService)
        {
            _commentService = commentService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerId = await GetLoggedInCustomerId();


            if (customerId == null)
            {
                return RedirectToAction("Login","Account");
            }


            var response = await _commentService.GetCommentsByCustomerAsync(customerId.Value);


            if (!response.Status)
            {
                TempData["Error"] = response.Message;

                return View(new List<MyCommentViewModel>());
            }


            return View(response.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeCommentRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid comment.";

                return RedirectToAction( "Details","Recipe",new { id = model.RecipeId });
            }


            var customerId = await GetLoggedInCustomerId();


            if (customerId == null)
            {
                return RedirectToAction( "Login","Account");
            }


            var response =await _commentService.AddCommentAsync(model,customerId.Value);


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
        public async Task<IActionResult> Edit(Guid id)
        {
            var customerId = await GetLoggedInCustomerId();


            if (customerId == null)
            {
                return RedirectToAction("Login","Account");
            }


            var response = await _commentService.GetCommentByIdAsync(id);


            if (!response.Status ||
                response.Data == null)
            {
                TempData["Error"] = response.Message;

                return RedirectToAction(nameof(Index));
            }


            var comment = response.Data;



            var currentComments = await _commentService.GetCommentsByCustomerAsync(customerId.Value);


            if (!currentComments.Status || currentComments.Data == null || !currentComments.Data.Any(x => x.Id == id))
            {
                TempData["Error"] = "You can only edit your own comment.";

                return RedirectToAction(nameof(Index));
            }


            var model =
                new UpdateRecipeCommentRequestModel
                {
                    RecipeId = comment.RecipeId,

                    Comment = comment.Comment
                };


            ViewBag.CommentId = id;

            ViewBag.RecipeName =
                comment.RecipeName;


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
      Guid id,
      UpdateRecipeCommentRequestModel model)
        {
            var customerId =
                await GetLoggedInCustomerId();

            if (customerId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            if (!ModelState.IsValid)
            {
                var existingComment =
                    await _commentService.GetCommentByIdAsync(id);

                ViewBag.CommentId = id;

                if (existingComment.Status &&
                    existingComment.Data != null)
                {
                    ViewBag.RecipeName =
                        existingComment.Data.RecipeName;
                }

                return View(model);
            }

            var response =
                await _commentService.UpdateCommentAsync(
                    id,
                    model,
                    customerId.Value);

            if (!response.Status)
            {
                TempData["Error"] = response.Message;

                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] =
                "Comment updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            Guid id,
            Guid recipeId)
        {
            var customerId =
                await GetLoggedInCustomerId();


            if (customerId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }


            var response =
                await _commentService
                    .DeleteCommentAsync(
                        id,
                        customerId.Value);


            if (!response.Status)
            {
                TempData["Error"] =
                    response.Message;
            }
            else
            {
                TempData["Success"] =
                    "Comment deleted successfully.";
            }


            return RedirectToAction(
                nameof(Index));
        }

        private async Task<Guid?>
            GetLoggedInCustomerId()
        {
            var userIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);


            if (userIdClaim == null)
            {
                return null;
            }


            if (!Guid.TryParse(
                    userIdClaim.Value,
                    out Guid userId))
            {
                return null;
            }


            var customerResponse =
                await _customerService
                    .GetCustomerByUserIdAsync(
                        userId);


            if (!customerResponse.Status ||
                customerResponse.Data == null)
            {
                return null;
            }


            return customerResponse.Data.Id;
        }
    }
}