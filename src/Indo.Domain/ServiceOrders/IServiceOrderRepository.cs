using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceOrders
{
    public interface IServiceOrderRepository : IRepository<ServiceOrder, Guid>
    {
    }
}
