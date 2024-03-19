using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesQuotations
{
    public class SalesQuotationReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public SalesQuotationStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime QuotationValidUntilDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public SalesQuotationPipeline Pipeline { get; set; }
        public string PipelineString { get; set; }
    }
}
