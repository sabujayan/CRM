using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Indo.Employees
{
    public class EmployeeReadDto : AuditedEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeNumber { get; set; }
        public EmployeeGroup EmployeeGroup { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Bandwidth { get; set; }
        public string SkillList { get; set; }
        public List<string> EmployeeSkillId { get; set; }
        public string ClientNameList { get; set; }
        public List<string> EmployeeClientId { get; set; }
        public string ProjectNameList { get; set; }
        public List<string> EmployeeProjectId { get; set; }

    }

}
