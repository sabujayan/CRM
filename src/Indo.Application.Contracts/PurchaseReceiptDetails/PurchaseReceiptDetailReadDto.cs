using Indo.PurchaseReceipts;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid PurchaseReceiptId { get; set; }
        public string PurchaseReceiptNumber { get; set; }
        public DateTime PurchaseReceiptDate { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string VendorName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public PurchaseReceiptStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
