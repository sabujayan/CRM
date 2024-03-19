using Indo.Clientes;
using Indo.Employees;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmployeeSkills
{
    public class EmployeeSkillMatrices : FullAuditedAggregateRoot<Guid>
    {
        public Guid SkillsId { get; set; }
        public Guid EmployeeId { get; set; }

        private EmployeeSkillMatrices() { }
        internal EmployeeSkillMatrices(
             Guid id,
             [NotNull] Guid employeeid,
             [NotNull] Guid skillsid
             )
             : base(id)
        {

            EmployeeId = employeeid;
            SkillsId = skillsid;
        }
    }
}
