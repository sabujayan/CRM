using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.TransferOrders
{
    public class TransferOrder : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public TransferOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public Guid ToWarehouseId { get; set; }
        public Guid ReturnId { get; set; }
        public Guid OriginalId { get; set; }

        private TransferOrder() { }
        internal TransferOrder(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid fromWarehouseId,
            [NotNull] Guid toWarehouseId,
            [NotNull] DateTime orderDate
            ) 
            : base(id)
        {
            SetName(number);
            FromWarehouseId = fromWarehouseId;
            ToWarehouseId = toWarehouseId;
            OrderDate = orderDate;
            Status = TransferOrderStatus.Draft;
        }        
        internal TransferOrder ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: TransferOrderConsts.MaxNumberLength
                );
        }
    }
}
