using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    public class MyExpensePlansController : Controller
    {
        private readonly AppDbContext _context;

        public MyExpensePlansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyExpensePlans
        public async Task<IActionResult> Index()
        {
            string? LoggedInUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; 
            if (string.IsNullOrWhiteSpace(LoggedInUserId))
                return NotFound();
            var appDbContext = _context.MyExpensePlans.Where(p => p.UserId.Equals(LoggedInUserId));
            return View(await appDbContext.ToListAsync());
        }

        // GET: MyExpensePlans/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MyExpensePlans == null)
            {
                return NotFound();
            }

            var myExpensePlan = await _context.MyExpensePlans
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myExpensePlan == null)
            {
                return NotFound();
            }

            return View(myExpensePlan);
        }

        // GET: MyExpensePlans/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: MyExpensePlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UniqueId,UserId,ExpenseTags,Discription,Amount,IsMandatory,CreatedDate,LastUpdatedDate")] MyExpensePlan myExpensePlan)
        {
            if (ModelState.IsValid)
            {
                myExpensePlan.UniqueId = Guid.NewGuid();
                _context.Add(myExpensePlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myExpensePlan.UserId);
            return View(myExpensePlan);
        }

        // GET: MyExpensePlans/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.MyExpensePlans == null)
            {
                return NotFound();
            }

            var myExpensePlan = await _context.MyExpensePlans.FindAsync(id);
            if (myExpensePlan == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myExpensePlan.UserId);
            return View(myExpensePlan);
        }

        // POST: MyExpensePlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UniqueId,UserId,ExpenseTags,Discription,Amount,IsMandatory,CreatedDate,LastUpdatedDate")] MyExpensePlan myExpensePlan)
        {
            if (id != myExpensePlan.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myExpensePlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyExpensePlanExists(myExpensePlan.UniqueId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", myExpensePlan.UserId);
            return View(myExpensePlan);
        }

        // GET: MyExpensePlans/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MyExpensePlans == null)
            {
                return NotFound();
            }

            var myExpensePlan = await _context.MyExpensePlans
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myExpensePlan == null)
            {
                return NotFound();
            }

            return View(myExpensePlan);
        }

        // POST: MyExpensePlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MyExpensePlans == null)
            {
                return Problem("Entity set 'AppDbContext.MyExpensePlans'  is null.");
            }
            var myExpensePlan = await _context.MyExpensePlans.FindAsync(id);
            if (myExpensePlan != null)
            {
                _context.MyExpensePlans.Remove(myExpensePlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyExpensePlanExists(Guid id)
        {
            return (_context.MyExpensePlans?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
    }
}
