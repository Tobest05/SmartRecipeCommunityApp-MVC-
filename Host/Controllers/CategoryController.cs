using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
