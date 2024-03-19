using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.TransferOrders
{
    public interface ITransferOrderRepository : IRepository<TransferOrder, Guid>
    {
    }
}
