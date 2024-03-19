using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.LeadRatings;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.LeadRating
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public LeadRatingUpdateViewModel LeadRating { get; set; }

        private readonly ILeadRatingAppService _leadRatingAppService;
        public UpdateModel(ILeadRatingAppService leadRatingAppService)
        {
            _leadRatingAppService = leadRatingAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _leadRatingAppService.GetAsync(id);
            LeadRating = ObjectMapper.Map<LeadRatingReadDto, LeadRatingUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _leadRatingAppService.UpdateAsync(
                    LeadRating.Id,
                    ObjectMapper.Map<LeadRatingUpdateViewModel, LeadRatingUpdateDto>(LeadRating)
                    );
                return NoContent();
            }
            catch (LeadRatingAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class LeadRatingUpdateViewModel
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
