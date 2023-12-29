using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;
using MyBudgetPlanner.ViewModels;

namespace MyBudgetPlanner.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult RegisterNewUser()
        {
            ViewBag.ActiveTabId = 4;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(AppUser appUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    appUser.UserName = appUser.Email;
                    var result = await _userManager.CreateAsync(appUser, appUser.Password);
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                        var result2 = await _userManager.AddToRoleAsync(appUser, "User");
                        await _signInManager.SignInAsync(appUser, isPersistent: false).ConfigureAwait(false);
                        MasterData.Tags.ForEach(t =>
                        {
                            _context.MyExpensePlans.Add(new MyExpensePlan() { UserId = appUser.Id, ExpenseName = t.TagName, Description = t.Description, IsMandatory = true, CreatedDate = DateTime.UtcNow });
                        });
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Planner");
                    }
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(appUser);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LoginName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.LoginName);
                    var role = await _userManager.GetRolesAsync(user);
                    return role.Contains("SuperAdmin")
                        ? RedirectToAction("Index", "Home", new { Area = "SuperAdmin" })
                        : RedirectToAction("Index", "MyExpensePlans");
                }
                else { ModelState.AddModelError("", "Invalid Email Id or Password"); }
            }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            ViewBag.ActiveTabId = 1;
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public JsonResult GetCitiesByStateId(int Id)
        //{
        //    var states = _context.Cities.Where(c => c.StateId.Equals(Id)).OrderBy(c => c.Name);
        //    return Json(new SelectList(states, "UniqueId", "Name"));
        //}

        ///

        public async Task<string> CreateMasterUser()
        {
            var resultStr = string.Empty;
            try
            {
                AppUser appUser = new AppUser()
                {
                    UserName = "admin@bpst.com",
                    Password = "Admin@20",
                    ConfirmPassword = "Admin@20",
                    PhoneNumber = "9999999999",
                };

                var result = await _userManager.CreateAsync(appUser, appUser.Password);
                if (result.Succeeded)
                {
                    var userRoles = _context.Roles.ToList();
                    foreach (var role in userRoles)
                        await _userManager.AddToRoleAsync(appUser, role.Name).ConfigureAwait(false);
                    resultStr = "Master User Created Successfully.";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        resultStr = "Some Error: " + error.Code;
                    }
                }

            }
            catch (Exception ex)
            {
                resultStr = "Some Error: " + ex.Message;
            }
            return resultStr;
        }

    }
}
