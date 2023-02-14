using AppMVC.DataAccess.Data;
using AppMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> listCategory = _db.Categories;
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
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Create category successfully!";
                return RedirectToAction("Index");
            }

            return View(category);
        }


        public IActionResult Edit(int? id)
        {
            if (id is (null or 0))
                return NotFound();

            var category = _db.Categories.Find(id);

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
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"]="Update category successfully!";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if (id is (null or 0))
                return NotFound();

            var category = _db.Categories.Find(id);

            if (category is null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var category = _db.Categories.Find(id);

            if (category is null) return NotFound();

            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"]="Delete category successfully!";

            return RedirectToAction("Index");
        }
    }
}
