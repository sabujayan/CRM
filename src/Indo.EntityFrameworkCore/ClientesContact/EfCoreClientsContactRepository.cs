using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ClientesContact
{
    public class EfCoreClientsContactRepository
   : EfCoreRepository<IndoDbContext, ClientsContact, Guid>,
            IClientsContactRepository
    {
        public EfCoreClientsContactRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
