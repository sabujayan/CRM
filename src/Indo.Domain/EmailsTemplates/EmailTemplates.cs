using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.EmailsTemplates
{
    public class EmailTemplates : FullAuditedAggregateRoot<Guid>
    {
        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
        private EmailTemplates() { }

        internal EmailTemplates(
          Guid id,
          [NotNull] string templateName
          )
          : base(id)
        {
            SetName(templateName);
        }
        internal EmailTemplates ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            TemplateName = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: EmailTemplatesConsts.MaxNameLength
                );
        }
    }
}
