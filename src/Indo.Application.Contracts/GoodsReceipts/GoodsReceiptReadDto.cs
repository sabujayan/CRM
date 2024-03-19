using System;
using Volo.Abp.Application.Dtos;

namespace Indo.GoodsReceipts
{
    public class GoodsReceiptReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public Guid DeliveryOrderId { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string Description { get; set; }
        public GoodsReceiptStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }
        public Guid ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }
    }
}
