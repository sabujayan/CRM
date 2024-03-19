using System;
using Indo.Currencies;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Indo.ProjectOrderDetails
{
    public class ProjectOrderDetail : FullAuditedAggregateRoot<Guid>
    {
        public Guid ProjectOrderId { get; set; }
        public string ProjectTask { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }

        private ProjectOrderDetail() { }
        internal ProjectOrderDetail(
            Guid id,
            [NotNull] Guid projectOrderId,
            [NotNull] string projectTask,
            [NotNull] float quantity,
            [NotNull] float price
            ) 
            : base(id)
        {
            ProjectOrderId = projectOrderId;
            ProjectTask = projectTask;
            Quantity = quantity;
            Price = price;
        }  

        public void Recalculate()
        {
            Total = Quantity * Price;
        }
    }
}
