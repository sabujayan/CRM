using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Currencies;
using Indo.Warehouses;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Companies
{
    public class CompanyAppService : IndoAppService, ICompanyAppService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly WarehouseManager _warehouseManager;
        private readonly CompanyManager _companyManager;
        public CompanyAppService(
            ICompanyRepository companyRepository,
            CompanyManager companyManager,
            IWarehouseRepository warehouseRepository,
            WarehouseManager warehouseManager,
            ICurrencyRepository currencyRepository)
        {
            _companyRepository = companyRepository;
            _companyManager = companyManager;
            _currencyRepository = currencyRepository;
            _warehouseRepository = warehouseRepository;
            _warehouseManager = warehouseManager;
        }
        public async Task<CompanyReadDto> GetAsync(Guid id)
        {
            var queryable = await _companyRepository.GetQueryableAsync();
            var query = from company in queryable
                        join currency in _currencyRepository on company.CurrencyId equals currency.Id
                        join warehouse in _warehouseRepository on company.DefaultWarehouseId equals warehouse.Id
                        where company.Id == id
                        select new { company, currency, warehouse };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Company), id);
            }
            var dto = ObjectMapper.Map<Company, CompanyReadDto>(queryResult.company);
            dto.CurrencyName = queryResult.currency.Name;
            dto.DefaultWarehouseName = queryResult.warehouse.Name;
            return dto;
        }
        public async Task<CompanyReadDto> GetDefaultCompanyAsync()
        {
            var defaultcompany = await _companyManager.GetDefaultCompanyAsync();
            var queryable = await _companyRepository.GetQueryableAsync();
            var query = from company in queryable
                        join currency in _currencyRepository on company.CurrencyId equals currency.Id
                        join warehouse in _warehouseRepository on company.DefaultWarehouseId equals warehouse.Id
                        where company.Id.Equals(defaultcompany.Id)
                        select new { company, currency, warehouse };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            var dto = ObjectMapper.Map<Company, CompanyReadDto>(queryResult.company);
            dto.CurrencyName = queryResult.currency.Name;
            dto.DefaultWarehouseName = queryResult.warehouse.Name;
            return dto;
        }
        public async Task<List<CompanyReadDto>> GetListAsync()
        {
            var queryable = await _companyRepository.GetQueryableAsync();
            var query = from company in queryable
                        join currency in _currencyRepository on company.CurrencyId equals currency.Id
                        join warehouse in _warehouseRepository on company.DefaultWarehouseId equals warehouse.Id
                        select new { company, currency, warehouse };           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Company, CompanyReadDto>(x.company);
                dto.CurrencyName = x.currency.Name;
                dto.DefaultWarehouseName = x.warehouse.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<ListResultDto<CurrencyLookupDto>> GetCurrencyLookupAsync()
        {
            var list = await _currencyRepository.GetListAsync();
            return new ListResultDto<CurrencyLookupDto>(
                ObjectMapper.Map<List<Currency>, List<CurrencyLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<WarehouseLookupDto>> GetWarehouseLookupAsync()
        {
            var query = await _warehouseRepository.GetQueryableAsync();
            var list = await AsyncExecuter.ToListAsync(query.Where(x => x.Virtual.Equals(false)));
            return new ListResultDto<WarehouseLookupDto>(
                ObjectMapper.Map<List<Warehouse>, List<WarehouseLookupDto>>(list)
            );
        }
        public async Task<CompanyReadDto> CreateAsync(CompanyCreateDto input)
        {
            var obj = await _companyManager.CreateAsync(
                input.Name,
                input.CurrencyId,
                input.DefaultWarehouseId
            );

            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;

            await _companyRepository.InsertAsync(obj);

            return ObjectMapper.Map<Company, CompanyReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, CompanyUpdateDto input)
        {
            var obj = await _companyRepository.GetAsync(id);

            if (obj.Name != input.Name)
            {
                await _companyManager.ChangeNameAsync(obj, input.Name);
            }

            obj.CurrencyId = input.CurrencyId;
            obj.DefaultWarehouseId = input.DefaultWarehouseId;
            obj.Phone = input.Phone;
            obj.Email = input.Email;
            obj.Street = input.Street;
            obj.City = input.City;
            obj.State = input.State;
            obj.ZipCode = input.ZipCode;

            await _companyRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _companyRepository.DeleteAsync(id);
        }
    }
}
