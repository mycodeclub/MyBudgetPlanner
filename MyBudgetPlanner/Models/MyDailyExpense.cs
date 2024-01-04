using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class MyDailyExpense
    {
        [Key]
        public Guid UniqueId { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public Guid? ExpenseUnderPlanId { get; set; }
        [ForeignKey("ExpenseUnderPlanId")]
        public MyExpensePlan? ExpenseUnderPlan { get; set; }

        public string? Description { get; set; } = string.Empty;

        [Required]
        public int Amount { get; set; } = 0;

        public bool IsMandatory { get; set; } = false;

        public DateTime ExpenseDate { get; set; }

        [NotMapped]
        public int ExpenseDay { get { return ExpenseDate.Day; } }
        [NotMapped]
        public int ExpenseMonth { get { return ExpenseDate.Month; } }
        [NotMapped]
        public int ExpenseYear { get { return ExpenseDate.Year; } }


        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

    }
}
