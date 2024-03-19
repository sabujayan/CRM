using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.CustomerCreditNotes
{
    public class EfCoreCustomerCreditNoteRepository
        : EfCoreRepository<IndoDbContext, CustomerCreditNote, Guid>,
            ICustomerCreditNoteRepository
    {
        public EfCoreCustomerCreditNoteRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
