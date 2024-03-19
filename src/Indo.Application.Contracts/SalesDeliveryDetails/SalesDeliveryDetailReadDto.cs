using Indo.SalesDeliveries;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid SalesDeliveryId { get; set; }
        public string SalesDeliveryNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string SalesOrderNumber { get; set; }
        public string CustomerName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public SalesDeliveryStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
