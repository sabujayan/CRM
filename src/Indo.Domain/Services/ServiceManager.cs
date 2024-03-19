using System;
using System.Linq;
using System.Threading.Tasks;
using Indo.Uoms;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Services
{
    public class ServiceManager : DomainService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUomRepository _uomRepository;

        public ServiceManager(
            IServiceRepository serviceRepository,
            IUomRepository uomRepository
            )
        {
            _serviceRepository = serviceRepository;
            _uomRepository = uomRepository;
        }
        public async Task<Service> CreateAsync(
            [NotNull] string name,
            [NotNull] Guid uomId)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull<Guid>(uomId, nameof(uomId));

            var existing = await _serviceRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new ServiceAlreadyExistsException(name);
            }

            return new Service(
                GuidGenerator.Create(),
                name,
                uomId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Service service,
           [NotNull] string newName)
        {
            Check.NotNull(service, nameof(service));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _serviceRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != service.Id)
            {
                throw new ServiceAlreadyExistsException(newName);
            }

            service.ChangeName(newName);
        }

        public async Task<string> GetServiceUomName(Service service)
        {
            var result = "";
            var uom = await _uomRepository.GetAsync(service.UomId);
            if (uom != null)
            {
                result = uom.Name;
            }
            return result;
        }
    }
}
