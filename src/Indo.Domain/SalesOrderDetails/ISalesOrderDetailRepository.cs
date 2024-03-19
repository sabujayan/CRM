using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesOrderDetails
{
    public interface ISalesOrderDetailRepository : IRepository<SalesOrderDetail, Guid>
    {
    }
}
