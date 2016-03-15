using FormsIntro.ViewModels;
using Microsoft.AspNet.Mvc;

namespace FormsIntro.Controllers
{
    // This controller is used only to demonstrate working with forms.
    public class DemoRegistrationController : Controller
    {

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult Index()
        {
              return View();
        }
    }
}
