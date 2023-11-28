using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBudgetPlanner.Models
{
    public class Income
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string IncomeSource { get; set; } = string.Empty;
        public string IncomeDiscription { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
        public byte Month { get; set; } = 0;
        public int Year { get; set; } = 0;
        // is a recurring
        public bool IsPerMonth { get; set; } = false;
        public string IncomeType { get { return IsPerMonth ? "Monthly" : "OneTime"; } }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

    }
}
