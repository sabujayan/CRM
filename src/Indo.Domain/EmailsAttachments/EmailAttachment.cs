using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmailsAttachments
{
    public class EmailAttachment : FullAuditedAggregateRoot<Guid>
    {
        public Guid TemplateId { get; set; }
        public string DocUrl { get; set; }
        private EmailAttachment() { }
        internal EmailAttachment(
             Guid id,
             [NotNull] Guid templateId
             )
             : base(id)
        {

            TemplateId = templateId;
        }
    }
}
