using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;
using System.Diagnostics;

namespace MyBudgetPlanner.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
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
            return RedirectToAction("Dashboard", "Home");
            // return RedirectToAction("Create", "MyDailyExpences");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var expensePlan = await _context.MyExpensePlans.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            ViewBag.ExpensePlan = expensePlan;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
