using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AlphaKiloWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index() {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
        }
        public IActionResult Details(int prodId) {
            ShoppingCart cart = new() {
                Product = _unitOfWork.Product.Get(u => u.Id == prodId, includeProperties: "Category"),
                Count = 1,
                ProductId = prodId
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart) {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            ShoppingCart existingCart = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);
            if (existingCart != null) {
                existingCart.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            else {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            _unitOfWork.SaveChanges();
            TempData["success"] = "Cart updated!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
