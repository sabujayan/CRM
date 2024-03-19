using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Notes
{
    public interface INoteRepository : IRepository<Note, Guid>
    {
    }
}
