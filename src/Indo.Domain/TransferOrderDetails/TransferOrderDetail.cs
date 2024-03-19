using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.TransferOrderDetails
{
    public class TransferOrderDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid TransferOrderId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private TransferOrderDetail() { }
        internal TransferOrderDetail(
            Guid id,
            [NotNull] Guid transferOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            TransferOrderId = transferOrderId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
