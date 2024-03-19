using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.PurchaseReceipts
{
    public class PurchaseReceipt : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public PurchaseReceiptStatus Status { get; set; }
        public DateTime ReceiptDate { get; set; }
        public Guid PurchaseOrderId { get; set; }

        private PurchaseReceipt() { }
        internal PurchaseReceipt(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid purchaseOrderId,
            [NotNull] DateTime receiptDate
            ) 
            : base(id)
        {
            SetName(number);
            PurchaseOrderId = purchaseOrderId;
            ReceiptDate = receiptDate;
            Status = PurchaseReceiptStatus.Draft;
        }        
        internal PurchaseReceipt ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: PurchaseReceiptConsts.MaxNumberLength
                );
        }
    }
}
