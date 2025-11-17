using Microsoft.AspNetCore.Mvc;

namespace J3M.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
