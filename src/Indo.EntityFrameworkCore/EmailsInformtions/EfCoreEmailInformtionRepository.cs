using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.EmailsInformtions
{
    public class EfCoreEmailInformtionRepository
         : EfCoreRepository<IndoDbContext, EmailInformtion, Guid>,
             IEmailInformtionRepository
    {
        public EfCoreEmailInformtionRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
