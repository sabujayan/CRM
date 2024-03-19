using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.StockAdjustments
{
    public interface IStockAdjustmentRepository : IRepository<StockAdjustment, Guid>
    {
    }
}
