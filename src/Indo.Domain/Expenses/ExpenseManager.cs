using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Expenses
{
    public class ExpenseManager : DomainService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseManager(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }
        public async Task<Expense> CreateAsync(
            [NotNull] string number,
            [NotNull] DateTime expenseDate,
            [NotNull] Guid employeeId,
            [NotNull] Guid expenseTypeId,
            [NotNull] Guid customerId
            )
        {
            Check.NotNullOrWhiteSpace(number, nameof(number));
            Check.NotNull(expenseDate, nameof(expenseDate));
            Check.NotNull(employeeId, nameof(employeeId));
            Check.NotNull(expenseTypeId, nameof(expenseTypeId));
            Check.NotNull(customerId, nameof(customerId));

            var existing = await _expenseRepository.FindAsync(x => x.Number.Equals(number));
            if (existing != null)
            {
                throw new ExpenseAlreadyExistsException(number);
            }

            return new Expense(
                GuidGenerator.Create(),
                number,
                expenseDate,
                employeeId,
                expenseTypeId,
                customerId
            );
        }
        public async Task ChangeNumberAsync(
           [NotNull] Expense expense,
           [NotNull] string newNumber)
        {
            Check.NotNull(expense, nameof(expense));
            Check.NotNullOrWhiteSpace(newNumber, nameof(newNumber));

            var existing = await _expenseRepository.FindAsync(x => x.Number.Equals(newNumber));
            if (existing != null && existing.Id != expense.Id)
            {
                throw new ExpenseAlreadyExistsException(newNumber);
            }

            expense.ChangeNumber(newNumber);
        }
    }
}
