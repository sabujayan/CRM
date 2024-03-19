using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesOrders
{
    public interface ISalesOrderRepository : IRepository<SalesOrder, Guid>
    {
    }
}
