using AppMVC.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Web.Controllers
{
    public class TypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Models.Type> listType = _unitOfWork.Type.GetAll(); 
            return View(listType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.Type type) { 
            if (ModelState.IsValid)
            {
                _unitOfWork.Type.Add(type);
                _unitOfWork.Save();
                TempData["success"] = "Create type successfully!";
                return RedirectToAction("Index");
            }

            return View(type);
        }

        public IActionResult Edit(int? id)
        {
            if (id is (null or 0))
                return NotFound();

            var type = _unitOfWork.Type.GetFirstOrDefault(s => s.Id == id);

            if (type is null)
                return NotFound();

            return View(type);
        }

        [HttpPost]
        public IActionResult Edit(Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Type.Update(type);
                _unitOfWork.Save();
                TempData["success"] = "Create update successfully!";
                return RedirectToAction("Index");
            }

            return View(type);
        }


        public IActionResult Delete(int? id)
        {
            if (id is (null or 0))
                return NotFound();

            var type = _unitOfWork.Type.GetFirstOrDefault(s => s.Id == id);

            if (type is null)
                return NotFound();

            return View(type);
        }

        [HttpPost]
        public IActionResult Delete(Models.Type type)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Type.Remove(type);
                _unitOfWork.Save();
                TempData["success"] = "Create remove successfully!";
                return RedirectToAction("Index");
            }

            return View(type);
        }
    }
}
