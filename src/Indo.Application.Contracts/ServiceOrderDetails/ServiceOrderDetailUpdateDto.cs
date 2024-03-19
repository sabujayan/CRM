using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.ServiceOrderDetails
{
    public class ServiceOrderDetailUpdateDto
    {

        [Required]
        public Guid ServiceOrderId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
