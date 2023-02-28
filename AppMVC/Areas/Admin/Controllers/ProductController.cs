using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> listProduct = _unitOfWork.Product.GetAll();
            return View(listProduct);
        }

        public IActionResult Upsert(int? id)
        {
            var product = new Product();
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString()});
            IEnumerable<SelectListItem> typeList = _unitOfWork.Type.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            if (id is null or 0)
                // create product
                return View(product);
            else
            {
                // update product
            }
            return View(product);
        }
      
    }
}
