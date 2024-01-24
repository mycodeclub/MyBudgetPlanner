using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class ExpensePlanMaster
    {
        [Key]
        public Guid UniqueId { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public string ExpenseName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;

        
        [Display(Name = "Is Mandatory")]
        public bool IsMandatory { get; set; } = false;
        public DateTime CreatedDate { get; internal set; }
        public DateTime LastUpdatedDate { get; internal set; }
    }
}
