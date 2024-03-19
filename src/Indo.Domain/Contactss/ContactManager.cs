using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Contacts
{
    public class ContactManager : DomainService
    {
        private readonly IContactRepository _contactRepository;

        public ContactManager(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<Contact> CreateAsync(
            [NotNull] string name,
            [NotNull] Guid customerId
            )
        {
            await Task.Yield();

            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(customerId, nameof(customerId));

            return new Contact(
                GuidGenerator.Create(),
                name,
                customerId
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Contact contact,
           [NotNull] string newName)
        {
            await Task.Yield();

            Check.NotNull(contact, nameof(contact));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            contact.ChangeName(newName);
        }
    }
}
