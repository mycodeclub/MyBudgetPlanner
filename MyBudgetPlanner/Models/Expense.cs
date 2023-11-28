using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class Expense
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string ExpenseTags { get; set; } = string.Empty;
        public string Discription { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
        public byte Month { get; set; } = 0;
        public int Year { get; set; } = 0;

        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

    }
}
