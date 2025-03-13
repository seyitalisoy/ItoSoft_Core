using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers.Cart;
using UI.Models;


//using UI.Helpers.Cart;
using UI.Models.Cart;

namespace UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public CartController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId).Data;
            if (product == null) return NotFound();

            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();

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

            SessionHelper.Set(HttpContext.Session, "Cart", cart);
            return RedirectToAction("Index");
        }

        //[HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
            {
                cart.RemoveAll(c => c.ProductId == productId);
                SessionHelper.Set(HttpContext.Session, "Cart", cart);
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult IncreaseQuantity(int productId)
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
                cartItem.Price = cartItem.UnitPrice * cartItem.Quantity; // Doğru hesaplama
                SessionHelper.Set(HttpContext.Session, "Cart", cart);

                return Json(new { success = true, newQuantity = cartItem.Quantity, newPrice = cartItem.Price });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult DecreaseQuantity(int productId)
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");

            if (cart != null)
            {
                var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if (cartItem != null && cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cartItem.Price = cartItem.UnitPrice * cartItem.Quantity; // Doğru hesaplama
                    SessionHelper.Set(HttpContext.Session, "Cart", cart);

                    return Json(new { success = true, newQuantity = cartItem.Quantity, newPrice = cartItem.Price });
                }
                else
                {
                    return Json(new { success = false, message = "Miktar 1'den daha az olamaz." });
                }
            }

            return Json(new { success = false, message = "Sepet bulunamadı." });
        }



        public ActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        // Sipariş tamamlanacak
        [HttpPost]
        public ActionResult CompleteOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart.Count == 0)
            {
                return RedirectToAction("Index");
            }

            string userName= "User1";
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
                    Adress = model.Address,
                    OrderDate = DateTime.Now,
                    UserName = userName
                };

                _orderService.Add(order);
            }

            SessionHelper.Remove(HttpContext.Session,"Cart");

            return RedirectToAction("OrderCompleted");

        }

        public ActionResult OrderCompleted()
        {
            return View();
        }


    }
}
