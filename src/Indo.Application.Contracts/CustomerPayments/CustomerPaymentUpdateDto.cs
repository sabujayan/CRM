using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.CustomerPayments
{
    public class CustomerPaymentUpdateDto
    {

        [Required]
        [StringLength(CustomerPaymentConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public Guid CashAndBankId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public float Amount { get; set; }
    }
}
