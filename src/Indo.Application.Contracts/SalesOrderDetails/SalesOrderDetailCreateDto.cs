using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.SalesOrderDetails
{
    public class SalesOrderDetailCreateDto
    {

        [Required]
        public Guid SalesOrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
