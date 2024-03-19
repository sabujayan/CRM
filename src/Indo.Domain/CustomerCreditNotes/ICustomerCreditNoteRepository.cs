using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerCreditNotes
{
    public interface ICustomerCreditNoteRepository : IRepository<CustomerCreditNote, Guid>
    {
    }
}
