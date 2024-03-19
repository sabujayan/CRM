using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Resources
{
    public interface IResourceRepository : IRepository<Resource, Guid>
    {
    }
}
