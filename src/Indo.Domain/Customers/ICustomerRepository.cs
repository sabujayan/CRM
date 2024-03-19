using System;
using Volo.Abp.Domain.Repositories;

namespace Indo.Customers
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
    }
}
