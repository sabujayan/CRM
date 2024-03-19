using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.Products;
using Indo.SalesDeliveries;
using Indo.Employees;
using Indo.SalesOrderDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Indo.SalesQuotations;
using Indo.CustomerInvoices;

namespace Indo.SalesOrders
{
    public class SalesOrderAppService : IndoAppService, ISalesOrderAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly SalesOrderManager _salesOrderManager;
        private readonly ISalesOrderDetailRepository _salesOrderDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        private readonly SalesDeliveryManager _salesDeliveryManager;
        private readonly IProductRepository _productRepository;
        private readonly ISalesQuotationRepository _salesQuotationRepository;
        public SalesOrderAppService(
            ISalesOrderRepository salesOrderRepository,
            SalesOrderManager salesOrderManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            ISalesOrderDetailRepository salesOrderDetailRepository,
            SalesDeliveryManager salesDeliveryManager,
            IProductRepository productRepository,
            ICompanyAppService companyAppService,
            ISalesQuotationRepository salesQuotationRepository
            )
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderManager = salesOrderManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _employeeAppService = employeeAppService;
            _salesOrderDetailRepository = salesOrderDetailRepository;
            _companyAppService = companyAppService;
            _salesDeliveryManager = salesDeliveryManager;
            _productRepository = productRepository;
            _salesQuotationRepository = salesQuotationRepository;
        }

        public async Task<OrderCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new OrderCountDto();
            result.CountDraft = _salesOrderRepository.Where(x => x.Status == SalesOrderStatus.Draft).Count();
            result.CountConfirm = _salesOrderRepository.Where(x => x.Status == SalesOrderStatus.Confirm).Count();
            result.CountCancelled = _salesOrderRepository.Where(x => x.Status == SalesOrderStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummarySubTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
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
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
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
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
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
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
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
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<float> GetTotalCOGSAsync()
        {
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join salesOrderDetail in _salesOrderDetailRepository on salesOrder.Id equals salesOrderDetail.SalesOrderId
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        where salesOrder.Status == SalesOrderStatus.Confirm
                        select new { salesOrder, salesOrderDetail, product };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var result = queryResult.Sum(x => x.product.Price * x.salesOrderDetail.Quantity);
            return result;
        }
        public async Task<float> GetTotalSalesAsync()
        {
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join salesOrderDetail in _salesOrderDetailRepository on salesOrder.Id equals salesOrderDetail.SalesOrderId
                        join product in _productRepository on salesOrderDetail.ProductId equals product.Id
                        where salesOrder.Status == SalesOrderStatus.Confirm
                        select new { salesOrder, salesOrderDetail, product };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var result = queryResult.Sum(x => x.product.RetailPrice * x.salesOrderDetail.Quantity);
            return result;
        }
        public async Task<float> GetTotalQtyAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _salesOrderDetailRepository
                .Where(x => x.SalesOrderId.Equals(id))
                .Sum(x => x.Quantity);
            return result;
        }
        public async Task<float> GetTotalQtyByYearMonthAsync(int year, int month)
        {
            var result = 0f;
            var lists = _salesOrderRepository
                .Where(x => x.OrderDate.Year.Equals(year) && x.OrderDate.Month.Equals(month))
                .ToList();
            foreach (var item in lists)
            {
                result = result + await GetTotalQtyAsync(item.Id);
            }
            return result;
        }
        public async Task<SalesOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesOrder.SalesExecutiveId equals employee.Id
                        where salesOrder.Id == id
                        select new { salesOrder, customer, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(SalesOrder), id);
            }
            var dto = ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(queryResult.salesOrder);
            dto.CustomerName = queryResult.customer.Name;
            dto.SalesExecutiveName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)queryResult.salesOrder.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<SalesOrderReadDto>> GetListAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesOrder.SalesExecutiveId equals employee.Id
                        select new { salesOrder, customer, employee };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(x.salesOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<SalesOrderReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesOrder.SalesExecutiveId equals employee.Id
                        select new { salesOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(x.salesOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.Total = _salesOrderDetailRepository.Where(y => y.SalesOrderId.Equals(x.salesOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;

                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<SalesOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesOrder.SalesExecutiveId equals employee.Id
                        where salesOrder.CustomerId == customerId
                        select new { salesOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(x.salesOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.Total = _salesOrderDetailRepository.Where(y => y.SalesOrderId.Equals(x.salesOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;

                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<SalesOrderReadDto>> GetListWithTotalByQuotationAsync(Guid salesQuotationId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _salesOrderRepository.GetQueryableAsync();
            var query = from salesOrder in queryable
                        join customer in _customerRepository on salesOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on salesOrder.SalesExecutiveId equals employee.Id
                        where salesOrder.SourceDocumentId == salesQuotationId
                        select new { salesOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(x.salesOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:SalesOrderStatus:{(int)x.salesOrder.Status}"];
                dto.Total = _salesOrderDetailRepository.Where(y => y.SalesOrderId.Equals(x.salesOrder.Id)).Sum(z => z.Total);
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
        public async Task<SalesOrderReadDto> CreateAsync(SalesOrderCreateDto input)
        {
            var obj = await _salesOrderManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.SalesExecutiveId,
                input.OrderDate
            );

            obj.Description = input.Description;

            await _salesOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, SalesOrderUpdateDto input)
        {
            var obj = await _salesOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _salesOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.SalesExecutiveId = input.SalesExecutiveId;
            obj.Description = input.Description;
            obj.OrderDate = input.OrderDate;

            await _salesOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _salesOrderRepository.DeleteAsync(id);
        }
        public async Task<SalesOrderReadDto> ConfirmAsync(Guid salesOrderId)
        {
            var obj = await _salesOrderManager.ConfirmAsync(salesOrderId);
            return ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(obj);
        }
        public async Task<SalesDeliveryReadDto> GenerateConfirmDeliveryAsync(Guid salesOrderId)
        {
            var delivery = await _salesDeliveryManager.GenerateSalesDeliveryFromSalesAsync(salesOrderId);
            await _salesDeliveryManager.ConfirmSalesDeliveryAsync(delivery.Id);
            return ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(delivery);
        }
        public async Task<SalesDeliveryReadDto> GenerateDraftDeliveryAsync(Guid salesOrderId)
        {
            var delivery = await _salesDeliveryManager.GenerateSalesDeliveryFromSalesAsync(salesOrderId);
            return ObjectMapper.Map<SalesDelivery, SalesDeliveryReadDto>(delivery);
        }
        public async Task<SalesOrderReadDto> CancelAsync(Guid salesOrderId)
        {
            var obj = await _salesOrderManager.CancelAsync(salesOrderId);
            return ObjectMapper.Map<SalesOrder, SalesOrderReadDto>(obj);
        }
        public async Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid salesOrderId)
        {
            var obj = await _salesOrderManager.GenerateInvoice(salesOrderId);
            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }
    }
}
