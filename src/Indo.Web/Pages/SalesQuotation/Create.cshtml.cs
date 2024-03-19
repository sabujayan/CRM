using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.SalesQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesQuotation
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateSalesQuotationViewModel SalesQuotation { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly ISalesQuotationAppService _salesQuotationAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            ISalesQuotationAppService salesQuotationAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _salesQuotationAppService = salesQuotationAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            SalesQuotation = new CreateSalesQuotationViewModel();
            SalesQuotation.QuotationDate = DateTime.Now;
            SalesQuotation.QuotationValidUntilDate = SalesQuotation.QuotationDate.AddDays(14);
            SalesQuotation.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.SalesQuotation);

            var customerLookup = await _salesQuotationAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _salesQuotationAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesQuotationAppService.CreateAsync(
                    ObjectMapper.Map<CreateSalesQuotationViewModel, SalesQuotationCreateDto>(SalesQuotation)
                    );
                return NoContent();

            }
            catch (SalesQuotationAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateSalesQuotationViewModel
        {
            [Required]
            [SelectItems(nameof(Customers))]
            [DisplayName("Customer")]
            public Guid CustomerId { get; set; }

            [Required]
            [SelectItems(nameof(SalesExecutives))]
            [DisplayName("Sales Executive")]
            public Guid SalesExecutiveId { get; set; }

            [Required]
            [StringLength(SalesQuotationConsts.MaxNumberLength)]
            public string Number { get; set; }

            [TextArea]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Quotation Date")]
            public DateTime QuotationDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayName("Quotation Valid Until Date")]
            public DateTime QuotationValidUntilDate { get; set; }
        }
    }
}
