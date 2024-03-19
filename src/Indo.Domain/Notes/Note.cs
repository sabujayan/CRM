using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Notes
{
    public class Note : FullAuditedAggregateRoot<Guid>
    {
        public string Description { get; set; }
        public Guid CustomerId { get; set; }

        private Note() { }
        internal Note(
            Guid id,
            [NotNull] string description,
            [NotNull] Guid customerId
            ) 
            : base(id)
        {
            Description = description;
            CustomerId = customerId;
        }    
    }
}
