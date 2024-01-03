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
    public class MyExpensePlansController : BaseController
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

            var myExpensePlan = await _context.MyExpensePlans.FirstOrDefaultAsync(m => m.UniqueId == id);
            if (myExpensePlan == null)
            {
                return NotFound();
            }

            return View(myExpensePlan);
        }



        // GET: MyExpensePlans/Create
        public async Task<IActionResult> Create(Guid? id)
        {
            MyExpensePlan plan;
            if (id == null)
            {
                plan = new MyExpensePlan()
                {
                    IsMandatory = true,
                    CreatedDate = DateTime.UtcNow
                };
            }
            else
            {
                string? LoggedInUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                plan = await _context.MyExpensePlans.Where(p => p.UniqueId.Equals(id) && p.UserId.Equals(LoggedInUserId)).FirstOrDefaultAsync();
            }
            return View(plan);
        }

        // POST: MyExpensePlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MyExpensePlan myExpensePlan)
        {
            if (ModelState.IsValid)
            {
                if (myExpensePlan.UniqueId.Equals(Guid.Empty)) // create 
                {
                    myExpensePlan.UniqueId = Guid.NewGuid();
                    myExpensePlan.CreatedDate = DateTime.UtcNow;
                    myExpensePlan.UserId = GetLoggedInUserId();
                    _context.Add(myExpensePlan);
                }
                else if (myExpensePlan.UserId == GetLoggedInUserId()) // update 
                {
                    myExpensePlan.LastUpdatedDate = DateTime.UtcNow;
                    _context.Update(myExpensePlan);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
