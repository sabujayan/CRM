using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.CashAndBanks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.CashAndBank
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CashAndBankUpdateViewModel CashAndBank { get; set; }

        private readonly ICashAndBankAppService _cashAndBankAppService;
        public UpdateModel(ICashAndBankAppService cashAndBankAppService)
        {
            _cashAndBankAppService = cashAndBankAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _cashAndBankAppService.GetAsync(id);
            CashAndBank = ObjectMapper.Map<CashAndBankReadDto, CashAndBankUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _cashAndBankAppService.UpdateAsync(
                    CashAndBank.Id,
                    ObjectMapper.Map<CashAndBankUpdateViewModel, CashAndBankUpdateDto>(CashAndBank)
                    );
                return NoContent();
            }
            catch (CashAndBankAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CashAndBankUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(CashAndBankConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
