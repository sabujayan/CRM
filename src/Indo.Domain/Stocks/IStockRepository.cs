using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Stocks
{
    public interface IStockRepository : IRepository<Stock, Guid>
    {
    }
}
