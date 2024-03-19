using Indo.EntityFrameworkCore;
using Indo.LeadsAddress;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.LeadAddress
{
    public class EfCoreLeadAddressMatrixRepository : EfCoreRepository<IndoDbContext, LeadsAddressMatrix, Guid>, ILeadAddressMatrixRepository
    {
        public EfCoreLeadAddressMatrixRepository(IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
