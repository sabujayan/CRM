using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Employees;
using Indo.Expenses;
using Indo.ExpenseTypes;
using Indo.Uoms;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Indo.ExpenseDetails
{
    public class ExpenseDetailAppService : IndoAppService, IExpenseDetailAppService
    {
        private readonly CompanyAppService _companyAppService;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IExpenseDetailRepository _expenseDetailRepository;
        private readonly ExpenseDetailManager _expenseDetailManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IExpenseTypeRepository _expenseTypeRepository;
        public ExpenseDetailAppService(
            CompanyAppService companyAppService,
            IExpenseDetailRepository expenseDetailRepository,
            ExpenseDetailManager expenseDetailManager,
            IEmployeeRepository employeeRepository,
            IExpenseTypeRepository expenseTypeRepository,
            IExpenseRepository expenseRepository)
        {
            _expenseDetailRepository = expenseDetailRepository;
            _expenseDetailManager = expenseDetailManager;
            _expenseRepository = expenseRepository;
            _companyAppService = companyAppService;
            _employeeRepository = employeeRepository;
            _expenseTypeRepository = expenseTypeRepository;
        }
        public async Task<ExpenseDetailReadDto> GetAsync(Guid id)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _expenseDetailRepository.GetQueryableAsync();
            var query = from expenseDetail in queryable
                        join expense in _expenseRepository on expenseDetail.ExpenseId equals expense.Id
                        where expenseDetail.Id == id
                        select new { expenseDetail, expense };
            var queryResult = await AsyncExecuter.FirstOrDefaultAsync(query);
            if (queryResult == null)
            {
                throw new EntityNotFoundException(typeof(ExpenseDetail), id);
            }
            var dto = ObjectMapper.Map<ExpenseDetail, ExpenseDetailReadDto>(queryResult.expenseDetail);
            dto.CurrencyName = company.CurrencyName;
            dto.PriceString = queryResult.expenseDetail.Price.ToString("##,##.00");
            return dto;
        }
        public async Task<PagedResultDto<ExpenseDetailReadDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _expenseDetailRepository.GetQueryableAsync();
            var query = from expenseDetail in queryable
                        join expense in _expenseRepository on expenseDetail.ExpenseId equals expense.Id
                        select new { expenseDetail, expense };
            query = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ExpenseDetail, ExpenseDetailReadDto>(x.expenseDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.PriceString = x.expenseDetail.Price.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = await _expenseDetailRepository.GetCountAsync();

            return new PagedResultDto<ExpenseDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<List<ExpenseDetailReadDto>> GetListDetailAsync()
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _expenseDetailRepository.GetQueryableAsync();
            var query = from expenseDetail in queryable
                        join expense in _expenseRepository on expenseDetail.ExpenseId equals expense.Id
                        join employee in _employeeRepository on expense.EmployeeId equals employee.Id
                        join expenseType in _expenseTypeRepository on expense.ExpenseTypeId equals expenseType.Id
                        select new { expenseDetail, expense, employee, expenseType };
            
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ExpenseDetail, ExpenseDetailReadDto>(x.expenseDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.ExpenseNumber = x.expense.Number;
                dto.ExpenseDate = x.expense.ExpenseDate;
                dto.PriceString = x.expenseDetail.Price.ToString("##,##.00");
                dto.EmployeeName = x.employee.Name;
                dto.ExpenseTypeName = x.expenseType.Name;
                return dto;
            })
                .OrderByDescending(x => x.ExpenseDate)
                .ToList();

            return dtos;
        }
        public async Task<PagedResultDto<ExpenseDetailReadDto>> GetListByExpenseAsync(Guid expenseId)
        {
            var company = await _companyAppService.GetDefaultCompanyAsync();
            var queryable = await _expenseDetailRepository.GetQueryableAsync();
            var query = from expenseDetail in queryable
                        join expense in _expenseRepository on expenseDetail.ExpenseId equals expense.Id
                        where expenseDetail.ExpenseId.Equals(expenseId)
                        select new { expenseDetail, expense };
            var queryResult = await AsyncExecuter.ToListAsync(query);
            var dtos = queryResult.Select(x =>
            {
                var dto = ObjectMapper.Map<ExpenseDetail, ExpenseDetailReadDto>(x.expenseDetail);
                dto.CurrencyName = company.CurrencyName;
                dto.PriceString = x.expenseDetail.Price.ToString("##,##.00");
                return dto;
            }).ToList();

            var totalCount = dtos.Count;

            return new PagedResultDto<ExpenseDetailReadDto>(
                totalCount,
                dtos
            );
        }
        public async Task<ListResultDto<ExpenseLookupDto>> GetExpenseLookupAsync()
        {
            var list = await _expenseRepository.GetListAsync();
            return new ListResultDto<ExpenseLookupDto>(
                ObjectMapper.Map<List<Expense>, List<ExpenseLookupDto>>(list)
            );
        }
        public async Task<ExpenseDetailReadDto> CreateAsync(ExpenseDetailCreateDto input)
        {
            var obj = await _expenseDetailManager.CreateAsync(
                input.ExpenseId,
                input.SummaryNote,
                input.Price
            );

            await _expenseDetailRepository.InsertAsync(obj);

            return ObjectMapper.Map<ExpenseDetail, ExpenseDetailReadDto>(obj);
        }
        public async Task UpdateAsync(Guid id, ExpenseDetailUpdateDto input)
        {
            var obj = await _expenseDetailRepository.GetAsync(id);

            obj.SummaryNote = input.SummaryNote;
            obj.Price = input.Price;

            await _expenseDetailRepository.UpdateAsync(obj);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _expenseDetailRepository.DeleteAsync(id);
        }
    }
}
