using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.VendorBills
{
    public interface IVendorBillRepository : IRepository<VendorBill, Guid>
    {
    }
}
