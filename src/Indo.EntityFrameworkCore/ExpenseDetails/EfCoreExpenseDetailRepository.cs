using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ExpenseDetails
{
    public class EfCoreExpenseDetailRepository
        : EfCoreRepository<IndoDbContext, ExpenseDetail, Guid>,
            IExpenseDetailRepository
    {
        public EfCoreExpenseDetailRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
