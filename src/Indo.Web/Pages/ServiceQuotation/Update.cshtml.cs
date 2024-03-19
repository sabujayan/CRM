using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ServiceQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class UpdateModel : IndoPageModel
    {
        [BindProperty]
        public ServiceQuotationUpdateViewModel ServiceQuotation { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public List<SelectListItem> SalesExecutives { get; set; }

        private readonly IServiceQuotationAppService _serviceQuotationAppService;
        public UpdateModel(IServiceQuotationAppService serviceQuotationAppService)
        {
            _serviceQuotationAppService = serviceQuotationAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _serviceQuotationAppService.GetAsync(id);
            ServiceQuotation = ObjectMapper.Map<ServiceQuotationReadDto, ServiceQuotationUpdateViewModel>(dto);

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
                await _serviceQuotationAppService.UpdateAsync(
                    ServiceQuotation.Id,
                    ObjectMapper.Map<ServiceQuotationUpdateViewModel, ServiceQuotationUpdateDto>(ServiceQuotation)
                );
                return NoContent();

            }
            catch (ServiceQuotationAlreadyExistsException ex)
            {
                throw new UserFriendlyException($"{ex.Code}");
            }
        }

        public class ServiceQuotationUpdateViewModel
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

            [HiddenInput]
            public ServiceQuotationPipeline Pipeline { get; set; }
        }
    }
}
