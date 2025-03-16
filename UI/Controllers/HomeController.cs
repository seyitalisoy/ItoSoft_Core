using System.Diagnostics;
using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UI.Extensions;
using UI.Helpers.Cart;
using UI.Helpers.Redis;
using UI.Models;
using UI.Models.Cart;
using UI.Models.Identity;

namespace UI.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RedisHelper _redisHelper;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RedisHelper redisHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _redisHelper = redisHelper;
        }

        public IActionResult Index()
        {
            TempData["ShowNavbar"] = false;
            return View();
        }

        public IActionResult SignUp()
        {
            TempData["ShowWelcomeMessage"] = true;
            TempData["ShowNavbar"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            
            TempData["ShowNavbar"] = false;
            if (!ModelState.IsValid)
            {
                return View();
            }


            var user = new AppUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                Adress1 = request.Adress1,
                AdressTitle1 = request.AdressTitle1
            };

            var identityResult = await _userManager.CreateAsync(user, request.PasswordConfirm);


            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["SuccessMessage"] = "Üyelik kayýt iþlemi baþarýlý.";
                return RedirectToAction("SignUp");
            }

            foreach (IdentityError item in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }
            return View();
        }

        public IActionResult SignIn()
        {
            TempData["ShowNavbar"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {
            TempData["ShowNavbar"] = false;
            TempData["ShowWelcomeMessage"] = false;
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var userResult = await _userManager.FindByEmailAsync(request.Email);

            if (userResult == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(userResult, request.Password, request.RememberMe, true);

            if (signInResult.Succeeded)
            {
                await SyncCartToRedis(userResult);

                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Dakika boyunca giriþ yapamazsýnýz" });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya þifre yanlýþ" });

            return View();
        }

        private async Task SyncCartToRedis(AppUser user)
        {
            var userId = user.Id;
            var sessionCart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");

            if (sessionCart != null && sessionCart.Count > 0)
            {
                var redisCart = _redisHelper.GetCart(userId) ?? new List<CartItem>();

                foreach (var item in sessionCart)
                {
                    var redisItem = redisCart.FirstOrDefault(c => c.ProductId == item.ProductId);

                    if (redisItem != null)
                    {
                        redisItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        redisCart.Add(item);
                    }
                }
                _redisHelper.SetCart(userId, redisCart);

                HttpContext.Session.Remove("Cart");
            }
        }
    }
}
