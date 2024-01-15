using MyBudgetPlanner.Models;

namespace MyBudgetPlanner.DataBase
{
    public static class MasterData
    {
        public static List<Income> Incomes = new List<Income>() {

                new() { IncomeSource = "Salary / Paycheck", IsPerMonth = true },
                new() { IncomeSource = "Receiving Rent", IsPerMonth = true },
                new() { IncomeSource = "Bonus", IsPerMonth = false },
                new() { IncomeSource = "Sold Old Laptop", IsPerMonth = false }
            };

        public static List<ExpenseTagMaster> Tags = new List<ExpenseTagMaster>()
            {
                new() { TagName = "Survival", Description = "Basic survival costing" },
                new() { TagName = "Entertainment", Description = "Movie, Weekend Party, Holiday" },
                 new() { TagName = "Transportation", Description = "Cab, Car / Bike Refueling" },
         };
        //        public static List<MyExpensePlan> Expenses = Tags.Select(t => new MyExpensePlan() { ExpenseTagCommaList = t.TagName, Discription = t.Description, IsMandatory = true }).ToList();


    }
}
