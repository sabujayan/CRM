using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.CustomerPayments
{
    public interface ICustomerPaymentRepository : IRepository<CustomerPayment, Guid>
    {
    }
}
