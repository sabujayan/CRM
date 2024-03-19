using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Vendors
{
    public class VendorManager : DomainService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorManager(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }
        public async Task<Vendor> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _vendorRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new VendorAlreadyExistsException(name);
            }

            return new Vendor(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Vendor vendor,
           [NotNull] string newName)
        {
            Check.NotNull(vendor, nameof(vendor));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _vendorRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != vendor.Id)
            {
                throw new VendorAlreadyExistsException(newName);
            }

            vendor.ChangeName(newName);
        }
    }
}
