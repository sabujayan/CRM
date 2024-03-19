using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid GoodsReceiptId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private GoodsReceiptDetail() { }
        internal GoodsReceiptDetail(
            Guid id,
            [NotNull] Guid goodsReceiptId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            GoodsReceiptId = goodsReceiptId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
