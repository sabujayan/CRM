using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.PurchaseOrders
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder, Guid>
    {
    }
}
