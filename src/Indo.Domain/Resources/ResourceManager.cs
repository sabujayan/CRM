using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Resources
{
    public class ResourceManager : DomainService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceManager(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }
        public async Task<Resource> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _resourceRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ResourceAlreadyExistsException(name);
            }

            return new Resource(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Resource resource,
           [NotNull] string newName)
        {
            Check.NotNull(resource, nameof(resource));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _resourceRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != resource.Id)
            {
                throw new ResourceAlreadyExistsException(newName);
            }

            resource.ChangeName(newName);
        }
    }
}
