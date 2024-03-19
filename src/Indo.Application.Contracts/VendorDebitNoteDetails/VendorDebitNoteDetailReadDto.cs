using Indo.VendorDebitNotes;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorDebitNoteDetails
{
    public class VendorDebitNoteDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid VendorDebitNoteId { get; set; }
        public string VendorDebitNoteNumber { get; set; }
        public DateTime DebitNoteDate { get; set; }
        public DateTime DebitNoteDueDate { get; set; }
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public Guid UomId { get; set; }
        public string UomName { get; set; }
        public float Price { get; set; }
        public string PriceString { get; set; }
        public string CurrencyName { get; set; }
        public float Quantity { get; set; }
        public float SubTotal { get; set; }
        public string SubTotalString { get; set; }
        public float DiscAmt { get; set; }
        public string DiscAmtString { get; set; }
        public float BeforeTax { get; set; }
        public string BeforeTaxString { get; set; }
        public float TaxRate { get; set; }
        public float TaxAmount { get; set; }
        public string TaxAmountString { get; set; }
        public float Total { get; set; }
        public string TotalString { get; set; }
        public VendorDebitNoteStatus Status { get; set; }
        public string StatusString { get; set; }
        public string Period { get; set; }
    }
}
