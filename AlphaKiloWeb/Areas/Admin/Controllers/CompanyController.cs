using AlphaKilo.DataAccess.Repository.IRepository;
using AlphaKilo.Models.ViewModels;
using AlphaKilo.Models;
using AlphaKilo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaKiloWeb.Areas.Admin.Controllers {
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id) {
            if (id == null || id == 0) {
                return View();
            }
            else {
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company? newCompany) {
            if (newCompany == null || newCompany.Id == 0) {
                _unitOfWork.Company.Add(newCompany);
                TempData["success"] = $"Company '{newCompany.Name}' added successfully!";
            }
            else {
                _unitOfWork.Company.Update(newCompany);
                TempData["success"] = $"Company '{newCompany.Name}' updated successfully!";
            }
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id) {
            var toDelete = _unitOfWork.Company.Get(u => u.Id == id);
            if (toDelete == null || toDelete.Id == 0) {
                return Json(new { success = false, message = $"Company with id: '{id}' not found." });
            }
            _unitOfWork.Company.Remove(toDelete);
            _unitOfWork.SaveChanges();
            return Json(new { success = true, message = $"Company with id: '{id}' was deleted successfully!" });
        }
        #endregion
    }
}
