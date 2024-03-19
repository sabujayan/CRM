using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Customers;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Contacts
{
    public class ContactAppService : IndoAppService, IContactAppService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ContactManager _contactManager;
        private readonly ICustomerRepository _customerRepository;
        public ContactAppService(
            IContactRepository contactRepository,
            ContactManager contactManager,
            ICustomerRepository customerRepository
            )
        {
            _contactRepository = contactRepository;
            _contactManager = contactManager;
            _customerRepository = customerRepository;
        }
        public async Task<ContactReadDto> GetAsync(Guid id)
        {
            var queryable = await _contactRepository.GetQueryableAsync();
            var query = from contact in queryable
                        join customer in _customerRepository on contact.CustomerId equals customer.Id
                        where contact.Id == id
                        select new { contact, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Contact), id);
            }
            var dto = ObjectMapper.Map<Contact, ContactReadDto>(queryResult.contact);
            dto.CustomerName = queryResult.customer.Name;

            return dto;
        }
        public async Task<List<ContactReadDto>> GetListAsync()
        {
            var queryable = await _contactRepository.GetQueryableAsync();
            var query = from contact in queryable
                        join customer in _customerRepository on contact.CustomerId equals customer.Id
                        select new { contact, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Contact, ContactReadDto>(x.contact);
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<ContactReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _contactRepository.GetQueryableAsync();
            var query = from contact in queryable
                        join customer in _customerRepository on contact.CustomerId equals customer.Id
                        where contact.CustomerId == customerId
                        select new { contact, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Contact, ContactReadDto>(x.contact);
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<ContactReadDto> CreateAsync(ContactCreateDto input)
        {
            var obj = await _contactManager.CreateAsync(
                input.Name,
                input.CustomerId
            );

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;

            await _contactRepository.InsertAsync(obj);

            return ObjectMapper.Map<Contact, ContactReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ContactUpdateDto input)
        {
            var obj = await _contactRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _contactManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;
            obj.CustomerId = input.CustomerId;

            await _contactRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _contactRepository.DeleteAsync(id);
        }
    }
}
