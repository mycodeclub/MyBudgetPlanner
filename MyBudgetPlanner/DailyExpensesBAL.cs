using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.DataBase;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner
{
    /// <summary>
    /// Invalid access of _loggedInUserId, not populating from constructor , need to find some other process ....
    /// </summary>
    public class DailyExpensesBAL
    {
        private readonly AppDbContext _context;
        private readonly string _loggedInUserId;

        public DailyExpensesBAL(AppDbContext context, string loggedInUserId)
        {
            _context = context;
            _loggedInUserId = loggedInUserId;
        }

        public async Task<List<MyDailyExpense>> GetDailyExpenses()
        {
            return await _context.MyDailyExpenses.Where(e => e.UserId.Equals(_loggedInUserId)).ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<int, MyDailyExpense>>> GetDailyExpensesGroupByYear()
        {
            return (await GetDailyExpenses()).GroupBy(e => e.ExpenseYear);
        }
        public async Task<IEnumerable<MyDailyExpense>> GetDailyExpenses(int year)
        {
            return await _context.MyDailyExpenses.Where(e => e.UserId.Equals(_loggedInUserId) && e.ExpenseDate.Year.Equals(year)).ToListAsync();
        }
        public async Task<IEnumerable<MyDailyExpense>> GetDailyExpenses(int year, int month)
        {
            return await _context.MyDailyExpenses.Where(e => e.UserId.Equals(_loggedInUserId) && e.ExpenseDate.Year.Equals(year) && e.ExpenseDate.Month.Equals(month)).ToListAsync();
        } 
    }
}
