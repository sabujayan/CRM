using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.DeliveryOrderDetails
{
    public class DeliveryOrderDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid DeliveryOrderId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private DeliveryOrderDetail() { }
        internal DeliveryOrderDetail(
            Guid id,
            [NotNull] Guid deliveryOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            DeliveryOrderId = deliveryOrderId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
