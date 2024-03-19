using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.ImportantDates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Indo.Web.Pages.ImportantDate
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateImportantDateViewModel ImportantDate { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IImportantDateAppService _importantDateAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IImportantDateAppService importantDateAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _importantDateAppService = importantDateAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGet()
        {
            ImportantDate = new CreateImportantDateViewModel();
            ImportantDate.Name = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.ImportantDate);
            ImportantDate.StartTime = DateTime.Now;
            ImportantDate.EndTime = ImportantDate.StartTime.AddHours(1);

            var customerLookup = await _importantDateAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var dto = ObjectMapper.Map<CreateImportantDateViewModel, ImportantDateCreateDto>(ImportantDate);
                await _importantDateAppService.CreateAsync(dto);
                return NoContent();
            }
            catch (ImportantDateAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateImportantDateViewModel
        {
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [StringLength(ImportantDateConsts.MaxNameLength)]
            public string Name { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DisplayName("Start Date Time")]
            public DateTime StartTime { get; set; }

            [Required]
            [DisplayName("End Date Time")]
            public DateTime EndTime { get; set; }

            public string Location { get; set; }
        }
    }
}
