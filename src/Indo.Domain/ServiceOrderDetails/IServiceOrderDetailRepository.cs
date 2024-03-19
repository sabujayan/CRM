using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceOrderDetails
{
    public interface IServiceOrderDetailRepository : IRepository<ServiceOrderDetail, Guid>
    {
    }
}
