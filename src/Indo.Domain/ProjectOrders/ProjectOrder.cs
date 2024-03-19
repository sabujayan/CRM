using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ProjectOrders
{
    public class ProjectOrder : FullAuditedAggregateRoot<Guid>
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public ProjectOrderRating Rating { get; set; }
        public ProjectOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid SalesExecutiveId { get; set; }

        private ProjectOrder() { }
        internal ProjectOrder(
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
            Rating = ProjectOrderRating.Fivestar;
            Status = ProjectOrderStatus.Draft;
        }        
        internal ProjectOrder ChangeName([NotNull] string number)
        {
            SetName(number);
            return this;
        }
        private void SetName([NotNull] string number)
        {
            Number = Check.NotNullOrWhiteSpace(
                number,
                nameof(number),
                maxLength: ProjectOrderConsts.MaxNumberLength
                );
        }
    }
}
