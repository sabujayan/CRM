using Indo.Customers;
using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Skills
{
    public class EfCoreSkillRepository : EfCoreRepository<IndoDbContext, Skill, Guid>,
            ISkillRepository
    {
        public EfCoreSkillRepository(
           IDbContextProvider<IndoDbContext> dbContextProvider)
           : base(dbContextProvider)
        {
        }
    }
}
