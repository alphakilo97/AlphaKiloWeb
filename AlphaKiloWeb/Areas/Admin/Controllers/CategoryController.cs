using AlphaKilo.DataAccess.Data;
using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using AlphaKilo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaKiloWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CategoryController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index() {
            List<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category newCategory) {
            if (double.TryParse(newCategory.Name, out _)) {
                ModelState.AddModelError("name", "Category Name cannot be numeric");
            }
            if (ModelState.IsValid) {
                _unitOfWork.Category.Add(newCategory);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Category '{newCategory.Name}' created successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Category id.");
            }
            //Category? existingCategory = _db.Categories.Find(id);
            Category? existingCategory = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? existingCategory2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();
            if (existingCategory == null) {
                return NotFound($"Category with id: {id} doesn't exist.");
            }
            return View(existingCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category newCategory) {
            if (double.TryParse(newCategory.Name, out _)) {
                ModelState.AddModelError("name", "Category Name cannot be numeric");
            }
            if (ModelState.IsValid) {
                _unitOfWork.Category.Update(newCategory);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Category '{newCategory.Name}' edited successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Category id.");
            }
            //Category? toDelete = _db.Categories.Find(id);
            Category? toDelete = _unitOfWork.Category.Get(u => u.Id == id);
            if (toDelete == null) {
                return NotFound($"Category with id: {id} doesn't exist.");
            }
            return View(toDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Category? toDelete = _unitOfWork.Category.Get(u => u.Id == id);
            if (toDelete != null) {
                var name = toDelete.Name;
                _unitOfWork.Category.Remove(toDelete);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Category '{name}' removed successfully!";
                return RedirectToAction("Index");
            }
            return NotFound($"Category with id: {id} doesn't exist.");
        }
    }
}
