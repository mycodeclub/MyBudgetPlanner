using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class MyDailyExpence
    {
        [Key]
        public Guid UniqueId { get; set; }

        public string ExpenseTagCommaList { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        [Required]
        public int Amount { get; set; } = 0;

        public bool IsMandatory { get; set; } = false;

        public DateTime ExpenceDate { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        [NotMapped]
        public List<string>? ExpenseTagIds { get; set; }
        [NotMapped]
        public string? ExpenseTagNames { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
