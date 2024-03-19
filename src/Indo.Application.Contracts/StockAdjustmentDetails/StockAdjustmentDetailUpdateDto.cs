using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.StockAdjustmentDetails
{
    public class StockAdjustmentDetailUpdateDto
    {

        [Required]
        public Guid StockAdjustmentId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
