using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.LeadSources;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.LeadSource
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public LeadSourceUpdateViewModel LeadSource { get; set; }

        private readonly ILeadSourceAppService _leadSourceAppService;
        public UpdateModel(ILeadSourceAppService leadSourceAppService)
        {
            _leadSourceAppService = leadSourceAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _leadSourceAppService.GetAsync(id);
            LeadSource = ObjectMapper.Map<LeadSourceReadDto, LeadSourceUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _leadSourceAppService.UpdateAsync(
                    LeadSource.Id,
                    ObjectMapper.Map<LeadSourceUpdateViewModel, LeadSourceUpdateDto>(LeadSource)
                    );
                return NoContent();
            }
            catch (LeadSourceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class LeadSourceUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
