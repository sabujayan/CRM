using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Resources;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Resource
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public ResourceUpdateViewModel Resource { get; set; }

        private readonly IResourceAppService _resourceAppService;
        public UpdateModel(IResourceAppService resourceAppService)
        {
            _resourceAppService = resourceAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _resourceAppService.GetAsync(id);
            Resource = ObjectMapper.Map<ResourceReadDto, ResourceUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _resourceAppService.UpdateAsync(
                    Resource.Id,
                    ObjectMapper.Map<ResourceUpdateViewModel, ResourceUpdateDto>(Resource)
                    );
                return NoContent();
            }
            catch (ResourceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ResourceUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(ResourceConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
