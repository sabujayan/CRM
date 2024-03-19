using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ExpenseDetails;
using Indo.Expenses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Expense
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public ExpenseDetailUpdateViewModel ExpenseDetail { get; set; }
        public List<SelectListItem> Expenses { get; set; }

        private readonly IExpenseDetailAppService _expenseDetailAppService;
        public UpdateDetailModel(IExpenseDetailAppService expenseDetailAppService)
        {
            _expenseDetailAppService = expenseDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _expenseDetailAppService.GetAsync(id);
            ExpenseDetail = ObjectMapper.Map<ExpenseDetailReadDto, ExpenseDetailUpdateViewModel>(dto);

            var expenseLookup = await _expenseDetailAppService.GetExpenseLookupAsync();
            Expenses = expenseLookup.Items
                .Where(x => x.Id.Equals(ExpenseDetail.ExpenseId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _expenseDetailAppService.UpdateAsync(
                    ExpenseDetail.Id,
                    ObjectMapper.Map<ExpenseDetailUpdateViewModel, ExpenseDetailUpdateDto>(ExpenseDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class ExpenseDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
