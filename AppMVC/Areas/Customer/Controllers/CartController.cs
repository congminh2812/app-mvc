using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Web.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
