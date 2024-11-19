using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using AlphaKilo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaKiloWeb.Areas.Admin.Controllers {

    [Area("Admin")]
    public class ProductController : Controller{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Upsert(int? id) {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            ProductVM viewmodel = new ProductVM {
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
        public IActionResult Delete(int? id) {
            if (id == null || id == 0) {
                return NotFound($"{id} is not a valid Product id.");
            }
            Product? toDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (toDelete == null) {
                return NotFound($"Product with id: {id} doesn't exist.");
            }
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            ProductVM viewmodel = new ProductVM {
                CategoryList = CategoryList,
                Product = toDelete
            };
            return View(viewmodel);
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

