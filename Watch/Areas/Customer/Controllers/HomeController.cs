using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models;

namespace Watch.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofWork;
        public HomeController(ILogger<HomeController> logger, IUnitofWork  unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            //retrive all the product
            IEnumerable<Product> prodList = _unitofWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(prodList);
        }
         public IActionResult Details(int id)
        {
            //retrive all the product
            ShopingCart cartObj = new()
            {
                Count=1,
                Product = _unitofWork.Product.GetFirstorDefault(u => u.Id == id, includeProperties: "Category,CoverType")
            };
            return View(cartObj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}