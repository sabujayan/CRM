using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.DeliveryOrders
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder, Guid>
    {
    }
}
