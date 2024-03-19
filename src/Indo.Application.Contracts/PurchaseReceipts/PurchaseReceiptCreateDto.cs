using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceiptCreateDto
    {

        [Required]
        [StringLength(PurchaseReceiptConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; }

        [Required]
        public Guid PurchaseOrderId { get; set; }
    }
}
