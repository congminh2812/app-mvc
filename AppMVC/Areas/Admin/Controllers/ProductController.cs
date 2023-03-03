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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment=webHostEnvironment;
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
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file is not null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string uploads = Path.Combine(wwwRootPath, @"images\product\");
                    string extension = Path.GetExtension(file.FileName);

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @$"\images\product\{fileName + extension}";
                }

                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        #region API CALL
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> listProduct = _unitOfWork.Product.GetAll();
            return Json(new { data = listProduct });
        }
        #endregion
    }
}
