using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Warehouses
{
    public interface IWarehouseRepository : IRepository<Warehouse, Guid>
    {
    }
}
