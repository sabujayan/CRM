using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrderCreateDto
    {

        [Required]
        [StringLength(PurchaseOrderConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required]
        public Guid BuyerId { get; set; }
    }
}
