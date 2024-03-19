using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.ServiceQuotationDetails;
using Indo.ServiceQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.ServiceQuotation
{
    public class CreateDetailModel : IndoPageModel
    {
        [BindProperty]
        public CreateServiceQuotationDetailViewModel ServiceQuotationDetail { get; set; }
        public List<SelectListItem> ServiceQuotations { get; set; }
        public List<SelectListItem> Services { get; set; }

        private readonly IServiceQuotationDetailAppService _serviceQuotationDetailAppService;
        public CreateDetailModel(IServiceQuotationDetailAppService serviceQuotationDetailAppService)
        {
            _serviceQuotationDetailAppService = serviceQuotationDetailAppService;
        }
        public async Task OnGetAsync(Guid serviceQuotationId)
        {
            ServiceQuotationDetail = new CreateServiceQuotationDetailViewModel();
            ServiceQuotationDetail.ServiceQuotationId = serviceQuotationId;

            var serviceQuotationLookup = await _serviceQuotationDetailAppService.GetServiceQuotationLookupAsync();
            ServiceQuotations = serviceQuotationLookup.Items
                .Where(x => x.Id.Equals(serviceQuotationId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var serviceLookup = await _serviceQuotationDetailAppService.GetServiceLookupAsync();
            Services = serviceLookup.Items
                .Select(x => new SelectListItem($"{x.Name} [Price: {x.Price.ToString("##,##.00")}]", x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _serviceQuotationDetailAppService.CreateAsync(
                    ObjectMapper.Map<CreateServiceQuotationDetailViewModel, ServiceQuotationDetailCreateDto>(ServiceQuotationDetail)
                    );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }
        public class CreateServiceQuotationDetailViewModel
        {
            [Required]
            [SelectItems(nameof(ServiceQuotations))]
            [DisplayName("Service Quotations")]
            public Guid ServiceQuotationId { get; set; }

            [Required]
            [SelectItems(nameof(Services))]
            [DisplayName("Services")]
            public Guid ServiceId { get; set; }

            [Required]
            public float Quantity { get; set; }

            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }
        }
    }
}
