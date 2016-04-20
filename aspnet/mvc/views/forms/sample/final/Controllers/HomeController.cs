using FormsTagHelper.ViewModels;
using Microsoft.AspNet.Mvc;

namespace FormsTagHelper.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new CountryViewModel();
            model.Country = "CA";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = model.Country +  " selected";
                return RedirectToAction("IndexSuccess", new {id=message});
            }

            // If we got this far, something failed; redisplay form.
            return View(model);
        }

        public IActionResult IndexMultiSelect()
        {
            var model = new CountryViewModelIEnumerable();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexMultiSelect(CountryViewModelIEnumerable model)
        {
            if (ModelState.IsValid)
            {
                string strCountriesSelected="";
                foreach (string s in model.CountryCodes)
                {
                    strCountriesSelected = strCountriesSelected + " " + s;
                }
                return RedirectToAction("IndexSuccess", new { id = strCountriesSelected });
            }

            return View(model);
        }

        public IActionResult IndexGroup()
        {
            var model = new CountryViewModelGroup();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexGroup(CountryViewModelGroup model)
        {
            if (ModelState.IsValid)
            {
                var message = model.Country + " selected";
                return RedirectToAction("IndexSuccess", new {id=message});
            }

            return View(model);
        }

        public IActionResult IndexEnum()
        {
            var model = new CountryEnumViewModel();
            model.EnumCountry = CountryEnum.Spain;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexEnum(CountryEnumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = model.EnumCountry + " selected";
                return RedirectToAction("IndexSuccess", new {id=message});
            }

            return View(model);
        }

        public IActionResult IndexEmpty(int id)
        {
            var ViewPage = (id != 0) ? "IndexEmptyTemplate" : "IndexEmpty";

            return View(ViewPage, new CountryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IndexEmpty(CountryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = !System.String.IsNullOrEmpty(model.Country) ? model.Country
                    : "No slection";
                message += " Selected";
                return RedirectToAction("IndexSuccess", new { id = message });
            }

            return View(model);
        }

        public IActionResult IndexSuccess(string id)
        {
            ViewData["Message"] = id;
            return View();
        }
    }
}
