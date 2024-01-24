using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    public class ExpenseMasterController : BaseController
    {
        private readonly AppDbContext _context;

        public ExpenseMasterController (AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var masters = await _context.ExpenseMaster.ToListAsync();
            return View(masters);
        }
        public async Task<IActionResult> Create(Guid id)
        {
            ExpensePlanMaster? ExpensePlan = null;
            if (!id.Equals(Guid.Empty))
                ExpensePlan = await _context.ExpenseMaster.FindAsync(id);
            return View(ExpensePlan);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExpensePlanMaster ExpensePlan)
        {
            if (ExpensePlan != null)
            {
                if (ExpensePlan.UniqueId.Equals(Guid.Empty))
                {
                    ExpensePlan.UniqueId = Guid.NewGuid();
                    ExpensePlan.CreatedDate = DateTime.UtcNow;
                    ExpensePlan.UserId = GetLoggedInUserId();
                    _context.Add(ExpensePlan);
                }
                else if (ExpensePlan.UserId.Equals(GetLoggedInUserId()))
                {
                    ExpensePlan.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(ExpensePlan);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ExpensePlan);


        }

    }
}
