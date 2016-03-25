using FormsTagHelper.ViewModels;
using Microsoft.AspNet.Mvc;

namespace FormsTagHelper.Controllers
{
    // This controller is used only to demonstrate working with forms.
    public class DemoController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string success = "HttpPost Register success";
                return RedirectToAction("Index", new { id = success });
            }

            // If we got this far, something failed, redisplay form.
            return View(model);
        }

        public IActionResult Register2()
        {
            return View();
        }

        public IActionResult Index(string ID)
        {
            ViewData["Message"] = ID;
            return View();
        }



        // rTODO Used - remove unused above

        public IActionResult RegisterTextArea()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterTextArea(DescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string success = "HttpPost Register success " + model.Description;
                return RedirectToAction("Index", new { id = success });
            }

            return View(model);
        }
        public IActionResult RegisterFormOnly()
        {
            return View();
        }

        public IActionResult RegisterInput()
        {
            return View();
        }

        public IActionResult RegisterAddress()
        {
            return View();
        }

        public IActionResult RegisterValidation()
        {
            return View();
        }

        public IActionResult RegisterLabel()
        {
            return View();
        }
        public IActionResult z()
        {
            return View();
        }

    }
}
