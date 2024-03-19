using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.CustomerPayments
{
    public class CustomerPaymentCreateDto
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

        [Required]
        public string SourceDocument{ get; set; }

        [Required]
        public Guid SourceDocumentId { get; set; }

        [Required]
        public NumberSequenceModules SourceDocumentModule { get; set; }
    }
}
