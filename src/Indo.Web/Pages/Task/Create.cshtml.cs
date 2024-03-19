using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Indo.Web.Pages.Tasks
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateTaskViewModel Task { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> Activities { get; set; }

        private readonly ITaskAppService _taskAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ITaskAppService taskAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _taskAppService = taskAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async System.Threading.Tasks.Task OnGet()
        {
            Task = new CreateTaskViewModel();
            Task.Name = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Task);
            Task.StartTime = DateTime.Now;
            Task.EndTime = Task.StartTime.AddHours(1);
            Task.IsDone = false;

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
                var dto = ObjectMapper.Map<CreateTaskViewModel, TaskCreateDto>(Task);
                await _taskAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (TaskAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateTaskViewModel
        {
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
