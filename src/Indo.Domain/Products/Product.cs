using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Products
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public float TaxRate { get; set; }
        public float RetailPrice { get; set; }
        public Guid UomId { get; set; }

        private Product() { }
        internal Product(
            Guid id,
            [NotNull] string name,
            [NotNull] Guid uomId
            ) 
            : base(id)
        {
            SetName(name);
            UomId = uomId;
        }        
        internal Product ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProductConsts.MaxNameLength
                );
        }
    }
}
