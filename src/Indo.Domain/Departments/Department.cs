using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Departments
{
    public class Department : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private Department() { }
        internal Department(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
        }        
        internal Department ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: DepartmentConsts.MaxNameLength
                );
        }
    }
}
