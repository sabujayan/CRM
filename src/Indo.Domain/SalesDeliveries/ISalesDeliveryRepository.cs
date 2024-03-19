using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesDeliveries
{
    public interface ISalesDeliveryRepository : IRepository<SalesDelivery, Guid>
    {
    }
}
