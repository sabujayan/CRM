using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.VendorBillDetails
{
    public class EfCoreVendorBillDetailRepository
        : EfCoreRepository<IndoDbContext, VendorBillDetail, Guid>,
            IVendorBillDetailRepository
    {
        public EfCoreVendorBillDetailRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
