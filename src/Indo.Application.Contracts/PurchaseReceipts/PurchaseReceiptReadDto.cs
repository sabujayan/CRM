using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceiptReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public PurchaseReceiptStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime ReceiptDate { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
    }
}
