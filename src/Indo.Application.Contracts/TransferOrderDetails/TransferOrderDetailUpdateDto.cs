using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.TransferOrderDetails
{
    public class TransferOrderDetailUpdateDto
    {

        [Required]
        public Guid TransferOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
