using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.VendorBillDetails
{
    public class VendorBillDetailCreateDto
    {

        [Required]
        public Guid VendorBillId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public Guid UomId { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public float TaxRate { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float DiscAmt { get; set; }
    }
}
