using AppMVC.DataAccess.Repository.IRepository;
using AppMVC.Models;
using AppMVC.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AppMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> listCompany = _unitOfWork.Company.GetAll();
            return View(listCompany);
        }

        public IActionResult Upsert(int? id)
        {
            var company = new Company();
            if (id is not (null or 0))
            {
                company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Create company successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Update company successfully";
                }


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        #region CALL APIS

        public IActionResult GetAll()
        {
            var list = _unitOfWork.Company.GetAll();

            return Json(new {data = list});
        }

        public IActionResult Delete(int id)
        {
            var company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);

            if (company is null)
                return Json(new { success = false, message = "Not found company" });

                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();

            return Json(new { success = true, message = "Delete company successfully" });
        }

        #endregion
    }
}
