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
    public class CompanyController : Controller
    {
        private readonly IUnitofWork _unitOfWork;

        //get here with dependency Injection.
        public CompanyController(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            {
                if (id == null || id == 0)
                {
                    return View(company);//Create
                }
                else
                {
                    //Update
                    company = _unitOfWork.Company.GetFirstorDefault(u => u.Id == id);
                    return View(company);
                }

            }        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Updated Successfully";
                }
                return RedirectToAction("Index");
            }
         return View(obj);
        }

        //it is used to do spi calls and display all the product
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var compList = _unitOfWork.Company.GetAll();
            //After Retrive All Data then we Convert to Json and return back
            return Json(new { data = compList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstorDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion
    }
}
