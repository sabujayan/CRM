using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ExpenseTypes
{
    public class ExpenseType : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private ExpenseType() { }
        internal ExpenseType(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
        }        
        internal ExpenseType ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ExpenseTypeConsts.MaxNameLength
                );
        }
    }
}
