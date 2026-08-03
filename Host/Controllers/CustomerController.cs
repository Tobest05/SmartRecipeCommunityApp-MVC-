
using Application.Dto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        
        public async Task<IActionResult> Index()
        {
            var response = await _customerService.GetAllCustomerAsync();
            return View(response.Data);
        }

       
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterCustomerRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _customerService.RegisterAsync(model);

            if (!response.Status)
            {
                ViewBag.Message = response.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
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
                ProfileImageUrl = response.Data.ProfileImageUrl,
                Bio = response.Data.Bio
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            if (!response.Status)
                return NotFound();

            return View(response.Data);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _customerService.DeleteCustomerAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
