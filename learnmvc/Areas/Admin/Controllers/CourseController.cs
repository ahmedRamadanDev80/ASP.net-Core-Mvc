using learnmvc.DataAccess;
using learnmvc.DataAccess.Repository.IRepository;
using learnmvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace learnmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Course> kolo = _unitOfWork.Course.GetAll();
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
        public IActionResult Create(Course item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Course.Add(item);
                _unitOfWork.Save();
                TempData["success"] = " Course Created Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();
            //var item = _db.courses.Find(id);
            var item = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course item)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Course.Update(item);
                _unitOfWork.Save();
                TempData["success"] = " Course Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            var item = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCourse(int? id)
        {
            var item = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id);
            if (item == null) return NotFound();

            _unitOfWork.Course.Remove(item);
            _unitOfWork.Save();
            TempData["success"] = " Course Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
