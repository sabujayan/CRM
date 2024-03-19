using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Currencies;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Indo.Web.Pages.Currency
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CurrencyUpdateViewModel Currency { get; set; }

        private readonly ICurrencyAppService _currencyAppService;
        public UpdateModel(ICurrencyAppService currencyAppService)
        {
            _currencyAppService = currencyAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _currencyAppService.GetAsync(id);
            Currency = ObjectMapper.Map<CurrencyReadDto, CurrencyUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _currencyAppService.UpdateAsync(
                    Currency.Id,
                    ObjectMapper.Map<CurrencyUpdateViewModel, CurrencyUpdateDto>(Currency)
                    );
                return NoContent();
            }
            catch (CurrencyAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CurrencyUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(CurrencyConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            public string Symbol { get; set; }
        }
    }
}
