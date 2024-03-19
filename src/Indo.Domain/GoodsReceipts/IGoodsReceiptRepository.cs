using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.GoodsReceipts
{
    public interface IGoodsReceiptRepository : IRepository<GoodsReceipt, Guid>
    {
    }
}
