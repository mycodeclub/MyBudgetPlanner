using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    [Authorize]
    public class MyDailyExpencesController : BaseController
    {
        private readonly AppDbContext _context;

        public MyDailyExpencesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyDailyExpences
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MyExpenses.Include(m => m.User).Where(e => e.UserId == GetLoggedInUserId()); 
            return View(await appDbContext.ToListAsync());
        }

        // GET: MyDailyExpences/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpence = await _context.MyExpenses
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myDailyExpence == null)
            {
                return NotFound();
            }

            return View(myDailyExpence);
        }

        // GET: MyDailyExpences/Create
        public async Task<IActionResult> Create(Guid id)
        {
            MyDailyExpence? expence = await _context.MyExpenses.FindAsync(keyValues: id);
            expence ??= new MyDailyExpence()
            {
                ExpenceDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };
            expence.ExpenseTagIds = expence.ExpenseTags.Split(",").ToList();
            var MyExpensePlans = await _context.MyExpensePlans
                .Where(ep => ep.UserId == GetLoggedInUserId()).ToListAsync();
            ViewBag.MyExpensePlans = MyExpensePlans;
            return View(expence);
        }

        // POST: MyDailyExpences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MyDailyExpence myDailyExpence)
        {
            myDailyExpence.ExpenseTags = string.Join(",", myDailyExpence.ExpenseTagIds);

            if (ModelState.IsValid)
            {
                if (myDailyExpence.Equals(Guid.Empty))
                {
                    myDailyExpence.UniqueId = Guid.NewGuid();
                    myDailyExpence.CreatedDate = DateTime.Now;
                    _context.Add(myDailyExpence);
                }
                else
                {
                    myDailyExpence.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(myDailyExpence);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MyExpensePlans = await _context.MyExpensePlans
               .Where(ep => ep.UserId == GetLoggedInUserId()).ToListAsync();
            return View(myDailyExpence);
        }


        // GET: MyDailyExpences/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpence = await _context.MyExpenses
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myDailyExpence == null)
            {
                return NotFound();
            }

            return View(myDailyExpence);
        }

        // POST: MyDailyExpences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MyExpenses == null)
            {
                return Problem("Entity set 'AppDbContext.MyExpenses'  is null.");
            }
            var myDailyExpence = await _context.MyExpenses.FindAsync(id);
            if (myDailyExpence != null)
            {
                _context.MyExpenses.Remove(myDailyExpence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyDailyExpenceExists(Guid id)
        {
            return (_context.MyExpenses?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
    }
}
