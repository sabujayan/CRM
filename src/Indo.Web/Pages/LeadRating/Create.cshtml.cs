using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.LeadRatings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.LeadRating
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateLeadRatingViewModel LeadRating { get; set; }

        private readonly ILeadRatingAppService _leadRatingAppService;
        public CreateModel(ILeadRatingAppService leadRatingAppService)
        {
            _leadRatingAppService = leadRatingAppService;
        }
        public void OnGet()
        {
            LeadRating = new CreateLeadRatingViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateLeadRatingViewModel, LeadRatingCreateDto>(LeadRating);
                await _leadRatingAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (LeadRatingAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateLeadRatingViewModel
        {
            [Required]
            [StringLength(LeadRatingConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
