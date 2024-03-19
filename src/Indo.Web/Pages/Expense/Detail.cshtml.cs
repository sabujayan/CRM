using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Expenses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Expense
{
    public class DetailModel : IndoPageModel
    {
        public DetailExpenseViewModel Expense { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> ExpenseTypes { get; set; }

        private readonly IExpenseAppService _expenseAppService;
        public DetailModel(IExpenseAppService expenseAppService)
        {
            _expenseAppService = expenseAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _expenseAppService.GetAsync(id);
            Expense = ObjectMapper.Map<ExpenseReadDto, DetailExpenseViewModel>(dto);

            var customerLookup = await _expenseAppService.GetEmployeeLookupAsync();
            Employees = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _expenseAppService.GetExpenseTypeLookupAsync();
            ExpenseTypes = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public class DetailExpenseViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Employees))]
            [DisplayName("Employee")]
            public Guid EmployeeId { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeStreet { get; set; }
            public string EmployeeCity { get; set; }
            public string EmployeeState { get; set; }
            public string EmployeeZipCode { get; set; }
            public string EmployeePhone { get; set; }
            public string EmployeeEmail { get; set; }

            [SelectItems(nameof(ExpenseTypes))]
            [DisplayName("Expense Type")]
            public Guid ExpenseTypeId { get; set; }
            public string ExpenseTypeName { get; set; }

            [StringLength(ExpenseConsts.MaxNumberLength)]
            public string Number { get; set; }
            public string Description { get; set; }
            public DateTime ExpenseDate { get; set; }
            public string CurrencyName { get; set; }
        }


	}
}
