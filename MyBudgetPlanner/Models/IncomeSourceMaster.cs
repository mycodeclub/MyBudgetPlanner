﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyBudgetPlanner.Models
{
    public class IncomeSourceMaster
    {
        [Key]
        public Guid UniqueId { get; set; }

        [Display(Name = "Income Source")]
        public string IncomeSource { get; set; } = string.Empty;

        [Display(Name = "Income Discription")]
        public string? IncomeDiscription { get; set; } = string.Empty;
        public int Amount { get; set; } = 0;
       
        // is a recurring

        [Display(Name = "Is PerMonth")]
        public bool IsPerMonth { get; set; } = false;
        [NotMapped]
        public string IncomeType { get { return IsPerMonth ? "Monthly" : "OneTime"; } }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

    }
}
