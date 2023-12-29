using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var myDailyExpences = await _context.MyDailyExpenses.Where(e => e.UserId == GetLoggedInUserId()).ToListAsync();
            var plan = await _context.MyExpensePlans.Where(p => p.UserId.Equals(GetLoggedInUserId())).ToListAsync();
            foreach (var exp in myDailyExpences)
            {
                var ex = plan.Where(e => exp.ExpenseTagCommaList.Split(',').Contains(e.UniqueId.ToString())).ToList();
                exp.ExpenseTagNames = string.Join(Environment.NewLine, ex.Select(item => $"{item.Description}"));
                if (exp.ExpenseTagNames.Length > 20)
                    exp.ExpenseTagNames = exp.ExpenseTagNames.Substring(0, 20) + "...";
                // string.Join(Environment.NewLine, ex.Select(item => $"{item.Description}: {item.ExpenseName}"));  
            }
            return View(myDailyExpences);
        }

        // GET: MyDailyExpences/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MyDailyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpence = await _context.MyDailyExpenses
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
            MyDailyExpence? expence = await _context.MyDailyExpenses.FindAsync(keyValues: id);
            expence ??= new MyDailyExpence()
            {
                ExpenceDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };
            expence.ExpenseTagIds = expence.ExpenseTagCommaList.Split(",").ToList();
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
            myDailyExpence.ExpenseTagCommaList = string.Join(",", myDailyExpence.ExpenseTagIds);

            if (ModelState.IsValid)
            {
                if (myDailyExpence.UniqueId.Equals(Guid.Empty))
                {
                    myDailyExpence.UniqueId = Guid.NewGuid();
                    myDailyExpence.CreatedDate = DateTime.Now;
                    myDailyExpence.UserId = GetLoggedInUserId();
                    _context.Add(myDailyExpence);
                }
                else if (myDailyExpence.UserId == GetLoggedInUserId())
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
            if (id == null || _context.MyDailyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpence = await _context.MyDailyExpenses
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
            if (_context.MyDailyExpenses == null)
            {
                return Problem("Entity set 'AppDbContext.MyDailyExpenses'  is null.");
            }
            var myDailyExpence = await _context.MyDailyExpenses.FindAsync(id);
            if (myDailyExpence != null)
            {
                _context.MyDailyExpenses.Remove(myDailyExpence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyDailyExpenceExists(Guid id)
        {
            return (_context.MyDailyExpenses?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
    }
}
