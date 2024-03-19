using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.LeadSources
{
    public class LeadSource : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private LeadSource() { }
        internal LeadSource(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
        }        
        internal LeadSource ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: LeadSourceConsts.MaxNameLength
                );
        }
    }
}
