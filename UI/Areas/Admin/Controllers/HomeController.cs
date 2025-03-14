using System.Data;
using System.Diagnostics;
using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Areas.Admin.Models;
using UI.Areas.Admin.Models.Identity;
using UI.Models;
using UI.Models.Identity;


namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> UserList()
        //{
        //    var userList = await _userManager.Users.ToListAsync();
        //    var userViewModelList = userList.Select(x => new UserViewModel
        //    {
        //        Id = x.Id,
        //        Email = x.Email,
        //        Name = x.FullName,
        //        UserName = x.UserName
        //    }).ToList();


        //    return View(userViewModelList);
        //}

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync(); // Kullanýcýlarý al
            var userRoleViewModels = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Kullanýcýnýn rollerini al
                userRoleViewModels.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            return View(userRoleViewModels);
        }

    }
}
