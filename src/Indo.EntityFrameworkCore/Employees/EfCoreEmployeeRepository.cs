using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.Employees
{
    public class EfCoreEmployeeRepository
        : EfCoreRepository<IndoDbContext, Employee, Guid>,
            IEmployeeRepository
    {
        public EfCoreEmployeeRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        Task IEmployeeRepository.GetEmployeeLookupAsync()
        {
            throw new NotImplementedException();
        }
    }
}
