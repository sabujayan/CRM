using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.CashAndBanks
{
    public class CashAndBankManager : DomainService
    {
        private readonly ICashAndBankRepository _cashAndBankRepository;

        public CashAndBankManager(ICashAndBankRepository cashAndBankRepository)
        {
            _cashAndBankRepository = cashAndBankRepository;
        }
        public async Task<CashAndBank> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _cashAndBankRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new CashAndBankAlreadyExistsException(name);
            }

            return new CashAndBank(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] CashAndBank cashAndBank,
           [NotNull] string newName)
        {
            Check.NotNull(cashAndBank, nameof(cashAndBank));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _cashAndBankRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != cashAndBank.Id)
            {
                throw new CashAndBankAlreadyExistsException(newName);
            }

            cashAndBank.ChangeName(newName);
        }
    }
}
