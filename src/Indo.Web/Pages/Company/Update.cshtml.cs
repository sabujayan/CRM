using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Companies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Company
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CompanyUpdateViewModel Company { get; set; }
        public List<SelectListItem> Currencies { get; set; }
        public List<SelectListItem> Warehouses { get; set; }

        private readonly ICompanyAppService _companyAppService;
        public UpdateModel(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _companyAppService.GetAsync(id);
            Company = ObjectMapper.Map<CompanyReadDto, CompanyUpdateViewModel>(dto);

            var currencyLookup = await _companyAppService.GetCurrencyLookupAsync();
            Currencies = currencyLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var warehouseLookup = await _companyAppService.GetWarehouseLookupAsync();
            Warehouses = warehouseLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _companyAppService.UpdateAsync(
                    Company.Id,
                    ObjectMapper.Map<CompanyUpdateViewModel, CompanyUpdateDto>(Company)
                    );
                return NoContent();
            }
            catch (CompanyAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CompanyUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Currencies))]
            [DisplayName("Currency")]
            [Required]
            public Guid CurrencyId { get; set; }

            [SelectItems(nameof(Warehouses))]
            [DisplayName("Warehouses")]
            [Required]
            public Guid DefaultWarehouseId { get; set; }

            [Required]
            [StringLength(CompanyConsts.MaxNameLength)]
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            [TextArea]
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
        }
    }
}
