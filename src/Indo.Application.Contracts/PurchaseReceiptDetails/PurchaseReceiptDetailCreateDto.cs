using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.PurchaseReceiptDetails
{
    public class PurchaseReceiptDetailCreateDto
    {

        [Required]
        public Guid PurchaseReceiptId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
