using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.SalesDeliveries
{
    public class SalesDeliveryUpdateDto
    {

        [Required]
        [StringLength(SalesDeliveryConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public Guid SalesOrderId { get; set; }
    }
}
