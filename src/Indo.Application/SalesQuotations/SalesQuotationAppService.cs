using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Currencies;
using Indo.Customers;
using Indo.Employees;
using Indo.SalesOrders;
using Indo.SalesQuotationDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.SalesQuotations
{
    public class SalesQuotationAppService : IndoAppService, ISalesQuotationAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        private readonly SalesQuotationManager _salesQuotationManager;
        private readonly ISalesQuotationDetailRepository _salesQuotationDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        public SalesQuotationAppService(
            ISalesQuotationRepository salesQuotationRepository,
            SalesQuotationManager salesQuotationManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            ISalesQuotationDetailRepository salesQuotationDetailRepository,
            ICompanyAppService companyAppService)
        {
            _salesQuotationRepository = salesQuotationRepository;
            _salesQuotationManager = salesQuotationManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _employeeAppService = employeeAppService;
            _salesQuotationDetailRepository = salesQuotationDetailRepository;
            _companyAppService = companyAppService;
        }

        public async Task<QuotationCountDto> GetCountQuotationAsync()
        {
            await Task.Yield();
            var result = new QuotationCountDto();
            result.CountDraft = _salesQuotationRepository.Where(x => x.Status == SalesQuotationStatus.Draft).Count();
            result.CountConfirm = _salesQuotationRepository.Where(x => x.Status == SalesQuotationStatus.Confirm).Count();
            result.CountCancelled = _salesQuotationRepository.Where(x => x.Status == SalesQuotationStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _salesQuotationDetailRepository
                .Where(x => x.SalesQuotationId.Equals(id))
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
            result = _salesQuotationDetailRepository
                .Where(x => x.SalesQuotationId.Equals(id))
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
            result = _salesQuotationDetailRepository
                .Where(x => x.SalesQuotationId.Equals(id))
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
            result = _salesQuotationDetailRepository
                .Where(x => x.SalesQuotationId.Equals(id))
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
            result = _salesQuotationDetailRepository
                .Where(x => x.SalesQuotationId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<SalesQuotationReadDto> GetAsync(Guid id)
        {
            var queryable = await _salesQuotationRepository.GetQueryableAsync();
            var query = from salesQuotation in queryable
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesQuotation.SalesExecutiveId equals employee.Id
                        where salesQuotation.Id == id
                        select new { salesQuotation, customer, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesQuotation), id);
            }
            var dto = ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(queryResult.salesQuotation);
            dto.CustomerName = queryResult.customer.Name;
            dto.SalesExecutiveName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)queryResult.salesQuotation.Status}"];
            dto.PipelineString = L[$"Enum:SalesQuotationPipeline:{(int)queryResult.salesQuotation.Pipeline}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<SalesQuotationReadDto>> GetListAsync()
        {
            var queryable = await _salesQuotationRepository.GetQueryableAsync();
            var query = from salesQuotation in queryable
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesQuotation.SalesExecutiveId equals employee.Id
                        select new { salesQuotation, customer, employee };
                   
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(x.salesQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.PipelineString = L[$"Enum:SalesQuotationPipeline:{(int)x.salesQuotation.Pipeline}"];
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<List<SalesQuotationReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _salesQuotationRepository.GetQueryableAsync();
            var query = from salesQuotation in queryable
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesQuotation.SalesExecutiveId equals employee.Id
                        select new { salesQuotation, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(x.salesQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.PipelineString = L[$"Enum:SalesQuotationPipeline:{(int)x.salesQuotation.Pipeline}"];
                dto.Total = _salesQuotationDetailRepository.Where(y => y.SalesQuotationId.Equals(x.salesQuotation.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.QuotationDate)
                .ToList();

            return dtos;
        }
        public async Task<List<SalesQuotationReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _salesQuotationRepository.GetQueryableAsync();
            var query = from salesQuotation in queryable
                        join customer in _customerRepository on salesQuotation.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesQuotation.SalesExecutiveId equals employee.Id
                        where salesQuotation.CustomerId == customerId
                        select new { salesQuotation, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(x.salesQuotation);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesQuotationStatus:{(int)x.salesQuotation.Status}"];
                dto.PipelineString = L[$"Enum:SalesQuotationPipeline:{(int)x.salesQuotation.Pipeline}"];
                dto.Total = _salesQuotationDetailRepository.Where(y => y.SalesQuotationId.Equals(x.salesQuotation.Id)).Sum(z => z.Total);
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
        public async Task<SalesOrderReadDto> ConvertToOrderAsync(Guid salesQuotationId)
        {
            var obj = await _salesQuotationManager.ConvertToOrder(salesQuotationId);
            return ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(obj);
        }
        public async Task<SalesQuotationReadDto> CreateAsync(SalesQuotationCreateDto input)
        {
            var obj = await _salesQuotationManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.SalesExecutiveId,
                input.QuotationDate,
                input.QuotationValidUntilDate
            );

            obj.Description = input.Description;

            await _salesQuotationRepository.InsertAsync(obj);

            return ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesQuotationUpdateDto input)
        {
            var obj = await _salesQuotationRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _salesQuotationManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.SalesExecutiveId = input.SalesExecutiveId;
            obj.Description = input.Description;
            obj.QuotationDate = input.QuotationDate;
            obj.QuotationValidUntilDate = input.QuotationValidUntilDate;
            obj.Pipeline = input.Pipeline;

            await _salesQuotationRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesQuotationRepository.DeleteAsync(id);
        }

        public async Task<SalesQuotationReadDto> ConfirmAsync(Guid salesQuotationId)
        {
            var obj = await _salesQuotationManager.ConfirmAsync(salesQuotationId);
            return ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(obj);
        }

        public async Task<SalesQuotationReadDto> CancelAsync(Guid salesQuotationId)
        {
            var obj = await _salesQuotationManager.CancelAsync(salesQuotationId);
            return ObjectMapper.Map<SalesQuotation, SalesQuotationReadDto>(obj);
        }
    }
}
