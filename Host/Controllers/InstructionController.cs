using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class InstructionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
