using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Warehouses
{
    public class WarehouseManager : DomainService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseManager(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        public async Task<Warehouse> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _warehouseRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new WarehouseAlreadyExistsException(name);
            }

            return new Warehouse(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Warehouse warehouse,
           [NotNull] string newName)
        {
            Check.NotNull(warehouse, nameof(warehouse));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _warehouseRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != warehouse.Id)
            {
                throw new WarehouseAlreadyExistsException(newName);
            }

            warehouse.ChangeName(newName);
        }

        public async Task<Warehouse> GetDefaultWarehouseAsync()
        {
            await Task.Yield();

            return _warehouseRepository.Where(x => x.Virtual.Equals(false)).FirstOrDefault();
        }
        public async Task<Warehouse> GetDefaultWarehouseVendorAsync()
        {
            await Task.Yield();

            return _warehouseRepository.Where(x => x.Virtual.Equals(true) && x.Name.Equals("Vendor")).FirstOrDefault();
        }
        public async Task<Warehouse> GetDefaultWarehouseCustomerAsync()
        {
            await Task.Yield();

            return _warehouseRepository.Where(x => x.Virtual.Equals(true) && x.Name.Equals("Customer")).FirstOrDefault();
        }
        public async Task<Warehouse> GetDefaultWarehouseIntransitAsync()
        {
            await Task.Yield();

            return _warehouseRepository.Where(x => x.Virtual.Equals(true) && x.Name.Equals("InTransit")).FirstOrDefault();
        }
        public async Task<Warehouse> GetDefaultWarehouseAdjustmentAsync()
        {
            await Task.Yield();

            return _warehouseRepository.Where(x => x.Virtual.Equals(true) && x.Name.Equals("Adjustment")).FirstOrDefault();
        }
    }
}
