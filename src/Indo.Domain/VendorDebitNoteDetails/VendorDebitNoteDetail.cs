using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.VendorDebitNoteDetails
{
    public class VendorDebitNoteDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid VendorDebitNoteId { get; set; }
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        public Guid UomId { get; set; }
        public float Price { get; set; }
        public float DiscAmt { get; set; }
        public float SubTotal { get; set; }
        public float BeforeTax { get; set; }
        public float TaxRate { get; set; }
        public float TaxAmount { get; set; }
        public float Total { get; set; }

        private VendorDebitNoteDetail() { }
        internal VendorDebitNoteDetail(
            Guid id,
            [NotNull] Guid vendorDebitNoteId,
            [NotNull] string productName,
            [NotNull] Guid uomId,
            [NotNull] float price,
            [NotNull] float taxRate,
            [NotNull] float quantity,
            [NotNull] float discAmt
            ) 
            : base(id)
        {
            VendorDebitNoteId = vendorDebitNoteId;
            ProductName = productName;
            UomId = uomId;
            Price = price;
            TaxRate = taxRate;
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
