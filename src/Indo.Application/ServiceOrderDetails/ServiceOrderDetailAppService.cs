using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.ServiceOrders;
using Indo.Services;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceOrderDetails
{
    public class ServiceOrderDetailAppService : IndoAppService, IServiceOrderDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUomRepository _uomRepository;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;
        private readonly ServiceOrderDetailManager _serviceOrderDetailManager;
        private readonly ICustomerRepository _customerRepository;
        public ServiceOrderDetailAppService(
            CompanyAppService companyAppService,
            IServiceOrderDetailRepository serviceOrderDetailRepository,
            ServiceOrderDetailManager serviceOrderDetailManager,
            IServiceOrderRepository serviceOrderRepository,
            IServiceRepository serviceRepository,
            ICustomerRepository customerRepository,
            IUomRepository uomRepository)
        {
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _serviceOrderDetailManager = serviceOrderDetailManager;
            _serviceOrderRepository = serviceOrderRepository;
            _serviceRepository = serviceRepository;
            _uomRepository = uomRepository;
            _companyAppService = companyAppService;
            _customerRepository = customerRepository;
        }
        public async Task<ServiceOrderDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceOrderDetailRepository.GetQueryableAsync();
            var query = from serviceOrderDetail in queryable
                        join serviceOrder in _serviceOrderRepository on serviceOrderDetail.ServiceOrderId equals serviceOrder.Id
                        join service in _serviceRepository on serviceOrderDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        where serviceOrderDetail.Id == id
                        select new { serviceOrderDetail, serviceOrder, service, uom };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ServiceOrderDetail), id);
            }
            var dto = ObjectMapper.Map<ServiceOrderDetail, ServiceOrderDetailReadDto>(queryResult.serviceOrderDetail);
            dto.ServiceName = queryResult.service.Name;
            dto.UomName = queryResult.uom.Name;
            dto.CurrencyName = company.CurrencyName;
            dto.Status = queryResult.serviceOrder.Status;
            dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)queryResult.serviceOrder.Status}"];
            return dto;
        }
        public async Task<PagedResultDto<ServiceOrderDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceOrderDetailRepository.GetQueryableAsync();
            var query = from serviceOrderDetail in queryable
                        join serviceOrder in _serviceOrderRepository on serviceOrderDetail.ServiceOrderId equals serviceOrder.Id
                        join service in _serviceRepository on serviceOrderDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        select new { serviceOrderDetail, serviceOrder, service, uom };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrderDetail, ServiceOrderDetailReadDto>(x.serviceOrderDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceOrder.Status;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                return dto;
            }).ToList();

            var totalCount = await _serviceOrderDetailRepository.GetCountAsync();

            return new PagedResultDto<ServiceOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<ServiceOrderDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceOrderDetailRepository.GetQueryableAsync();
            var query = from serviceOrderDetail in queryable
                        join serviceOrder in _serviceOrderRepository on serviceOrderDetail.ServiceOrderId equals serviceOrder.Id
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join service in _serviceRepository on serviceOrderDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        select new { serviceOrderDetail, serviceOrder, customer, service, uom };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrderDetail, ServiceOrderDetailReadDto>(x.serviceOrderDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceOrder.Status;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                dto.ServiceOrderNumber = x.serviceOrder.Number;
                dto.OrderDate = x.serviceOrder.OrderDate;
                dto.CustomerName = x.customer.Name;
                dto.PriceString = x.serviceOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.serviceOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.serviceOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.serviceOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.serviceOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.serviceOrderDetail.Total.ToString("##,##.00");
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<ServiceOrderDetailReadDto>> GetListByServiceOrderAsync(Guid serviceOrderId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceOrderDetailRepository.GetQueryableAsync();
            var query = from serviceOrderDetail in queryable
                        join serviceOrder in _serviceOrderRepository on serviceOrderDetail.ServiceOrderId equals serviceOrder.Id
                        join service in _serviceRepository on serviceOrderDetail.ServiceId equals service.Id
                        join uom in _uomRepository on service.UomId equals uom.Id
                        where serviceOrderDetail.ServiceOrderId.Equals(serviceOrderId)
                        select new { serviceOrderDetail, serviceOrder, service, uom };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrderDetail, ServiceOrderDetailReadDto>(x.serviceOrderDetail);
                dto.ServiceName = x.service.Name;
                dto.UomName = x.uom.Name;
                dto.CurrencyName = company.CurrencyName;
                dto.Status = x.serviceOrder.Status;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                dto.PriceString = x.serviceOrderDetail.Price.ToString("##,##.00");
                dto.SubTotalString = x.serviceOrderDetail.SubTotal.ToString("##,##.00");
                dto.DiscAmtString = x.serviceOrderDetail.DiscAmt.ToString("##,##.00");
                dto.BeforeTaxString = x.serviceOrderDetail.BeforeTax.ToString("##,##.00");
                dto.TaxAmountString = x.serviceOrderDetail.TaxAmount.ToString("##,##.00");
                dto.TotalString = x.serviceOrderDetail.Total.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<ServiceOrderDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<ServiceOrderLookupDto>> GetServiceOrderLookupAsync()
        {
            var list = await _serviceOrderRepository.GetListAsync();
            return new ListResultDto<ServiceOrderLookupDto>(
                ObjectMapper.Map<List<ServiceOrder>, List<ServiceOrderLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ServiceLookupDto>> GetServiceLookupAsync()
        {
            var list = await _serviceRepository.GetListAsync();
            return new ListResultDto<ServiceLookupDto>(
                ObjectMapper.Map<List<Service>, List<ServiceLookupDto>>(list)
            );
        }
        public async Task<ServiceOrderDetailReadDto> CreateAsync(ServiceOrderDetailCreateDto input)
        {
            var obj = await _serviceOrderDetailManager.CreateAsync(
                input.ServiceOrderId,
                input.ServiceId,
                input.Quantity,
                input.DiscAmt
            );
            await _serviceOrderDetailRepository.InsertAsync(obj);
            return ObjectMapper.Map<ServiceOrderDetail, ServiceOrderDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ServiceOrderDetailUpdateDto input)
        {
            var obj = await _serviceOrderDetailRepository.GetAsync(id);

            obj.ServiceId = input.ServiceId;
            obj.Quantity = input.Quantity;
            obj.DiscAmt = input.DiscAmt;

            var service = _serviceRepository.Where(x => x.Id.Equals(obj.ServiceId)).FirstOrDefault();
            
            obj.Price = service.Price;
            obj.TaxRate = service.TaxRate;
            obj.Recalculate();

            var uom = _uomRepository.Where(x => x.Id.Equals(service.UomId)).FirstOrDefault();
            obj.UomName = uom.Name;

            await _serviceOrderDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _serviceOrderDetailRepository.DeleteAsync(id);
        }
    }
}
