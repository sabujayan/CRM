using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid StockAdjustmentId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }

        private StockAdjustmentDetail() { }
        internal StockAdjustmentDetail(
            Guid id,
            [NotNull] Guid stockAdjustmentId,
            [NotNull] Guid productId,
            [NotNull] float quantity
            ) 
            : base(id)
        {
            StockAdjustmentId = stockAdjustmentId;
            ProductId = productId;
            Quantity = quantity;
        }  


    }
}
