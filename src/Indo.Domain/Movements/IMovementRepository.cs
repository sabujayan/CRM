using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Movements
{
    public interface IMovementRepository : IRepository<Movement, Guid>
    {
    }
}
