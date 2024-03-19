using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Indo.Employees;
using Indo.Expenses;
using Indo.ExpenseDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Application.Dtos;

namespace Indo.Web.Pages.Expense
{
    public class PrintDetailModel : IndoPageModel
    {
        [BindProperty]
        public ExpenseViewModel Expense { get; set; }
        public CompanyViewModel Company { get; set; }        
        public EmployeeViewModel Employee { get; set; }
        public PagedResultDto<ExpenseDetailViewModel> Details { get; set; }
        public float Total { get; set; }

        private readonly IExpenseAppService _expenseAppService;

        private readonly IExpenseDetailAppService _expenseDetailAppService;

        private readonly IEmployeeAppService _employeeAppService;

        private readonly ICompanyAppService _companyAppService;
        public PrintDetailModel(
            IExpenseAppService expenseAppService,
            IExpenseDetailAppService expenseDetailAppService,
            CompanyAppService companyAppService,
            IEmployeeAppService employeeAppService
            )
        {
            _expenseAppService = expenseAppService;
            _companyAppService = companyAppService;
            _employeeAppService = employeeAppService;
            _expenseDetailAppService = expenseDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            Expense = ObjectMapper.Map<ExpenseReadDto, ExpenseViewModel>(await _expenseAppService.GetAsync(id));

            Company = ObjectMapper.Map<CompanyReadDto, CompanyViewModel>(await _companyAppService.GetDefaultCompanyAsync());

            Employee = ObjectMapper.Map<EmployeeReadDto, EmployeeViewModel>(await _employeeAppService.GetAsync(Expense.EmployeeId));

            Total = await _expenseAppService.GetSummaryTotalAsync(Expense.Id);

            var dtos = await _expenseDetailAppService.GetListByExpenseAsync(Expense.Id);

            Details = ObjectMapper.Map<PagedResultDto<ExpenseDetailReadDto>, PagedResultDto<ExpenseDetailViewModel>>(dtos);

        }

        public class ExpenseViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }
            public Guid EmployeeId { get; set; }
            public Guid ExpenseTypeId { get; set; }
            public string Number { get; set; }
            public string ExpenseTypeName { get; set; }
            public string Description { get; set; }
            public DateTime ExpenseDate { get; set; }
            public string CurrencyName { get; set; }
        }

        public class CompanyViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public Guid CurrencyId { get; set; }
            public string CurrencyName { get; set; }
        }

        public class EmployeeViewModel
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }

        public class ExpenseDetailViewModel
        {
            public Guid ExpenseId { get; set; }
            public string SummaryNote { get; set; }
            public float Price { get; set; }
            public string CurrencyName { get; set; }

        }
    }
}
