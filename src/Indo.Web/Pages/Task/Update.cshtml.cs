using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Tasks
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public TaskUpdateViewModel Task { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Activities { get; set; }

        private readonly ITaskAppService _taskAppService;
        public UpdateModel(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }
        public async System.Threading.Tasks.Task OnGetAsync(Guid id)
        {
            var dto = await _taskAppService.GetAsync(id);
            Task = ObjectMapper.Map<TaskReadDto, TaskUpdateViewModel>(dto);

            var customerLookup = await _taskAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var activityLookup = await _taskAppService.GetActivityLookupAsync();
            Activities = activityLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _taskAppService.UpdateAsync(
                    Task.Id,
                    ObjectMapper.Map<TaskUpdateViewModel, TaskUpdateDto>(Task)
                    );
                return NoContent();
            }
            catch (TaskAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class TaskUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [SelectItems(nameof(Activities))]
            [DisplayName("Activity")]
            public Guid ActivityId { get; set; }

            [Required]
            [StringLength(TaskConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DisplayName("Start Date Time")]
            public DateTime StartTime { get; set; }

            [Required]
            [DisplayName("End Date Time")]
            public DateTime EndTime { get; set; }

            [DisplayName("Is Done?")]
            public bool IsDone { get; set; }

            public string Location { get; set; }
        }
    }
}
