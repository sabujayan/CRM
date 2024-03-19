using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Todos
{
    public interface ITodoRepository : IRepository<Todo, Guid>
    {
    }
}
