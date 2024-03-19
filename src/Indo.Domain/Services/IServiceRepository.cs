using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Services
{
    public interface IServiceRepository : IRepository<Service, Guid>
    {
    }
}
