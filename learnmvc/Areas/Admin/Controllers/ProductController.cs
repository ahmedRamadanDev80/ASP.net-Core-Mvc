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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }
        
        //GET
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text  = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text  = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(ProductVM); 

            }
            else
            {
                //update product
                ProductVM.Product = _unitOfWork.Product.GetFirstOrDefault(u=>u.Id == id);
                return View(ProductVM);
            }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM item, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
               
                string wwwRootPath =_hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products\");
                    var extension = Path.GetExtension(file.FileName);
             if(item.Product.ImageUrl != null)
                    {
                        var oldImage = Path.Combine(wwwRootPath, item.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }
                    using(var fileStreams = new FileStream(Path.Combine(uploads,fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    item.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if(item.Product.Id == 0) { 
                    _unitOfWork.Product.Add(item.Product);
                    TempData["success"] = " Product created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(item.Product);
                    TempData["success"] = " Product updated Successfully";
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
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data=productList});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var item = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            var oldImage = Path.Combine(_hostEnvironment.WebRootPath, item.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
            _unitOfWork.Product.Remove(item);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product Deleted Successfully" });
        }
        #endregion
    }
}
