using Microsoft.AspNetCore.Mvc;

namespace MyBudgetPlanner.Controllers
{
    public class PlannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
