using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.LeadSources
{
    public class LeadSourceManager : DomainService
    {
        private readonly ILeadSourceRepository _leadSourceRepository;

        public LeadSourceManager(ILeadSourceRepository leadSourceRepository)
        {
            _leadSourceRepository = leadSourceRepository;
        }
        public async Task<LeadSource> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _leadSourceRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new LeadSourceAlreadyExistsException(name);
            }

            return new LeadSource(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] LeadSource leadSource,
           [NotNull] string newName)
        {
            Check.NotNull(leadSource, nameof(leadSource));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _leadSourceRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != leadSource.Id)
            {
                throw new LeadSourceAlreadyExistsException(newName);
            }

            leadSource.ChangeName(newName);
        }
    }
}
