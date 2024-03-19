using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Uoms
{
    public interface IUomRepository : IRepository<Uom, Guid>
    {
    }
}
