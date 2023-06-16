using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch.DataAccess.Repository.IRepository;
using Watch.Models;
using Watch.Models.ViewModel;
using Watch.Utility;

namespace Watch.Areas.Customer.Controllers
{
    [Area("Customer")]
    //Only authorized user can access.
    [Authorize]
    public class CartController : Controller
    {
        //retrive all the shopping cart so we will using dependency injection
        private readonly IUnitofWork _unitofWork;
        //automaticaly bind shoppingcart property
        [BindProperty]
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
                includeProperties:"Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQty(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                //ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            //check user is logged in
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Want all the item of perticular user
            ShoppingCartVM = new ShoppingCartVM()
            {//We take product bcoz we want to take (retrive) product properties.
                ListCart = _unitofWork.ShopingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                OrderHeader =new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofWork.ApplicationUser.GetFirstorDefault(
                u => u.Id == claim.Value);
            //From ApplicationUser we populate the order header properties
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;


            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQty(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            //check user is logged in
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitofWork.ShopingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");

            //At Initial
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
           
            //Calculating the Order total
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQty(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            //push in the database
            _unitofWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofWork.Save();

            //Create a Order Detail here
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetails orderDetails = new()
                {//populate the order detail
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,//As we save order header above then we can access here
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitofWork.OrderDetail.Add(orderDetails);
                _unitofWork.Save();
            }

            _unitofWork.ShopingCart.RemoneRange(ShoppingCartVM.ListCart);
            _unitofWork.Save();
            return RedirectToAction("Index","Home");
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
