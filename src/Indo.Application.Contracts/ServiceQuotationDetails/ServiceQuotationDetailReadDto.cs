using Indo.ServiceQuotations;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid ServiceQuotationId { get; set; }
        public string ServiceQuotationNumber { get; set; }
        public DateTime QuotationDate { get; set; }
        public string CustomerName { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
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
        public ServiceQuotationStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
