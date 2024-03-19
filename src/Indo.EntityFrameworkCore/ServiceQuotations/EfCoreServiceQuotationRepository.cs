using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ServiceQuotations
{
    public class EfCoreServiceQuotationRepository
        : EfCoreRepository<IndoDbContext, ServiceQuotation, Guid>,
            IServiceQuotationRepository
    {
        public EfCoreServiceQuotationRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
