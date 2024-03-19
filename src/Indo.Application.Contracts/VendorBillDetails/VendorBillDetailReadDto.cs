using Indo.VendorBills;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.VendorBillDetails
{
    public class VendorBillDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid VendorBillId { get; set; }
        public string VendorBillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime BillDueDate { get; set; }
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
        public VendorBillStatus Status { get; set; }
        public string StatusString { get; set; }
        public string Period { get; set; }
    }
}
