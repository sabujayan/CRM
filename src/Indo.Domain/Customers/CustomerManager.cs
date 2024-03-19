using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Indo.Customers
{
    public class CustomerManager : DomainService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Customer> CreateAsync(
            [NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existing = await _customerRepository.FindAsync(x => x.Name.Equals(name));
            if (existing != null)
            {
                throw new CustomerAlreadyExistsException(name);
            }

            return new Customer(
                GuidGenerator.Create(),
                name
            );
        }
        public async Task ChangeNameAsync(
           [NotNull] Customer customer,
           [NotNull] string newName)
        {
            Check.NotNull(customer, nameof(customer));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existing = await _customerRepository.FindAsync(x => x.Name.Equals(newName));
            if (existing != null && existing.Id != customer.Id)
            {
                throw new CustomerAlreadyExistsException(newName);
            }

            customer.ChangeName(newName);
        }

        public async Task<Customer> ConvertLeadToCustomerAsync(Guid customerId)
        {
            var obj = await _customerRepository.GetAsync(customerId);
            if (obj.Status == CustomerStatus.Lead)
            {
                obj.Status = CustomerStatus.Customer;
                return await _customerRepository.UpdateAsync(obj, true);
            }
            else
            {
                throw new UserFriendlyException("Only Lead Can Be Converted!");
            }
        }

    }
}
