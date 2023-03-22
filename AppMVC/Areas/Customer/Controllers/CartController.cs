using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
using AppMVC.Utility;
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

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims!.Value, includeProperties: "Product"),
                OrderHeader = new()
            };

            foreach (var item in ShoppingCartVM.ListCart)
                item.Price = GetPriceBasedOnQuantity(item);

            ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ListCart.Sum(s => s.Price * s.Count);

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims!.Value, includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(s => s.Id == claims.Value);

            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;

            foreach (var item in ShoppingCartVM.ListCart)
                item.Price = GetPriceBasedOnQuantity(item);

            ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ListCart.Sum(s => s.Price * s.Count);

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims!.Value, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;

            if (!ModelState.IsValid) return View(ShoppingCartVM);

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in ShoppingCartVM.ListCart)
                item.Price = GetPriceBasedOnQuantity(item);

            ShoppingCartVM.OrderHeader.OrderTotal = ShoppingCartVM.ListCart.Sum(s => s.Price * s.Count);

            foreach (var item in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count,
                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Plus(int cartId)
        {
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
