using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Indo.Calendars;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Calendar
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public CalendarUpdateViewModel Calendar { get; set; }

        private readonly ICalendarAppService _calendarAppService;
        public UpdateModel(ICalendarAppService calendarAppService)
        {
            _calendarAppService = calendarAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _calendarAppService.GetAsync(id);
            Calendar = ObjectMapper.Map<CalendarReadDto, CalendarUpdateViewModel>(dto);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _calendarAppService.UpdateAsync(
                    Calendar.Id,
                    ObjectMapper.Map<CalendarUpdateViewModel, CalendarUpdateDto>(Calendar)
                    );
                return NoContent();
            }
            catch (CalendarAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CalendarUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
