﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBudgetPlanner.Models
{
    public class ExpenseTag
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public Guid? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

    }
}
