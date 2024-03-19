using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.TransferOrderDetails
{
    public interface ITransferOrderDetailRepository : IRepository<TransferOrderDetail, Guid>
    {
    }
}
