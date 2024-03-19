using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Clientes
{
    public class ClientsManager : DomainService
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientsManager(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task<Clients> CreateAsync(
           [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

           /* var existing = await _clientsRepository.FindAsync(x => x.Name.Equals(name));
          /*  if (existing != null)
            {
                throw new ClientsAlreadyExistsException(name);
            }*/

            return new Clients(
                GuidGenerator.Create(),
                name
            );
        }


        public async Task ChangeNameAsync(
           [NotNull] Clients clients,
           [NotNull] string newName)
        {
            Check.NotNull(clients, nameof(clients));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _clientsRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != clients.Id)
            {
                throw new ClientsAlreadyExistsException(newName);
            }

            clients.ChangeName(newName);
        }
    }
}
