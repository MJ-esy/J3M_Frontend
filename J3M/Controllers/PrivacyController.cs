using Microsoft.AspNetCore.Mvc;

namespace J3M.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EnglishPrivacy()
            {
                return PartialView("_EnglishPrivacy");
            }

            public IActionResult SwedishPrivacy()
            {
                return PartialView("_SwedishPrivacy");
            }
        

    }
}
