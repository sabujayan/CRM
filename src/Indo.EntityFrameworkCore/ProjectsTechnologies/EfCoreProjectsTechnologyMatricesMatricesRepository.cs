using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ProjectsTechnologies
{
   
    public class EfCoreProjectsTechnologyMatricesRepository
  : EfCoreRepository<IndoDbContext, ProjectsTechnologyMatrices, Guid>,
           IProjectsTechnologyMatricesRepository
    {
        public EfCoreProjectsTechnologyMatricesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
