using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.DeliveryOrders
{
    public class EfCoreDeliveryOrderRepository
        : EfCoreRepository<IndoDbContext, DeliveryOrder, Guid>,
            IDeliveryOrderRepository
    {
        public EfCoreDeliveryOrderRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
