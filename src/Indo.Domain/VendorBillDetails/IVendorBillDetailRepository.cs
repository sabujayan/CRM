using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorBillDetails
{
    public interface IVendorBillDetailRepository : IRepository<VendorBillDetail, Guid>
    {
    }
}
