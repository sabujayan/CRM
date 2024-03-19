using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Indo.Addresss
{
    public interface IAddressRepository : IRepository<AddressInfo, Guid>
    {
    }
}
