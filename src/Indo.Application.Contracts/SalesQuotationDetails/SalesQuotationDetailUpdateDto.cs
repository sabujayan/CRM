using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.SalesQuotationDetails
{
    public class SalesQuotationDetailUpdateDto
    {

        [Required]
        public Guid SalesQuotationId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
