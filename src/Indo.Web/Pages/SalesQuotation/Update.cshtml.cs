using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesQuotation
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public SalesQuotationUpdateViewModel SalesQuotation { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly ISalesQuotationAppService _salesQuotationAppService;
        public UpdateModel(ISalesQuotationAppService salesQuotationAppService)
        {
            _salesQuotationAppService = salesQuotationAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesQuotationAppService.GetAsync(id);
            SalesQuotation = ObjectMapper.Map<SalesQuotationReadDto, SalesQuotationUpdateViewModel>(dto);

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
                await _salesQuotationAppService.UpdateAsync(
                    SalesQuotation.Id,
                    ObjectMapper.Map<SalesQuotationUpdateViewModel, SalesQuotationUpdateDto>(SalesQuotation)
                );
                return NoContent();

            }
            catch (SalesQuotationAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class SalesQuotationUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

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

            [HiddenInput]
            public SalesQuotationPipeline Pipeline { get; set; }
        }
    }
}
