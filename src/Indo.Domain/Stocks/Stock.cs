using System;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.Stocks
{
    public class Stock : FullAuditedAggregateRoot<Guid>
    {
        public DateTime TransactionDate { get; set; }
        public string SourceDocument { get; set; }
        public Guid MovementId { get; set; }
        public Guid ProductId { get; set; }
        public float Qty { get; set; }
        public Guid WarehouseId { get; set; }
        public StockFlow Flow { get; set; }

        private Stock() { }
        internal Stock(
            Guid id,
            [NotNull] Guid movementId,
            [NotNull] Guid warehouseId,
            [NotNull] DateTime transactionDate,
            [NotNull] string sourceDocument,
            [NotNull] StockFlow flow,
            [NotNull] Guid productId,
            [NotNull] float qty
            ) 
            : base(id)
        {
            MovementId = movementId;
            WarehouseId = warehouseId;
            TransactionDate = transactionDate;
            SourceDocument = sourceDocument;
            Flow = flow;
            ProductId = productId;
            Qty = qty;
        }        
    }
}
