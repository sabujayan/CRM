using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.DeliveryOrderDetails
{
    public interface IDeliveryOrderDetailRepository : IRepository<DeliveryOrderDetail, Guid>
    {
    }
}
