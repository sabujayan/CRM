using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid ServiceQuotationId { get; set; }
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

        private ServiceQuotationDetail() { }
        internal ServiceQuotationDetail(
            Guid id,
            [NotNull] Guid serviceQuotationId,
            [NotNull] Guid serviceId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            ) 
            : base(id)
        {
            ServiceQuotationId = serviceQuotationId;
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
