using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
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
            var appDbContext = _context.MyExpenses.Include(m => m.User);
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
        public async Task<IActionResult> Create()
        {
            var loggedInUserId = GetLoggedInUserId();
            var myExpPlans = await _context.MyExpensePlans.Where(ep => ep.UserId == loggedInUserId).ToListAsync();
            ViewData["MyExpensePlans"] = myExpPlans;

            ViewData["UserId"] = new MultiSelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: MyDailyExpences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,ExpenseTags,Discription,Amount,Dateday,Month,Year,IsMandatory,UserId,CreatedDate,LastUpdatedDate")] MyDailyExpence myDailyExpence)
        {
            if (ModelState.IsValid)
            {
                myDailyExpence.UniqueId = Guid.NewGuid();
                _context.Add(myDailyExpence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myDailyExpence.UserId);
            return View(myDailyExpence);
        }

        // GET: MyDailyExpences/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.MyExpenses == null)
            {
                return NotFound();
            }

            var myDailyExpence = await _context.MyExpenses.FindAsync(id);
            if (myDailyExpence == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myDailyExpence.UserId);
            return View(myDailyExpence);
        }

        // POST: MyDailyExpences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UniqueId,ExpenseTags,Discription,Amount,Dateday,Month,Year,IsMandatory,UserId,CreatedDate,LastUpdatedDate")] MyDailyExpence myDailyExpence)
        {
            if (id != myDailyExpence.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myDailyExpence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyDailyExpenceExists(myDailyExpence.UniqueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myDailyExpence.UserId);
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
