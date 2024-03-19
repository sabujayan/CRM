using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Vendors
{
    public interface IVendorRepository : IRepository<Vendor, Guid>
    {
    }
}
