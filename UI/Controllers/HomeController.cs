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
    //[Authorize]
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
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
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
                TempData["SuccessMessage"] = "�yelik kay�t i�lemi ba�ar�l�.";
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
            return View();
        }


        //Redis Cart Transfer
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var userResult = await _userManager.FindByEmailAsync(request.Email);

            if (userResult == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya �ifre yanl��");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(userResult, request.Password, request.RememberMe, true);

            if (signInResult.Succeeded)
            {
                // Login olduktan sonra cart bilgisinin Redis'e aktar�lmas�
                await SyncCartToRedis(userResult);

                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Dakika boyunca giri� yapamazs�n�z" });
                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya �ifre yanl��" });

            return View();
        }

        private async Task SyncCartToRedis(AppUser user)
        {
            // Kullan�c� login oldu�unda Session'dan cart bilgisini al�yoruz
            var userId = user.Id;
            var sessionCart = SessionHelper.Get<List<CartItem>>(HttpContext.Session, "Cart");

            if (sessionCart != null && sessionCart.Count > 0)
            {
                // Redis'teki mevcut cart verisini al
                var redisCart = _redisHelper.GetCart(userId) ?? new List<CartItem>();

                // Session'dan gelen �r�nleri Redis'e aktar
                foreach (var item in sessionCart)
                {
                    var redisItem = redisCart.FirstOrDefault(c => c.ProductId == item.ProductId);

                    if (redisItem != null)
                    {
                        // E�er Redis'te bu �r�n zaten varsa miktar�n� art�r
                        redisItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        // E�er Redis'te yoksa, yeni �r�n ekle
                        redisCart.Add(item);
                    }
                }

                // G�ncellenmi� cart verisini Redis'e kaydet
                _redisHelper.SetCart(userId, redisCart);

                // Session'dan cart bilgisini sil
                HttpContext.Session.Remove("Cart");
            }
        }
    }
}
