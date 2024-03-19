using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.ServiceQuotations
{
    public class ServiceQuotationUpdateDto
    {

        [Required]
        [StringLength(ServiceQuotationConsts.MaxNumberLength)]
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
        public ServiceQuotationPipeline Pipeline { get; set; }
    }
}
