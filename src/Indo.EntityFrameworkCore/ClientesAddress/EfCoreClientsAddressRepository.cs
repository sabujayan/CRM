﻿using Indo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Indo.ClientesAddress
{
    public class EfCoreClientsAddressRepository
    : EfCoreRepository<IndoDbContext, ClientsAddress, Guid>,
            IClientsAddressRepository
    {
        public EfCoreClientsAddressRepository(
            IDbContextProvider<IndoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}