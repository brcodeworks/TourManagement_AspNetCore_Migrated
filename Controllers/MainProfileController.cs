using Microsoft.AspNetCore.Mvc;
using Tour_Management.Models;

namespace Tour_Management.Controllers
{
    public class MainProfileController : Controller
    {
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var vm = new ProfileViewModel
            {
                Email = email
            };

            return View(vm);
        }
    }
}