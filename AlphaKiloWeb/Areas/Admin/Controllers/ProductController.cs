using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using AlphaKilo.Models.ViewModels;
using AlphaKilo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaKiloWeb.Areas.Admin.Controllers {

    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller{
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public IActionResult Index() {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u=> new SelectListItem {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(products);
        }
        public IActionResult Upsert(int? id) {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            ProductVM viewmodel = new() {
                CategoryList = CategoryList,
                Product = new Product()
            };
            if(id == null || id == 0) {
                return View(viewmodel);
            }
            else {
                viewmodel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(viewmodel);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM newProduct, IFormFile? file) {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null) {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/product");

                if (!string.IsNullOrEmpty(newProduct.Product.ImageUrl)) {
                    var oldimagepath = Path.Combine(wwwRootPath, newProduct.Product.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldimagepath)) {
                        System.IO.File.Delete(oldimagepath);
                    }
                }

                using(var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create)) {
                    file.CopyTo(filestream);
                }
                newProduct.Product.ImageUrl = @"\images\product\" + filename; 
            }
            if (newProduct.Product.Id == 0) {
                _unitOfWork.Product.Add(newProduct.Product);
                TempData["success"] = $"Product '{newProduct.Product.Title}' added successfully!";
            }
            else {
                _unitOfWork.Product.Update(newProduct.Product);
                TempData["success"] = $"Product '{newProduct.Product.Title}' updated successfully!";
            }
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = products});
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {
            var toDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (toDelete == null || toDelete.Id == 0) {
                return Json(new { success = false, message = $"Product with id: '{id}' not found."});
            }
            if (!string.IsNullOrEmpty(toDelete.ImageUrl)) {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var oldimagepath = Path.Combine(wwwRootPath, toDelete.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldimagepath)) {
                    System.IO.File.Delete(oldimagepath);
                }
            }
            _unitOfWork.Product.Remove(toDelete);
            _unitOfWork.SaveChanges();
            return Json(new { success = true, message = $"Product with id: '{id}' was deleted successfully!" });
        }
        #endregion
    }
}

