using System;
using System.Diagnostics.CodeAnalysis;

using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.LeadsAddress
{
    public class LeadsAddressMatrix : FullAuditedAggregateRoot<Guid>
    {
        public Guid LeadsId { get; set; }
        public Guid AddressId { get; set; }
        public LeadsAddressMatrix() { }
        public LeadsAddressMatrix(
             Guid id,
             [NotNull] Guid leadsId,
             [NotNull] Guid addressId
             )
             : base(id)
        {

            LeadsId = leadsId;
            AddressId = addressId;
        }
    }
}
