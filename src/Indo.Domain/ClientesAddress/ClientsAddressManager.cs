using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ClientesAddress
{
    public class ClientsAddressManager : DomainService
    {
        private readonly IClientsAddressRepository _clientsAddressRepository;
        public ClientsAddressManager(IClientsAddressRepository clientsAddressRepository)
        {
            _clientsAddressRepository = clientsAddressRepository;
        }

        public async Task<ClientsAddress> CreateAsync(
         [NotNull] string name,
         [NotNull] Guid ClientsId,
         [NotNull] string Country,
         [NotNull] string State,
         [NotNull] string City,
         [NotNull] string Zip
         )
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(Country, nameof(Country));
            Check.NotNull(State, nameof(State));
            Check.NotNull(City, nameof(City));
            Check.NotNull(Zip, nameof(Zip));

            var existing = await _clientsAddressRepository.FindAsync(x => x.ClientsId.Equals(ClientsId));
            if (existing != null)
            {
                throw new ClientsAddressAlreadyExistsException(ClientsId);
            }

            return new ClientsAddress(
                GuidGenerator.Create(),
                name,
                ClientsId,
                Country,
                State,
                City,
                Zip
            );
        }
    }
}
