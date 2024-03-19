using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Products;
using Indo.Services;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Indo.Uoms
{
    public class UomAppService : IndoAppService, IUomAppService
    {
        private readonly IUomRepository _uomRepository;
        private readonly UomManager _uomManager;
        private readonly IProductRepository _productRepository;
        private readonly IServiceRepository _serviceRepository;
        public UomAppService(
            IUomRepository uomRepository,
            UomManager uomManager,
            IProductRepository productRepository,
            IServiceRepository serviceRepository
            )
        {
            _uomRepository = uomRepository;
            _uomManager = uomManager;
            _productRepository = productRepository;
            _serviceRepository = serviceRepository;
        }
        public async Task<UomReadDto> GetAsync(Guid id)
        {
            var obj = await _uomRepository.GetAsync(id);
            return ObjectMapper.Map<Uom, UomReadDto>(obj);
        }
        public async Task<List<UomReadDto>> GetListAsync()
        {
            var queryable = await _uomRepository.GetQueryableAsync();
            var query = from uom in queryable
                        select new { uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Uom, UomReadDto>(x.uom);
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<UomReadDto> CreateAsync(UomCreateDto input)
        {
            var obj = await _uomManager.CreateAsync(
                input.Name
            );

            obj.Description = input.Description;

            await _uomRepository.InsertAsync(obj);

            return ObjectMapper.Map<Uom, UomReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, UomUpdateDto input)
        {
            var obj = await _uomRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _uomManager.ChangeNameAsync(obj, input.Name);
            }

            obj.Description = input.Description;

            await _uomRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_productRepository.Where(x => x.UomId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            if (_serviceRepository.Where(x => x.UomId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _uomRepository.DeleteAsync(id);
        }
    }
}
