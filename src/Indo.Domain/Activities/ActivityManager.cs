using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Activities
{
    public class ActivityManager : DomainService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityManager(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
        public async Task<Activity> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _activityRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ActivityAlreadyExistsException(name);
            }

            return new Activity(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Activity activity,
           [NotNull] string newName)
        {
            Check.NotNull(activity, nameof(activity));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _activityRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != activity.Id)
            {
                throw new ActivityAlreadyExistsException(newName);
            }

            activity.ChangeName(newName);
        }
    }
}
