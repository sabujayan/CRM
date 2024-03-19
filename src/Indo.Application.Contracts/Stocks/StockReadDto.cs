using Indo.NumberSequences;
using System;
using Volo.Abp.Application.Dtos;

namespace Indo.Stocks
{
    public class StockReadDto : AuditedEntityDto<Guid>
    {
        public DateTime TransactionDate { get; set; }
        public string SourceDocument { get; set; }
        public Guid MovementId { get; set; }
        public string MovementNumber { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public float UnitPrice { get; set; }
        public string UomName { get; set; }
        public float Qty { get; set; }
        public float Valuation { get; set; }
        public Guid WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public StockFlow Flow { get; set; }
    }
}
