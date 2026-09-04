using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Host.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var response = await _customerService.GetAllCustomerAsync();
            return View(response.Data);
        }

       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterCustomerRequest model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"{item.Key} : {error.ErrorMessage}");
                    }

                }
                return View(model);
            }

            var response = await _customerService.RegisterAsync(model);

            if (!response.Status)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _customerService
                .GetCustomerByUserIdAsync(userId);

            if (!response.Status || response.Data == null)
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Dashboard));
            }

            return View(response.Data);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            if (!response.Status)
                return NotFound();

            var model = new UpdateCustomerRequest
            {
                Id = response.Data!.Id,
                FirstName = response.Data.FirstName,
                LastName = response.Data.LastName,
                ExistingImageUrl = response.Data.ProfileImageUrl,
                Bio = response.Data.Bio
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UpdateCustomerRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _customerService.UpdateCustomerAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        public async Task<IActionResult> Dashboard()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var response = await _customerService.GetDashboardAsync(userId);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View();
            }

            return View(response.Data);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
