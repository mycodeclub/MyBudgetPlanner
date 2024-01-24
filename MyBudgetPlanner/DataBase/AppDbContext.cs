using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.DataBase
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedRoles();

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<AppUser>().ToTable("ApplicationUser");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //       optionsBuilder.UseSqlServer(AppSettings.Instance.ConnectionString);
            }
        }


        public DbSet<Income> Incomes { get; set; }
        public DbSet<ExpenseTagMaster> ExpenseTagMaster { get; set; }
        public DbSet<MyExpensePlan> MyExpensePlans { get; set; }
        public DbSet<MyDailyExpense> MyDailyExpenses { get; set; }
        public DbSet<IncomeSourceMaster> IncomeMaster{ get; set; }
        public DbSet<ExpensePlanMaster> ExpenseMaster { get; set; }
    }
}
