using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ClientsProjects
{
    public class EfCoreClientsProjectsMatricesRepository
   : EfCoreRepository<IndoDbContext, ClientsProjectsMatrices, Guid>,
            IClientsProjectsMatricesRepository
    {
        public EfCoreClientsProjectsMatricesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
