using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Bookings;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Resources
{
    public class ResourceAppService : IndoAppService, IResourceAppService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IBookingRepository _bookingRepository;
        public ResourceAppService(
            IResourceRepository resourceRepository,
            ResourceManager resourceManager,
            IBookingRepository bookingRepository
            )
        {
            _resourceRepository = resourceRepository;
            _resourceManager = resourceManager;
            _bookingRepository = bookingRepository;
        }
        public async Task<ResourceReadDto> GetAsync(Guid id)
        {
            var obj = await _resourceRepository.GetAsync(id);
            return ObjectMapper.Map<Resource, ResourceReadDto>(obj);
        }
        public async Task<List<ResourceReadDto>> GetListAsync()
        {
            var queryable = await _resourceRepository.GetQueryableAsync();
            var query = from resource in queryable
                        select new { resource };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Resource, ResourceReadDto>(x.resource);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ResourceReadDto> CreateAsync(ResourceCreateDto input)
        {
            var obj = await _resourceManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _resourceRepository.InsertAsync(obj);

            return ObjectMapper.Map<Resource, ResourceReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ResourceUpdateDto input)
        {
            var obj = await _resourceRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _resourceManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _resourceRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_bookingRepository.Where(x => x.ResourceId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _resourceRepository.DeleteAsync(id);
        }
    }
}
