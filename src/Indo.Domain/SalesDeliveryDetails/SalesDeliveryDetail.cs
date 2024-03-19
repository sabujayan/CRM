using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid SalesDeliveryId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private SalesDeliveryDetail() { }
        internal SalesDeliveryDetail(
            Guid id,
            [NotNull] Guid salesDeliveryId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            SalesDeliveryId = salesDeliveryId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
