using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.StockAdjustmentDetails
{
    public interface IStockAdjustmentDetailRepository : IRepository<StockAdjustmentDetail, Guid>
    {
    }
}
