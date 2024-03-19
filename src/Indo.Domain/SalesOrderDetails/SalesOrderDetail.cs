using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.SalesOrderDetails
{
    public class SalesOrderDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid SalesOrderId { get; set; }
        public Guid ProductId { get; set; }
        public float Quantity { get; set; }
        public string UomName { get; set; }
        public float Price { get; set; }
        public float DiscAmt { get; set; }
        public float SubTotal { get; set; }
        public float BeforeTax { get; set; }
        public float TaxRate { get; set; }
        public float TaxAmount { get; set; }
        public float Total { get; set; }

        private SalesOrderDetail() { }
        internal SalesOrderDetail(
            Guid id,
            [NotNull] Guid salesOrderId,
            [NotNull] Guid productId,
            [NotNull] float quantity,
            [NotNull] float discAmt
            ) 
            : base(id)
        {
            SalesOrderId = salesOrderId;
            ProductId = productId;
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
