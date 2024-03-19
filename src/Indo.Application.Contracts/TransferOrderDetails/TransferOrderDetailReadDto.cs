using Indo.TransferOrders;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.TransferOrderDetails
{
    public class TransferOrderDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid TransferOrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public TransferOrderStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
