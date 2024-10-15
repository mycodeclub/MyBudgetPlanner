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
        private static readonly string[] RainbowColors = { "#FF0000", "#FF7F00", "#FFFF00", "#00FF00", "#0000FF", "#4B0082", "#8A2BE2" };
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        private async Task UserLogin()
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync("mr.ankitsahai@gmail.com", "mr.ankitsahai@gmail.com", true, lockoutOnFailure: false);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;

            }
        }
        public async Task<IActionResult> Index()
        {
            await UserLogin();
            return RedirectToAction(  "Index", "MyDailyExpenses");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.ExpensePlan = await _context.MyExpensePlans.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            ViewBag.monthlyIncome = await _context.Incomes.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            var income = await _context.Incomes.Where(predicate: p => p.UserId.Equals(GetLoggedInUserId()) && p.Year.Equals(DateTime.Now.Year) && p.Month.Equals(DateTime.Now.Month)).ToListAsync();
            ViewBag.Income = income;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<ActionResult> ChartData()
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

            var planColor =  "rgb(0, 0, 255)"; // Blue color for plan
            var expenseColor ="rgb(255, 0, 0)"; // Red color for expense

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
                backgroundColor = planColor, // Blue color for plan
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
                backgroundColor = expenseColor, // Red color for expense
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

        public static string[] GenerateRandomColors(int count)
        {
            var random = new Random();
            var colors = new List<string>();

            for (var i = 0; i < count; i++)
            {
                if (i < RainbowColors.Length)
                {
                    colors.Add(RainbowColors[i]);
                }
                else
                {
                    // If more than 7 items, combine two random colors
                    var color1 = RainbowColors[random.Next(0, RainbowColors.Length)];
                    var color2 = RainbowColors[random.Next(0, RainbowColors.Length)];

                    // Ensure the same color isn't picked twice
                    while (color2 == color1)
                    {
                        color2 = RainbowColors[random.Next(0, RainbowColors.Length)];
                    }

                    // Blend colors by taking the average of their RGB values
                    var blendedColor = BlendColors(color1, color2);
                    colors.Add(blendedColor);
                }
            }

            return colors.ToArray();
        }
        private static string BlendColors(string color1, string color2)
        {
            // Convert hex colors to RGB
            var r1 = Convert.ToInt32(color1.Substring(1, 2), 16);
            var g1 = Convert.ToInt32(color1.Substring(3, 2), 16);
            var b1 = Convert.ToInt32(color1.Substring(5, 2), 16);

            var r2 = Convert.ToInt32(color2.Substring(1, 2), 16);
            var g2 = Convert.ToInt32(color2.Substring(3, 2), 16);
            var b2 = Convert.ToInt32(color2.Substring(5, 2), 16);

            // Blend colors by taking the average of their RGB values
            var blendedR = (r1 + r2) / 2;
            var blendedG = (g1 + g2) / 2;
            var blendedB = (b1 + b2) / 2;

            // Convert blended RGB values back to hex
            var blendedColor = $"#{blendedR:X2}{blendedG:X2}{blendedB:X2}";

            return blendedColor;
        }
    }

}
 
