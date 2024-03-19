using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Technologies
{
    public class Technology : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        private Technology() { }
        internal Technology(
            Guid id,
            [NotNull] string name
            )
            : base(id)
        {
            SetName(name);
        }
        internal Technology ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: TechnologyConsts.MaxNameLength
                );
        }
    }
}
