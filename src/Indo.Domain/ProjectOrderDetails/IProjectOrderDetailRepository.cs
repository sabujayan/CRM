using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ProjectOrderDetails
{
    public interface IProjectOrderDetailRepository : IRepository<ProjectOrderDetail, Guid>
    {
    }
}
