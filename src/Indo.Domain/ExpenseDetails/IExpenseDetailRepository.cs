using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.ExpenseDetails
{
    public interface IExpenseDetailRepository : IRepository<ExpenseDetail, Guid>
    {
    }
}
