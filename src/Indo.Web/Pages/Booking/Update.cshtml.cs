using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.Bookings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.Booking
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public BookingUpdateViewModel Booking { get; set; }
        public List<SelectListItem> Resources { get; set; }

        private readonly IBookingAppService _bookingAppService;
        public UpdateModel(IBookingAppService bookingAppService)
        {
            _bookingAppService = bookingAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _bookingAppService.GetAsync(id);
            Booking = ObjectMapper.Map<BookingReadDto, BookingUpdateViewModel>(dto);

            var resourceLookup = await _bookingAppService.GetResourceLookupAsync();
            Resources = resourceLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _bookingAppService.UpdateAsync(
                    Booking.Id,
                    ObjectMapper.Map<BookingUpdateViewModel, BookingUpdateDto>(Booking)
                    );
                return NoContent();
            }
            catch (BookingAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class BookingUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
