using System;
using Indo.Currencies;
using Indo.NumberSequences;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.VendorBills
{
    public class VendorBill : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string TermCondition { get; set; }
        public string PaymentNote { get; set; }
        public VendorBillStatus Status { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime BillDueDate { get; set; }
        public Guid VendorId { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }
        public NumberSequenceModules SourceDocumentModule { get; set; }

        private VendorBill() { }
        internal VendorBill(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] DateTime billDate,
            [NotNull] DateTime billDueDate
            ) 
            : base(id)
        {
            SetName(number);
            VendorId = vendorId;
            BillDate = billDate;
            BillDueDate = billDueDate;
            Status = VendorBillStatus.Draft;
        }        
        internal VendorBill ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: VendorBillConsts.MaxNumberLength
                );
        }
    }
}
