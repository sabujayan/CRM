using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Indo.Addresss
{
    public class AddressManager : DomainService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressManager(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressInfo> CreateAsync(string street, string state, string country, string city, string zipCode)
        {

            var address = new AddressInfo(GuidGenerator.Create(), street, state, country, city, zipCode);
            return await _addressRepository.InsertAsync(address);
        }
    }
}
