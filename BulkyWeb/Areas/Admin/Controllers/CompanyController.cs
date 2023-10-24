using Mazen.DataAccess.Repository.IRepository;
using Mazen.Models;
using Mazen.Models.ViewModels;
using Mazen.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace MazenWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            CompanyVM CompanyVM = new() { Company = new Company()};
            if (id == null || id == 0)
            {
                //create
                return View(CompanyVM);
            }
            else
            {
                //update
                CompanyVM.Company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(CompanyVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(CompanyVM CompanyVM)
        {

            if (ModelState.IsValid)
            {
                if (CompanyVM.Company.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyVM.Company);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyVM.Company);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(CompanyVM);
            }
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return Json(new { data = objCompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
