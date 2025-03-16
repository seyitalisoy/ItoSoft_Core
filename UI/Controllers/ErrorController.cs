using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ShowNavbar = false;
            return View();
        }

        public IActionResult PageNotFound()
        {
            ViewBag.ShowNavbar = false;
            ViewBag.Is404Page = true;
            return View();
        }
    }

}
