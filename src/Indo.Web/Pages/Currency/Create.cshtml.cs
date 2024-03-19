using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Currencies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;

namespace Indo.Web.Pages.Currency
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCurrencyViewModel Currency { get; set; }

        private readonly ICurrencyAppService _currencyAppService;
        public CreateModel(ICurrencyAppService currencyAppService)
        {
            _currencyAppService = currencyAppService;
        }
        public void OnGet()
        {
            Currency = new CreateCurrencyViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateCurrencyViewModel, CurrencyCreateDto>(Currency);
                await _currencyAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (CurrencyAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCurrencyViewModel
        {
            [Required]
            [StringLength(CurrencyConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            public string Symbol { get; set; }
        }
    }
}
