using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Departments
{
    public class DepartmentReadDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
