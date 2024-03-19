using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ExpenseTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ExpenseType
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateExpenseTypeViewModel ExpenseType { get; set; }

        private readonly IExpenseTypeAppService _expenseTypeAppService;
        public CreateModel(IExpenseTypeAppService expenseTypeAppService)
        {
            _expenseTypeAppService = expenseTypeAppService;
        }
        public void OnGet()
        {
            ExpenseType = new CreateExpenseTypeViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateExpenseTypeViewModel, ExpenseTypeCreateDto>(ExpenseType);
                await _expenseTypeAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (ExpenseTypeAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateExpenseTypeViewModel
        {
            [Required]
            [StringLength(ExpenseTypeConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
