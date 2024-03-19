using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.ServiceOrders
{
    public class ServiceOrderUpdateDto
    {

        [Required]
        [StringLength(ServiceOrderConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid SalesExecutiveId { get; set; }
    }
}
