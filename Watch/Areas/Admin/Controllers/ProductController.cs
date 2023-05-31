using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Watch.DataAccess;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models;
using Watch.Models.ViewModel;

namespace Watch.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitOfWork;
        //webhosting is used when we work with the files.
        private readonly IWebHostEnvironment _webHostEnvironment;

        //get here with dependency Injection.
        public ProductController(IUnitofWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            // IEnumerable<Product> objcategoryList = _unitOfWork.Product.GetAll();
            // return View(objcategoryList);
            return View();
        }

        //GET
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //Product product = new();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            //       u => new SelectListItem
            //        {
            //           Text = u.Name,
            //           Value = u.Id.ToString()
            //       });

            //   IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            //       u => new SelectListItem
            //       {
            //           Text = u.Name,
            //           Value = u.Id.ToString()
            //       }) ;

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                })
            };
            if (id == null || id == 0)
            {
                //Create
                //  ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.GetFirstorDefault(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageURL != null)
                    {
                        var oldImg = Path.Combine(wwwRootPath, obj.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageURL = @"\Images\products\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product Updated Successfully";
                }
                // _unitOfWork.Save();
                // TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
         return View(obj);
        }

        //it is used to do spi calls and display all the product
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var prodList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            //After Retrive All Data then we Convert to Json and return back
            return Json(new { data = prodList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            //Deleting the old image if the image is exists.
            var OldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(OldImgPath))
            {
                System.IO.File.Delete(OldImgPath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion
    }
}
