using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.VendorPayments
{
    public class VendorPaymentUpdateDto
    {

        [Required]
        [StringLength(VendorPaymentConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public Guid CashAndBankId { get; set; }

        [Required]
        public Guid VendorId { get; set; }

        [Required]
        public float Amount { get; set; }
    }
}
