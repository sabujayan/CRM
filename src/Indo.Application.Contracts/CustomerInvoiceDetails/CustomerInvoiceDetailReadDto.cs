using Indo.CustomerInvoices;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.CustomerInvoiceDetails
{
    public class CustomerInvoiceDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid CustomerInvoiceId { get; set; }
        public string CustomerInvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public string CustomerName { get; set; }
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
        public CustomerInvoiceStatus Status { get; set; }
        public string StatusString { get; set; }
        public string Period { get; set; }
    }
}
