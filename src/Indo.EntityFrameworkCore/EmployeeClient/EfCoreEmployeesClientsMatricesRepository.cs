using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.EmployeeClient
{
    public class EfCoreEmployeesClientsMatricesRepository
    : EfCoreRepository<IndoDbContext, EmployeesClientsMatrices, Guid>,
            IEmployeesClientsMatricesRepository
    {
        public EfCoreEmployeesClientsMatricesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
