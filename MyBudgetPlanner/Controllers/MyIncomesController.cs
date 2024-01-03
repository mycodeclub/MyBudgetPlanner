using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.Controllers
{
    public class MyIncomesController : Controller
    {
        private readonly AppDbContext _context;

        public MyIncomesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MyIncomes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Incomes.Include(i => i.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: MyIncomes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // GET: MyIncomes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: MyIncomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Income income)
        {
            if (income!=null)
            {
                income.UniqueId = Guid.NewGuid();
                _context.Add(income);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            return View(income);
        }

        // GET: MyIncomes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            return View(income);
        }

        // POST: MyIncomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UniqueId,IncomeSource,IncomeDiscription,Amount,Month,Year,IsPerMonth,UserId")] Income income)
        {
            if (id != income.UniqueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(income);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.UniqueId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            return View(income);
        }

        // GET: MyIncomes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.UniqueId == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: MyIncomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Incomes == null)
            {
                return Problem("Entity set 'AppDbContext.Incomes'  is null.");
            }
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeExists(Guid id)
        {
          return (_context.Incomes?.Any(e => e.UniqueId == id)).GetValueOrDefault();
        }
    }
}
