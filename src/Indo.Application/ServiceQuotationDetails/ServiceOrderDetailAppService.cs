using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ServiceQuotations;
using Indo.Services;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceQuotationDetails
{
    public class ServiceQuotationDetailAppService : IndoAppService, IServiceQuotationDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IServiceQuotationRepository _serviceQuotationRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IServiceQuotationDetailRepository _serviceQuotationDetailRepository;
        private readonly ServiceQuotationDetailManager _serviceQuotationDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public ServiceQuotationDetailAppService(
            CompanyAppService companyAppService,
            IServiceQuotationDetailRepository serviceQuotationDetailRepository,
            ServiceQuotationDetailManager serviceQuotationDetailManager,
            IServiceQuotationRepository serviceQuotationRepository,
            IServiceRepository serviceRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _serviceQuotationDetailRepository = serviceQuotationDetailRepository;
            _serviceQuotationDetailManager = serviceQuotationDetailManager;
            _serviceQuotationRepository = serviceQuotationRepository;
            _serviceRepository = serviceRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<ServiceQuotationDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceQuotationDetailRepository.GetQueryableAsync();
            var query = from serviceQuotationDetail in queryable
                        join serviceQuotation in _serviceQuotationRepository on serviceQuotationDetail.ServiceQuotationId equals serviceQuotation.Id
                        join service in _serviceRepository on serviceQuotationDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        where serviceQuotationDetail.Id == id
                        select new { serviceQuotationDetail, serviceQuotation, service, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ServiceQuotationDetail), id);
            }
            var dto = ObjectMapper.Map<ServiceQuotationDetail, ServiceQuotationDetailReadDto>(queryResult.serviceQuotationDetail);
            dto.ServiceName = queryResult.service.Name;
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.serviceQuotation.Status;
            dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)queryResult.serviceQuotation.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<ServiceQuotationDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceQuotationDetailRepository.GetQueryableAsync();
            var query = from serviceQuotationDetail in queryable
                        join serviceQuotation in _serviceQuotationRepository on serviceQuotationDetail.ServiceQuotationId equals serviceQuotation.Id
                        join service in _serviceRepository on serviceQuotationDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        select new { serviceQuotationDetail, serviceQuotation, service, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotationDetail, ServiceQuotationDetailReadDto>(x.serviceQuotationDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceQuotation.Status;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _serviceQuotationDetailRepository.GetCountAsync();

            return new PagedResultDto<ServiceQuotationDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<ServiceQuotationDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceQuotationDetailRepository.GetQueryableAsync();
            var query = from serviceQuotationDetail in queryable
                        join serviceQuotation in _serviceQuotationRepository on serviceQuotationDetail.ServiceQuotationId equals serviceQuotation.Id
                        join customer in _customerRepository on serviceQuotation.CustomerId equals customer.Id
                        join service in _serviceRepository on serviceQuotationDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        select new { serviceQuotationDetail, serviceQuotation, customer, service, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotationDetail, ServiceQuotationDetailReadDto>(x.serviceQuotationDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceQuotation.Status;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                dto.ServiceQuotationNumber = x.serviceQuotation.Number;
                dto.QuotationDate = x.serviceQuotation.QuotationDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.serviceQuotationDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.serviceQuotationDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.serviceQuotationDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.serviceQuotationDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.serviceQuotationDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.serviceQuotationDetail.Total.ToString("##,##.00");
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<ServiceQuotationDetailReadDto>> GetListByServiceQuotationAsync(Guid serviceQuotationId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceQuotationDetailRepository.GetQueryableAsync();
            var query = from serviceQuotationDetail in queryable
                        join serviceQuotation in _serviceQuotationRepository on serviceQuotationDetail.ServiceQuotationId equals serviceQuotation.Id
                        join service in _serviceRepository on serviceQuotationDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        where serviceQuotationDetail.ServiceQuotationId.Equals(serviceQuotationId)
                        select new { serviceQuotationDetail, serviceQuotation, service, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotationDetail, ServiceQuotationDetailReadDto>(x.serviceQuotationDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceQuotation.Status;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                dto.PriceString = x.serviceQuotationDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.serviceQuotationDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.serviceQuotationDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.serviceQuotationDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.serviceQuotationDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.serviceQuotationDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<ServiceQuotationDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<ServiceQuotationLookupDto>> GetServiceQuotationLookupAsync()
        {
            var list = await _serviceQuotationRepository.GetListAsync();
            return new ListResultDto<ServiceQuotationLookupDto>(
                ObjectMapper.Map<List<ServiceQuotation>, List<ServiceQuotationLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ServiceLookupDto>> GetServiceLookupAsync()
        {
            var list = await _serviceRepository.GetListAsync();
            return new ListResultDto<ServiceLookupDto>(
                ObjectMapper.Map<List<Service>, List<ServiceLookupDto>>(list)
            );
        }
        public async Task<ServiceQuotationDetailReadDto> CreateAsync(ServiceQuotationDetailCreateDto input)
        {
            var obj = await _serviceQuotationDetailManager.CreateAsync(
                input.ServiceQuotationId,
                input.ServiceId,
                input.Quantity,
                input.DiscAmt
            );
            await _serviceQuotationDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<ServiceQuotationDetail, ServiceQuotationDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ServiceQuotationDetailUpdateDto input)
        {
            var obj = await _serviceQuotationDetailRepository.GetAsync(id);

            obj.ServiceId = input.ServiceId;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            var service = _serviceRepository.Where(x => x.Id.Equals(obj.ServiceId)).FirstOrDefault();
            
            obj.Price = service.Price;
            obj.TaxRate = service.TaxRate;
            obj.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(service.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _serviceQuotationDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _serviceQuotationDetailRepository.DeleteAsync(id);
        }
    }
}
