using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Uoms
{
    public class UomManager : DomainService
    {
        private readonly IUomRepository _uomRepository;

        public UomManager(IUomRepository uomRepository)
        {
            _uomRepository = uomRepository;
        }
        public async Task<Uom> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _uomRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new UomAlreadyExistsException(name);
            }

            return new Uom(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Uom uom,
           [NotNull] string newName)
        {
            Check.NotNull(uom, nameof(uom));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _uomRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != uom.Id)
            {
                throw new UomAlreadyExistsException(newName);
            }

            uom.ChangeName(newName);
        }
    }
}
