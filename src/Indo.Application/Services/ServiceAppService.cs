using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.ServiceOrderDetails;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Services
{
    public class ServiceAppService : IndoAppService, IServiceAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IUomRepository _uomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ServiceManager _serviceManager;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;
        public ServiceAppService(
            CompanyAppService companyAppService,
            IServiceRepository serviceRepository,
            ServiceManager serviceManager,
            IUomRepository uomRepository,
            IServiceOrderDetailRepository serviceOrderDetailRepository
            )
        {
            _serviceRepository = serviceRepository;
            _serviceManager = serviceManager;
            _uomRepository = uomRepository;
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _companyAppService = companyAppService;
        }
        public async Task<ServiceReadDto> GetAsync(Guid id)
        {
            var queryable = await _serviceRepository.GetQueryableAsync();
            var query = from service in queryable
                        join uom in _uomRepository on service.UomId equals uom.Id
                        where service.Id == id
                        select new { service, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Service), id);
            }
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var dto = ObjectMapper.Map<Service, ServiceReadDto>(queryResult.service);
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            return dto;
        }
        public async Task<List<ServiceReadDto>> GetListAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceRepository.GetQueryableAsync();
            var query = from service in queryable
                        join uom in _uomRepository on service.UomId equals uom.Id
                        select new { service, uom };           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Service, ServiceReadDto>(x.service);
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<ListResultDto<UomLookupDto>> GetUomLookupAsync()
        {
            var list = await _uomRepository.GetListAsync();
            return new ListResultDto<UomLookupDto>(
                ObjectMapper.Map<List<Uom>, List<UomLookupDto>>(list)
            );
        }
        public async Task<ServiceReadDto> CreateAsync(ServiceCreateDto input)
        {
            var obj = await _serviceManager.CreateAsync(
                input.Name,
                input.UomId
            );

            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;

            await _serviceRepository.InsertAsync(obj);

            return ObjectMapper.Map<Service, ServiceReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ServiceUpdateDto input)
        {
            var obj = await _serviceRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _serviceManager.ChangeNameAsync(obj, input.Name);
            }

            obj.UomId = input.UomId;
            obj.Price = input.Price;
            obj.TaxRate = input.TaxRate;

            await _serviceRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            if (_serviceOrderDetailRepository.Where(x => x.ServiceId.Equals(id)).Any())
            {
                throw new UserFriendlyException("Unable to delete. Already have transaction.");
            }
            await _serviceRepository.DeleteAsync(id);
        }
    }
}
