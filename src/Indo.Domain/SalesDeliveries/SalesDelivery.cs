using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.SalesDeliveries
{
    public class SalesDelivery : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public SalesDeliveryStatus Status { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid SalesOrderId { get; set; }

        private SalesDelivery() { }
        internal SalesDelivery(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid salesOrderId,
            [NotNull] DateTime deliveryDate
            ) 
            : base(id)
        {
            SetName(number);
            SalesOrderId = salesOrderId;
            DeliveryDate = deliveryDate;
            Status = SalesDeliveryStatus.Draft;
        }        
        internal SalesDelivery ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: SalesDeliveryConsts.MaxNumberLength
                );
        }
    }
}
