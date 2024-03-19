using Indo.Clientes;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Leads
{
    public class EfCoreLeadsRepository : EfCoreRepository<IndoDbContext, LeadsInfo, Guid>, ILeadsRepository
    {
        public EfCoreLeadsRepository(IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<List<LeadsInfo>> GetAllLeadsWithAddressAsync()
        {
            throw new NotImplementedException();
        }
    }
}
