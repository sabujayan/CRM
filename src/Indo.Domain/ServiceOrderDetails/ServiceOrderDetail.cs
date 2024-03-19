using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ServiceOrderDetails
{
    public class ServiceOrderDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid ServiceOrderId { get; set; }
        public Guid ServiceId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }
        public float Price { get; set; }
        public float DiscAmt { get; set; }
        public float SubTotal { get; set; }
        public float BeforeTax { get; set; }
        public float TaxRate { get; set; }
        public float TaxAmount { get; set; }
        public float Total { get; set; }

        private ServiceOrderDetail() { }
        internal ServiceOrderDetail(
            Guid id,
            [NotNull] Guid serviceOrderId,
            [NotNull] Guid serviceId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            ) 
            : base(id)
        {
            ServiceOrderId = serviceOrderId;
            ServiceId = serviceId;
            Quantity = quantity;
            DiscAmt = discAmt;
        }  

        public void Recalculate()
        {
            SubTotal = Quantity * Price;
            BeforeTax = SubTotal - DiscAmt;
            TaxAmount = (TaxRate / 100f) * BeforeTax;
            Total = BeforeTax + TaxAmount;
        }

    }
}
