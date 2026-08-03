using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class FavouriteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
