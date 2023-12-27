using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetPlanner.Models;
using System.Diagnostics;

namespace MyBudgetPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        private async Task AdminLogin()
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync("ankit@bpst.com", "ankit@bpst.com", true, lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }
        }
        public async Task<IActionResult> Index()
        {
            await AdminLogin();
            return RedirectToAction("Create", "MyDailyExpences", new { Area = "Edu" });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
