using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.PurchaseOrderDetails
{
    public interface IPurchaseOrderDetailRepository : IRepository<PurchaseOrderDetail, Guid>
    {
    }
}
