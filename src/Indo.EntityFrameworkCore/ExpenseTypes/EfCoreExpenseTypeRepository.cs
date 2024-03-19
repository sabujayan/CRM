using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ExpenseTypes
{
    public class EfCoreExpenseTypeRepository
        : EfCoreRepository<IndoDbContext, ExpenseType, Guid>,
            IExpenseTypeRepository
    {
        public EfCoreExpenseTypeRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
