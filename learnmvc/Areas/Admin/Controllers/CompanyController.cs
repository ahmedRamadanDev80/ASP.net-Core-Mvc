using learnmvc.DataAccess;
using learnmvc.DataAccess.Repository;
using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using learnmvc.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace learnmvc.Areas.Admin.Controllers
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

            return View();
        }
        
        //GET
        public IActionResult Upsert(int? id)
        {

            Company Company = new();
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(Company); 

            }
            else
            {
                //update product
                Company = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id == id);
                return View(Company);
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company item, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if(item.Id == 0) { 
                    _unitOfWork.Company.Add(item);
                    TempData["success"] = " Company created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(item);
                    TempData["success"] = "Compnay updated Successfully";
                }
                _unitOfWork.Save();
                //TempData["success"] = " Product created Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _unitOfWork.Company.GetAll();
            return Json(new {data=CompanyList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var item = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            _unitOfWork.Company.Remove(item);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company Deleted Successfully" });
        }
        #endregion
    }
}
