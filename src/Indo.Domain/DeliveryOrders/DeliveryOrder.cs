using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.DeliveryOrders
{
    public class DeliveryOrder : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public Guid TransferOrderId { get; set; }
        public string Description { get; set; }
        public DeliveryOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }

        private DeliveryOrder() { }
        internal DeliveryOrder(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid transferOrderId,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime orderDate
            ) 
            : base(id)
        {
            SetName(number);
            TransferOrderId = transferOrderId;
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            OrderDate = orderDate;
            Status = DeliveryOrderStatus.Draft;
        }        
        internal DeliveryOrder ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: DeliveryOrderConsts.MaxNumberLength
                );
        }
    }
}
