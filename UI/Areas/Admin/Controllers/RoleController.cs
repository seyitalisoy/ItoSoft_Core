using Entities.Concrete.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI.Areas.Admin.Models.Identity;
using UI.Extensions;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> RoleList()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleListViewModel
            {
                Id = x.Id,
                Name = x.Name!
            }).ToListAsync();

            return View(roles);
        }


        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel request)
        {
            var result = await _roleManager.CreateAsync(new AppRole
            {
                Name = request.Name
            });

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            TempData["RoleAdded"] = true;
            return View();
        }


        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir rol bulunamadı.");
                return View();
            }

            return View(new UpdateRoleViewModel
            {
                Id = role.Id,
                Name = role!.Name!
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir rol bulunamadı.");
                return View();
            }

            role.Name = request.Name;

            await _roleManager.UpdateAsync(role);

            TempData["RoleUpdated"] = true;

            return View();
        }


        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir rol bulunamadı.");
                TempData["RoleNotDeleted"] = true;
                return RedirectToAction(nameof(RoleController.RoleList));
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                TempData["RoleNotDeleted"] = true;
                return RedirectToAction(nameof(RoleController.RoleList));
            }

            TempData["RoleDeleted"] = true;
            return RedirectToAction(nameof(RoleController.RoleList));
        }

        public async Task<IActionResult> AssignRoleToUser(string userId)
        {
            var user = (await _userManager.FindByIdAsync(userId))!;

            ViewBag.userId = userId;

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(user);

            var roleViewModelList = new List<AssignRoleToUserViewModel>();



            var userGeneralVM = new UserGeneralViewModel
            {
                Email = user.Email,
                FullName = user.FirstName+" "+user.LastName,
                Username = user.UserName
            };


            foreach (var role in roles)
            {
                var assignRoleToUserVM = new AssignRoleToUserViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };

                if (userRoles.Contains(role.Name))
                {
                    assignRoleToUserVM.IsExist = true;
                }

                roleViewModelList.Add(assignRoleToUserVM);
            }

            var userRoleInfoVM = new UserRoleInfoViewModel
            {
                AssignRoleToUserViewModel = roleViewModelList,
                UserGeneralViewModel = userGeneralVM,
            };

            return View(userRoleInfoVM);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, UserRoleInfoViewModel requestList)
        {
            var user = (await _userManager.FindByIdAsync(userId))!;

            foreach (var role in requestList.AssignRoleToUserViewModel)
            {
                if (role.IsExist)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                }
            }

            TempData["RoleAssigned"] = true;

            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }

        //[HttpGet]
        //public async Task<IActionResult> RoleAssign(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    var roles = _roleManager.Roles.ToList();
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var RoleAssings = new List<RoleAssignModels>();

        //    roles.ForEach(role => RoleAssings.Add(new RoleAssignModels
        //    {

        //        HasAssign = userRoles.Contains(role.Name),
        //        Id = int.Parse(role.Id),
        //        Name = role.Name
        //    }));
        //    ViewBag.UserName = user.UserName;
        //    return View(RoleAssings);
        //}
        //[HttpPost]
        //public async Task<IActionResult> RoleAssign(List<RoleAssignModels> models, string id)
        //{
        //    ///bütün kullanıcıları getiriyor ????
        //    var user = await _userManager.FindByIdAsync(id);
        //    foreach (var role in models)
        //    {
        //        if (role.HasAssign)
        //        {
        //            await _userManager.AddToRoleAsync(user, role.Name);
        //        }
        //        else
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, role.Name);
        //        }
        //    }
        //    return RedirectToAction("RoleAssign");
        //}





        //[HttpPost]
        //public async Task<IActionResult> AssignRoleToUser(string userId, UserRoleViewModel requestList)
        //{

        //    return RedirectToAction("UserList", "Home");
        //}



        //public async Task<IActionResult> AssignRoleToUser(string userId)
        //{
        //    var user = (await _userManager.FindByIdAsync(userId))!;

        //    ViewBag.userId = userId;

        //    var roles = await _roleManager.Roles.ToListAsync();

        //    var userRoles = await _userManager.GetRolesAsync(user);

        //    var roleViewModelList = new List<AssignRoleToUserViewModel>();



        //    var userGeneralVM = new UserGeneralViewModel
        //    {
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        Username = user.UserName,
        //    };


        //    foreach (var role in roles)
        //    {
        //        var assignRoleToUserVM = new AssignRoleToUserViewModel
        //        {
        //            RoleId = role.Id,
        //            RoleName = role.Name,
        //        };

        //        if (userRoles.Contains(role.Name))
        //        {
        //            assignRoleToUserVM.IsExist = true;
        //        }

        //        roleViewModelList.Add(assignRoleToUserVM);
        //    }

        //    var userRoleInfoVM = new UserRoleInfoViewModel
        //    {
        //        AssignRoleToUserViewModel = roleViewModelList,
        //        UserGeneralViewModel = userGeneralVM,
        //    };

        //    return View(userRoleInfoVM);
        //}


        //[HttpPost]
        //public async Task<IActionResult> AssignRoleToUser(string userId, UserRoleInfoViewModel requestList)
        //{
        //    var user = (await _userManager.FindByIdAsync(userId))!;

        //    foreach (var role in requestList.AssignRoleToUserViewModel)
        //    {
        //        if (role.IsExist)
        //        {
        //            await _userManager.AddToRoleAsync(user, role.RoleName);
        //        }
        //        else
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, role.RoleName);
        //        }
        //    }

        //    TempData["RoleAssigned"] = true;

        //    return RedirectToAction(nameof(DashboardController.UserList), "Dashboard");
        //}


        //[HttpPost]
        //public async Task<IActionResult> AssignRoleToUser(string userId, UserRoleInfoViewModel requestList)
        //{
        //    var user = (await _userManager.FindByIdAsync(userId))!;

        //    foreach (var role in requestList.AssignRoleToUserViewModel)
        //    {
        //        if (role.IsExist)
        //        {
        //            await _userManager.AddToRoleAsync(user, role.RoleName);
        //        }
        //        else
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, role.RoleName);
        //        }
        //    }

        //    TempData["RoleAssigned"] = true;

        //    return RedirectToAction(nameof(HomeController.UserList), "Home");
        //}
    }
}
