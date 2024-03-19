using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.ClientesContact
{
    public class ClientsContactManager :DomainService
    {
        private readonly IClientsContactRepository _clientsContactRepository;
        public ClientsContactManager(IClientsContactRepository clientsContactRepository)
        {
            _clientsContactRepository = clientsContactRepository;
        }

        public async Task<ClientsContact> CreateAsync(
         [NotNull] string email,
         [NotNull] string phonenumber,
         [NotNull] Guid ClientsId
        )
        {
            Check.NotNull(email, nameof(email));
            Check.NotNull(phonenumber, nameof(phonenumber));
            var existing = await _clientsContactRepository.FindAsync(x => x.ClientsId.Equals(ClientsId));
            if (existing != null)
            {
                throw new ClientsContactAlreadyExistsException(email);
            }
            return new ClientsContact(
                GuidGenerator.Create(),
                email,
                phonenumber,
                ClientsId
            );
        }


        public async Task ChangeNameAsync(
          [NotNull] ClientsContact ClientContact,
          [NotNull] string newName)
        {
            Check.NotNull(ClientContact, nameof(ClientContact));
            Check.NotNull(newName, nameof(newName));

            var existing = await _clientsContactRepository.FindAsync(x => x.Email.Equals(newName));
            if (existing != null && existing.Id != ClientContact.Id)
            {
                throw new ClientsContactAlreadyExistsException(newName);
            }

            ClientContact.ChangeName(newName);
        }
    }
}
