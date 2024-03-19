using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Service
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ServiceUpdateViewModel Service { get; set; }
        public List<SelectListItem> Uoms { get; set; }

        private readonly IServiceAppService _serviceAppService;
        public UpdateModel(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _serviceAppService.GetAsync(id);
            Service = ObjectMapper.Map<ServiceReadDto, ServiceUpdateViewModel>(dto);


            var uomLookup = await _serviceAppService.GetUomLookupAsync();
            Uoms = uomLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _serviceAppService.UpdateAsync(
                    Service.Id,
                    ObjectMapper.Map<ServiceUpdateViewModel, ServiceUpdateDto>(Service)
                );
                return NoContent();

            }
            catch (ServiceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ServiceUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
