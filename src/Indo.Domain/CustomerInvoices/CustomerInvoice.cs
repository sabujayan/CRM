using System;
using Indo.Currencies;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.CustomerInvoices
{
    public class CustomerInvoice : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }
        public CustomerInvoiceStatus Status { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public Guid CustomerId { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }

        private CustomerInvoice() { }
        internal CustomerInvoice(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] DateTime invoiceDate,
            [NotNull] DateTime invoiceDueDate
            ) 
            : base(id)
        {
            SetName(number);
            CustomerId = customerId;
            InvoiceDate = invoiceDate;
            InvoiceDueDate = invoiceDueDate;
            Status = CustomerInvoiceStatus.Draft;
        }        
        internal CustomerInvoice ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: CustomerInvoiceConsts.MaxNumberLength
                );
        }
    }
}
