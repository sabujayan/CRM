using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.TransferOrders
{
    public class TransferOrderUpdateDto
    {

        [Required]
        [StringLength(TransferOrderConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid FromWarehouseId { get; set; }

        [Required]
        public Guid ToWarehouseId { get; set; }
    }
}
