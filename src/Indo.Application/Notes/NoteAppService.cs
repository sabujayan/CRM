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

namespace Indo.Notes
{
    public class NoteAppService : IndoAppService, INoteAppService
    {
        private readonly INoteRepository _expenseRepository;
        private readonly NoteManager _expenseManager;
        private readonly ICustomerRepository _customerRepository;
        public NoteAppService(
            INoteRepository expenseRepository,
            NoteManager expenseManager,
            ICustomerRepository customerRepository
            )
        {
            _expenseRepository = expenseRepository;
            _expenseManager = expenseManager;
            _customerRepository = customerRepository;
        }
        public async Task<NoteReadDto> GetAsync(Guid id)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        where expense.Id == id
                        select new { expense, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Note), id);
            }
            var dto = ObjectMapper.Map<Note, NoteReadDto>(queryResult.expense);
            dto.CustomerName = queryResult.customer.Name;

            return dto;
        }
        public async Task<List<NoteReadDto>> GetListAsync()
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        select new { expense, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Note, NoteReadDto>(x.expense);
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<NoteReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        where expense.CustomerId == customerId
                        select new { expense, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Note, NoteReadDto>(x.expense);
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
        public async Task<NoteReadDto> CreateAsync(NoteCreateDto input)
        {
            var obj = await _expenseManager.CreateAsync(
                input.Description,
                input.CustomerId
            );

            await _expenseRepository.InsertAsync(obj);

            return ObjectMapper.Map<Note, NoteReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, NoteUpdateDto input)
        {
            var obj = await _expenseRepository.GetAsync(id);


            obj.Description = input.Description;
            obj.CustomerId = input.CustomerId;

            await _expenseRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _expenseRepository.DeleteAsync(id);
        }
    }
}
