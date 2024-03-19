using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ProjectOrderDetails
{
    public class EfCoreProjectOrderDetailRepository
        : EfCoreRepository<IndoDbContext, ProjectOrderDetail, Guid>,
            IProjectOrderDetailRepository
    {
        public EfCoreProjectOrderDetailRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
