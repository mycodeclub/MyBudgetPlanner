using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;
using System.Diagnostics;
using System.Linq;

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
        public async Task<ActionResult> GetChartData()
        {
            var expensePlan = await _context.MyExpensePlans.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            var DailyExpense = await _context.MyDailyExpenses.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            var income = await _context.Incomes.Where(predicate: p => p.UserId.Equals(GetLoggedInUserId()) && p.Year.Equals(DateTime.Now.Year) && p.Month.Equals(DateTime.Now.Month)).ToListAsync();
            var expenseAmount = expensePlan.Select(d => d.Amount).ToList();
            var expenseName = expensePlan.Select(d => d.ExpenseName).ToList();
            var UniqueItemId = expensePlan.Select(d => d.UniqueId).ToList();
            var sumByUniqueId = _context.MyDailyExpenses
             .Where(item => UniqueItemId.Contains((Guid)item.ExpenseUnderPlanId))
             .GroupBy(item => item.ExpenseUnderPlanId)
             .Select(group => new
             {
                 ExpenseUnderPlanId = group.Key,
                 SumAmount = group.Sum(item => item.Amount)
             })
             .ToList();
            var expenseColors = GenerateRandomColors(expenseName.Count);
            var planData = expenseAmount.ToArray();
            var expenseData = sumByUniqueId.Select(d => d.SumAmount).ToArray();

            var chartData = new
            {
                labels = expenseName.ToArray(),
                datasets = new[]
                {
                new
                {
                    label = "plan",
                    backgroundColor = expenseColors,
                    borderColor = "rgba(75, 192, 192, 1)",
                    hoverBackgroundColor = "rgba(75, 192, 192, 0.4)",
                    hoverBorderColor = "rgba(75, 192, 192, 1)",
                    data = planData,
                    barPercentage = 0.75,
                    categoryPercentage = 0.5
                },
                new
                {
                    label = "Expense",
                    backgroundColor =expenseColors,
                    borderColor = "#dee2e6",
                    hoverBackgroundColor = "#dee2e6",
                    hoverBorderColor = "#dee2e6",
                    data = expenseData,
                    barPercentage = 0.75,
                    categoryPercentage = 0.5
                }
            }
            };

            return Json(chartData);
        }
 
        private string[] GenerateRandomColors(int count)
        {
            var random = new Random();
            var colors = new List<string>();

            for (var i = 0; i < count; i++)
            {
                var color = string.Format("#{0:X6}", random.Next(0x1000000));
                colors.Add(color);
            }

            return colors.ToArray();
        }

    }
}
