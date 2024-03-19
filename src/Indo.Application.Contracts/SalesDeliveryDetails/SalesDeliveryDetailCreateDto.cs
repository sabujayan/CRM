using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.SalesDeliveryDetails
{
    public class SalesDeliveryDetailCreateDto
    {

        [Required]
        public Guid SalesDeliveryId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }
    }
}
