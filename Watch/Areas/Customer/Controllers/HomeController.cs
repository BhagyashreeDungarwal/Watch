using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
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
         public IActionResult Details(int productId)
        {
            //retrive all the product
            ShopingCart cartObj = new()
            {
                Count=1,
                //productid and product.id both can be populate
                ProductId =productId,
                Product = _unitofWork.Product.GetFirstorDefault(u => u.Id == productId, includeProperties: "Category,CoverType")
            };
            return View(cartObj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
         public IActionResult Details(ShopingCart shopingCart)
        {//it is gave access to the user who is logged in
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shopingCart.ApplicationUserId = claim.Value;

            //it is used if the user already add a product and add same product then that cannot create a new entry just retrive the old entry
            ShopingCart cartFromDb = _unitofWork.ShopingCart.GetFirstorDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == shopingCart.ProductId);

            if (cartFromDb  == null)
            {
                _unitofWork.ShopingCart.Add(shopingCart);
            }
            else
            {//for these we create a increment and decrement in the shopping cart repository then we can access here.
                _unitofWork.ShopingCart.IncrementCount(cartFromDb, shopingCart.Count);
            }
          
            _unitofWork.Save();

            return RedirectToAction(nameof(Index));
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