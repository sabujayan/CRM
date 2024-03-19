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

namespace Indo.LeadRatings
{
    public class LeadRatingAppService : IndoAppService, ILeadRatingAppService
    {
        private readonly ILeadRatingRepository _leadRatingRepository;
        private readonly LeadRatingManager _leadRatingManager;
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICustomerRepository _customerRepository;
        public LeadRatingAppService(
            ILeadRatingRepository leadRatingRepository,
            LeadRatingManager leadRatingManager,
            IProductRepository productRepository,
            IServiceRepository serviceRepository,
            ICustomerRepository customerRepository
            )
        {
            _leadRatingRepository = leadRatingRepository;
            _leadRatingManager = leadRatingManager;
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
            _customerRepository = customerRepository;
        }
        public async Task<LeadRatingReadDto> GetAsync(Guid id)
        {
            var obj = await _leadRatingRepository.GetAsync(id);
            return ObjectMapper.Map<LeadRating, LeadRatingReadDto>(obj);
        }
        public async Task<List<LeadRatingReadDto>> GetListAsync()
        {
            var queryable = await _leadRatingRepository.GetQueryableAsync();
            var query = from leadRating in queryable
                        select new { leadRating };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<LeadRating, LeadRatingReadDto>(x.leadRating);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<LeadRatingReadDto> CreateAsync(LeadRatingCreateDto input)
        {
            var obj = await _leadRatingManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _leadRatingRepository.InsertAsync(obj);

            return ObjectMapper.Map<LeadRating, LeadRatingReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, LeadRatingUpdateDto input)
        {
            var obj = await _leadRatingRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _leadRatingManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _leadRatingRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_customerRepository.Where(x => x.LeadRatingId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _leadRatingRepository.DeleteAsync(id);
        }
    }
}
