using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Customers;
using Indo.Employees;
using Indo.ExpenseDetails;
using Indo.ExpenseTypes;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.Expenses
{
    public class ExpenseAppService : IndoAppService, IExpenseAppService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ExpenseManager _expenseManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        private readonly CompanyAppService _companyAppService;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly ICustomerRepository _customerRepository;
        public ExpenseAppService(
            IExpenseRepository expenseRepository,
            ExpenseManager expenseManager,
            IExpenseTypeRepository expenseTypeRepository,
            IEmployeeRepository employeeRepository,
            CompanyAppService companyAppService,
            IExpenseDetailRepository expenseDetailRepository,
            ICustomerRepository customerRepository
            )
        {
            _expenseRepository = expenseRepository;
            _expenseManager = expenseManager;
            _expenseTypeRepository = expenseTypeRepository;
            _employeeRepository = employeeRepository;
            _companyAppService = companyAppService;
            _expenseDetailRepository = expenseDetailRepository;
            _customerRepository = customerRepository;
        }
        public async Task<ExpenseReadDto> GetAsync(Guid id)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join employee in _employeeRepository on expense.EmployeeId equals employee.Id
                        join expenseType in _expenseTypeRepository on expense.ExpenseTypeId equals expenseType.Id
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        where expense.Id == id
                        select new { expense, employee, expenseType, customer };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(Expense), id);
            }
            var dto = ObjectMapper.Map<Expense, ExpenseReadDto>(queryResult.expense);
            dto.EmployeeName = queryResult.employee.Name;
            dto.EmployeeStreet = queryResult.employee.Street;
            dto.EmployeeCity = queryResult.employee.City;
            dto.EmployeeState = queryResult.employee.State;
            dto.EmployeeZipCode = queryResult.employee.ZipCode;
            dto.EmployeePhone = queryResult.employee.Phone;
            dto.EmployeeEmail = queryResult.employee.Email;
            dto.ExpenseTypeName = queryResult.expenseType.Name;
            dto.CustomerName = queryResult.customer.Name;

            var defaultCompany = await _companyAppService.GetDefaultCompanyAsync();
            dto.CurrencyName = defaultCompany.CurrencyName;

            return dto;
        }
        public async Task<List<ExpenseReadDto>> GetListAsync()
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join employee in _employeeRepository on expense.EmployeeId equals employee.Id
                        join expenseType in _expenseTypeRepository on expense.ExpenseTypeId equals expenseType.Id
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        select new { expense, employee, expenseType, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Expense, ExpenseReadDto>(x.expense);
                dto.EmployeeName = x.employee.Name;
                dto.ExpenseTypeName = x.expenseType.Name;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<List<ExpenseReadDto>> GetListByCustomerAsync(Guid customerId)
        {
            var queryable = await _expenseRepository.GetQueryableAsync();
            var query = from expense in queryable
                        join employee in _employeeRepository on expense.EmployeeId equals employee.Id
                        join expenseType in _expenseTypeRepository on expense.ExpenseTypeId equals expenseType.Id
                        join customer in _customerRepository on expense.CustomerId equals customer.Id
                        where expense.CustomerId == customerId
                        select new { expense, employee, expenseType, customer };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<Expense, ExpenseReadDto>(x.expense);
                dto.EmployeeName = x.employee.Name;
                dto.ExpenseTypeName = x.expenseType.Name;
                dto.CustomerName = x.customer.Name;
                return dto;
            }).ToList();
            return dtos;
        }
        public async Task<float> GetSummaryTotalAsync(Guid id)
        {
            await Task.Yield();
            var result = 0f;
            result = _expenseDetailRepository
                .Where(x => x.ExpenseId.Equals(id))
                .Sum(x => x.Price);
            return result;
        }
        public async Task<string> GetSummaryTotalInStringAsync(Guid id)
        {
            var result = await GetSummaryTotalAsync(id);
            return result.ToString("##,##.00");
        }
        public async Task<ListResultDto<EmployeeLookupDto>> GetEmployeeLookupAsync()
        {
            var list = await _employeeRepository.GetListAsync();
            return new ListResultDto<EmployeeLookupDto>(
                ObjectMapper.Map<List<Employee>, List<EmployeeLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<ExpenseTypeLookupDto>> GetExpenseTypeLookupAsync()
        {
            var list = await _expenseTypeRepository.GetListAsync();
            return new ListResultDto<ExpenseTypeLookupDto>(
                ObjectMapper.Map<List<ExpenseType>, List<ExpenseTypeLookupDto>>(list)
            );
        }
        public async Task<ListResultDto<CustomerLookupDto>> GetCustomerLookupAsync()
        {
            var list = await _customerRepository.GetListAsync();
            return new ListResultDto<CustomerLookupDto>(
                ObjectMapper.Map<List<Customer>, List<CustomerLookupDto>>(list)
            );
        }
        public async Task<ExpenseReadDto> CreateAsync(ExpenseCreateDto input)
        {
            var obj = await _expenseManager.CreateAsync(
                input.Number,
                input.ExpenseDate,
                input.EmployeeId,
                input.ExpenseTypeId,
                input.CustomerId
            );

            obj.Description = input.Description;

            await _expenseRepository.InsertAsync(obj);

            return ObjectMapper.Map<Expense, ExpenseReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ExpenseUpdateDto input)
        {
            var obj = await _expenseRepository.GetAsync(id);

            if (obj.Number != input.Number)
            {
                await _expenseManager.ChangeNumberAsync(obj, input.Number);
            }

            obj.Description = input.Description;
            obj.ExpenseDate = input.ExpenseDate;
            obj.EmployeeId = input.EmployeeId;
            obj.ExpenseTypeId = input.ExpenseTypeId;
            obj.CustomerId = input.CustomerId;

            await _expenseRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _expenseRepository.DeleteAsync(id);
        }
    }
}
