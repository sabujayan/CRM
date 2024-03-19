using System;
using Volo.Abp.Application.Dtos;

namespace Indo.StockAdjustments
{
    public class StockAdjustmentReadDto : AuditedEntityDto<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public StockAdjustmentStatus Status { get; set; }
        public string StatusString { get; set; }
        public StockAdjustmentType AdjustmentType { get; set; }
        public string AdjustmentTypeString { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
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
