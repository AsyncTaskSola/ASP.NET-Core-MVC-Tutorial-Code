using System.Collections.Generic;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
   // [Authorize(Roles = "Administrators")]
    [Authorize(Policy = "仅限管理员")]
    public class RoleController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// 角色列表 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddViewModel roleAddView)
        {
            if (!ModelState.IsValid)
            {
                return View(roleAddView);
            }

            var role = new IdentityRole
            {
                Name = roleAddView.RoleName
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(roleAddView);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("Index");
            }

            var roleEditViewModel = new RoleEditViewModel
            {
                Id = id,
                RoleName = role.Name,
                Users = new List<string>()
            };
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                //判断用户是否在这角色里
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    roleEditViewModel.Users.Add(user.UserName);
                }
            }

            return View(roleEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleEditViewModel roleEditViewModel)
        {
            var role = await _roleManager.FindByIdAsync(roleEditViewModel.Id);
            if (role != null)
            {
                role.Name = roleEditViewModel.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return View(roleEditViewModel);
                }

                ModelState.AddModelError(string.Empty, "更新出错");
                return View(roleEditViewModel);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role!=null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty,"删除用户出错");
            }
            ModelState.AddModelError(string.Empty,"没找到用户角色");
            return View("Index", await _roleManager.Roles.ToListAsync());
        }

        #region 添加用户到角色

        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToAction("Index");
            }
            //把user和role链接在一起

            var vm = new UserRoleViewModel
            {
                RoleId = role.Id,
            };
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                //只有没有在这角色里的用户才能添加到角色
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(user);
                }
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);
            if (user != null && role != null)
            {
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { id = role.Id });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(userRoleViewModel);
                }
            }
            ModelState.AddModelError(string.Empty, "没找到用户或角色");
            return View("Index", await _roleManager.Roles.ToListAsync());
        }

        #endregion



        public async Task<IActionResult> DeleteUserToRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new UserRoleViewModel
            {
                RoleId = role.Id
            };
            var users =await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    vm.Users.Add(user);
                }
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);
            if (user != null && role != null)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditRole", new {id = role.Id});
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty,error.Description);
                    }

                    return View(userRoleViewModel);
                }
                ModelState.AddModelError(string.Empty, "用户不在角色里");
                return View(userRoleViewModel);
            }
            ModelState.AddModelError(string.Empty, "用户或角色没找到");
            return View(userRoleViewModel);

        }
    }
}
