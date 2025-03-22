using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using UI.Areas.Admin.Models.Identity;
using UI.Extensions;
using UI.Models.Identity;

namespace UI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);


            var userViewModel = new AppUser
            {
                FirstName = currentUser!.FirstName,
                LastName = currentUser!.LastName,
                UserName = currentUser!.UserName,
                Email = currentUser!.Email,
                PhoneNumber = currentUser!.PhoneNumber,
                Adress1 = currentUser!.Adress1,
                AdressTitle1 = currentUser!.AdressTitle1,
                AdressTitle2 = currentUser!.AdressTitle2,
                Adress2 = currentUser!.Adress2
            };

            return View(userViewModel);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IActionResult> EditUser()
        {
            var currentUser = await _userManager.FindByNameAsync(User?.Identity?.Name!);

            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View();
            }

            var editUserViewModel = new EditUserViewModel()
            {

                UserName = currentUser!.UserName!,
                Email = currentUser!.Email!,
                Phone = currentUser!.PhoneNumber!,
                FirstName = currentUser!.FirstName,
                LastName = currentUser!.LastName,
                Adress1 = currentUser!.Adress1,
                Adress2 = currentUser!.Adress2,
                AdressTitle1 = currentUser!.AdressTitle1,
                AdressTitle2 = currentUser!.AdressTitle2                
            };

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel request)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                return View(request);
            }

            if (request.UserName != currentUser.UserName)
            {
                var isExist = await _userManager.FindByNameAsync(request.UserName);

                if (isExist != null)
                {
                    ModelState.AddModelError(string.Empty, "Bu kullanıcı adı başkası tarafından kullanılıyor.");
                    return View(request);
                }

            }

            currentUser!.UserName = request.UserName;
            currentUser!.Email = request.Email;
            currentUser!.PhoneNumber = request.Phone;
            currentUser.FirstName = request.FirstName;
            currentUser.LastName = request.LastName;
            currentUser.Adress1 = request.Adress1;
            currentUser.Adress2 = request.Adress2;
            currentUser.AdressTitle2 = request.AdressTitle2;
            currentUser.AdressTitle1 = request.AdressTitle1;

            var updatedUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updatedUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updatedUserResult.Errors);

                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();

            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Kullanıcı bilgileri güncellendi";

            return View(request);
        }

        public async Task<IActionResult> ChangePassword()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {

            if (!ModelState.IsValid)
            {

                return View();
            }

            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");

                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (!resultChangePassword.Succeeded)
            {

                ModelState.AddModelErrorList(resultChangePassword.Errors);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, false);

            TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi";            

            return RedirectToAction("ChangePassword");
        }
    }
}
