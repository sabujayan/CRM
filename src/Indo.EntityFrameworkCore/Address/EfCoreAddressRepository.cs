using Indo.Addresss;
using Indo.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Address
{
    public class EfCoreAddressRepository : EfCoreRepository<IndoDbContext, AddressInfo, Guid>, IAddressRepository
    {
        public EfCoreAddressRepository(IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
