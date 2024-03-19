using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Customers;
using Indo.Employees;
using Indo.ServiceOrders;
using Indo.ServiceQuotationDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ServiceQuotations
{
    public class ServiceQuotationAppService : IndoAppService, IServiceQuotationAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IServiceQuotationRepository _serviceQuotationRepository;
        private readonly ServiceQuotationManager _serviceQuotationManager;
        private readonly IServiceQuotationDetailRepository _serviceQuotationDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        public ServiceQuotationAppService(
            IServiceQuotationRepository serviceQuotationRepository,
            ServiceQuotationManager serviceQuotationManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            IServiceQuotationDetailRepository serviceQuotationDetailRepository,
            ICompanyAppService companyAppService
            )
        {
            _serviceQuotationRepository = serviceQuotationRepository;
            _serviceQuotationManager = serviceQuotationManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _employeeAppService = employeeAppService;
            _serviceQuotationDetailRepository = serviceQuotationDetailRepository;
            _companyAppService = companyAppService;
        }

        public async Task<QuotationCountDto> GetCountQuotationAsync()
        {
            await Task.Yield();
            var result = new QuotationCountDto();
            result.CountDraft = _serviceQuotationRepository.Where(x => x.Status == ServiceQuotationStatus.Draft).Count();
            result.CountConfirm = _serviceQuotationRepository.Where(x => x.Status == ServiceQuotationStatus.Confirm).Count();
            result.CountCancelled = _serviceQuotationRepository.Where(x => x.Status == ServiceQuotationStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _serviceQuotationDetailRepository
                .Where(x => x.ServiceQuotationId.Equals(id))
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
            result = _serviceQuotationDetailRepository
                .Where(x => x.ServiceQuotationId.Equals(id))
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
            result = _serviceQuotationDetailRepository
                .Where(x => x.ServiceQuotationId.Equals(id))
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
            result = _serviceQuotationDetailRepository
                .Where(x => x.ServiceQuotationId.Equals(id))
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
            result = _serviceQuotationDetailRepository
                .Where(x => x.ServiceQuotationId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<ServiceQuotationReadDto> GetAsync(Guid id)
        {
            var queryable = await _serviceQuotationRepository.GetQueryableAsync();
            var query = from serviceQuotation in queryable
                        join customer in _customerRepository on serviceQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceQuotation.SalesExecutiveId equals employee.Id
                        where serviceQuotation.Id == id
                        select new { serviceQuotation, customer, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ServiceQuotation), id);
            }
            var dto = ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(queryResult.serviceQuotation);
            dto.CustomerName = queryResult.customer.Name;
            dto.SalesExecutiveName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)queryResult.serviceQuotation.Status}"];
            dto.PipelineString = L[$"Enum:ServiceQuotationPipeline:{(int)queryResult.serviceQuotation.Pipeline}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<ServiceQuotationReadDto>> GetListAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _serviceQuotationRepository.GetQueryableAsync();
            var query = from serviceQuotation in queryable
                        join customer in _customerRepository on serviceQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceQuotation.SalesExecutiveId equals employee.Id
                        select new { serviceQuotation, customer, employee };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(x.serviceQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                dto.PipelineString = L[$"Enum:ServiceQuotationPipeline:{(int)x.serviceQuotation.Pipeline}"]; 
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ServiceQuotationReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _serviceQuotationRepository.GetQueryableAsync();
            var query = from serviceQuotation in queryable
                        join customer in _customerRepository on serviceQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceQuotation.SalesExecutiveId equals employee.Id
                        select new { serviceQuotation, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(x.serviceQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                dto.PipelineString = L[$"Enum:ServiceQuotationPipeline:{(int)x.serviceQuotation.Pipeline}"];
                dto.Total = _serviceQuotationDetailRepository.Where(y => y.ServiceQuotationId.Equals(x.serviceQuotation.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ServiceQuotationReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _serviceQuotationRepository.GetQueryableAsync();
            var query = from serviceQuotation in queryable
                        join customer in _customerRepository on serviceQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on serviceQuotation.SalesExecutiveId equals employee.Id
                        where serviceQuotation.CustomerId == customerId
                        select new { serviceQuotation, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(x.serviceQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ServiceQuotationStatus:{(int)x.serviceQuotation.Status}"];
                dto.PipelineString = L[$"Enum:ServiceQuotationPipeline:{(int)x.serviceQuotation.Pipeline}"];
                dto.Total = _serviceQuotationDetailRepository.Where(y => y.ServiceQuotationId.Equals(x.serviceQuotation.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
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
        public async Task<ServiceOrderReadDto> ConvertToOrderAsync(Guid serviceQuotationId)
        {
            var obj = await _serviceQuotationManager.ConvertToOrder(serviceQuotationId);
            return ObjectMapper.Map<ServiceOrder, ServiceOrderReadDto>(obj);
        }
        public async Task<ServiceQuotationReadDto> CreateAsync(ServiceQuotationCreateDto input)
        {
            var obj = await _serviceQuotationManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.SalesExecutiveId,
                input.QuotationDate,
                input.QuotationValidUntilDate
            );

            obj.Description = input.Description;

            await _serviceQuotationRepository.InsertAsync(obj);

            return ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ServiceQuotationUpdateDto input)
        {
            var obj = await _serviceQuotationRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _serviceQuotationManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.SalesExecutiveId = input.SalesExecutiveId;
            obj.Description = input.Description;
            obj.QuotationDate = input.QuotationDate;
            obj.QuotationValidUntilDate = input.QuotationValidUntilDate;
            obj.Pipeline = input.Pipeline;

            await _serviceQuotationRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _serviceQuotationRepository.DeleteAsync(id);
        }

        public async Task<ServiceQuotationReadDto> ConfirmAsync(Guid serviceQuotationId)
        {
            var obj = await _serviceQuotationManager.ConfirmAsync(serviceQuotationId);
            return ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(obj);
        }

        public async Task<ServiceQuotationReadDto> CancelAsync(Guid serviceQuotationId)
        {
            var obj = await _serviceQuotationManager.CancelAsync(serviceQuotationId);
            return ObjectMapper.Map<ServiceQuotation, ServiceQuotationReadDto>(obj);
        }
    }
}
