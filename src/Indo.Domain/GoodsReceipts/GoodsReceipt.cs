using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.GoodsReceipts
{
    public class GoodsReceipt : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public Guid DeliveryOrderId { get; set; }
        public string Description { get; set; }
        public GoodsReceiptStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }

        private GoodsReceipt() { }
        internal GoodsReceipt(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid deliveryOrderId,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime orderDate
            ) 
            : base(id)
        {
            SetName(number);
            DeliveryOrderId = deliveryOrderId;
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            OrderDate = orderDate;
            Status = GoodsReceiptStatus.Draft;
        }        
        internal GoodsReceipt ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: GoodsReceiptConsts.MaxNumberLength
                );
        }
    }
}
