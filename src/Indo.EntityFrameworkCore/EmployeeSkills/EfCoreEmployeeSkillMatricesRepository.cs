using Indo.Contacts;
using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.EmployeeSkills
{
    public class EfCoreEmployeeSkillMatricesRepository
        : EfCoreRepository<IndoDbContext, EmployeeSkillMatrices, Guid>,
            IEmployeeSkillMatricesRepository
    {
        public EfCoreEmployeeSkillMatricesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
