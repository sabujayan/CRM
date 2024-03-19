using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.SalesQuotations
{
    public class SalesQuotationCreateDto
    {

        [Required]
        [StringLength(SalesQuotationConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime QuotationDate { get; set; }

        [Required]
        public DateTime QuotationValidUntilDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid SalesExecutiveId { get; set; }
    }
}
