using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.GoodsReceiptDetails
{
    public class GoodsReceiptDetailUpdateDto
    {

        [Required]
        public Guid GoodsReceiptId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
