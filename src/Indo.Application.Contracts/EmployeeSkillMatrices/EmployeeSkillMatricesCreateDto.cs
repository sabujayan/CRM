using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmployeeSkillMatrices
{
    public class EmployeeSkillMatricesCreateDto: FullAuditedAggregateRoot<Guid>
    {
        public Guid SkillsId { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
