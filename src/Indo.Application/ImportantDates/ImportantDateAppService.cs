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

namespace Indo.ImportantDates
{
    public class ImportantDateAppService : IndoAppService, IImportantDateAppService
    {
        private readonly IImportantDateRepository _importantDateRepository;
        private readonly ImportantDateManager _importantDateManager;
        private readonly ICustomerRepository _customerRepository;
        public ImportantDateAppService(
            IImportantDateRepository importantDateRepository,
            ImportantDateManager importantDateManager,
            ICustomerRepository customerRepository
            )
        {
            _importantDateRepository = importantDateRepository;
            _importantDateManager = importantDateManager;
            _customerRepository = customerRepository;
        }
        public async Task<ImportantDateReadDto> GetAsync(Guid id)
        {
            var queryable = await _importantDateRepository.GetQueryableAsync();
            var query = from importantDate in queryable
                        join customer in _customerRepository on importantDate.CustomerId equals customer.Id
                        where importantDate.Id == id
                        select new { importantDate, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ImportantDate), id);
            }
            var dto = ObjectMapper.Map<ImportantDate, ImportantDateReadDto>(queryResult.importantDate);
            dto.CustomerName = queryResult.customer.Name;

            return dto;
        }
        public async Task<List<ImportantDateReadDto>> GetListAsync()
        {
            var queryable = await _importantDateRepository.GetQueryableAsync();
            var query = from importantDate in queryable
                        join customer in _customerRepository on importantDate.CustomerId equals customer.Id
                        select new { importantDate, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ImportantDate, ImportantDateReadDto>(x.importantDate);
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<ImportantDateReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _importantDateRepository.GetQueryableAsync();
            var query = from importantDate in queryable
                        join customer in _customerRepository on importantDate.CustomerId equals customer.Id
                        where importantDate.CustomerId == customerId
                        select new { importantDate, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ImportantDate, ImportantDateReadDto>(x.importantDate);
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
        public async Task<ImportantDateReadDto> CreateAsync(ImportantDateCreateDto input)
        {
            var obj = await _importantDateManager.CreateAsync(
                input.Name,
                input.StartTime,
                input.EndTime,
                input.CustomerId
            );

            obj.Description = input.Description;
            obj.Location = input.Location;

            await _importantDateRepository.InsertAsync(obj);

            return ObjectMapper.Map<ImportantDate, ImportantDateReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ImportantDateUpdateDto input)
        {
            var obj = await _importantDateRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _importantDateManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;
            obj.Location = input.Location;
            obj.StartTime = input.StartTime;
            obj.EndTime = input.EndTime;
            obj.CustomerId = input.CustomerId;

            await _importantDateRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _importantDateRepository.DeleteAsync(id);
        }
    }
}
