using Microsoft.AspNetCore.Mvc;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models;
using Watch.DataAccess;

namespace Watch.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //Retrive the data from database
        //private readonly ApplicationDbContext _db;
        private readonly IUnitofWork _unitOfWork;

        //Access The data
        public CategoryController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objcategoryList = _unitOfWork.Category.GetAll();
            return View(objcategoryList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
                return View(obj);
        }

        //GET
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.WatchCategories.Find(id);
            //var categoryFromDb = _db.GetFirstorDefault(u => u.Id == id);
            var categoryFromDb = _unitOfWork.Category.GetFirstorDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
         //GET
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.WatchCategories.Find(id);
            //var categoryFromDb = _db.GetFirstorDefault(u => u.Id ==id);
            var categoryFromDb = _unitOfWork.Category.GetFirstorDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstorDefault(u => u.Id == id);
            if (id == null || id == 0)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
            }

    }
}
