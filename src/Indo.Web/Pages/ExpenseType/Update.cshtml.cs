using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.ExpenseTypes;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ExpenseType
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public ExpenseTypeUpdateViewModel ExpenseType { get; set; }

        private readonly IExpenseTypeAppService _expenseTypeAppService;
        public UpdateModel(IExpenseTypeAppService expenseTypeAppService)
        {
            _expenseTypeAppService = expenseTypeAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _expenseTypeAppService.GetAsync(id);
            ExpenseType = ObjectMapper.Map<ExpenseTypeReadDto, ExpenseTypeUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _expenseTypeAppService.UpdateAsync(
                    ExpenseType.Id,
                    ObjectMapper.Map<ExpenseTypeUpdateViewModel, ExpenseTypeUpdateDto>(ExpenseType)
                    );
                return NoContent();
            }
            catch (ExpenseTypeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ExpenseTypeUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(ExpenseTypeConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
