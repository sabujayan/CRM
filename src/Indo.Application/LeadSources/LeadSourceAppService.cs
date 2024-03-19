using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Customers;
using Indo.Products;
using Indo.Services;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.LeadSources
{
    public class LeadSourceAppService : IndoAppService, ILeadSourceAppService
    {
        private readonly ILeadSourceRepository _leadSourceRepository;
        private readonly LeadSourceManager _leadSourceManager;
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICustomerRepository _customerRepository;
        public LeadSourceAppService(
            ILeadSourceRepository leadSourceRepository,
            LeadSourceManager leadSourceManager,
            IProductRepository productRepository,
            IServiceRepository serviceRepository,
            ICustomerRepository customerRepository
            )
        {
            _leadSourceRepository = leadSourceRepository;
            _leadSourceManager = leadSourceManager;
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
            _customerRepository = customerRepository;
        }
        public async Task<LeadSourceReadDto> GetAsync(Guid id)
        {
            var obj = await _leadSourceRepository.GetAsync(id);
            return ObjectMapper.Map<LeadSource, LeadSourceReadDto>(obj);
        }
        public async Task<List<LeadSourceReadDto>> GetListAsync()
        {
            var queryable = await _leadSourceRepository.GetQueryableAsync();
            var query = from leadSource in queryable
                        select new { leadSource };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<LeadSource, LeadSourceReadDto>(x.leadSource);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<LeadSourceReadDto> CreateAsync(LeadSourceCreateDto input)
        {
            var obj = await _leadSourceManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _leadSourceRepository.InsertAsync(obj);

            return ObjectMapper.Map<LeadSource, LeadSourceReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, LeadSourceUpdateDto input)
        {
            var obj = await _leadSourceRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _leadSourceManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _leadSourceRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_customerRepository.Where(x => x.LeadSourceId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _leadSourceRepository.DeleteAsync(id);
        }
    }
}
