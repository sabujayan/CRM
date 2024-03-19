using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Warehouses
{
    public class Warehouse : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultConfig { get; set; }
        public bool Virtual { get; set; }

        private Warehouse() { }
        internal Warehouse(
            Guid id,
            [NotNull] string name
            ) 
            : base(id)
        {
            SetName(name);
            Virtual = false;
            DefaultConfig = false;
        }        
        internal Warehouse ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: WarehouseConsts.MaxNameLength
                );
        }
    }
}
