using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Expenses;
using Indo.NumberSequences;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Expense
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateExpenseViewModel Expense { get; set; }
        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> ExpenseTypes { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IExpenseAppService _expenseAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IExpenseAppService expenseAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _expenseAppService = expenseAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            Expense = new CreateExpenseViewModel();
            Expense.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Expense);
            Expense.ExpenseDate = DateTime.Now;

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
                await _expenseAppService.CreateAsync(
                    ObjectMapper.Map<CreateExpenseViewModel, ExpenseCreateDto>(Expense)
                    );
                return NoContent();

            }
            catch (ExpenseAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateExpenseViewModel
        {

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
