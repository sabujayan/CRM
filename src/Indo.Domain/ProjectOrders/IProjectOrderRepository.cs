using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ProjectOrders
{
    public interface IProjectOrderRepository : IRepository<ProjectOrder, Guid>
    {
    }
}
