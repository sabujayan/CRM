using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.TransferOrderDetails
{
    public class TransferOrderDetailCreateDto
    {

        [Required]
        public Guid TransferOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
