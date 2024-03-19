using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmailsInformtions
{
    public class EmailInformtion : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public Guid TemplateId { get; set; }
        public bool Status { get; set; }
        public bool IsSent { get; set; }
        public EmailInformtion(Guid id, [NotNull] Guid templateId, [NotNull] Guid employeeId)
        : base(id)
        {
            TemplateId = templateId;
            EmployeeId = employeeId;
        }
    }
}
