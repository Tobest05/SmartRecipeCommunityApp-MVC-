using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
