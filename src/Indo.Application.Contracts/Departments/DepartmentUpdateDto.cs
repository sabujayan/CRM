using System.ComponentModel.DataAnnotations;


namespace Indo.Departments
{
    public class DepartmentUpdateDto
    {

        [Required]
        [StringLength(DepartmentConsts.MaxNameLength)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
