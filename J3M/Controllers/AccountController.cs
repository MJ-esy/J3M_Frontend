using Microsoft.AspNetCore.Mvc;

namespace J3M.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
  