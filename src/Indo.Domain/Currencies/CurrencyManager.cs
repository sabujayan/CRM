using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Currencies
{
    public class CurrencyManager : DomainService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyManager(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        public async Task<Currency> CreateAsync(
            [NotNull] string name,
            [NotNull] string symbol
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(symbol, nameof(symbol));

            var existing = await _currencyRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new CurrencyAlreadyExistsException(name);
            }

            return new Currency(
                GuidGenerator.Create(),
                name,
                symbol
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Currency currency,
           [NotNull] string newName)
        {
            Check.NotNull(currency, nameof(currency));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _currencyRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != currency.Id)
            {
                throw new CurrencyAlreadyExistsException(newName);
            }

            currency.ChangeName(newName);
        }
    }
}
