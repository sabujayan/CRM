using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Activities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Activity
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateActivityViewModel Activity { get; set; }

        private readonly IActivityAppService _activityAppService;
        public CreateModel(IActivityAppService activityAppService)
        {
            _activityAppService = activityAppService;
        }
        public void OnGet()
        {
            Activity = new CreateActivityViewModel();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateActivityViewModel, ActivityCreateDto>(Activity);
                await _activityAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (ActivityAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateActivityViewModel
        {
            [Required]
            [StringLength(ActivityConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }
        }
    }
}
