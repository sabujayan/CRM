using Indo.ProjectOrders;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid ProjectOrderId { get; set; }
        public string ProjectOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string ProjectTask { get; set; }
        public float Price { get; set; }
        public string PriceString { get; set; }
        public string CurrencyName { get; set; }
        public float Quantity { get; set; }
        public float Total { get; set; }
        public string TotalString { get; set; }
        public ProjectOrderStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
