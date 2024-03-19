using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Resource
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateResourceViewModel Resource { get; set; }

        private readonly IResourceAppService _resourceAppService;
        public CreateModel(IResourceAppService resourceAppService)
        {
            _resourceAppService = resourceAppService;
        }
        public void OnGet()
        {
            Resource = new CreateResourceViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateResourceViewModel, ResourceCreateDto>(Resource);
                await _resourceAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (ResourceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateResourceViewModel
        {
            [Required]
            [StringLength(ResourceConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
