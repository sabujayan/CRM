using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ExpenseDetails;
using Indo.Expenses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Expense
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateExpenseDetailViewModel ExpenseDetail { get; set; }
        public List<SelectListItem> Expenses { get; set; }

        private readonly IExpenseDetailAppService _expenseDetailAppService;
        public CreateDetailModel(IExpenseDetailAppService expenseDetailAppService)
        {
            _expenseDetailAppService = expenseDetailAppService;
        }
        public async Task OnGetAsync(Guid expenseId)
        {
            ExpenseDetail = new CreateExpenseDetailViewModel();
            ExpenseDetail.ExpenseId = expenseId;

            var expenseLookup = await _expenseDetailAppService.GetExpenseLookupAsync();
            Expenses = expenseLookup.Items
                .Where(x => x.Id.Equals(expenseId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _expenseDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateExpenseDetailViewModel, ExpenseDetailCreateDto>(ExpenseDetail)
                    );
                return NoContent();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Posting Error: {ex.InnerException}");
            }
        }
        public class CreateExpenseDetailViewModel
        {
            [Required]
            [SelectItems(nameof(Expenses))]
            [DisplayName("Expense")]
            public Guid ExpenseId { get; set; }

            [Required]
            [DisplayName("Summary Note")]
            public string SummaryNote { get; set; }

            [Required]
            public float Price { get; set; }
        }
    }
}
