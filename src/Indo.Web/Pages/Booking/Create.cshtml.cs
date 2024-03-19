using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Indo.Web.Pages.Booking
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateBookingViewModel Booking { get; set; }
        public List<SelectListItem> Resources { get; set; }

        private readonly IBookingAppService _bookingAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IBookingAppService bookingAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _bookingAppService = bookingAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGet()
        {
            Booking = new CreateBookingViewModel();
            Booking.Name = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.Booking);
            Booking.StartTime = DateTime.Now;
            Booking.EndTime = Booking.StartTime.AddHours(1);
            Booking.IsDone = false;

            var resourceLookup = await _bookingAppService.GetResourceLookupAsync();
            Resources = resourceLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateBookingViewModel, BookingCreateDto>(Booking);
                await _bookingAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (BookingAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateBookingViewModel
        {
            [SelectItems(nameof(Resources))]
            [DisplayName("Resource")]
            public Guid ResourceId { get; set; }

            [Required]
            [StringLength(BookingConsts.MaxNameLength)]
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
