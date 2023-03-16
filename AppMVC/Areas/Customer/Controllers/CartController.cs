using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppMVC.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims!.Value, includeProperties: "Product")
            };

            foreach (var item in ShoppingCartVM.ListCart)
                item.Price = GetPriceBasedOnQuantity(item);

            ShoppingCartVM.CartTotal = ShoppingCartVM.ListCart.Sum(s => s.Price * s.Count);

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity!;
            //var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //ShoppingCartVM = new ShoppingCartVM
            //{
            //    ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims!.Value, includeProperties: "Product")
            //};

            //foreach (var item in ShoppingCartVM.ListCart)
            //    item.Price = GetPriceBasedOnQuantity(item);

            //ShoppingCartVM.CartTotal = ShoppingCartVM.ListCart.Sum(s => s.Price * s.Count);

            //return View(ShoppingCartVM);

            return View();
        }

        public IActionResult Plus(int cartId) { 
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            if (cart is not null)
            {
                _unitOfWork.ShoppingCart.Increasement(cart, 1);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            if (cart.Count - 1 <= 0)
                _unitOfWork.ShoppingCart.Remove(cart);
            else 
                _unitOfWork.ShoppingCart.Decreasement(cart, 1);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);

            if (cart is not null)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
                return shoppingCart.Product.Price;
            else if (shoppingCart.Count > 50 && shoppingCart.Count < 100)
                return shoppingCart.Product.Price50;
            else return shoppingCart.Product.Price100;
        }
    }
}
