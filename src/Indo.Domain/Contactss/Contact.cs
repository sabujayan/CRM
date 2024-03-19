using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Contacts
{
    public class Contact : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid CustomerId { get; set; }

        private Contact() { }
        internal Contact(
            Guid id,
            [NotNull] string name,
            [NotNull] Guid customerId
            ) 
            : base(id)
        {
            SetName(name);
            CustomerId = customerId;
        }        
        internal Contact ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContactConsts.MaxNameLength
                );
        }
    }
}
