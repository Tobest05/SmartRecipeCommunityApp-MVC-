using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
