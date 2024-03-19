using System;
using Indo.Currencies;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.VendorDebitNotes
{
    public class VendorDebitNote : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string PaymentNote { get; set; }
        public VendorDebitNoteStatus Status { get; set; }
        public DateTime DebitNoteDate { get; set; }
        public Guid VendorId { get; set; }
        public Guid VendorBillId { get; set; }

        private VendorDebitNote() { }
        internal VendorDebitNote(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] DateTime debitNoteDate,
            [NotNull] Guid vendorBillId
            ) 
            : base(id)
        {
            SetName(number);
            VendorId = vendorId;
            DebitNoteDate = debitNoteDate;
            VendorBillId = vendorBillId;
            Status = VendorDebitNoteStatus.Draft;
        }        
        internal VendorDebitNote ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: VendorDebitNoteConsts.MaxNumberLength
                );
        }
    }
}
