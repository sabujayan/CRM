using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.LeadRatings
{
    public class LeadRatingManager : DomainService
    {
        private readonly ILeadRatingRepository _leadRatingRepository;

        public LeadRatingManager(ILeadRatingRepository leadRatingRepository)
        {
            _leadRatingRepository = leadRatingRepository;
        }
        public async Task<LeadRating> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _leadRatingRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new LeadRatingAlreadyExistsException(name);
            }

            return new LeadRating(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] LeadRating leadRating,
           [NotNull] string newName)
        {
            Check.NotNull(leadRating, nameof(leadRating));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _leadRatingRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != leadRating.Id)
            {
                throw new LeadRatingAlreadyExistsException(newName);
            }

            leadRating.ChangeName(newName);
        }
    }
}
