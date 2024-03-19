using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Activities;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Activity
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public ActivityUpdateViewModel Activity { get; set; }

        private readonly IActivityAppService _activityAppService;
        public UpdateModel(IActivityAppService activityAppService)
        {
            _activityAppService = activityAppService;
        }
        public async System.Threading.Tasks.Task OnGetAsync(Guid id)
        {
            var dto = await _activityAppService.GetAsync(id);
            Activity = ObjectMapper.Map<ActivityReadDto, ActivityUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _activityAppService.UpdateAsync(
                    Activity.Id,
                    ObjectMapper.Map<ActivityUpdateViewModel, ActivityUpdateDto>(Activity)
                    );
                return NoContent();
            }
            catch (ActivityAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ActivityUpdateViewModel
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
