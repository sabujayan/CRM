using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNoteCreateDto
    {

        [Required]
        [StringLength(CustomerCreditNoteConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public CustomerCreditNoteStatus Status { get; set; }

        [Required]
        public DateTime CreditNoteDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid CustomerInvoiceId { get; set; }
    }
}
