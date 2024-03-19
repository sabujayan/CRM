using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ProjectEmployee
{
    public class EmployeesProjectsMatricesCreateDto : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectsId { get; set; }
    }
}
