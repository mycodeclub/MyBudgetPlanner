using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    [Authorize]
    public class MyDailyExpensesController : BaseController
    {
        private readonly AppDbContext _context;
        // private readonly DailyExpensesBAL _dailyExpensesBAL;
        public MyDailyExpensesController(AppDbContext context)
        {
            _context = context;
            //    _dailyExpensesBAL = new DailyExpensesBAL(context, GetLoggedInUserId());
        }

        // GET: MyDailyExpenses
        public async Task<IActionResult> Index()
        {
            var myDailyExpenses = await _context.MyDailyExpenses.Include(e => e.ExpenseUnderPlan).Where(e => e.UserId == GetLoggedInUserId()).GroupBy(e => e.ExpenseUnderPlan.ExpenseName).ToListAsync();
            return View(myDailyExpenses);
        }

        // GET: MyDailyExpenses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MyDailyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpense = await _context.MyDailyExpenses
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myDailyExpense == null)
            {
                return NotFound();
            }

            return View(myDailyExpense);
        }

        // GET: MyDailyExpenses/Create
        public async Task<IActionResult> Create(Guid id)
        {
            MyDailyExpense? Expense = await _context.MyDailyExpenses.FindAsync(keyValues: id);
            Expense ??= new MyDailyExpense()
            {
                ExpenseDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };
            var MyExpensePlans = await _context.MyExpensePlans
               .Where(ep => ep.UserId == GetLoggedInUserId()).ToListAsync();
            ViewBag.MyExpensePlans = MyExpensePlans;
            return View(Expense);
        }

        // POST: MyDailyExpenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MyDailyExpense myDailyExpense)
        {

            if (ModelState.IsValid)
            {
                if (myDailyExpense.UniqueId.Equals(Guid.Empty))
                {
                    myDailyExpense.UniqueId = Guid.NewGuid();
                    myDailyExpense.CreatedDate = DateTime.Now;
                    myDailyExpense.UserId = GetLoggedInUserId();
                    _context.Add(myDailyExpense);
                }
                else if (myDailyExpense.UserId == GetLoggedInUserId())
                {
                    myDailyExpense.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(myDailyExpense);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MyExpensePlans = await _context.MyExpensePlans
               .Where(ep => ep.UserId == GetLoggedInUserId()).ToListAsync();
            return View(myDailyExpense);
        }


        // GET: MyDailyExpenses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MyDailyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpense = await _context.MyDailyExpenses
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myDailyExpense == null)
            {
                return NotFound();
            }

            return View(myDailyExpense);
        }

        // POST: MyDailyExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MyDailyExpenses == null)
            {
                return Problem("Entity set 'AppDbContext.MyDailyExpenses'  is null.");
            }
            var myDailyExpense = await _context.MyDailyExpenses.FindAsync(id);
            if (myDailyExpense != null)
            {
                _context.MyDailyExpenses.Remove(myDailyExpense);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyDailyExpenseExists(Guid id)
        {
            return (_context.MyDailyExpenses?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
    }
}
