using Microsoft.AspNetCore.Mvc;

namespace AlphaKiloWeb.Controllers {
    public class CategoryController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
