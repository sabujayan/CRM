using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.PurchaseOrders
{
    public class PurchaseOrder : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public PurchaseOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid VendorId { get; set; }
        public Guid BuyerId { get; set; }

        private PurchaseOrder() { }
        internal PurchaseOrder(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid vendorId,
            [NotNull] Guid buyerId,
            [NotNull] DateTime orderDate
            ) 
            : base(id)
        {
            SetName(number);
            VendorId = vendorId;
            BuyerId = buyerId;
            OrderDate = orderDate;
            Status = PurchaseOrderStatus.Draft;
        }        
        internal PurchaseOrder ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: PurchaseOrderConsts.MaxNumberLength
                );
        }
    }
}
