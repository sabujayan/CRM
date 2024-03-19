using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.StockAdjustmentDetails
{
    public class EfCoreStockAdjustmentDetailRepository
        : EfCoreRepository<IndoDbContext, StockAdjustmentDetail, Guid>,
            IStockAdjustmentDetailRepository
    {
        public EfCoreStockAdjustmentDetailRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
