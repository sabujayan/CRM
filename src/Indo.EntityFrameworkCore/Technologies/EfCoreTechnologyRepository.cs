using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;


namespace Indo.Technologies
{
    public class EfCoreTechnologyRepository
        : EfCoreRepository<IndoDbContext, Technology, Guid>,
            ITechnologyRepository
    {
        public EfCoreTechnologyRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
