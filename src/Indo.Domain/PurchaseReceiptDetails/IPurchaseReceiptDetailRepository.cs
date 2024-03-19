using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.PurchaseReceiptDetails
{
    public interface IPurchaseReceiptDetailRepository : IRepository<PurchaseReceiptDetail, Guid>
    {
    }
}
