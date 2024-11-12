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
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category newCategory) {
            if(double.TryParse(newCategory.Name, out double i)){
                ModelState.AddModelError("name", "Category Name cannot be numeric");
            }
            if(ModelState.IsValid){
                _db.Categories.Add(newCategory);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
