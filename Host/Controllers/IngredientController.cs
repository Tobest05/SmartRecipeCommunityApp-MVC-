using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class IngredientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
