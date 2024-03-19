using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.ExpenseDetails
{
    public class ExpenseDetailCreateDto
    {

        [Required]
        public Guid ExpenseId { get; set; }

        [Required]
        public string SummaryNote { get; set; }

        [Required]
        public float Price { get; set; }
    }
}
