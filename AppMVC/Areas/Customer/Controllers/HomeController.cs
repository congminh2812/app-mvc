using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
using AppMVC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AppMVC.Web.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork=unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> listProduct = _unitOfWork.Product.GetAll(includeProperties: "Category,Type");
            return View(listProduct);
        }

        public IActionResult Details(int productId)
        {
            var shoppingCart = new ShoppingCart
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(s => s.Id == productId, includeProperties: "Category,Type")
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claims!.Value;

            var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.ApplicationUserId == shoppingCart.ApplicationUserId && s.ProductId == shoppingCart.ProductId);

            if (cartFromDb is null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionShoppingCart, _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claims.Value).Count());
            }
            else
            {
                _unitOfWork.ShoppingCart.Increasement(cartFromDb, shoppingCart.Count);
                _unitOfWork.Save();
            }


            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}