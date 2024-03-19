using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.NumberSequences;
using Indo.ServiceQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class CreateModel : IndoPageModel
    {
        [BindProperty]
        public CreateServiceQuotationViewModel ServiceQuotation { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IServiceQuotationAppService _serviceQuotationAppService;

        private readonly INumberSequenceAppService _numberSequenceAppService;
        public CreateModel(
            IServiceQuotationAppService serviceQuotationAppService,
            INumberSequenceAppService numberSequenceAppService
            )
        {
            _serviceQuotationAppService = serviceQuotationAppService;
            _numberSequenceAppService = numberSequenceAppService;
        }
        public async Task OnGetAsync()
        {
            ServiceQuotation = new CreateServiceQuotationViewModel();
            ServiceQuotation.QuotationDate = DateTime.Now;
            ServiceQuotation.QuotationValidUntilDate = ServiceQuotation.QuotationDate.AddDays(14);
            ServiceQuotation.Number = await _numberSequenceAppService.GetNextNumberAsync(NumberSequenceModules.ServiceQuotation);

            var customerLookup = await _serviceQuotationAppService.GetCustomerLookupAsync();
            Customers = customerLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();

            var salesExectuiveLookup = await _serviceQuotationAppService.GetSalesExecutiveLookupAsync();
            SalesExecutives = salesExectuiveLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _serviceQuotationAppService.CreateAsync(
                    ObjectMapper.Map<CreateServiceQuotationViewModel, ServiceQuotationCreateDto>(ServiceQuotation)
                    );
                return NoContent();

            }
            catch (ServiceQuotationAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }
        public class CreateServiceQuotationViewModel
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
            [StringLength(ServiceQuotationConsts.MaxNumberLength)]
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
