using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ExpenseTypes
{
    public interface IExpenseTypeRepository : IRepository<ExpenseType, Guid>
    {
    }
}
