using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerCreditNoteDetails
{
    public interface ICustomerCreditNoteDetailRepository : IRepository<CustomerCreditNoteDetail, Guid>
    {
    }
}
