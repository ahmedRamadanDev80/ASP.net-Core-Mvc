using learnmvc.DataAccess;
using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace learnmvc.Areas.Admin.Controllers
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
            IEnumerable<Category> kolo = _unitOfWork.Category.GetAll();
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
        public IActionResult Create(Category item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(item);
                _unitOfWork.Save();
                TempData["success"] = " Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            var item = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(item);
                _unitOfWork.Save();
                TempData["success"] = " Category Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            var item = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var item = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            _unitOfWork.Category.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = " Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
