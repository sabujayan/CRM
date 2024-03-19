using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.Calendars;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using System.ComponentModel;

namespace Indo.Web.Pages.Calendar
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateCalendarViewModel Calendar { get; set; }

        private readonly ICalendarAppService _calendarAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ICalendarAppService calendarAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _calendarAppService = calendarAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGet()
        {
            Calendar = new CreateCalendarViewModel();
            Calendar.Name = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Calendar);
            Calendar.StartTime = DateTime.UtcNow;
            Calendar.EndTime = Calendar.StartTime.AddHours(1);
            Calendar.IsDone = false;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateCalendarViewModel, CalendarCreateDto>(Calendar);
                await _calendarAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (CalendarAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateCalendarViewModel
        {
            [Required]
            [StringLength(CalendarConsts.MaxNameLength)]
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
