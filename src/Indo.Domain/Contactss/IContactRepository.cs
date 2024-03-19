using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Contacts
{
    public interface IContactRepository : IRepository<Contact, Guid>
    {
    }
}
