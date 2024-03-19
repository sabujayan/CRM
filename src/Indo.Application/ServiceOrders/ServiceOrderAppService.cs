using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.CustomerInvoices;
using Indo.Customers;
using Indo.Employees;
using Indo.SalesQuotations;
using Indo.ServiceOrderDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceOrders
{
    public class ServiceOrderAppService : IndoAppService, IServiceOrderAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ServiceOrderManager _serviceOrderManager;
        private readonly IServiceOrderDetailRepository _serviceOrderDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        public ServiceOrderAppService(
            IServiceOrderRepository serviceOrderRepository,
            ServiceOrderManager serviceOrderManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            IServiceOrderDetailRepository serviceOrderDetailRepository,
            ICompanyAppService companyAppService,
            ISalesQuotationRepository salesQuotationRepository
            )
        {
            _serviceOrderRepository = serviceOrderRepository;
            _serviceOrderManager = serviceOrderManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _employeeAppService = employeeAppService;
            _serviceOrderDetailRepository = serviceOrderDetailRepository;
            _companyAppService = companyAppService;
            _salesQuotationRepository = salesQuotationRepository;
        }

        public async Task<OrderCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new OrderCountDto();
            result.CountDraft = _serviceOrderRepository.Where(x => x.Status == ServiceOrderStatus.Draft).Count();
            result.CountConfirm = _serviceOrderRepository.Where(x => x.Status == ServiceOrderStatus.Confirm).Count();
            result.CountCancelled = _serviceOrderRepository.Where(x => x.Status == ServiceOrderStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceOrderDetailRepository
                .Where(x => x.ServiceOrderId.Equals(id))
                .Sum(x => x.SubTotal);
            return result;
        }
        public async Task<string> GetSummarySubTotalInStringAsync(Guid id)
        {
            var result = await GetSummarySubTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryDiscAmtAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceOrderDetailRepository
                .Where(x => x.ServiceOrderId.Equals(id))
                .Sum(x => x.DiscAmt);
            return result;
        }
        public async Task<string> GetSummaryDiscAmtInStringAsync(Guid id)
        {
            var result = await GetSummaryDiscAmtAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryBeforeTaxAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceOrderDetailRepository
                .Where(x => x.ServiceOrderId.Equals(id))
                .Sum(x => x.BeforeTax);
            return result;
        }
        public async Task<string> GetSummaryBeforeTaxInStringAsync(Guid id)
        {
            var result = await GetSummaryBeforeTaxAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTaxAmountAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceOrderDetailRepository
                .Where(x => x.ServiceOrderId.Equals(id))
                .Sum(x => x.TaxAmount);
            return result;
        }
        public async Task<string> GetSummaryTaxAmountInStringAsync(Guid id)
        {
            var result = await GetSummaryTaxAmountAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetSummaryTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceOrderDetailRepository
                .Where(x => x.ServiceOrderId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<ServiceOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _serviceOrderRepository.GetQueryableAsync();
            var query = from serviceOrder in queryable
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceOrder.SalesExecutiveId equals employee.Id
                        where serviceOrder.Id == id
                        select new { serviceOrder, customer, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ServiceOrder), id);
            }
            var dto = ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(queryResult.serviceOrder);
            dto.CustomerName = queryResult.customer.Name;
            dto.SalesExecutiveName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)queryResult.serviceOrder.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<ServiceOrderReadDto>> GetListAsync()
        {
            var queryable = await _serviceOrderRepository.GetQueryableAsync();
            var query = from serviceOrder in queryable
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceOrder.SalesExecutiveId equals employee.Id
                        select new { serviceOrder, customer, employee };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(x.serviceOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ServiceOrderReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _serviceOrderRepository.GetQueryableAsync();
            var query = from serviceOrder in queryable
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceOrder.SalesExecutiveId equals employee.Id
                        select new { serviceOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(x.serviceOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                dto.Total = _serviceOrderDetailRepository.Where(y => y.ServiceOrderId.Equals(x.serviceOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ServiceOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _serviceOrderRepository.GetQueryableAsync();
            var query = from serviceOrder in queryable
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceOrder.SalesExecutiveId equals employee.Id
                        where serviceOrder.CustomerId == customerId
                        select new { serviceOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(x.serviceOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                dto.Total = _serviceOrderDetailRepository.Where(y => y.ServiceOrderId.Equals(x.serviceOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ServiceOrderReadDto>> GetListWithTotalByQuotationAsync(Guid serviceQuotationId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _serviceOrderRepository.GetQueryableAsync();
            var query = from serviceOrder in queryable
                        join customer in _customerRepository on serviceOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceOrder.SalesExecutiveId equals employee.Id
                        where serviceOrder.SourceDocumentId == serviceQuotationId
                        select new { serviceOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(x.serviceOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceOrderStatus:{(int)x.serviceOrder.Status}"];
                dto.Total = _serviceOrderDetailRepository.Where(y => y.ServiceOrderId.Equals(x.serviceOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            list = list.Where(x => x.Status == CustomerStatus.Customer).ToList();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<SalesExecutiveLookupDto>> GetSalesExecutiveLookupAsync()
        {
            var list = await _employeeAppService.GetSalesListAsync();
            return new ListResultDto<SalesExecutiveLookupDto>(
                ObjectMapper.Map<List<EmployeeReadDto>, List<SalesExecutiveLookupDto>>(list)
            );
        }
        public async Task<ServiceOrderReadDto> CreateAsync(ServiceOrderCreateDto input)
        {
            var obj = await _serviceOrderManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.SalesExecutiveId,
                input.OrderDate
            );

            obj.Description = input.Description;

            await _serviceOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ServiceOrderUpdateDto input)
        {
            var obj = await _serviceOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _serviceOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.SalesExecutiveId = input.SalesExecutiveId;
            obj.Description = input.Description;
            obj.OrderDate = input.OrderDate;

            await _serviceOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _serviceOrderRepository.DeleteAsync(id);
        }

        public async Task<ServiceOrderReadDto> ConfirmAsync(Guid serviceOrderId)
        {
            var obj = await _serviceOrderManager.ConfirmAsync(serviceOrderId);
            return ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(obj);
        }

        public async Task<ServiceOrderReadDto> CancelAsync(Guid serviceOrderId)
        {
            var obj = await _serviceOrderManager.CancelAsync(serviceOrderId);
            return ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(obj);
        }
        public async Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid serviceOrderId)
        {
            var obj = await _serviceOrderManager.GenerateInvoice(serviceOrderId);
            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }
    }
}
