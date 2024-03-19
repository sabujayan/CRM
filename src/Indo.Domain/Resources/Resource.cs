using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Resources
{
    public class Resource : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private Resource() { }
        internal Resource(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
        }        
        internal Resource ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ResourceConsts.MaxNameLength
                );
        }
    }
}
