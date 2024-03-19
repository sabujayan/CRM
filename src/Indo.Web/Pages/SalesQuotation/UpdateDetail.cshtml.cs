using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Indo.SalesQuotationDetails;
using Indo.SalesQuotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Indo.Web.Pages.SalesQuotation
{
    public class UpdateDetailModel : IndoPageModel
    {
        [BindProperty]
        public SalesQuotationDetailUpdateViewModel SalesQuotationDetail { get; set; }
        public List<SelectListItem> SalesQuotations { get; set; }
        public List<SelectListItem> Products { get; set; }
        public SalesQuotationStatus Status { get; set; }

        private readonly ISalesQuotationDetailAppService _salesQuotationDetailAppService;
        public UpdateDetailModel(ISalesQuotationDetailAppService salesQuotationDetailAppService)
        {
            _salesQuotationDetailAppService = salesQuotationDetailAppService;
        }
        public async Task OnGetAsync(Guid id)
        {
            var dto = await _salesQuotationDetailAppService.GetAsync(id);
            Status = dto.Status;
            SalesQuotationDetail = ObjectMapper.Map<SalesQuotationDetailReadDto, SalesQuotationDetailUpdateViewModel>(dto);

            var salesQuotationLookup = await _salesQuotationDetailAppService.GetSalesQuotationLookupAsync();
            SalesQuotations = salesQuotationLookup.Items
                .Where(x => x.Id.Equals(SalesQuotationDetail.SalesQuotationId))
                .Select(x => new SelectListItem(x.Number, x.Id.ToString()))
                .ToList();

            var productLookup = await _salesQuotationDetailAppService.GetProductLookupAsync();
            Products = productLookup.Items
                .Select(x => new SelectListItem($"{x.Name} [Price: {x.RetailPrice.ToString("##,##.00")}]", x.Id.ToString()))
                .ToList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _salesQuotationDetailAppService.UpdateAsync(
                    SalesQuotationDetail.Id,
                    ObjectMapper.Map<SalesQuotationDetailUpdateViewModel, SalesQuotationDetailUpdateDto>(SalesQuotationDetail)
                );
                return NoContent();

            }
            catch (Exception)
            {
                throw new UserFriendlyException($"Posting Error");
            }
        }

        public class SalesQuotationDetailUpdateViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [Required]
            [SelectItems(nameof(SalesQuotations))]
            [DisplayName("Sales Quotations")]
            public Guid SalesQuotationId { get; set; }

            [Required]
            [SelectItems(nameof(Products))]
            [DisplayName("Product")]
            public Guid ProductId { get; set; }

            [Required]
            public float Quantity { get; set; }

            [DisplayName("Discount Amount")]
            public float DiscAmt { get; set; }
        }
    }
}
