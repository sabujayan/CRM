using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ImportantDates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ImportantDate
{
    public class UpdateModel : IndoPageModel
    {        

        [BindProperty]
        public ImportantDateUpdateViewModel ImportantDate { get; set; }
        public List<SelectListItem> Customers { get; set; }

        private readonly IImportantDateAppService _importantDateAppService;
        public UpdateModel(IImportantDateAppService importantDateAppService)
        {
            _importantDateAppService = importantDateAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _importantDateAppService.GetAsync(id);
            ImportantDate = ObjectMapper.Map<ImportantDateReadDto, ImportantDateUpdateViewModel>(dto);

            var customerLookup = await _importantDateAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _importantDateAppService.UpdateAsync(
                    ImportantDate.Id,
                    ObjectMapper.Map<ImportantDateUpdateViewModel, ImportantDateUpdateDto>(ImportantDate)
                    );
                return NoContent();
            }
            catch (ImportantDateAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class ImportantDateUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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
