using System;
using Volo.Abp.Application.Dtos;

namespace Indo.SalesDeliveries
{
    public class SalesDeliveryReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public SalesDeliveryStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
