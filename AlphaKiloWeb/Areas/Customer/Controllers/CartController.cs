using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models;
using AlphaKilo.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlphaKiloWeb.Areas.Customer.Controllers {

    [Area("Customer")]
    [Authorize]
    public class CartController(IUnitOfWork unitOfWork) : Controller {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public IActionResult Index() {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM() {
                ShoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
            };
            foreach (var cart in ShoppingCartVM.ShoppingCarts) {
                cart.Price = GetCartPriceByQuantity(cart);
                ShoppingCartVM.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId) {
            ShoppingCart toUpdate = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            toUpdate.Count++;
            _unitOfWork.ShoppingCart.Update(toUpdate);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Added 1 item.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId) {
            ShoppingCart toUpdate = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (toUpdate.Count <= 1) {
                _unitOfWork.ShoppingCart.Remove(toUpdate);
            }
            else {
                toUpdate.Count--;
                _unitOfWork.ShoppingCart.Update(toUpdate);
            }
            _unitOfWork.SaveChanges();
            TempData["success"] = "Removed 1 item.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId) {
            ShoppingCart toRemove = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(toRemove);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Removed product.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary() {
            return View();
        }

        private static double GetCartPriceByQuantity(ShoppingCart cart) {
            if (cart.Count <= 50) {
                return cart.Product.ListPrice;
            }
            if (cart.Count <= 100) {
                return cart.Product.Price50;
            }
            return cart.Product.Price100;
        }
    }
}
