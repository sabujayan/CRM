using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Service
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateServiceViewModel Service { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IServiceAppService _serviceAppService;
        public CreateModel(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }
        public async Task OnGetAsync()
        {
            Service = new CreateServiceViewModel();

            var uomLookup = await _serviceAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _serviceAppService.CreateAsync(
                    ObjectMapper.Map<CreateServiceViewModel, ServiceCreateDto>(Service)
                    );
                return NoContent();

            }
            catch (ServiceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateServiceViewModel
        {
            [SelectItems(nameof(Uoms))]
            [DisplayName("UoM")]
            public Guid UomId { get; set; }

            [Required]
            [StringLength(ServiceConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            public float Price { get; set; }

            [Required]
            [DisplayName("Tax Rate")]
            public float TaxRate { get; set; }
        }
    }
}
