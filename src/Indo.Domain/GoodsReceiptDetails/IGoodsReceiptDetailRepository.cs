using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.GoodsReceiptDetails
{
    public interface IGoodsReceiptDetailRepository : IRepository<GoodsReceiptDetail, Guid>
    {
    }
}
