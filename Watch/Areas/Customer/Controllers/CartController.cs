using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models.ViewModel;

namespace Watch.Areas.Customer.Controllers
{
    [Area("Customer")]
    //Only authorized user can access.
    [Authorize]
    public class CartController : Controller
    {
        //retrive all the shopping cart so we will using dependency injection
        private readonly IUnitofWork _unitofWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            //check user is logged in
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Want all the item of perticular user
            ShoppingCartVM = new ShoppingCartVM()
            {//We take product bcoz we want to take product property.
                ListCart = _unitofWork.ShopingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties:"Product")
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQty(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            ////check user is logged in
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ////Want all the item of perticular user
            //ShoppingCartVM = new ShoppingCartVM()
            //{//We take product bcoz we want to take product property.
            //    ListCart = _unitofWork.ShopingCart.GetAll(u => u.ApplicationUserId == claim.Value,
            //    includeProperties:"Product")
            //};
            //foreach (var cart in ShoppingCartVM.ListCart)
            //{
            //    cart.Price = GetPriceBasedOnQty(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            //    ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            //}
            //return View(ShoppingCartVM);
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitofWork.ShopingCart.GetFirstorDefault(u => u.Id == cartId);
            _unitofWork.ShopingCart.IncrementCount(cart, 1);
            _unitofWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitofWork.ShopingCart.GetFirstorDefault(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitofWork.ShopingCart.Remove(cart);
            }
            else
            {
                _unitofWork.ShopingCart.DecrementCount(cart, 1);
            }
            _unitofWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitofWork.ShopingCart.GetFirstorDefault(u => u.Id == cartId);
            _unitofWork.ShopingCart.Remove(cart);
            _unitofWork.Save();

            return RedirectToAction(nameof(Index));
        }


        //claculate the price as per quatity
        private double GetPriceBasedOnQty(double quantity,double price,double  price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else{

                if (quantity <= 100)
                {
                    return price50; 
                }
                return price100;
            }
            
        }
    }
}
