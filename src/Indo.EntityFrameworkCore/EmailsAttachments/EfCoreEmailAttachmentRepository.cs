using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.EmailsAttachments
{
    public class EfCoreEmailAttachmentRepository
         : EfCoreRepository<IndoDbContext, EmailAttachment, Guid>,
             IEmailAttachmentRepository
    {
        public EfCoreEmailAttachmentRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
