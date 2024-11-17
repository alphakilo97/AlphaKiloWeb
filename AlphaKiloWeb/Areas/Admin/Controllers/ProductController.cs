using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaKiloWeb.Areas.Admin.Controllers {

    [Area("Admin")]
    public class ProductController : Controller{
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u=> new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(products);
        }
        public IActionResult Create() {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            //ViewBag.CategoryList = CategoryList;
            ViewData["CategoryList"] = CategoryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product newProduct) {
            if (ModelState.IsValid) {
                _unitOfWork.Product.Add(newProduct);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Product '{newProduct.Title}' added successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Product id.");
            }
            Product? existingProduct = _unitOfWork.Product.Get(u => u.Id == id);
            if (existingProduct == null) {
                return NotFound($"Product with id: {id} doesn't exist.");
            }
            return View(existingProduct);
        }
        [HttpPost]
        public IActionResult Edit(Product newProduct) {
            if (ModelState.IsValid) {
                _unitOfWork.Product.Update(newProduct);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Product '{newProduct.Title}' edited successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Product id.");
            }
            Product? toDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (toDelete == null) {
                return NotFound($"Product with id: {id} doesn't exist.");
            }
            return View(toDelete);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) {
            Product? toDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (toDelete != null) {
                var title = toDelete.Title;
                _unitOfWork.Product.Remove(toDelete);
                _unitOfWork.SaveChanges();
                TempData["success"] = $"Product '{title}' removed successfully!";
                return RedirectToAction("Index");
            }
            return NotFound($"Product with id: {id} doesn't exist.");
        }
    }
}

