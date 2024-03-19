using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerInvoiceDetails
{
    public interface ICustomerInvoiceDetailRepository : IRepository<CustomerInvoiceDetail, Guid>
    {
    }
}
