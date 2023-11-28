using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class MyExpensePlan
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public string ExpenseTags { get; set; } = string.Empty;
        public string Discription { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
        public bool IsMandatory { get; set; } = false;
        public DateTime CreatedDate { get; internal set; }
        public DateTime LastUpdatedDate { get; internal set; }
    }
}
