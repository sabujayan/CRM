using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ProjectEmployee
{
    public class EfCoreEmployeesProjectsMatricesRepository
   : EfCoreRepository<IndoDbContext, EmployeesProjectsMatrices, Guid>,
            IEmployeesProjectsMatricesRepository
    {
        public EfCoreEmployeesProjectsMatricesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
