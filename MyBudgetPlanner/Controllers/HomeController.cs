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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.ExpensePlan = await _context.MyExpensePlans.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            var income = await _context.Incomes.Where(predicate: p => p.UserId.Equals(GetLoggedInUserId()) && p.Year.Equals(DateTime.Now.Year) && p.Month.Equals(DateTime.Now.Month)).ToListAsync();
            ViewBag.Income = income;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
