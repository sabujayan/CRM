using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.Stocks
{
    public class StockCreateDto
    {
        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string SourceDocument { get; set; }

        [Required]
        public Guid MovementId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Qty { get; set; }

        [Required]
        public Guid WarehouseId { get; set; }

        [Required]
        public StockFlow Flow { get; set; }
    }
}
