using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.EmailsTemplates
{
    public class EfCoreEmailTemplatesRepository
    : EfCoreRepository<IndoDbContext, EmailTemplates, Guid>,
          IEmailTemplatesRepository
    {
        public EfCoreEmailTemplatesRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
