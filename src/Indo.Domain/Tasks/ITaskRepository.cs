using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Tasks
{
    public interface ITaskRepository : IRepository<Task, Guid>
    {
    }
}
