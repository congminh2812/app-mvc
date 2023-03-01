using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
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
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });
            IEnumerable<SelectListItem> typeList = _unitOfWork.Type.GetAll().Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            var productVM = new ProductVM
            {
                Product =new Product(),
                CategoryList = categoryList,
                TypeList = typeList,
            };

            if (id is null or 0)
            {
                // create product
                return View(productVM);
            }
            else
            {
                // update product
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {

            }

            return View(productVM);
        }

    }
}
