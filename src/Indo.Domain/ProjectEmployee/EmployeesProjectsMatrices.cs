using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ProjectEmployee
{
    public class EmployeesProjectsMatrices : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectsId { get; set; }
        private EmployeesProjectsMatrices() { }
        internal EmployeesProjectsMatrices(
             Guid id,
             [NotNull] Guid employeeid,
             [NotNull] Guid projectsid
             )
             : base(id)
        {

            EmployeeId = employeeid;
            ProjectsId = projectsid;
        }
    }
}
