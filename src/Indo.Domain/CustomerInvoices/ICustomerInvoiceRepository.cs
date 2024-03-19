using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerInvoices
{
    public interface ICustomerInvoiceRepository : IRepository<CustomerInvoice, Guid>
    {
    }
}
