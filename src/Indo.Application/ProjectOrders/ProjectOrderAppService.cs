using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.CustomerInvoices;
using Indo.Customers;
using Indo.Employees;
using Indo.ProjectOrderDetails;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ProjectOrders
{
    public class ProjectOrderAppService : IndoAppService, IProjectOrderAppService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IProjectOrderRepository _projectOrderRepository;
        private readonly ProjectOrderManager _projectOrderManager;
        private readonly IProjectOrderDetailRepository _projectOrderDetailRepository;
        private readonly ICompanyAppService _companyAppService;
        public ProjectOrderAppService(
            IProjectOrderRepository projectOrderRepository,
            ProjectOrderManager projectOrderManager,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeAppService employeeAppService,
            IProjectOrderDetailRepository projectOrderDetailRepository,
            ICompanyAppService companyAppService)
        {
            _projectOrderRepository = projectOrderRepository;
            _projectOrderManager = projectOrderManager;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _projectOrderDetailRepository = projectOrderDetailRepository;
            _companyAppService = companyAppService;
            _employeeAppService = employeeAppService;
        }

        public async Task<OrderCountDto> GetCountOrderAsync()
        {
            await Task.Yield();
            var result = new OrderCountDto();
            result.CountDraft = _projectOrderRepository.Where(x => x.Status == ProjectOrderStatus.Draft).Count();
            result.CountConfirm = _projectOrderRepository.Where(x => x.Status == ProjectOrderStatus.Confirm).Count();
            result.CountCancelled = _projectOrderRepository.Where(x => x.Status == ProjectOrderStatus.Cancelled).Count();
            return result;
        }
        public async Task<float> GetSummaryTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _projectOrderDetailRepository
                .Where(x => x.ProjectOrderId.Equals(id))
                .Sum(x => x.Total);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
                yield return day;
        }
        public async Task<List<MonthlyEarningDto>> GetListConfirmAndCancelledMonthlyEarning(int monthsCount)
        {
            var result = new List<MonthlyEarningDto>();
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-monthsCount);
            foreach (var item in EachMonth(startDate, endDate))
            {
                result.Add(new MonthlyEarningDto
                {
                    MonthName = item.ToString("MMM"),
                    Amount = await GetSummaryTotalConfirmByYearMonthAsync(item.Year, item.Month),
                    AmountConfirm = await GetSummaryTotalConfirmByYearMonthAsync(item.Year, item.Month),
                    AmountCancelled = await GetSummaryTotalCancelByYearMonthAsync(item.Year, item.Month)
                });
            }
            return result;
        }
        public async Task<List<MonthlyEarningDto>> GetListConfirmMonthlyEarning(int monthsCount)
        {
            var result = new List<MonthlyEarningDto>();
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-monthsCount);
            foreach (var item in EachMonth(startDate, endDate))
            {
                var obj = new MonthlyEarningDto
                {
                    MonthName = item.ToString("MMM"),
                    Amount = await GetSummaryTotalConfirmByYearMonthAsync(item.Year, item.Month),
                    AmountConfirm = await GetSummaryTotalConfirmByYearMonthAsync(item.Year, item.Month),
                    AmountCancelled = await GetSummaryTotalCancelByYearMonthAsync(item.Year, item.Month)
                };
                result.Add(obj);
            }
            return result;
        }
        public async Task<float> GetSummaryTotalByYearMonthAsync(int year, int month)
        {
            var result = 0f;
            var projectOrders = _projectOrderRepository
                .Where(x => x.OrderDate.Year.Equals(year) && x.OrderDate.Month.Equals(month))
                .ToList();
            foreach (var item in projectOrders)
            {
                result = result + await GetSummaryTotalAsync(item.Id);
            }
            return result;
        }
        private async Task<float> GetSummaryTotalConfirmByYearMonthAsync(int year, int month)
        {
            var result = 0f;
            var projectOrders = _projectOrderRepository
                .Where(x => x.OrderDate.Year.Equals(year) && x.OrderDate.Month.Equals(month) && x.Status == ProjectOrderStatus.Confirm)
                .ToList();
            foreach (var item in projectOrders)
            {
                result = result + await GetSummaryTotalAsync(item.Id);
            }
            return result;
        }
        private async Task<float> GetSummaryTotalCancelByYearMonthAsync(int year, int month)
        {
            var result = 0f;
            var projectOrders = _projectOrderRepository
                .Where(x => x.OrderDate.Year.Equals(year) && x.OrderDate.Month.Equals(month) && x.Status == ProjectOrderStatus.Cancelled)
                .ToList();
            foreach (var item in projectOrders)
            {
                result = result + await GetSummaryTotalAsync(item.Id);
            }
            return result;
        }
        public async Task<ProjectOrderReadDto> GetAsync(Guid id)
        {
            var queryable = await _projectOrderRepository.GetQueryableAsync();
            var query = from projectOrder in queryable
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on projectOrder.SalesExecutiveId equals employee.Id
                        where projectOrder.Id == id
                        select new { projectOrder, customer, employee };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ProjectOrder), id);
            }
            var dto = ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(queryResult.projectOrder);
            dto.CustomerName = queryResult.customer.Name;
            dto.CustomerStreet = queryResult.customer.Street;
            dto.CustomerCity = queryResult.customer.City;
            dto.CustomerState = queryResult.customer.State;
            dto.CustomerZipCode = queryResult.customer.ZipCode;
            dto.CustomerPhone = queryResult.customer.Phone;
            dto.CustomerEmail = queryResult.customer.Email;
            dto.SalesExecutiveName = queryResult.employee.Name;
            dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)queryResult.projectOrder.Status}"];

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<ProjectOrderReadDto>> GetListAsync()
        {
            var queryable = await _projectOrderRepository.GetQueryableAsync();
            var query = from projectOrder in queryable
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on projectOrder.SalesExecutiveId equals employee.Id
                        select new { projectOrder, customer, employee };
           
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(x.projectOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ProjectOrderReadDto>> GetListWithTotalAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _projectOrderRepository.GetQueryableAsync();
            var query = from projectOrder in queryable
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on projectOrder.SalesExecutiveId equals employee.Id
                        select new { projectOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(x.projectOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                dto.Total = _projectOrderDetailRepository.Where(y => y.ProjectOrderId.Equals(x.projectOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ProjectOrderReadDto>> GetListWithTotalByCustomerAsync(Guid customerId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();

            var queryable = await _projectOrderRepository.GetQueryableAsync();
            var query = from projectOrder in queryable
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on projectOrder.SalesExecutiveId equals employee.Id
                        where projectOrder.CustomerId == customerId
                        select new { projectOrder, customer, employee };

            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(x.projectOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                dto.Total = _projectOrderDetailRepository.Where(y => y.ProjectOrderId.Equals(x.projectOrder.Id)).Sum(z => z.Total);
                dto.CurrencyName = company.CurrencyName;
                return dto;
            })
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return dtos;
        }
        public async Task<List<ProjectOrderReadDto>> GetListLastFiveOrderAsync()
        {
            var queryable = await _projectOrderRepository.GetQueryableAsync();
            var query = from projectOrder in queryable
                        join customer in _customerRepository on projectOrder.CustomerId equals customer.Id
                        join employee in _employeeRepository on projectOrder.SalesExecutiveId equals employee.Id
                        select new { projectOrder, customer, employee };
            query = query
                .OrderByDescending(x => x.projectOrder.CreationTime)
                .Take(5);
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(x.projectOrder);
                dto.CustomerName = x.customer.Name;
                dto.SalesExecutiveName = x.employee.Name;
                dto.StatusString = L[$"Enum:ProjectOrderStatus:{(int)x.projectOrder.Status}"];
                return dto;
            }).ToList();

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
        public async Task<ListResultDto<RatingLookupDto>> GetRatingLookupAsync()
        {
            await Task.Yield();

            var result = from ProjectOrderRating x in Enum.GetValues(typeof(ProjectOrderRating))
                             select new RatingLookupDto { Id = (int)x, Name = x.ToString() };

            return new ListResultDto<RatingLookupDto>(result.ToList());
        }
        public async Task<ProjectOrderReadDto> CreateAsync(ProjectOrderCreateDto input)
        {
            var obj = await _projectOrderManager.CreateAsync(
                input.Number,
                input.CustomerId,
                input.SalesExecutiveId,
                input.OrderDate
            );

            obj.Description = input.Description;
            obj.Rating = input.Rating;

            await _projectOrderRepository.InsertAsync(obj);

            return ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ProjectOrderUpdateDto input)
        {
            var obj = await _projectOrderRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _projectOrderManager.ChangeNameAsync(obj, input.Number);
            }

            obj.CustomerId = input.CustomerId;
            obj.SalesExecutiveId = input.SalesExecutiveId;
            obj.Description = input.Description;
            obj.Rating = input.Rating;
            obj.OrderDate = input.OrderDate;

            await _projectOrderRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _projectOrderRepository.DeleteAsync(id);
        }

        public async Task<ProjectOrderReadDto> ConfirmAsync(Guid projectOrderId)
        {
            var obj = await _projectOrderManager.ConfirmAsync(projectOrderId);
            return ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(obj);
        }

        public async Task<ProjectOrderReadDto> CancelAsync(Guid projectOrderId)
        {
            var obj = await _projectOrderManager.CancelAsync(projectOrderId);
            return ObjectMapper.Map<ProjectOrder, ProjectOrderReadDto>(obj);
        }
        public async Task<CustomerInvoiceReadDto> GenerateInvoiceAsync(Guid projectOrderId)
        {
            var obj = await _projectOrderManager.GenerateInvoice(projectOrderId);
            return ObjectMapper.Map<CustomerInvoice, CustomerInvoiceReadDto>(obj);
        }
    }
}
