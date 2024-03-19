using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Services
{
    public class Service : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public float TaxRate { get; set; }
        public Guid UomId { get; set; }

        private Service() { }
        internal Service(
            Guid id,
            [NotNull] string name,
            [NotNull] Guid uomId
            ) 
            : base(id)
        {
            SetName(name);
            UomId = uomId;
        }        
        internal Service ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ServiceConsts.MaxNameLength
                );
        }
    }
}
