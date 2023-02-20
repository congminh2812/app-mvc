using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> listCategory = _unitOfWork.Category.GetAll();
            return View(listCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The DisplayOrder cannot match with Name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Create category successfully!";
                return RedirectToAction("Index");
            }

            return View(category);
        }


        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _unitOfWork.Category.GetFirstOrDefault(s => s.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The DisplayOrder cannot match with Name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Update category successfully!";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _unitOfWork.Category.GetFirstOrDefault(s => s.Id == id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(s => s.Id == id);

            if (category is null) return NotFound();

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["success"] = "Delete category successfully!";

            return RedirectToAction("Index");
        }
    }
}
