using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Projectes
{
    public class EfCoreProjectsRepository
     : EfCoreRepository<IndoDbContext, Projects, Guid>,
            IProjectsRepository
    {
        public EfCoreProjectsRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<Projects> FindByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<Projects>> GetAllProjectsAsync(int SkipCount, int MaxResultCount, string sorting, string Filter = null, string nameFilter = null, string startdate = null, string enddate = null, string estimateFilter = null, string technologyFilter = null)
        {
            throw new NotImplementedException();
        }
    }
}
