using learnmvc.DataAccess;
using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace learnmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> kolo = _unitOfWork.CoverType.GetAll();
            return View(kolo);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(item);
                _unitOfWork.Save();
                TempData["success"] = " CoverType Created Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var item = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(item);
                _unitOfWork.Save();
                TempData["success"] = " CoverType Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            var item = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCoverType(int? id)
        {
            var item = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            _unitOfWork.CoverType.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = " CoverType Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
