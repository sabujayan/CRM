using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.DeliveryOrders
{
    public class DeliveryOrderUpdateDto
    {

        [Required]
        [StringLength(DeliveryOrderConsts.MaxNumberLength)]
        public string Number { get; set; }

        [Required]
        public Guid TransferOrderId { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid FromWarehouseId { get; set; }

        [Required]
        public Guid ToWarehouseId { get; set; }
    }
}
