using Indo.StockAdjustments;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetailReadDto : AuditedEntityDto<Guid>
    {
        public Guid StockAdjustmentId { get; set; }
        public string StockAdjustmentNumber { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string WarehouseName { get; set; }
        public StockAdjustmentType AdjustmentType { get; set; }
        public string AdjustmentTypeString { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string UomName { get; set; }
        public float Quantity { get; set; }
        public StockAdjustmentStatus Status { get; set; }
        public string StatusString { get; set; }
    }
}
