using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
