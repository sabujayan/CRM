using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CashAndBanks
{
    public interface ICashAndBankRepository : IRepository<CashAndBank, Guid>
    {
    }
}
