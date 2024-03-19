using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CashAndBank
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCashAndBankViewModel CashAndBank { get; set; }

        private readonly ICashAndBankAppService _cashAndBankAppService;
        public CreateModel(ICashAndBankAppService cashAndBankAppService)
        {
            _cashAndBankAppService = cashAndBankAppService;
        }
        public void OnGet()
        {
            CashAndBank = new CreateCashAndBankViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateCashAndBankViewModel, CashAndBankCreateDto>(CashAndBank);
                await _cashAndBankAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (CashAndBankAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCashAndBankViewModel
        {
            [Required]
            [StringLength(CashAndBankConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
