using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.PurchaseOrderDetails
{
    public class PurchaseOrderDetailCreateDto
    {

        [Required]
        public Guid PurchaseOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
