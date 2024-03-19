using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.PurchaseReceipts
{
    public interface IPurchaseReceiptRepository : IRepository<PurchaseReceipt, Guid>
    {
    }
}
