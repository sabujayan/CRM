using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.Employees
{
    public class EmployeeUpdateDto
    {
        [Required]
        [StringLength(EmployeeConsts.MaxNameLength)]
        public string Name { get; set; }
        public string EmployeeNumber { get; set; }
        public string Position { get; set; }
        public Guid DepartmentId { get; set; }
        public EmployeeGroup EmployeeGroup { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Bandwidth { get; set; }
        public string skilllist { get; set; }
        public string clientnamelist { get; set; }
        public string projectnamelist { get; set; }
    }
}
