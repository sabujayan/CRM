using System;
using Volo.Abp.Application.Dtos;

namespace Indo.TransferOrders
{
    public class TransferOrderReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public TransferOrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; }
        public Guid ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; }
        public Guid ReturnId { get; set; }
        public string ReturnNumber { get; set; }
        public Guid OriginalId { get; set; }
        public string OriginalNumber { get; set; }
    }
}
