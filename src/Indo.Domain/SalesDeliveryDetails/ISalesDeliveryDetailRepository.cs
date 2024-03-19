using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesDeliveryDetails
{
    public interface ISalesDeliveryDetailRepository : IRepository<SalesDeliveryDetail, Guid>
    {
    }
}
