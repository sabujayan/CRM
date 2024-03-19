using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.SalesOrders
{
    public class SalesOrder : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public SalesOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SalesExecutiveId { get; set; }
        public string SourceDocument { get; set; }
        public Guid SourceDocumentId { get; set; }

        private SalesOrder() { }
        internal SalesOrder(
            Guid id,
            [NotNull] string number,
            [NotNull] Guid customerId,
            [NotNull] Guid salesExecutiveId,
            [NotNull] DateTime orderDate
            ) 
            : base(id)
        {
            SetName(number);
            CustomerId = customerId;
            SalesExecutiveId = salesExecutiveId;
            OrderDate = orderDate;
            Status = SalesOrderStatus.Draft;
        }        
        internal SalesOrder ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: SalesOrderConsts.MaxNumberLength
                );
        }
    }
}
