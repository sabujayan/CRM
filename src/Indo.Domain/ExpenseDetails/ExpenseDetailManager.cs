using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ExpenseDetails
{
    public class ExpenseDetailManager : DomainService
    {
        private readonly IExpenseDetailRepository _expenseDetailRepository;

        public ExpenseDetailManager(IExpenseDetailRepository expenseDetailRepository)
        {
            _expenseDetailRepository = expenseDetailRepository;
        }
        public async Task<ExpenseDetail> CreateAsync(
            [NotNull] Guid expenseId,
            [NotNull] string summaryNote,
            [NotNull] float price
            )
        {
            Check.NotNull<Guid>(expenseId, nameof(expenseId));
            Check.NotNull<string>(summaryNote, nameof(summaryNote));
            Check.NotNull<float>(price, nameof(price));

            await Task.Yield();

            var expenseDetail = new ExpenseDetail(
                GuidGenerator.Create(),
                expenseId,
                summaryNote,
                price
            );


            return expenseDetail;
        }
    }
}
