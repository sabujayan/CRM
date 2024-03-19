using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ExpenseTypes
{
    public class ExpenseTypeManager : DomainService
    {
        private readonly IExpenseTypeRepository _expenseTypeRepository;

        public ExpenseTypeManager(IExpenseTypeRepository expenseTypeRepository)
        {
            _expenseTypeRepository = expenseTypeRepository;
        }
        public async Task<ExpenseType> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _expenseTypeRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ExpenseTypeAlreadyExistsException(name);
            }

            return new ExpenseType(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] ExpenseType expenseType,
           [NotNull] string newName)
        {
            Check.NotNull(expenseType, nameof(expenseType));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _expenseTypeRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != expenseType.Id)
            {
                throw new ExpenseTypeAlreadyExistsException(newName);
            }

            expenseType.ChangeName(newName);
        }
    }
}
