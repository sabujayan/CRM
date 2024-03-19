using System;
using Volo.Abp.Application.Dtos;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrderReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string CurrencyName { get; set; }
        public float Total { get; set; }
    }
}
