using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ServiceOrders
{
    public class ServiceOrderReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public ServiceOrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
    }
}
