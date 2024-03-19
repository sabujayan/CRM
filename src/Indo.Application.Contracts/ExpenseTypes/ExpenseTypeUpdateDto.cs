using System.ComponentModel.DataAnnotations;


namespace Indo.ExpenseTypes
{
    public class ExpenseTypeUpdateDto
    {

        [Required]
        [StringLength(ExpenseTypeConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
