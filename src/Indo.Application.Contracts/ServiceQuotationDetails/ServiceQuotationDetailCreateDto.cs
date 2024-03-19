using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationDetailCreateDto
    {

        [Required]
        public Guid ServiceQuotationId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
