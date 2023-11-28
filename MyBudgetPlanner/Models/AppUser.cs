using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{

    public class AppUser : IdentityUser
    {
        [NotMapped]
        public string Password { get; set; } = string.Empty;
        [NotMapped]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
    }
}
