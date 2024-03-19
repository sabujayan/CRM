using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ImportantDates
{
    public class ImportantDateManager : DomainService
    {
        private readonly IImportantDateRepository _importantDateRepository;

        public ImportantDateManager(IImportantDateRepository importantDateRepository)
        {
            _importantDateRepository = importantDateRepository;
        }
        public async Task<ImportantDate> CreateAsync(
            [NotNull] string name,
            [NotNull] DateTime startTime,
            [NotNull] DateTime endTime,
            [NotNull] Guid customerId
            )
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(startTime, nameof(startTime));
            Check.NotNull(endTime, nameof(endTime));
            Check.NotNull(customerId, nameof(customerId));

            var existing = await _importantDateRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ImportantDateAlreadyExistsException(name);
            }

            return new ImportantDate(
                GuidGenerator.Create(),
                name,
                startTime,
                endTime,
                customerId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] ImportantDate importantDate,
           [NotNull] string newName)
        {
            Check.NotNull(importantDate, nameof(importantDate));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _importantDateRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != importantDate.Id)
            {
                throw new ImportantDateAlreadyExistsException(newName);
            }

            importantDate.ChangeName(newName);
        }
    }
}
