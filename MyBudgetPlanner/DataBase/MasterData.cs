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

        public static List<ExpenseTag> Tags = new List<ExpenseTag>()
            {
                new() { TagName = "House Rent", Description = "living in different city" },
                new() { TagName = "Electricity Bill-SelfHome", Description = "Own Home" },
                new() { TagName = "Grocery", Description = "Home Grocery" },
                new() { TagName = "Transportation", Description = "Cab, Car / Bike Refueling" },
                new() { TagName = "Childs Education", Description = "School, Tution Fee & Kids Pocket money" },
                new() { TagName = "Savings", Description = "Savings" },
                new() { TagName = "Entertainment", Description = "Movie, Weekend Party, Fun activity ETC" },
        };
        //        public static List<MyExpensePlan> Expenses = Tags.Select(t => new MyExpensePlan() { ExpenseTags = t.TagName, Discription = t.Description, IsMandatory = true }).ToList();


    }
}
