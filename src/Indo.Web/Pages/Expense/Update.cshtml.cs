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
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ExpenseUpdateViewModel Expense { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> ExpenseTypes { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IExpenseAppService _expenseAppService;
        public UpdateModel(IExpenseAppService expenseAppService)
        {
            _expenseAppService = expenseAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _expenseAppService.GetAsync(id);
            Expense = ObjectMapper.Map<ExpenseReadDto, ExpenseUpdateViewModel>(dto);


            var employeeLookup = await _expenseAppService.GetEmployeeLookupAsync();
            Employees = employeeLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var expenseTypeLookup = await _expenseAppService.GetExpenseTypeLookupAsync();
            ExpenseTypes = expenseTypeLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var customerLookup = await _expenseAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _expenseAppService.UpdateAsync(
                    Expense.Id,
                    ObjectMapper.Map<ExpenseUpdateViewModel, ExpenseUpdateDto>(Expense)
                );
                return NoContent();

            }
            catch (ExpenseAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ExpenseUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(Employees))]
            [DisplayName("Employee")]
            public Guid EmployeeId { get; set; }

            [Required]
            [SelectItems(nameof(ExpenseTypes))]
            [DisplayName("Expense Type")]
            public Guid ExpenseTypeId { get; set; }

            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [StringLength(ExpenseConsts.MaxNumberLength)]
            public string Number { get; set; }


            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Expense Date")]
            public DateTime ExpenseDate { get; set; }
        }
    }
}
