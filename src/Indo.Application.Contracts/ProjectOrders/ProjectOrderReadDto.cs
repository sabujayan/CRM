using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ProjectOrders
{
    public class ProjectOrderReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public ProjectOrderRating Rating { get; set; }
        public ProjectOrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZipCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public string SalesExecutiveName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
    }
}
