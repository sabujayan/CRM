using System;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.VendorPayments
{
    public class VendorPayment : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public DateTime PaymentDate { get; set; }
        public Guid CashAndBankId { get; set; }
        public Guid VendorId { get; set; }
        public float Amount { get; set; }
        public float Debit { get; set; }
        public float Credit { get; set; }
        public float Balance { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }
        public VendorPaymentStatus Status { get; set; }

        private VendorPayment() { }
        internal VendorPayment(
            Guid id,
            [NotNull] string number,
            [NotNull] DateTime paymentDate,
            [NotNull] Guid cashAndBankId,
            [NotNull] Guid customerId,
            [NotNull] float amount,
            [NotNull] string sourceDocument,
            [NotNull] Guid sourceDocumentId,
            [NotNull] NumberSequenceModules sourceDocumentModule
            ) 
            : base(id)
        {
            SetNumber(number);
            PaymentDate = paymentDate;
            CashAndBankId = cashAndBankId;
            VendorId = customerId;
            Amount = amount;
            SourceDocument = sourceDocument;
            SourceDocumentId = sourceDocumentId;
            SourceDocumentModule = sourceDocumentModule;
            Status = VendorPaymentStatus.Draft;

            if (sourceDocumentModule == NumberSequenceModules.Bill)
            {
                Debit = 0;
                Credit = Amount;
                Balance = Debit - Credit;
            }

            if (sourceDocumentModule == NumberSequenceModules.DebitNote)
            {
                Debit = Amount;
                Credit = 0;
                Balance = Debit - Credit;
            }
        }        
        internal VendorPayment ChangeNumber([NotNull] string number)
        {
            SetNumber(number);
            return this;
        }
        private void SetNumber([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: VendorPaymentConsts.MaxNumberLength
                );
        }
    }
}
