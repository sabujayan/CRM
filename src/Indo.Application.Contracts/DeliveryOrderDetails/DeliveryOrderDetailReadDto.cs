using Indo.DeliveryOrders;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.DeliveryOrderDetails
{
    public class DeliveryOrderDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid DeliveryOrderId { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransferOrderNumber { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public DeliveryOrderStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
