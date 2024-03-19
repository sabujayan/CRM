using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid PurchaseReceiptId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private PurchaseReceiptDetail() { }
        internal PurchaseReceiptDetail(
            Guid id,
            [NotNull] Guid purchaseReceiptId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            PurchaseReceiptId = purchaseReceiptId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
