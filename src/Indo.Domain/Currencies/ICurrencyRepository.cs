﻿using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Currencies
{
    public interface ICurrencyRepository : IRepository<Currency, Guid>
    {
    }
}
