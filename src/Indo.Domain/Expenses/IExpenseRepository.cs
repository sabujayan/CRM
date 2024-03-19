using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Expenses
{
    public interface IExpenseRepository : IRepository<Expense, Guid>
    {
    }
}
