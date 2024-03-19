using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Companies
{
    public class Company : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid DefaultWarehouseId { get; set; }

        private Company() { }
        internal Company(
            Guid id,
            [NotNull] string name,
            [NotNull] Guid currencyId,
            [NotNull] Guid defaultWarehouseId
            ) 
            : base(id)
        {
            SetName(name);
            CurrencyId = currencyId;
            DefaultWarehouseId = defaultWarehouseId;
        }        
        internal Company ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: CompanyConsts.MaxNameLength
                );
        }
    }
}
