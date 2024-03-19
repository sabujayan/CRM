using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.LeadSources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.LeadSource
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateLeadSourceViewModel LeadSource { get; set; }

        private readonly ILeadSourceAppService _leadSourceAppService;
        public CreateModel(ILeadSourceAppService leadSourceAppService)
        {
            _leadSourceAppService = leadSourceAppService;
        }
        public void OnGet()
        {
            LeadSource = new CreateLeadSourceViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateLeadSourceViewModel, LeadSourceCreateDto>(LeadSource);
                await _leadSourceAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (LeadSourceAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateLeadSourceViewModel
        {
            [Required]
            [StringLength(LeadSourceConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
