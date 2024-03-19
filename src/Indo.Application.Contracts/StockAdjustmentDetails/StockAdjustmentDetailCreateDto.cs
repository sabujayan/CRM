using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetailCreateDto
    {

        [Required]
        public Guid StockAdjustmentId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
