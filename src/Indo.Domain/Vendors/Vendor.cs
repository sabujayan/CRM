using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Vendors
{
    public class Vendor : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        private Vendor() { }
        internal Vendor(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
        }        
        internal Vendor ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: VendorConsts.MaxNameLength
                );
        }
    }
}
