using Indo.NumberSequences;
using System;
using System.ComponentModel.DataAnnotations;


namespace Indo.CustomerInvoices
{
    public class CustomerInvoiceUpdateDto
    {

        [Required]
        [StringLength(CustomerInvoiceConsts.MaxNumberLength)]
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public DateTime InvoiceDueDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
    }
}
