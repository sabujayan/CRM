using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Expenses
{
    public class ExpenseCreateDto
    {

        [Required]
        [StringLength(ExpenseConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid ExpenseTypeId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
