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
                TempData["success"] = $"Category '{newCategory.Name}' created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id) {
            if(id == null || id == 0) {
                return NotFound($"{id} is not a valid Category id.");
            }
            Category? existingCategory = _db.Categories.Find(id);
            //Category? existingCategory1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? existingCategory2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (existingCategory == null) {
                return NotFound($"Category with id: {id} doesn't exist.");
            }
            return View(existingCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category newCategory) {
            if (double.TryParse(newCategory.Name, out double i)) {
                ModelState.AddModelError("name", "Category Name cannot be numeric");
            }
            if (ModelState.IsValid) {
                _db.Categories.Update(newCategory);
                _db.SaveChanges();
                TempData["success"] = $"Category '{newCategory.Name}' edited successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Category id.");
            }
            Category? toDelete = _db.Categories.Find(id);
            if (toDelete == null) {
                return NotFound($"Category with id: {id} doesn't exist.");
            }
            return View(toDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Category? toDelete = _db.Categories.Find(id);
            if (toDelete != null) {
                var name = toDelete.Name;
                _db.Categories.Remove(toDelete);
                _db.SaveChanges();
                TempData["success"] = $"Category '{name}' removed successfully!";
                return RedirectToAction("Index");
            }
            return NotFound($"Category with id: {id} doesn't exist.");
        }
    }
}
