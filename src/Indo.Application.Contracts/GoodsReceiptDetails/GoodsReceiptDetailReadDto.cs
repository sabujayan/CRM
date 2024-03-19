using Indo.GoodsReceipts;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid GoodsReceiptId { get; set; }
        public string ReceiptOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string TransferOrderNumber { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public GoodsReceiptStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
