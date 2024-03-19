using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.GoodsReceipts
{
    public class GoodsReceiptCreateDto
    {

        [Required]
        [StringLength(GoodsReceiptConsts.MaxNumberLength)]
        public string Number { get; set; }

        [Required]
        public Guid DeliveryOrderId { get; set; }
        public string Description { get; set; }

        [Required]
        public GoodsReceiptStatus Status { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid FromWarehouseId { get; set; }

        [Required]
        public Guid ToWarehouseId { get; set; }
    }
}
