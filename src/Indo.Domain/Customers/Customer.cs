using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Customers
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string RootFolder { get; set; }
        public CustomerStatus Status { get; set; }
        public Guid LeadSourceId { get; set; }
        public Guid LeadRatingId { get; set; }
        public CustomerStage Stage { get; set; }

        private Customer() { }
        internal Customer(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
            Status = CustomerStatus.Lead;
            Stage = CustomerStage.SalesQualified;
        }        
        internal Customer ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CustomerConsts.MaxNameLength
                );
        }
    }
}
