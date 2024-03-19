using System;
using Volo.Abp.Application.Dtos;

namespace Indo.DeliveryOrders
{
    public class DeliveryOrderReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public Guid TransferOrderId { get; set; }
        public string TransferOrderNumber { get; set; }
        public string Description { get; set; }
        public DeliveryOrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }
        public Guid ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }
    }
}
