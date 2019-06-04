
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{

    [Authorize(Roles = "Administrators")] //要有用户权限才能进行一下的操作
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// 获取用户管理
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]//防止csrf攻击
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddUser(/*[Bind("AnotherUser")]*/UserCreateViewModel userAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAddViewModel);
            }

            var user = new ApplicationUser
            {
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Email,
                IdCardNo = userAddViewModel.IdCardNo,
                BirthDate = userAddViewModel.BirthDate
            };
            var result = await _userManager.CreateAsync(user, userAddViewModel.PassWord);
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

            return View(userAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "删除发生错误");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
            }

            return View("Index", await _userManager.Users.ToListAsync());
        }



        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var cliams = await _userManager.GetClaimsAsync(user);

            var vm = new UserEditViewModel
            {
                Id = user.Id,
                BirthDate = user.BirthDate,
                Email = user.Email,
                IdCardNo = user.IdCardNo,
                UserName = user.UserName,
                Claims = cliams.Select(x => x.Value).ToList()
            };
            //return View(user);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserEditViewModel userEdit)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.FindByIdAsync(id);
                if (result != null)
                {
                    result.UserName = userEdit.UserName;
                    result.Email = userEdit.Email;
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ManageClaims(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            var user = await _userManager.Users.Include(x => x.Claims).Where(x => x.Id == userId)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var LeftClaims = AllClaimTypes.AllClaimTypeList.Except(user.Claims.Select(x => x.ClaimType)).ToList();
            var vm = new ManageClaimsViewModel
            {
                userId = userId,
                AllClims = /*AllClaimTypes.AllClaimTypeList*/LeftClaims
            };
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> ManageClaims(ManageClaimsViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.userId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var claim = new IdentityUserClaim<string>
            {
                ClaimType = vm.CliamId,
                ClaimValue = vm.CliamId
            };
            user.Claims.Add(claim);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Edit", new { id = user.Id });
            }
            ModelState.AddModelError(string.Empty, "编辑用户claims发生错误");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Removeclaim(string claimid, string Claim)
        {
            var user = await _userManager.Users.Include(x => x.Claims).Where(x => x.Id == claimid)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var claims = user.Claims.Where(x => x.ClaimType == Claim).ToList();
            foreach (var VARIABLE in claims)
            {
                user.Claims.Remove(VARIABLE);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Edit", new { id = claimid });
            }
            ModelState.AddModelError(string.Empty, "编辑用户Claims时发生错误");
            return RedirectToAction("Edit", new { id = claimid });
        }
    }
}
