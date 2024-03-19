using System;
using Indo.Currencies;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.CustomerCreditNotes
{
    public class CustomerCreditNote : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public CustomerCreditNoteStatus Status { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CustomerInvoiceId { get; set; }

        private CustomerCreditNote() { }
        internal CustomerCreditNote(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] DateTime creditNoteDate,
            [NotNull] Guid customerInvoiceId
            ) 
            : base(id)
        {
            SetName(number);
            CustomerId = customerId;
            CreditNoteDate = creditNoteDate;
            CustomerInvoiceId = customerInvoiceId;
            Status = CustomerCreditNoteStatus.Draft;
        }        
        internal CustomerCreditNote ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: CustomerCreditNoteConsts.MaxNumberLength
                );
        }
    }
}
