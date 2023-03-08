using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Detail(int id)
        {
            var shoppingCart = new ShoppingCart
            {
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(s => s.Id == id, includeProperties: "Category,Type")
            };

            return View(shoppingCart);
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