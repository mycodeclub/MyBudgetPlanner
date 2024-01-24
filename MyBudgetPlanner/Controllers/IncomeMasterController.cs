
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    public class IncomeMasterController : BaseController
    {
        private readonly AppDbContext _context;

        public IncomeMasterController (AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.IncomeMaster.ToListAsync());
        
        }

        public async Task<IActionResult> Create(Guid id)
        {
            IncomeSourceMaster? IncomeSource = null;
            if (!id.Equals(Guid.Empty))
                IncomeSource = await _context.IncomeMaster.FindAsync(id);
            return View(IncomeSource);
        }
        [HttpPost]
        public async Task<IActionResult> Create(IncomeSourceMaster IncomeSource)
        {
            if (IncomeSource != null)
            {
                if (IncomeSource.UniqueId.Equals(Guid.Empty))
                {
                    IncomeSource.UniqueId = Guid.NewGuid();
                    IncomeSource.CreatedDate = DateTime.UtcNow;
                    IncomeSource.UserId = GetLoggedInUserId();
                    _context.Add(IncomeSource);
                }

                else if (IncomeSource.UserId.Equals(GetLoggedInUserId()))
                {
                    IncomeSource.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(IncomeSource);
                }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
            }
            return View(IncomeSource);


        }


    }
}