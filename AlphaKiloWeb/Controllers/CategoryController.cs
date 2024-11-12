using AlphaKiloWeb.Data;
using AlphaKiloWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaKiloWeb.Controllers {
    public class CategoryController : Controller {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            List<Category> categoryList = _db.Categories.ToList();
            return View(categoryList);
        }
    }
}
