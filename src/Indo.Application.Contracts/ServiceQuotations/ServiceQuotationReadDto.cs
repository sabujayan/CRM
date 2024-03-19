using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceQuotations
{
    public class ServiceQuotationReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public ServiceQuotationStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime QuotationValidUntilDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public ServiceQuotationPipeline Pipeline { get; set; }
        public string PipelineString { get; set; }
    }
}
