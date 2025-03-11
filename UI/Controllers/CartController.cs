using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers.Cart;

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
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session,"Cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _productService.GetById(productId).Data;
            if (product == null) return NotFound();

            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session,"Cart") ?? new List<CartItem>();

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
                    Price = product.Price,
                    Quantity = 1
                });
            }

            SessionHelper.Set(HttpContext.Session,"Cart", cart);
            return RedirectToAction("Index");
        }

        //[HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");
            if (cart != null)
            {
                cart.RemoveAll(c => c.ProductId == productId);
                SessionHelper.Set(HttpContext.Session,"Cart", cart);
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
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session,"Cart") ?? new List<CartItem>(); // Eğer null ise boş bir liste başlat
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
                SessionHelper.Set(HttpContext.Session,"Cart", cart);
                return Json(new { success = true, newQuantity = cartItem.Quantity });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult DecreaseQuantity(int productId)
        {
            var cart = SessionHelper.Get<List<CartItem>>(HttpContext.Session,"Cart");

            if (cart != null)
            {
                var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
                if (cartItem != null && cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    SessionHelper.Set(HttpContext.Session,"Cart", cart);
                    return Json(new { success = true, newQuantity = cartItem.Quantity });
                }
                else
                {
                    return Json(new { success = false, message = "Miktar 1'den daha az olamaz." });
                }
            }

            return Json(new { success = false, message = "Sepet bulunamadı." });
        }




        //[HttpGet]
        //public IActionResult DecreaseQuantity(int productId)
        //{
        //    // Sepeti session'dan al
        //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        //    if (cart != null)
        //    {
        //        // Sepetteki ürünü bul
        //        var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

        //        if (cartItem != null)
        //        {
        //            // Miktarı azalt
        //            if (cartItem.Quantity > 1)
        //            {
        //                cartItem.Quantity--;
        //            }
        //            else
        //            {
        //                // Miktar 1 ise ürünü sepetten çıkar
        //                cart.RemoveAll(c => c.ProductId == productId);
        //            }
        //        }

        //        // Güncellenmiş sepeti session'a kaydet
        //        HttpContext.Session.SetObjectAsJson("Cart", cart);
        //    }

        //    // Sepet sayfasına yönlendir
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public IActionResult IncreaseQuantity(int productId)
        //{
        //    // Sepeti session'dan al
        //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

        //    if (cart != null)
        //    {
        //        // Sepetteki ürünü bul
        //        var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

        //        if (cartItem != null)
        //        {
        //            // Miktarı arttır
        //            cartItem.Quantity++;
        //        }
        //        else
        //        {
        //            // Eğer ürün sepette yoksa yeni bir ürün ekle
        //            var product = _productService.GetById(productId).Data;
        //            if (product != null)
        //            {
        //                cart.Add(new CartItem
        //                {
        //                    ProductId = product.ProductId,
        //                    ProductName = product.ProductName,
        //                    Price = product.Price,
        //                    Quantity = 1
        //                });
        //            }
        //        }

        //        // Güncellenmiş sepeti session'a kaydet
        //        HttpContext.Session.SetObjectAsJson("Cart", cart);
        //    }

        //    // Sepet sayfasına yönlendir
        //    return RedirectToAction("Index");
        //}



    }
}
