using System.Security.Claims;
using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Helpers.Cart;
using UI.Helpers.Redis;
using UI.Models;
using UI.Models.Cart;
using UI.Models.Identity;

namespace UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly RedisHelper _redisHelper;
        private readonly UserManager<AppUser> _userManager;

        public CartController(IProductService productService, IOrderService orderService, RedisHelper redisHelper, UserManager<AppUser> userManager)
        {
            _productService = productService;
            _orderService = orderService;
            _redisHelper = redisHelper;
            _userManager = userManager;
        }

        private string? GetUserId()
        {
            return User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        }

        public IActionResult Index()
        {
            var userId = GetUserId();
            List<CartItem> cart;

            if (userId != null)
            {
                cart = _redisHelper.GetCart(userId) ?? new List<CartItem>();
            }
            else
            {
                cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();
            }

            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId).Data;
            if (product == null) return NotFound();

            var userId = GetUserId();
            List<CartItem> cart;

            if (userId != null)
            {
                cart = _redisHelper.GetCart(userId) ?? new List<CartItem>();
            }
            else
            {
                cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();
            }

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = product.Price,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            if (userId != null)
            {
                _redisHelper.SetCart(userId, cart);
            }
            else
            {
                SessionHelper.Set(HttpContext.Session, "Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            List<CartItem> cart;

            if (userId != null)
            {
                cart = _redisHelper.GetCart(userId);
                if (cart != null)
                {
                    cart.RemoveAll(c => c.ProductId == productId);
                    _redisHelper.SetCart(userId, cart);
                }
            }
            else
            {
                cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");
                if (cart != null)
                {
                    cart.RemoveAll(c => c.ProductId == productId);
                    SessionHelper.Set(HttpContext.Session, "Cart", cart);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            var userId = GetUserId();
            if (userId != null)
            {
                _redisHelper.ClearCart(userId);
            }
            else
            {
                HttpContext.Session.Remove("Cart");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        public async Task<IActionResult> CheckoutUser()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new CheckOutUserViewModel
            {
                AddressList = new List<SelectListItem>()
            };

            if (!string.IsNullOrEmpty(user.Adress1))
            {
                model.AddressList.Add(new SelectListItem { Value = user.Adress1, Text = user.AdressTitle1 });
            }
            if (!string.IsNullOrEmpty(user.Adress2))
            {
                model.AddressList.Add(new SelectListItem { Value = user.Adress2, Text = user.AdressTitle2 });
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult CompleteOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");

            int orderNumber = Math.Abs(Guid.NewGuid().GetHashCode());

            foreach (var item in cart)
            {
                var product = _productService.GetById(item.ProductId).Data;
                product.UnitsInStock -= item.Quantity;
                _productService.Update(product);

                var order = new Order
                {
                    OrderId = orderNumber,
                    ProductId = item.ProductId,
                    ProductAmount = item.Quantity,
                    Price = item.Price,
                    AdressTitle = model.AddressTitle,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Adress = model.Address,
                    Email = model.Email,
                    Phone = model.Phone,
                    OrderDate = DateTime.Now,
                    UserId = "NoName"
                };

                _orderService.Add(order);
            }

            SessionHelper.Remove(HttpContext.Session, "Cart");

            return RedirectToAction("OrderCompleted");
        }

        [HttpPost]
        public async Task<ActionResult> CompleteOrderUser(CheckOutUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CheckoutUser", model);
            }
            var user = await _userManager.GetUserAsync(User);
            var cart = _redisHelper.GetCart(user.Id);

            int orderNumber = Math.Abs(Guid.NewGuid().GetHashCode());

            string selectedAddressTitle = "";
            if (user.Adress1 == model.SelectedAddress)
            {
                selectedAddressTitle = user.AdressTitle1;
            }
            else if (user.Adress2 == model.SelectedAddress)
            {
                selectedAddressTitle = user.AdressTitle2;
            }

            foreach (var item in cart)
            {
                var product = _productService.GetById(item.ProductId).Data;
                product.UnitsInStock -= item.Quantity;
                _productService.Update(product);

                var order = new Order
                {
                    OrderId = orderNumber,
                    ProductId = item.ProductId,
                    ProductAmount = item.Quantity,
                    Price = item.Price,
                    AdressTitle = selectedAddressTitle,  
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Adress = model.SelectedAddress,  
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    OrderDate = DateTime.Now,
                    UserId = user.Id,
                };

                _orderService.Add(order);
            }

            _redisHelper.ClearCart(user.Id);

            return RedirectToAction("OrderCompleted");
        }

        [HttpGet]
        public JsonResult IncreaseQuantity(int productId)
        {
            var userId = GetUserId();
            List<CartItem> cart;

            if (userId != null)
            {
                cart = _redisHelper.GetCart(userId) ?? new List<CartItem>();
            }
            else
            {                
                cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();
            }

            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
                cartItem.Price = cartItem.UnitPrice * cartItem.Quantity; 

                if (userId != null)
                {
                    _redisHelper.SetCart(userId, cart);
                }
                else
                {
                    SessionHelper.Set(HttpContext.Session, "Cart", cart);
                }

                return Json(new { success = true, newQuantity = cartItem.Quantity, newPrice = cartItem.Price });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult DecreaseQuantity(int productId)
        {
            var userId = GetUserId();
            List<CartItem> cart;

            if (userId != null)
            {
                cart = _redisHelper.GetCart(userId);
            }
            else
            {
                cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");
            }

            if (cart != null)
            {
                var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if (cartItem != null && cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cartItem.Price = cartItem.UnitPrice * cartItem.Quantity;

                    if (userId != null)
                    {
                        _redisHelper.SetCart(userId, cart);
                    }
                    else
                    {
                        SessionHelper.Set(HttpContext.Session, "Cart", cart);
                    }

                    return Json(new { success = true, newQuantity = cartItem.Quantity, newPrice = cartItem.Price });
                }
                else
                {
                    return Json(new { success = false, message = "Miktar 1'den daha az olamaz." });
                }
            }

            return Json(new { success = false, message = "Sepet bulunamadı." });
        }

        public ActionResult OrderCompleted()
        {
            return View();
        }


    }
}
